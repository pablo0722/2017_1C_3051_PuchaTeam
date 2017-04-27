using System;
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
            public t_Objeto3D.t_instancia zombie;
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombie-TgcScene.xml";
        private const float VELOCIDAD_ZOMBIE = -2F;
        private const float VIDA_ZOMBIE_COMUN = 3; //10










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        public int _ZombieN; // Se usa para ir creando los zombies conforme transcurre el tiempo

        // NO ESTATICAS
        public t_Objeto3D _Zombie;
        protected GameModel _game;
        public List<t_ZombieInstancia> _InstZombie;
        public float velocidad_zombie = VELOCIDAD_ZOMBIE;
        public float vida_zombie_comun = VIDA_ZOMBIE_COMUN;









        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_ZombieComun(GameModel game, string path_obj = PATH_OBJ)
        {
            _game = game;

            _ZombieN = 0;

            _Zombie = t_Objeto3D.Crear(_game, path_obj);

            _Zombie.Set_Size((float)0.25, (float)0.25, (float)0.25);

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
            _Zombie.Update(ShowBoundingBoxWithKey);

            // Colision con plantas
            for (int i = 0; i < _Zombie._instancias.Count; i++)
            {
                // Actualiza columna del zombie con la posicion actual
                if (_Zombie._instancias[i].pos.Z < t_EscenarioBase.PASTO_POS_Z_INICIAL + (t_EscenarioBase.PASTO_RAZON * (_InstZombie[i].columna - 0.5F)))
                {
                    t_ZombieInstancia zombie = _InstZombie[i];
                    zombie.columna--;
                    _InstZombie[i] = zombie;
                }

                // Se fija si la fila y columna actual del zombie coincide con la de alguna planta
                if (t_EscenarioBase.Is_PastoOcupado(_InstZombie[i].fila, _InstZombie[i].columna))
                {
                    bool encontrado = false;

                    for (int j = 0; j < _game._Girasol._InstGirasol.Count; j++)
                    {
                        if (_game._Girasol._InstPlanta[j].fila == _InstZombie[i].fila && _game._Girasol._InstPlanta[j].columna == _InstZombie[i].columna)
                        {
                            // coincide con un girasol
                            encontrado = true;
                            t_Planta.t_PlantaInstancia planta = _game._Girasol._InstPlanta[j];
                            planta.vida -= _game.ElapsedTime;
                            _game._Girasol._InstPlanta[j] = planta;

                            if (planta.vida <= 0)
                            {
                                _game._Girasol._Planta.Inst_Delete(_game._Girasol._Planta._instancias[j]);
                                _game._Girasol._InstGirasol.Remove(_game._Girasol._InstGirasol[j]);
                                _game._Girasol._InstPlanta.Remove(_game._Girasol._InstPlanta[j]);
                                _game._EscenarioBase.Set_PastoDesocupado(_InstZombie[i].fila, _InstZombie[i].columna);
                            }
                            break;
                        }
                    }

                    if (!encontrado)
                    {
                        for (int j = 0; j < _game._Lanzaguisantes._InstLanzaguisantes.Count; j++)
                        {
                            if (_game._Lanzaguisantes._InstPlanta[j].fila == _InstZombie[i].fila && _game._Lanzaguisantes._InstPlanta[j].columna == _InstZombie[i].columna)
                            {
                                // coincide con un lanzaguisantes
                                encontrado = true;
                                t_Planta.t_PlantaInstancia planta = _game._Lanzaguisantes._InstPlanta[j];
                                planta.vida -= _game.ElapsedTime;
                                _game._Lanzaguisantes._InstPlanta[j] = planta;

                                if (planta.vida <= 0)
                                {
                                    _game._Lanzaguisantes._Planta.Inst_Delete(_game._Lanzaguisantes._Planta._instancias[j]);
                                    _game._Lanzaguisantes._InstLanzaguisantes.Remove(_game._Lanzaguisantes._InstLanzaguisantes[j]);
                                    _game._Lanzaguisantes._InstPlanta.Remove(_game._Lanzaguisantes._InstPlanta[j]);
                                    _game._EscenarioBase.Set_PastoDesocupado(_InstZombie[i].fila, _InstZombie[i].columna);
                                }
                                break;
                            }
                        }
                    }

                    if (!encontrado)
                    {
                        for (int j = 0; j < _game._Patatapum._InstPatatapum.Count; j++)
                        {
                            if (_game._Patatapum._InstPlanta[j].fila == _InstZombie[i].fila && _game._Patatapum._InstPlanta[j].columna == _InstZombie[i].columna)
                            {
                                // coincide con un patatapum
                                encontrado = true;
                                t_Planta.t_PlantaInstancia planta = _game._Patatapum._InstPlanta[j];
                                planta.vida -= _game.ElapsedTime;
                                _game._Patatapum._InstPlanta[j] = planta;

                                if (planta.vida <= 0)
                                {
                                    _game._Patatapum._Planta.Inst_Delete(_game._Patatapum._Planta._instancias[j]);
                                    _game._Patatapum._InstPatatapum.Remove(_game._Patatapum._InstPatatapum[j]);
                                    _game._Patatapum._InstPlanta.Remove(_game._Patatapum._InstPlanta[j]);
                                    _game._EscenarioBase.Set_PastoDesocupado(_InstZombie[i].fila, _InstZombie[i].columna);
                                }
                                break;
                            }
                        }
                    }

                    if (!encontrado)
                    {
                        for (int j = 0; j < _game._repetidor._InstRepetidor.Count; j++)
                        {
                            if (_game._repetidor._InstPlanta[j].fila == _InstZombie[i].fila && _game._repetidor._InstPlanta[j].columna == _InstZombie[i].columna)
                            {
                                // coincide con un Repetidor
                                encontrado = true;
                                t_Planta.t_PlantaInstancia planta = _game._repetidor._InstPlanta[j];
                                planta.vida -= _game.ElapsedTime;
                                _game._repetidor._InstPlanta[j] = planta;

                                if (planta.vida <= 0)
                                {
                                    _game._repetidor._Planta.Inst_Delete(_game._repetidor._Planta._instancias[j]);
                                    _game._repetidor._InstRepetidor.Remove(_game._repetidor._InstRepetidor[j]);
                                    _game._repetidor._InstPlanta.Remove(_game._repetidor._InstPlanta[j]);
                                    _game._EscenarioBase.Set_PastoDesocupado(_InstZombie[i].fila, _InstZombie[i].columna);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // Se fija si la fila y columna actual del zombie NO coincide con la de alguna planta, el zombie avanza
                    Vector3 PosAux = _Zombie._instancias[i].pos;
                    _Zombie._instancias[i].pos = new Vector3(PosAux.X, PosAux.Y, PosAux.Z + velocidad_zombie * _game.ElapsedTime);
                }
            }

            if (GeneracionInfinitaDeZombies)
            {
                // Genera los zombies por tiempo infinitamente
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[0] * (_ZombieN + 1))
                {
                    int fila = _game._rand.Next(0, 5);

                    _Zombie.Inst_Create(-32 + 21 * fila, 0, 90);

                    t_ZombieInstancia zombie = new t_ZombieInstancia();
                    zombie.fila = fila;
                    zombie.vida = vida_zombie_comun;
                    zombie.columna = 13;
                    zombie.zombie = _Zombie._instancias[_Zombie._instancias.Count - 1];
                    _InstZombie.Add(zombie);

                    _ZombieN++;
                }
            }
            else if (_ZombieN < SegundosAEsperarParaCrearZombie.Count)
            {
                // Genera los zombies por tiempo
                if (_game._TiempoTranscurrido >= SegundosAEsperarParaCrearZombie[_ZombieN])
                {
                    int fila = _game._rand.Next(0, 5);

                    _Zombie.Inst_Create(-32 + 21 * fila, 0, 90);

                    t_ZombieInstancia zombie = new t_ZombieInstancia();
                    zombie.fila = fila;
                    zombie.vida = vida_zombie_comun;
                    zombie.columna = 13;
                    zombie.zombie = _Zombie._instancias[_Zombie._instancias.Count - 1];
                    _InstZombie.Add(zombie);

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
