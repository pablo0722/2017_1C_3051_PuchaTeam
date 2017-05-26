using TGC.Core.Shaders;
using TGC.Core.Utils;
using TGC.Core.SceneLoader;



namespace TGC.Group.Model
{
    public class t_shader
    {
        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private GameModel _game;
        private t_fog fog;
        private t_Girar Girar;
        private t_ToonShading ToonShading;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_shader(GameModel game)
        {
            _game = game;

            fog = new t_fog(game);
            Girar = new t_Girar(game);
            ToonShading = new t_ToonShading(game);
        }










        /******************************************************************************************/
        /*                                      RENDERIZA EFECTOS
        /******************************************************************************************/
        public void Render(TgcMesh mesh, t_Objeto3D.t_instancia.t_ShadersHabilitados shaders)
        {
            fog.Render(mesh);

            ToonShading.Render(mesh);

            if (shaders.Girar)
                Girar.Render(mesh);
        }
    }
}
