using Microsoft.DirectX;


namespace TGC.Group.Model
{
    public class t_SolComun
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\sol-TgcScene.xml";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private t_Objeto3D _Sol;
        private GameModel _game;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_SolComun( GameModel game)
        {
            _game = game;

            _Sol = t_Objeto3D.Crear(_game, PATH_OBJ);

            _Sol.Set_Transform(0, 0, 0,
                                (float)0.075, (float)0.075, (float)0.075,
                                0, 0, 0);

            _Sol.Inst_Create(0, 50, 0);
            _Sol.Inst_Create(20, 50, 30);

            _Sol.Inst_RotateAll(_game.ElapsedTime / GameModel.PI, 0, GameModel.PI / 2);
        }

        public static t_SolComun Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_SolComun(game);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, float Rotacion_SegundosPorVuelta)
        {
            _Sol.Update(ShowBoundingBoxWithKey);

            _Sol.Inst_RotateAll(_game.ElapsedTime * Rotacion_SegundosPorVuelta / (2*GameModel.PI), 0, 0);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Sol.Render();
        }
    }
}
