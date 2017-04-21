using System.Collections.Generic;

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
        static int _ZombieN; // Se usa para ir creando los zombies conforme transcurre el tiempo










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_ZombieComun(GameModel game)
        {
            _game = game;

            _ZombieN = 0;

            _Zombie = t_Objeto3D.Crear(_game, PATH_OBJ);

            _Zombie.Set_Size((float)0.25, (float)0.25, (float)0.25);

            /*
            _Zombie.Inst_Create(-32, 0, 70);
            _Zombie.Inst_Create(-32 + 21, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 2, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 3, 0, 70);
            _Zombie.Inst_Create(-32 + 21 * 4, 0, 70);
            */
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
        public void Update(bool ShowBoundingBoxWithKey, List<int> SegundosAEsperarParaCrearZombie, bool GeneracionInfinitaDeZombies)
        {
            if (_game.ElapsedTime < 1000)
            {
                _game._TiempoTranscurrido += _game.ElapsedTime;
            }

            _Zombie.Update(ShowBoundingBoxWithKey);

            _Zombie.Inst_MoveAll(0, 0, -0.01F);

            if (GeneracionInfinitaDeZombies)
            {
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[0] * (_ZombieN + 1))
                {
                    _Zombie.Inst_Create(-32 + 21 * _game._rand.Next(0, 5), 0, 70);

                    _ZombieN++;
                }
            }
            else if (_ZombieN < SegundosAEsperarParaCrearZombie.Count)
            {
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[_ZombieN])
                {
                    _Zombie.Inst_Create(-32 + 21 * _game._rand.Next(0, 5), 0, 70);

                    _ZombieN++;
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _game.Func_Text("Tiempo transcurrido: ", 800, 10);
            _game.Func_Text(_game._TiempoTranscurrido.ToString(), 950, 10);

            _Zombie.Render();
        }
    }
}
