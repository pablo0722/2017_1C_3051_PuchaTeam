using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;
using System.Drawing;
using Microsoft.DirectX.Direct3D;



namespace TGC.Group.Model
{
    public class t_Girar
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_SHADER = "..\\..\\Media\\Shaders\\girar.fx";
        private const float AMPLITUD = 0.01F;
        private const float FRECUENCIA = (1 / 10F); // en vueltas por segundo
        private const float OFFSET = AMPLITUD / 2;
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
        public t_Girar(GameModel game)
        {
            _game = game;

            effect = TgcShaders.loadEffect(PATH_SHADER);
            effect.SetValue("time", 0);
        }










        /******************************************************************************************/
        /*                                      RENDERIZA EFECTOS
        /******************************************************************************************/
        public void Render(TgcMesh mesh)
        {
            effect.SetValue("time", _game._TiempoTranscurrido);

            mesh.Effect = effect;
            mesh.Technique = "RenderScene";
        }
    }
}
