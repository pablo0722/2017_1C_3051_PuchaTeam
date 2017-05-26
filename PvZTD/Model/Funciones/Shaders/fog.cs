using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using System.Drawing;
using Microsoft.DirectX.Direct3D;



namespace TGC.Group.Model
{
    public class t_fog
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_SHADER = "..\\..\\Media\\Shaders\\fog.fx";
        private const float AMPLITUD = 0.01F;
        private const float FRECUENCIA = (1/10F); // en vueltas por segundo
        private const float OFFSET = AMPLITUD/2;
        private const int COLOR_R = 64; // Color Rojo
        private const int COLOR_G = 64; // Color Verde
        private const int COLOR_B = 64; // Color Azul










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private GameModel _game;
        private Effect effect;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_fog(GameModel game)
        {
            _game = game;

            effect = TgcShaders.loadEffect(PATH_SHADER);
            effect.SetValue("ColorFog", Color.FromArgb(COLOR_R, COLOR_G, COLOR_B).ToArgb());
            effect.SetValue("StartFogDistance", 20);
            effect.SetValue("EndFogDistance", 0);
        }










        /******************************************************************************************/
        /*                                      RENDERIZA EFECTOS
        /******************************************************************************************/
        public void Render(TgcMesh mesh)
        {
            effect.SetValue("CameraPos", TgcParserUtils.vector3ToFloat4Array(_game.Camara.Position));
            effect.SetValue("Density", FastMath.Sin(2 * GameModel.PI * FRECUENCIA * _game._TiempoTranscurrido) * AMPLITUD + OFFSET);

            mesh.Effect = effect;
            mesh.Technique = "RenderScene";
        }
    }
}
