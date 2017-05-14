using System.Collections.Generic;



namespace TGC.Group.Model.Funciones.Objetos.Plantas
{
    public class t_Repetidor : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\guisante-TgcScene.xml";
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\repetidora-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "..\\..\\Media\\Texturas\\HUD_repeater_sel.png";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_repeater.png";
        private const int PLANTA_VALOR = 200;
        private const float VIDA_PLANTA = 3;
        private const float TIEMPO_GUISANTE = 4;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_RepetidorInstancia
        {
            public struct guisante
            {
                public float x, y, z, zActual;
                public float tiempo;
                public t_Objeto3D.t_instancia GuisanteInst;
            };

            public int fila;
            public t_Objeto3D obj_guisante { get; set; }
            public t_Objeto3D.t_instancia Repetidor;
            public t_PlantaInstancia planta;
            public GameModel game;
            public List<guisante> guisantes;

            public t_RepetidorInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Repetidor)
            {
                this.game = game;

                this.Repetidor = Repetidor;

                guisantes = new List<guisante>();

                float x = Repetidor.pos.X;
                float y = Repetidor.pos.Y + 5;
                float z = Repetidor.pos.Z;

                this.planta = planta;

                fila = planta.fila;

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
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                obj_guisante.Update(ShowBoundingBoxWithKey);

                for (int i_guisante = 0; i_guisante < guisantes.Count; i_guisante++)
                {
                    if (obj_guisante._instancias[i_guisante].pos.Y != -1.5F)
                    {
                        for (int i = game._zombie._InstZombie.Count - 1; i >= 0; i--)
                        {
                            //El guisante esta en camino
                            t_ZombieComun.t_ZombieInstancia zombie = game._zombie._InstZombie[i];
                            if (fila == zombie.fila)
                            {
                                // Si estan en la misma fila, pueden chocar
                                if ((guisantes[i_guisante].zActual > zombie.zombie.pos.Z - 1) && (guisantes[i_guisante].zActual < zombie.zombie.pos.Z + 1))
                                {
                                    // Choca
                                    obj_guisante._instancias[i_guisante].pos.Y = -1.5F;
                                    zombie.vida--;
                                    game._zombie._InstZombie[i] = zombie;
                                    if (zombie.vida <= 0)
                                    {
                                        game._zombie._Zombie.Inst_Delete(zombie.zombie);
                                        game._zombie._InstZombie.Remove(zombie);
                                    }
                                    break;
                                }
                            }
                        }

                        if (obj_guisante._instancias[i_guisante].pos.Y != -1.5F)
                        {
                            for (int i = game._zombieCono._InstZombie.Count - 1; i >= 0; i--)
                            {
                                //El guisante esta en camino
                                t_ZombieComun.t_ZombieInstancia zombie = game._zombieCono._InstZombie[i];
                                if (fila == zombie.fila)
                                {
                                    // Si estan en la misma fila, pueden chocar
                                    if ((guisantes[i_guisante].zActual > zombie.zombie.pos.Z - 1) && (guisantes[i_guisante].zActual < zombie.zombie.pos.Z + 1))
                                    {
                                        // Choca
                                        obj_guisante._instancias[i_guisante].pos.Y = -1.5F;
                                        zombie.vida--;
                                        game._zombieCono._InstZombie[i] = zombie;
                                        if (zombie.vida <= 0)
                                        {
                                            game._zombieCono._Zombie.Inst_Delete(zombie.zombie);
                                            game._zombieCono._InstZombie.Remove(zombie);
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        if (obj_guisante._instancias[i_guisante].pos.Y != -1.5F)
                        {
                            for (int i = game._zombieBalde._InstZombie.Count - 1; i >= 0; i--)
                            {
                                //El guisante esta en camino
                                t_ZombieComun.t_ZombieInstancia zombie = game._zombieBalde._InstZombie[i];
                                if (fila == zombie.fila)
                                {
                                    // Si estan en la misma fila, pueden chocar
                                    if ((guisantes[i_guisante].zActual > zombie.zombie.pos.Z - 1) && (guisantes[i_guisante].zActual < zombie.zombie.pos.Z + 1))
                                    {
                                        // Choca
                                        obj_guisante._instancias[i_guisante].pos.Y = -1.5F;
                                        zombie.vida--;
                                        game._zombieBalde._InstZombie[i] = zombie;
                                        if (zombie.vida <= 0)
                                        {
                                            game._zombieBalde._Zombie.Inst_Delete(zombie.zombie);
                                            game._zombieBalde._InstZombie.Remove(zombie);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (guisantes[i_guisante].tiempo > 0)
                    {
                        guisante guisante = guisantes[i_guisante];
                        guisante.tiempo -= game.ElapsedTime;
                        guisante.zActual = obj_guisante._instancias[i_guisante].pos.Z += game.ElapsedTime * 50;
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

            private void colision(t_Objeto3D peas, t_ZombieComun zombies)
            {
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
                _InstRepetidor[_iPersonal].super();
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
