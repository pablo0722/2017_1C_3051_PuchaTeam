using Microsoft.DirectX;


namespace TGC.Group.Model
{
    public class t_Escenario1 : t_EscenarioBase
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ_CASA =        "..\\..\\Media\\Objetos\\CasaDave-TgcScene.xml";
        private const string PATH_OBJ_PLANO =       "..\\..\\Media\\Objetos\\plano-TgcScene.xml";
        private const string PATH_OBJ_PASTO =       "..\\..\\Media\\Objetos\\plano-TgcScene.xml";
        private const string PATH_OBJ_CEREBRO =     "..\\..\\Media\\Objetos\\Brain-TgcScene.xml";
        private const string PATH_OBJ_MOUNTAIN =    "..\\..\\Media\\Objetos\\Mountain-TgcScene.xml";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private t_Objeto3D _Mountain;
        private GameModel _game;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Escenario1(GameModel game) : base(game, PATH_OBJ_CASA, PATH_OBJ_PLANO, PATH_OBJ_PASTO, PATH_OBJ_CEREBRO)
        {
            _game = game;

            _Mountain = t_Objeto3D.Crear(_game, PATH_OBJ_MOUNTAIN);

            _Mountain.Inst_Create(-130, 0, 140);
            _Mountain.Inst_Create(-122, 0, 100);
            _Mountain.Inst_Create(-124, 0, 50);
            _Mountain.Inst_Create(-100, 0, 0);
            _Mountain.Inst_Create(-121, 0, -50);
            _Mountain.Inst_Create(-123, 0, -100);
            _Mountain.Inst_Create(-130, 0, -140);
        }

        public static t_Escenario1 Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_Escenario1(game);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public new void Update(bool ShowBoundingBoxWithKey, float Rotacion_SegundosPorVuelta)
        {
            base.Update(ShowBoundingBoxWithKey, Rotacion_SegundosPorVuelta);
            _Mountain.Update(ShowBoundingBoxWithKey);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();
            _Mountain.Render();
        }
    }
}
