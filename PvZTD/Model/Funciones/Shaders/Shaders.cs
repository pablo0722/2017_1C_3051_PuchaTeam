using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using TGC.Core.Direct3D;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using TGC.Core.Textures;



namespace TGC.Group.Model
{
    public class t_shader
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_SHADER = "..\\..\\Media\\Shaders\\ShadersComunes.fx";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private GameModel _game;
        private Effect effect;
        private Vector3 g_LightDir; // direccion de la luz actual
        private Vector3 g_LightPos; // posicion de la luz actual (la que estoy analizando)
        private Matrix g_LightView; // matriz de view del light
        private Matrix g_mShadowProj; // Projection matrix for shadow map
        private Surface g_pDSShadow; // Depth-stencil buffer for rendering to shadow map
        private Texture g_pShadowMap; // Texture to which the shadow map is rendered
        private bool DoShadow = false;
        public float time2 = 0;
        public float timeExplota = 0;
        public Vector4 ExplosionLightPosition = new Vector4(0, -100, 0, 1);
        private Texture texFuego;
        private Texture texFuegoAlpha;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_shader(GameModel game)
        {
            _game = game;

            time2 = -1;
            timeExplota = -1;

            effect = TgcShaders.loadEffect(PATH_SHADER);

            // Init Shaders Comunes
            InitFog();
            InitShadow();
            InitExplosion();

            // Init Shaders Especiales
            InitGirar();
            InitFuego();    // Para jalapeño
        }










        /******************************************************************************************/
        /*                                      INIT SHADERS
        /******************************************************************************************/

        // SHADERS COMUNES (Se aplican a todos los meshes)
        private void InitFog()
        {
            const int COLOR_R = 128; // Color Rojo
            const int COLOR_G = 128; // Color Verde
            const int COLOR_B = 128; // Color Azul

            effect.SetValue("FogColor", Color.FromArgb(COLOR_R, COLOR_G, COLOR_B).ToArgb());
            effect.SetValue("FogStartDistance", 20);
            effect.SetValue("FogEndDistance", 0);
        }

        private void InitShadow()
        {
            const int SHADOWMAP_SIZE = 1024;
            const float FAR_PLANE = 1500f;
            const float NEAR_PLANE = 2f;

            //--------------------------------------------------------------------------------------
            // Creo el shadowmap.
            // Format.R32F
            // Format.X8R8G8B8
            g_pShadowMap = new Texture(D3DDevice.Instance.Device, SHADOWMAP_SIZE, SHADOWMAP_SIZE,
                            1, Usage.RenderTarget, Format.R32F,
                            Pool.Default);

            // tengo que crear un stencilbuffer para el shadowmap manualmente
            // para asegurarme que tenga la el mismo tamano que el shadowmap, y que no tenga
            // multisample, etc etc.
            g_pDSShadow = D3DDevice.Instance.Device.CreateDepthStencilSurface(SHADOWMAP_SIZE,
                            SHADOWMAP_SIZE,
                            DepthFormat.D24S8,
                            MultiSampleType.None,
                            0,
                            true);

            // por ultimo necesito una matriz de proyeccion para el shadowmap, ya
            // que voy a dibujar desde el pto de vista de la luz.
            // El angulo tiene que ser mayor a 45 para que la sombra no falle en los extremos del cono de luz
            // de hecho, un valor mayor a 90 todavia es mejor, porque hasta con 90 grados es muy dificil
            // lograr que los objetos del borde generen sombras
            var aspectRatio = D3DDevice.Instance.AspectRatio;
            g_mShadowProj = Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(80), aspectRatio, 50, 5000);
            D3DDevice.Instance.Device.Transform.Projection =
                            Matrix.PerspectiveFovLH(Geometry.DegreeToRadian(45.0f), aspectRatio, NEAR_PLANE, FAR_PLANE);
        }

        private void InitExplosion()
        {
            const float ExplosionLightAttenuation = 0.01F;
            
            effect.SetValue("ExplosionLightPosition", ExplosionLightPosition);
            effect.SetValue("ExplosionLightAttenuation", ExplosionLightAttenuation);
        }

        // SHADERS ESPECIALES (Se aplican a algunos meshes)
        private void InitGirar()
        {
            effect.SetValue("time", 0);
        }

        private void InitFuego()
        {
            texFuego = Texture.FromBitmap(D3DDevice.Instance.Device,
                (Bitmap)Image.FromFile("..\\..\\Media\\Texturas\\fire\\fire_perturbed.jpg"), Usage.None, Pool.Managed);
            texFuegoAlpha = Texture.FromBitmap(D3DDevice.Instance.Device,
                (Bitmap)Image.FromFile("..\\..\\Media\\Texturas\\fire\\fire_perturbed_alpha.jpg"), Usage.None, Pool.Managed);
        }



        /******************************************************************************************/
        /*                                      RENDERIZA SHADERS (Sin post procesamiento)
        /******************************************************************************************/
        public void Render(TgcMesh mesh, t_Objeto3D.t_instancia.t_ShadersHabilitados shaders)
        {
            effect.SetValue("time2", time2);
            effect.SetValue("timeExplota", timeExplota);
            effect.SetValue("time", _game._TiempoTranscurrido);
            effect.SetValue("frameTime", _game.ElapsedTime);
            effect.SetValue("CameraPos", TgcParserUtils.vector3ToFloat4Array(_game.Camara.Position));

            // Renderiza shaders comunes
            RenderFog(mesh);
            RenderShadow(mesh);
            RenderLuzExplosion(mesh);

            // Setea tecnica de shaders comunes
            mesh.Effect = effect;
            if(DoShadow)
                mesh.Technique = "TecnicaShadowMap";
            else
                mesh.Technique = "TecnicaComun";

            // Renderiza shaders especiales
            if (shaders.Girar)
                RenderGirar(mesh);

            if (shaders.BolaDeFuego)
                RenderBolaDeFuego(mesh);

            if (shaders.BolaDeExplosion)
                RenderBolaDeExplosion(mesh);

            if (shaders.SuperGirasol)
                RenderSuperGirasol(mesh);

            if (shaders.Explosion)
                RenderExplosion(mesh);

            if(shaders.GirarSoles)
                RenderSol(mesh);

            if (shaders.fuegoJalapenio)
                fuegoJalapenio(mesh);

            if (shaders.JalapenioExplota)
                JalapenioExplota(mesh);
        }


            // SHADERS COMUNES
        private void RenderFog(TgcMesh mesh)
        {
            const float AMPLITUD = 0.002F;
            const float FRECUENCIA = (1 / 10F); // en vueltas por segundo
            const float OFFSET = AMPLITUD / 2;

            effect.SetValue("FogDensity", FastMath.Sin(2 * GameModel.PI * FRECUENCIA * _game._TiempoTranscurrido) * AMPLITUD + OFFSET);
        }

        private void RenderShadow(TgcMesh mesh)
        {
        }

        private void RenderLuzExplosion(TgcMesh mesh)
        {
            if (time2 >= 0 && time2 <= 3)
            {
                effect.SetValue("ExplosionLightOn", 1);
                effect.SetValue("ExplosionLightPosition", ExplosionLightPosition);
            }
            else
            {
                effect.SetValue("ExplosionLightOn", 0);
            }
        }

        // SHADERS ESPECIALES
        private void RenderGirar(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaGirar";
        }

        private void RenderBolaDeFuego(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaBolaDeFuego";
        }

        private void RenderBolaDeExplosion(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaBolaDeExplosion";
        }

        private void RenderExplosion(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaExplosion";
            TGC.Core.Direct3D.D3DDevice.Instance.Device.RenderState.AlphaBlendEnable = true;    // Activa canal alpha en shaders
        }

        private void RenderSuperGirasol(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaSuperGirasol";
        }

        public void RenderSol(TgcMesh mesh)
        {
            mesh.Technique = "RenderSol";
        }

        public void fuegoJalapenio(TgcMesh mesh)
        {
            effect.SetValue("texFuego", texFuego);
            effect.SetValue("texFuegoAlpha", texFuegoAlpha);

            mesh.Technique = "TecnicaFuego";

            TGC.Core.Direct3D.D3DDevice.Instance.Device.RenderState.AlphaBlendEnable = true;    // Activa canal alpha en shaders
        }

        public void JalapenioExplota(TgcMesh mesh)
        {
            mesh.Technique = "TecnicaExplotaJalapenio";
        }

        /******************************************************************************************/
        /*                                      SHADER DE POST PROCESAMIENTO
        /******************************************************************************************/
        public void PostProc()
        {
            g_LightPos = new Vector3(20, 100, 0);
            g_LightDir = new Vector3(-0.1F, -1, 0);
            g_LightDir.Normalize();


            //Genero el shadow map
            PostProcShadow();

            D3DDevice.Instance.Device.BeginScene();

            // Dibujo la escena pp dicha
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            
            _game.RenderScene();
            _game.RenderHud();
        }

        public void PostProcShadow()
        {
            // Calculo la matriz de view de la luz
            effect.SetValue("g_vLightPos", new Vector4(g_LightPos.X, g_LightPos.Y, g_LightPos.Z, 1));
            effect.SetValue("g_vLightDir", new Vector4(g_LightDir.X, g_LightDir.Y, g_LightDir.Z, 1));
            g_LightView = Matrix.LookAtLH(g_LightPos, g_LightPos + g_LightDir, new Vector3(0, 0, 1));

            // inicializacion standard:
            effect.SetValue("g_mProjLight", g_mShadowProj);
            effect.SetValue("g_mViewLightProj", g_LightView * g_mShadowProj);

            // Primero genero el shadow map, para ello dibujo desde el pto de vista de luz
            // a una textura, con el VS y PS que generan un mapa de profundidades.
            var pOldRT = D3DDevice.Instance.Device.GetRenderTarget(0);
            var pShadowSurf = g_pShadowMap.GetSurfaceLevel(0);
            D3DDevice.Instance.Device.SetRenderTarget(0, pShadowSurf);
            var pOldDS = D3DDevice.Instance.Device.DepthStencilSurface;
            D3DDevice.Instance.Device.DepthStencilSurface = g_pDSShadow;
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            D3DDevice.Instance.Device.BeginScene();

            // Hago el render de la escena pp dicha
            effect.SetValue("g_txShadow", g_pShadowMap);
            DoShadow = true;
            _game.RenderScene();
            _game.RenderHud();
            DoShadow = false;

            // Termino
            D3DDevice.Instance.Device.EndScene();

            //TextureLoader.Save("shadowmap.bmp", ImageFileFormat.Bmp, g_pShadowMap);

            // restuaro el render target y el stencil
            D3DDevice.Instance.Device.DepthStencilSurface = pOldDS;
            D3DDevice.Instance.Device.SetRenderTarget(0, pOldRT);
        }
    }
}
