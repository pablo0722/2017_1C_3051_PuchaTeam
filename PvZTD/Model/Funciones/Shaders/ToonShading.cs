using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Core.Direct3D;
using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_ToonShading
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_SHADER = "..\\..\\Media\\Shaders\\ToonShading.fx";
        private const float LIGHT_X = 0;
        private const float LIGHT_Y = 200;
        private const float LIGHT_Z = 0;
        private const float AMBIENT = 64;
        private const float DIFFUSE = 64;
        private const float SPECULAR = 64;
        private const float SPECULAR_POWER = 2;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private static GameModel _game = null;
        private static List<TgcMesh> MeshesConToonShading = null;
        private static Effect effect;
        private static Texture g_pRenderTarget;
        private static Texture g_pNormals;
        private static VertexBuffer g_pVBV3D;
        private static Surface g_pDepthStencil; // Depth-stencil buffer










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_ToonShading(GameModel game)
        {
            if (_game != null) return;

            _game = game;

            MeshesConToonShading = new List<TgcMesh>();

            effect = TgcShaders.loadEffect(PATH_SHADER);

            // Creo un depthbuffer sin multisampling, para que sea compatible con el render to texture

            // Nota:
            // El render to Texture no es compatible con el multisampling en dx9
            // Por otra parte la mayor parte de las placas de ultima generacion no soportan
            // mutisampling para texturas de punto flotante con lo cual
            // hay que suponer con generalidad que no se puede usar multisampling y render to texture

            // Para resolverlo hay que crear un depth buffer que no tenga multisampling,
            // (de lo contrario falla el zbuffer y se producen artifacts tipicos de que no tiene zbuffer)

            // Si uno quisiera usar el multisampling, la tecnica habitual es usar un RenderTarget
            // en lugar de una textura.
            // Por ejemplo en c++:
            //
            // Render Target formato color buffer con multisampling
            //
            //  g_pd3dDevice->CreateRenderTarget(Ancho,Alto,
            //          D3DFMT_A8R8G8B8, D3DMULTISAMPLE_2_SAMPLES, 0,
            //          FALSE, &g_pRenderTarget, NULL);
            //
            // Luego, ese RenderTarget NO ES una textura, y nosotros necesitamos acceder a esos
            // pixeles, ahi lo que se hace es COPIAR del rendertartet a una textura,
            // para poder trabajar con esos datos en el contexto del Pixel shader:
            //
            // Eso se hace con la funcion StretchRect:
            // copia de rendertarget ---> sceneSurface (que es la superficie asociada a una textura)
            // g_pd3dDevice->StretchRect(g_pRenderTarget, NULL, g_pSceneSurface, NULL, D3DTEXF_NONE);
            //
            // Esta tecnica se llama downsampling
            // Y tiene el costo adicional de la transferencia de memoria entre el rendertarget y la
            // textura, pero que no traspasa los limites de la GPU. (es decir es muy performante)
            // no es lo mismo que lockear una textura para acceder desde la CPU, que tiene el problema
            // de transferencia via AGP.

            g_pDepthStencil =
                D3DDevice.Instance.Device.CreateDepthStencilSurface(
                    D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth,
                    D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight,
                    DepthFormat.D24S8, MultiSampleType.None, 0, true);

            // inicializo el render target
            g_pRenderTarget = new Texture(D3DDevice.Instance.Device,
                D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth
                , D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.X8R8G8B8, Pool.Default);

            effect.SetValue("g_RenderTarget", g_pRenderTarget);


            // inicializo el mapa de normales
            g_pNormals = new Texture(D3DDevice.Instance.Device,
                D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth
                , D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight, 1, Usage.RenderTarget,
                Format.A16B16G16R16F, Pool.Default);

            effect.SetValue("g_Normals", g_pNormals);

            // Resolucion de pantalla
            effect.SetValue("screen_dx", D3DDevice.Instance.Device.PresentationParameters.BackBufferWidth);
            effect.SetValue("screen_dy", D3DDevice.Instance.Device.PresentationParameters.BackBufferHeight);


            //Se crean 2 triangulos con las dimensiones de la pantalla con sus posiciones ya transformadas
            // x = -1 es el extremo izquiedo de la pantalla, x=1 es el extremo derecho
            // Lo mismo para la Y con arriba y abajo
            // la Z en 1 simpre
            CustomVertex.PositionTextured[] vertices =
            {
                new CustomVertex.PositionTextured(-1, 1, 1, 0, 0),
                new CustomVertex.PositionTextured(1, 1, 1, 1, 0),
                new CustomVertex.PositionTextured(-1, -1, 1, 0, 1),
                new CustomVertex.PositionTextured(1, -1, 1, 1, 1)
            };
            //vertex buffer de los triangulos
            g_pVBV3D = new VertexBuffer(typeof(CustomVertex.PositionTextured),
                4, D3DDevice.Instance.Device, Usage.Dynamic | Usage.WriteOnly,
                CustomVertex.PositionTextured.Format, Pool.Default);
            g_pVBV3D.SetData(vertices, 0, LockFlags.None);
        }










        /******************************************************************************************/
        /*                                      RENDERIZA EFECTOS
        /******************************************************************************************/
        public void Render(TgcMesh mesh)
        {
            MeshesConToonShading.Add(mesh);
        }

        public static void RenderGlobal()
        {
            Vector3 lightPosition = new Vector3(LIGHT_X, LIGHT_Y, LIGHT_Z);

            //Cargar variables de shader
            effect.SetValue("fvLightPosition", TgcParserUtils.vector3ToFloat3Array(lightPosition));
            effect.SetValue("fvEyePosition",
                TgcParserUtils.vector3ToFloat3Array(_game.Camara.Position));
            effect.SetValue("k_la", AMBIENT);
            effect.SetValue("k_ld", DIFFUSE);
            effect.SetValue("k_ls", SPECULAR);
            effect.SetValue("fSpecularPower", SPECULAR_POWER);

            D3DDevice.Instance.Device.EndScene();

            // dibujo la escena una textura
            effect.Technique = "DefaultTechnique";

            // guardo el Render target anterior y seteo la textura como render target
            var pOldRT = D3DDevice.Instance.Device.GetRenderTarget(0);
            var pSurf = g_pRenderTarget.GetSurfaceLevel(0);
            D3DDevice.Instance.Device.SetRenderTarget(0, pSurf);

            // hago lo mismo con el depthbuffer, necesito el que no tiene multisampling
            var pOldDS = D3DDevice.Instance.Device.DepthStencilSurface;

            // Probar de comentar esta linea, para ver como se produce el fallo en el ztest
            // por no soportar usualmente el multisampling en el render to texture.
            D3DDevice.Instance.Device.DepthStencilSurface = g_pDepthStencil;


            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            D3DDevice.Instance.Device.BeginScene();

            for (int i = 0; i < MeshesConToonShading.Count; i++)
            {
                MeshesConToonShading[i].Effect = effect;
                MeshesConToonShading[i].Technique = "DefaultTechnique";
                MeshesConToonShading[i].UpdateMeshTransform();
                MeshesConToonShading[i].render();
            }
            D3DDevice.Instance.Device.EndScene();

            // genero el normal map:
            pSurf = g_pNormals.GetSurfaceLevel(0);
            D3DDevice.Instance.Device.SetRenderTarget(0, pSurf);
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            D3DDevice.Instance.Device.BeginScene();
            for (int i = 0; i < MeshesConToonShading.Count; i++)
            {
                MeshesConToonShading[i].Effect = effect;
                MeshesConToonShading[i].Technique = "NormalMap";
                MeshesConToonShading[i].UpdateMeshTransform();
                MeshesConToonShading[i].render();
            }

            D3DDevice.Instance.Device.EndScene();

            // restuaro el render target y el stencil
            D3DDevice.Instance.Device.DepthStencilSurface = pOldDS;
            D3DDevice.Instance.Device.SetRenderTarget(0, pOldRT);

            // dibujo el quad pp dicho :
            D3DDevice.Instance.Device.BeginScene();
            //effect.Technique = (bool)Modifiers["blurActivated"] ? "CopyScreen" : "EdgeDetect";
            effect.Technique = "EdgeDetect";
            D3DDevice.Instance.Device.VertexFormat = CustomVertex.PositionTextured.Format;
            D3DDevice.Instance.Device.SetStreamSource(0, g_pVBV3D, 0);
            effect.SetValue("g_Normals", g_pNormals);
            effect.SetValue("g_RenderTarget", g_pRenderTarget);

            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            effect.Begin(FX.None);
            effect.BeginPass(0);
            D3DDevice.Instance.Device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            effect.EndPass();
            effect.End();

            //D3DDevice.Instance.Device.EndScene();
            //D3DDevice.Instance.Device.Present();

            MeshesConToonShading.Clear();
        }
    }
}
