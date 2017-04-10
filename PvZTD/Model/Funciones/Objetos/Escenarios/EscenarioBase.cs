using Microsoft.DirectX;


namespace TGC.Group.Model
{
    public class t_EscenarioBase
    {
        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private t_Objeto3D _Casa;
        private t_Objeto3D _Plano;
        private t_Objeto3D _Pasto;
        private t_Objeto3D _Cerebro;
        private GameModel _game;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_EscenarioBase(GameModel game, string PathCasa, string PathPlano, string PathPasto, string PathCerebro)
        {
            _game = game;

            // Casa
            _Casa = t_Objeto3D.Crear(_game, PathCasa);

            _Casa.Set_Position(7.5F, -7, -115);
            _Casa.Set_Size(0.5F, 0.5F, 0.5F);
            _Casa.Set_Rotation(0, GameModel.PI / 2, 0);

            _Casa.Inst_Create();

            // Plano
            _Plano = t_Objeto3D.Crear(_game, PathPlano);

            _Plano.Set_Size(4, 1, 4);

            _Plano.Inst_Create(0, 0, 0);

            // Pasto
            _Pasto = t_Objeto3D.Crear(_game, PathPasto);

            _Pasto.Inst_Create(-130, 0, 140);
            _Pasto.Inst_Create(-122, 0, 100);
            _Pasto.Inst_Create(-124, 0, 50);
            _Pasto.Inst_Create(-100, 0, 0);
            _Pasto.Inst_Create(-121, 0, -50);

            // Cerebros
            _Cerebro = t_Objeto3D.Crear(_game, PathCerebro);

            _Cerebro.Set_Position(50, 5, -70);
            _Cerebro.Set_Size(0.03F, 0.03F, 0.03F);

            for(int i=0; i<5; i++)
            {
                _Cerebro.Inst_CreateAndSelect();
                _Cerebro.Inst_Move(-13 * 1.6F * i, 0, 0);
            }
        }

        public static t_EscenarioBase Crear(GameModel game, string PathCasa, string PathPlano, string PathPasto, string PathCerebro)
        {
            if (game != null && PathCasa != null && PathPlano != null && PathPasto != null && PathCerebro != null)
            {
                return new t_EscenarioBase(game, PathCasa, PathPlano, PathPasto, PathCerebro);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, float Rotacion_SegundosPorVuelta)
        {
            _Casa.Update(ShowBoundingBoxWithKey);
            _Plano.Update(ShowBoundingBoxWithKey);
            _Pasto.Update(ShowBoundingBoxWithKey);
            _Cerebro.Update(ShowBoundingBoxWithKey);

            _Cerebro.Inst_RotateAll(0, _game.ElapsedTime * Rotacion_SegundosPorVuelta / (2 * GameModel.PI), 0);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Casa.Render();
            _Plano.Render();
            _Pasto.Render();
            _Cerebro.Render();
        }
    }
}
