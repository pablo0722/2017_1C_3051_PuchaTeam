using System.Collections.Generic;
using Microsoft.DirectX;

namespace TGC.Group.Model
{
    public class t_ZombieComun
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_ZombieInstancia
        {
            public float vida;
            public int fila;
            public int columna;
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombie-TgcScene.xml";
        private const float VELOCIDAD_ZOMBIE = -2F;
        private const float VIDA_ZOMBIE_COMUN = 10;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        static int _ZombieN; // Se usa para ir creando los zombies conforme transcurre el tiempo
        
        // NO ESTATICAS
        protected t_Objeto3D _Zombie;
        protected GameModel _game;
        protected List<t_ZombieInstancia> _InstZombie;










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

            _InstZombie = new List<t_ZombieInstancia>();
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

            for (int i = 0; i < _Zombie._instancias.Count; i++)
            {
                if (_Zombie._instancias[i].pos.Z < t_EscenarioBase.PASTO_POS_Z_INICIAL + (t_EscenarioBase.PASTO_RAZON * (_InstZombie[i].columna - 0.5F)))
                {
                    t_ZombieInstancia zombie = _InstZombie[i];
                    zombie.columna--;
                    _InstZombie[i] = zombie;
                }

                if (t_EscenarioBase.Is_PastoOcupado(_InstZombie[i].fila, _InstZombie[i].columna))
                {
                    bool encontrado = false;
                    for(int j=0; j<_game._Girasol._InstGirasol.Count; j++)
                    {
                        if(_game._Girasol._InstPlanta[j].fila == _InstZombie[i].fila && _game._Girasol._InstPlanta[j].columna == _InstZombie[i].columna)
                        {
                            encontrado = true;
                            t_Planta.t_PlantaInstancia planta = _game._Girasol._InstPlanta[j];
                            planta.vida -= _game.ElapsedTime;
                            _game._Girasol._InstPlanta[j] = planta;

                            if(planta.vida <= 0)
                            {
                                _game._Girasol._Planta.Inst_Delete(j);
                                _game._Girasol._InstGirasol.Remove(_game._Girasol._InstGirasol[j]);
                                _game._Girasol._InstPlanta.Remove(_game._Girasol._InstPlanta[j]);
                            }
                        }
                    }
                }
                else
                {
                    Vector3 PosAux = _Zombie._instancias[i].pos;
                    _Zombie._instancias[i].pos = new Vector3(PosAux.X, PosAux.Y, PosAux.Z + VELOCIDAD_ZOMBIE*_game.ElapsedTime);
                }
            }

            if (GeneracionInfinitaDeZombies)
            {
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[0] * (_ZombieN + 1))
                {
                    t_ZombieInstancia zombie = new t_ZombieInstancia();
                    zombie.vida = VIDA_ZOMBIE_COMUN;
                    zombie.fila = _game._rand.Next(0, 5);
                    zombie.columna = 13;
                    _InstZombie.Add(zombie);

                    _Zombie.Inst_Create(-32 + 21 * zombie.fila, 0, 90);

                    _ZombieN++;
                }
            }
            else if (_ZombieN < SegundosAEsperarParaCrearZombie.Count)
            {
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[_ZombieN])
                {
                    _Zombie.Inst_Create(-32 + 21 * _game._rand.Next(0, 5), 0, 90);

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
