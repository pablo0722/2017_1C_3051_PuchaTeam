using System.Collections.Generic;



namespace TGC.Group.Model.Funciones.Objetos.Plantas
{
    public class t_Repetidor : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\guisante-TgcScene.xml";
        private const string PATH_SUPER_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\SuperGuisante2-TgcScene.xml";
        private const string PATH_EXPLOSION_OBJ = "..\\..\\Media\\Objetos\\Explosion-TgcScene.xml";
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\repetidora-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "..\\..\\Media\\Texturas\\HUD_repeater_sel.png";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_repeater.png";
        private const int PLANTA_VALOR = 200;
        private const float VIDA_PLANTA = 3;
        private const float TIEMPO_GUISANTE = 4;
        private const float DANIO_SUPER = 10;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_celda
        {
            public int fila;
            public int columna;
        }

        public class t_RepetidorInstancia
        {
            public struct guisante
            {
                public float x, y, z, zActual;
                public float tiempo;
                public t_Objeto3D.t_instancia GuisanteInst;
            };

            public float x, y, z;
            public int fila;
            public t_Objeto3D obj_guisante { get; set; }
            public t_Objeto3D SuperGuisante { get; set; }
            public t_Objeto3D obj_explosion { get; set; }
            public t_Objeto3D.t_instancia Repetidor;
            public t_PlantaInstancia planta;
            public GameModel _game;
            public List<guisante> guisantes;

            public t_RepetidorInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Repetidor)
            {
                this._game = game;

                this.Repetidor = Repetidor;

                guisantes = new List<guisante>();

                x = Repetidor.pos.X;
                y = Repetidor.pos.Y + 5;
                z = Repetidor.pos.Z;

                this.planta = planta;

                fila = planta.fila;

                obj_explosion = t_Objeto3D.Crear(game, PATH_EXPLOSION_OBJ);
                obj_explosion.Set_Size(0.04F, 0.04F, 0.04F);
                obj_explosion.Set_Position(x, -100, z);
                obj_explosion.Inst_CreateAndSelect(new Microsoft.DirectX.Vector3(0, -100, 0));
                obj_explosion.Inst_ShaderExplosion(true);
                obj_explosion.Inst_Set_PositionX(x);

                obj_guisante = t_Objeto3D.Crear(game, PATH_GUISANTE_OBJ);
                obj_guisante.Set_Size(0.04F, 0.04F, 0.04F);
                guisante guisante_aux;

                obj_guisante.Inst_CreateAndSelect(x, y, z);
                guisante_aux = new guisante();
                guisante_aux.GuisanteInst = obj_guisante._instanciaActual;
                guisante_aux.tiempo = TIEMPO_GUISANTE * 0F;
                guisante_aux.x = x;
                guisante_aux.y = y;
                guisante_aux.zActual = guisante_aux.z = z;
                guisantes.Add(guisante_aux);

                obj_guisante.Inst_CreateAndSelect(x, y, z);
                guisante_aux = new guisante();
                guisante_aux.GuisanteInst = obj_guisante._instanciaActual;
                guisante_aux.tiempo = TIEMPO_GUISANTE*0.05F;
                guisante_aux.x = x;
                guisante_aux.y = y;
                guisante_aux.zActual = guisante_aux.z = z;
                guisantes.Add(guisante_aux);

                // Guisantes para la super
                SuperGuisante = t_Objeto3D.Crear(game, PATH_SUPER_GUISANTE_OBJ);
                SuperGuisante.Set_Size(0.1F, 0.1F, 0.1F);
                SuperGuisante.Inst_CreateAndSelect(x, -100F, z);

                SuperGuisante.Inst_ShaderBolaDeExplosion(true);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                obj_guisante.Update(ShowBoundingBoxWithKey);
                SuperGuisante.Update(ShowBoundingBoxWithKey);

                colision(obj_guisante, _game._zombie);
                colision(obj_guisante, _game._zombieCono);
                colision(obj_guisante, _game._zombieBalde);

                if(_game.shader.time2 > 3)
                {
                    obj_explosion.Inst_Set_PositionY(-100);
                    obj_explosion.Inst_ShaderExplosion(false);
                }

                // Para onda expansiva de la super
                t_celda CeldaZombie;
                CeldaZombie = colision(SuperGuisante, _game._zombie);
                if(CeldaZombie.fila >= 0)
                {
                    OndaExpansiva(_game._zombie, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieCono, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieBalde, CeldaZombie.fila, CeldaZombie.columna);
                    obj_explosion.Inst_Set_PositionZ(t_EscenarioBase.PASTO_POS_Z_INICIAL + (t_EscenarioBase.PASTO_RAZON * (CeldaZombie.columna)));
                    obj_explosion.Inst_Set_PositionY(Repetidor.pos.Y + 5);
                    _game.shader.time2 = 0;
                    obj_explosion.Inst_ShaderExplosion(true);
                    _game.shader.ExplosionLightPosition = new Microsoft.DirectX.Vector4(obj_explosion._instanciaActual.pos.X, obj_explosion._instanciaActual.pos.Y+20, obj_explosion._instanciaActual.pos.Z, 1);
                }
                CeldaZombie = colision(SuperGuisante, _game._zombieCono);
                if (CeldaZombie.fila >= 0)
                {
                    OndaExpansiva(_game._zombie, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieCono, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieBalde, CeldaZombie.fila, CeldaZombie.columna);
                    obj_explosion.Inst_Set_PositionZ(t_EscenarioBase.PASTO_POS_Z_INICIAL + (t_EscenarioBase.PASTO_RAZON * (CeldaZombie.columna)));
                    obj_explosion.Inst_Set_PositionY(Repetidor.pos.Y + 5);
                    _game.shader.time2 = 0;
                    obj_explosion.Inst_ShaderExplosion(true);
                    _game.shader.ExplosionLightPosition = new Microsoft.DirectX.Vector4(obj_explosion._instanciaActual.pos.X, obj_explosion._instanciaActual.pos.Y+20, obj_explosion._instanciaActual.pos.Z, 1);
                }
                CeldaZombie = colision(SuperGuisante, _game._zombieBalde);
                if (CeldaZombie.fila >= 0)
                {
                    OndaExpansiva(_game._zombie, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieCono, CeldaZombie.fila, CeldaZombie.columna);
                    OndaExpansiva(_game._zombieBalde, CeldaZombie.fila, CeldaZombie.columna);
                    obj_explosion.Inst_Set_PositionZ(t_EscenarioBase.PASTO_POS_Z_INICIAL + (t_EscenarioBase.PASTO_RAZON * (CeldaZombie.columna)));
                    obj_explosion.Inst_Set_PositionY(Repetidor.pos.Y + 5);
                    _game.shader.time2 = 0;
                    obj_explosion.Inst_ShaderExplosion(true);
                    _game.shader.ExplosionLightPosition = new Microsoft.DirectX.Vector4(obj_explosion._instanciaActual.pos.X, obj_explosion._instanciaActual.pos.Y+20, obj_explosion._instanciaActual.pos.Z, 1);
                }

                // Avance de SuperGuisantes
                for (int j = 0; j < SuperGuisante._instancias.Count; j++)
                {
                    SuperGuisante.Inst_Select(SuperGuisante._instancias[j]);
                    if (SuperGuisante._instanciaActual.pos.Z > 150F)
                    {
                        SuperGuisante._instanciaActual.pos.Z = 0F;
                        SuperGuisante._instanciaActual.pos.Y = -100F;
                    }
                    if (SuperGuisante._instanciaActual.pos.Y != -100F)
                    {
                        SuperGuisante._instanciaActual.pos.Z += _game.ElapsedTime * 50;
                    }
                }

                // Avance de guisantes comunes
                for (int i_guisante = 0; i_guisante < guisantes.Count; i_guisante++)
                {
                    if (guisantes[i_guisante].tiempo > 0)
                    {
                        guisante guisante = guisantes[i_guisante];
                        guisante.tiempo -= _game.ElapsedTime;
                        guisante.zActual = obj_guisante._instancias[i_guisante].pos.Z += _game.ElapsedTime * 50;
                        guisantes[i_guisante] = guisante;
                    }
                    else
                    {
                        guisante guisante = guisantes[i_guisante];
                        guisante.tiempo = TIEMPO_GUISANTE;
                        obj_guisante._instancias[i_guisante].pos.X = guisante.x;
                        obj_guisante._instancias[i_guisante].pos.Y = guisante.y;
                        guisante.zActual = obj_guisante._instancias[i_guisante].pos.Z = guisante.z;
                        guisantes[i_guisante] = guisante;
                    }
                }
            }

            private void OndaExpansiva(t_ZombieComun zombies, int FilaCenter, int ComulnaCenter)
            {
                for (int i = zombies._InstZombie.Count - 1; i >= 0; i--)
                {
                    t_ZombieComun.t_ZombieInstancia zombie = zombies._InstZombie[i];

                    if (zombie.fila >= FilaCenter-1 && zombie.fila <= FilaCenter + 1 &&
                       zombie.columna >= ComulnaCenter - 1 && zombie.columna <= ComulnaCenter + 1)
                    {
                        zombie.vida-= DANIO_SUPER;
                        zombies._InstZombie[i] = zombie;
                        if (zombie.vida <= 0)
                        {
                            zombies._Zombie.Inst_Delete(zombie.zombie);
                            zombies._InstZombie.Remove(zombie);
                        }
                    }
                }
            }

            private t_celda colision(t_Objeto3D peas, t_ZombieComun zombies)
            {
                t_celda ret = new t_celda();
                ret.fila = -1;
                ret.columna = -1;

                for (int i_guisante = 0; i_guisante < peas._instancias.Count; i_guisante++)
                {
                    peas.Inst_Select(peas._instancias[i_guisante]);

                    if (peas._instanciaActual.pos.Y != -100F)
                    {
                        for (int i = zombies._InstZombie.Count - 1; i >= 0; i--)
                        {
                            //El guisante esta en camino
                            t_ZombieComun.t_ZombieInstancia zombie = zombies._InstZombie[i];
                            if (fila == zombie.fila)
                            {
                                // Si estan en la misma fila, pueden chocar
                                if ((peas._instanciaActual.pos.Z > zombie.zombie.pos.Z - 1) && (peas._instanciaActual.pos.Z < zombie.zombie.pos.Z + 3))
                                {
                                    // Choca
                                    ret.fila = zombie.fila;
                                    ret.columna = zombie.columna;

                                    peas._instanciaActual.pos.Y = -100F;
                                    peas._instanciaActual.pos.Z = 0F;
                                    zombie.vida--;
                                    zombies._InstZombie[i] = zombie;
                                    if (zombie.vida <= 0)
                                    {
                                        zombies._Zombie.Inst_Delete(zombie.zombie);
                                        zombies._InstZombie.Remove(zombie);
                                    }
                                }
                            }
                        }
                    }
                }

                return ret;
            }

            public void super()
            {
                for (int j = 0; j < SuperGuisante._instancias.Count; j++)
                {
                    SuperGuisante.Inst_Select(SuperGuisante._instancias[j]);

                    SuperGuisante._instanciaActual.pos.X = x;
                    SuperGuisante._instanciaActual.pos.Y = y;
                    SuperGuisante._instanciaActual.pos.Z = z;
                }
            }

            public void render()
            {
                obj_guisante.Render(true);
                SuperGuisante.Render(true);
                obj_explosion.Render(true);
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_RepetidorInstancia> _InstRepetidor;
        public bool Is_Personal = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Repetidor(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA_PLANTA)
        {
            _game = game;

            _Planta.Set_Transform(0, 2.1F, 0,
                                            0.06F, 0.06F, 0.06F,
                                            0, GameModel.PI, 0);

            _InstRepetidor = new List<t_RepetidorInstancia>();
        }

        public static t_Repetidor Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Repetidor(game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public new void Update(bool ShowBoundingBoxWithKey)
        {
            int LanzaguisanteCreado = base.Update(ShowBoundingBoxWithKey);



            if (_game._camara.Modo_Is_CamaraPersonal())
            {
                if (LanzaguisanteCreado == 3)
                {
                    // Se seleccionó una planta para controlar

                    Is_Personal = true;
                }
            }
            else
            {
                Is_Personal = false;
            }

            if (Is_Personal && LanzaguisanteCreado == 4)
            {
                // Se activo la super
                int i;
                for (i=0; i<_Planta._instancias.Count; i++)
                {
                    if (_Planta._instancias[i] == _instPersonal)
                    {
                        break;
                    }
                }
                _InstRepetidor[i].super();
            }

            for (int i = 0; i < _InstRepetidor.Count; i++)
            {
                _InstRepetidor[i].update(ShowBoundingBoxWithKey);
            }

            if (LanzaguisanteCreado == 2)
            {
                // Lanzaguisante ubicado
                t_RepetidorInstancia Repetidor = new t_RepetidorInstancia(_game, _InstPlanta[_InstPlanta.Count - 1], _Planta._instancias[_Planta._instancias.Count - 1]);
                _InstRepetidor.Add(Repetidor);
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();

            for (int i = 0; i < _InstRepetidor.Count; i++)
            {
                _InstRepetidor[i].render();
            }
        }
    }
}
