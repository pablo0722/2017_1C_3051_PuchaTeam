using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using System.Drawing;
using Microsoft.DirectX.Direct3D;



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
        private GameModel _game;
        private Effect effect;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_shader(GameModel game)
        {
            _game = game;

            effect = TgcShaders.loadEffect(PATH_SHADER);

            // Init Shaders Comunes
            InitFog();

            // Init Shaders Especiales
            InitGirar();
        }










        /******************************************************************************************/
        /*                                      INIT SHADERS
        /******************************************************************************************/

            // SHADERS COMUNES (Se aplican a todos los meshes)
        public void InitFog()
        {
            const int COLOR_R = 64; // Color Rojo
            const int COLOR_G = 64; // Color Verde
            const int COLOR_B = 64; // Color Azul

            effect.SetValue("FogColor", Color.FromArgb(COLOR_R, COLOR_G, COLOR_B).ToArgb());
            effect.SetValue("FogStartDistance", 20);
            effect.SetValue("FogEndDistance", 0);
        }

            // SHADERS ESPECIALES (Se aplican a algunos meshes)
        public void InitGirar()
        {
            effect.SetValue("time", 0);
        }










        /******************************************************************************************/
        /*                                      RENDERIZA SHADERS (Sin post procesamiento)
        /******************************************************************************************/
        public void Render(TgcMesh mesh, t_Objeto3D.t_instancia.t_ShadersHabilitados shaders)
        {
            effect.SetValue("time", _game._TiempoTranscurrido);
            effect.SetValue("CameraPos", TgcParserUtils.vector3ToFloat4Array(_game.Camara.Position));

            // Renderiza shaders comunes
            RenderFog(mesh);

            mesh.Effect = effect;
            mesh.Technique = "RenderComun";

            // Renderiza shaders especiales
            if (shaders.Girar)
                RenderGirar(mesh);
        }

            // SHADERS COMUNES
        public void RenderFog(TgcMesh mesh)
        {
            const float AMPLITUD = 0.005F;
            const float FRECUENCIA = (1 / 10F); // en vueltas por segundo
            const float OFFSET = AMPLITUD / 2;

            effect.SetValue("FogDensity", FastMath.Sin(2 * GameModel.PI * FRECUENCIA * _game._TiempoTranscurrido) * AMPLITUD + OFFSET);
        }

            // SHADERS ESPECIALES
        public void RenderGirar(TgcMesh mesh)
        {
            mesh.Technique = "RenderGirar";
        }










        /******************************************************************************************/
        /*                                      SHADER DE POST PROCESAMIENTO
        /******************************************************************************************/
        public static void PostProc()
        {

        }
    }
}
