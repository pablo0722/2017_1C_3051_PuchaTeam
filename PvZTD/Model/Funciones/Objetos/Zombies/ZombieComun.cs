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
        private const float VIDA_ZOMBIE_COMUN = 10; //10










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // NO ESTATICAS
        public string _NivelActual = null;
        public float[] _TiemposDeSiguientesZombies = null;
        public int _NZombieActual = 0;
        public t_Objeto3D _Zombie;
        protected GameModel _game;
        public List<t_ZombieInstancia> _InstZombie;
        public float velocidad_zombie = VELOCIDAD_ZOMBIE;
        public float vida_zombie_comun = VIDA_ZOMBIE_COMUN;
        public string _TxtZombieNivel = "Zcomun"; // Nombre que va a tener el zombie comun dentro del archivo de texto del nivel
        public static bool gameover = false;









        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_ZombieComun(GameModel game, string path_obj = PATH_OBJ)
        {
            _game = game;

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
        public void Update(bool ShowBoundingBoxWithKey, bool GeneracionInfinitaDeZombies)
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
                                if (_game._Girasol.Is_Personal)
                                {
                                    if (_game._Girasol._instPersonal == _game._Girasol._Planta._instancias[j])
                                    {
                                        _game._camara.Modo_Aerea();
                                    }
                                }
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
                                    if (_game._Lanzaguisantes.Is_Personal)
                                    {
                                        if (_game._Lanzaguisantes._instPersonal == _game._Lanzaguisantes._Planta._instancias[j])
                                        {
                                            _game._camara.Modo_Aerea();
                                        }
                                    }
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
                                    if (_game._Patatapum.Is_Personal)
                                    {
                                        if (_game._Patatapum._instPersonal == _game._Patatapum._Planta._instancias[j])
                                        {
                                            _game._camara.Modo_Aerea();
                                        }
                                    }
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
                                    if(_game._repetidor.Is_Personal)
                                    {
                                        if(_game._repetidor._instPersonal == _game._repetidor._Planta._instancias[j])
                                        {
                                            _game._camara.Modo_Aerea();
                                        }
                                    }
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
                    // Si la fila y columna actual del zombie NO coincide con la de alguna planta, el zombie avanza
                    Vector3 PosAux = _Zombie._instancias[i].pos;
                    if (PosAux.Z < -75)
                    {
                        gameover = true;
                    }
                    else
                    {
                        _Zombie._instancias[i].pos = new Vector3(PosAux.X, PosAux.Y, PosAux.Z + velocidad_zombie * _game.ElapsedTime);
                    }
                }
            }
            
            if (String.Compare(_NivelActual, _game._NivelActual)!=0)
            {
                gameover = false;
                _NivelActual = _game._NivelActual;
                string txt_nivel = System.IO.File.ReadAllText(_game._NivelActual);
                string[] tags = txt_nivel.Split('<', '>');
                for(int i=0; i<tags.Length; i++)
                {
                    if(String.Compare(tags[i], _TxtZombieNivel) == 0)
                    {
                        var tiempos = tags[i+1].Split(',');
                        _TiemposDeSiguientesZombies = new float[tiempos.Length - 2];
                        for (int j=1; j< tiempos.Length-1; j++)
                        {
                            _TiemposDeSiguientesZombies[j - 1] = int.Parse(tiempos[j]);
                        }
                        _NZombieActual = 0;
                        break;
                    }
                }
            }
            else
            {
                if (_TiemposDeSiguientesZombies != null)
                {
                    if (_NZombieActual < _TiemposDeSiguientesZombies.Length)
                    {
                        // Genera los zombies por tiempo
                        if (_TiemposDeSiguientesZombies[_NZombieActual] > 0)
                        {
                            while (_game._TiempoTranscurrido >= _TiemposDeSiguientesZombies[_NZombieActual])
                            {
                                int fila = _game._rand.Next(0, 5);

                                _Zombie.Inst_Create(-32 + 21 * fila, 0, 90);

                                t_ZombieInstancia zombie = new t_ZombieInstancia();
                                zombie.fila = fila;
                                zombie.vida = vida_zombie_comun;
                                zombie.columna = 13;
                                zombie.zombie = _Zombie._instancias[_Zombie._instancias.Count - 1];
                                _InstZombie.Add(zombie);

                                _NZombieActual++;
                                if (_NZombieActual >= _TiemposDeSiguientesZombies.Length) break;
                            }
                        }
                    }
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

            _Zombie.Render(true);
        }
    }
}
