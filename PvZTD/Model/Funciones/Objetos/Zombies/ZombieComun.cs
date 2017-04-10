namespace TGC.Group.Model
{
    public class t_ZombieComun
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombie-TgcScene.xml";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        protected t_Objeto3D _Zombie;
        protected GameModel _game;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_ZombieComun(GameModel game)
        {
            _game = game;

            _Zombie = t_Objeto3D.Crear(_game, PATH_OBJ);

            _Zombie.Set_Size((float)0.25, (float)0.25, (float)0.25);

            _Zombie.Inst_Create(-32, 0, 70);
            _Zombie.Inst_Create(-32 + 21, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 2, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 3, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 4, 0, 70);
        }

        public static t_ZombieComun Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_ZombieComun(game);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey)
        {
            _Zombie.Update(ShowBoundingBoxWithKey);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Zombie.Render();
        }
    }
}
