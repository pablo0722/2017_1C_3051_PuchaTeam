using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_Lanzaguisantes : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\guisante-TgcScene.xml";
        private const string PATH_SUPER_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\SuperGuisante-TgcScene.xml";
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\pea-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "..\\..\\Media\\Texturas\\HUD_Peashooter_sel.jpg";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_Peashooter.jpg";
        private const int PLANTA_VALOR = 100;
        private const float VIDA_PLANTA = 3;
        private const float TIEMPO_GUISANTE = 3;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_LanzaguisantesInstancia
        {
            public float x, y, z;
            public int fila;
            public float tiempo;
            public t_Objeto3D guisante { get; set; }
            public t_Objeto3D SuperGuisante { get; set; }
            public t_Objeto3D.t_instancia Lanzaguisante;
            public t_PlantaInstancia planta;
            public GameModel game;

            public t_LanzaguisantesInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Lanzaguisante)
            {
                this.game = game;

                this.Lanzaguisante = Lanzaguisante;

                tiempo = TIEMPO_GUISANTE;

                x = Lanzaguisante.pos.X;
                y = Lanzaguisante.pos.Y + 5;
                z = Lanzaguisante.pos.Z;

                this.planta = planta;

                fila = planta.fila;

                guisante = t_Objeto3D.Crear(game, PATH_GUISANTE_OBJ);
                guisante.Set_Size(0.04F, 0.04F, 0.04F);
                guisante.Inst_CreateAndSelect(x, y, z);

                // Guisantes para la super
                SuperGuisante = t_Objeto3D.Crear(game, PATH_SUPER_GUISANTE_OBJ);
                SuperGuisante.Set_Size(0.1F, 0.1F, 0.1F);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
                SuperGuisante.Inst_CreateAndSelect(x, -5F, z);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                guisante.Update(ShowBoundingBoxWithKey);
                SuperGuisante.Update(ShowBoundingBoxWithKey);

                colision(guisante, game._zombie);
                colision(guisante, game._zombieCono);
                colision(guisante, game._zombieBalde);

                colision(SuperGuisante, game._zombie);
                colision(SuperGuisante, game._zombieCono);
                colision(SuperGuisante, game._zombieBalde);


                for (int j = 0; j < SuperGuisante._instancias.Count; j++)
                {
                    SuperGuisante.Inst_Select(SuperGuisante._instancias[j]);
                    if (SuperGuisante._instanciaActual.pos.Z > 150F)
                    {
                        SuperGuisante._instanciaActual.pos.Z = 0F;
                        SuperGuisante._instanciaActual.pos.Y = -5F;
                    }
                    if (SuperGuisante._instanciaActual.pos.Y != -5F)
                    {
                        SuperGuisante._instanciaActual.pos.Z += game.ElapsedTime * 50;
                    }
                }

                if (tiempo > 0)
                {
                    tiempo -= game.ElapsedTime;

                    if (guisante._instanciaActual.pos.Y != -5F)
                    {
                        guisante._instanciaActual.pos.Z += game.ElapsedTime * 50;
                    }
                }
                else
                {
                    tiempo = TIEMPO_GUISANTE;
                    guisante._instanciaActual.pos.X = x;
                    guisante._instanciaActual.pos.Y = y;
                    guisante._instanciaActual.pos.Z = z;
                }
            }

            private void colision(t_Objeto3D peas, t_ZombieComun zombies)
            {
                for (int i = zombies._InstZombie.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < peas._instancias.Count; j++)
                    {
                        peas.Inst_Select(peas._instancias[j]);
                        if (peas._instanciaActual.pos.Y != -5F)
                        {
                            //El guisante esta en camino
                            t_ZombieComun.t_ZombieInstancia zombie = zombies._InstZombie[i];
                            if (fila == zombie.fila)
                            {
                                // Si estan en la misma fila, pueden chocar
                                if ((peas._instanciaActual.pos.Z > zombie.zombie.pos.Z - 1) && (peas._instanciaActual.pos.Z < zombie.zombie.pos.Z + 3))
                                {
                                    // Choca
                                    peas._instanciaActual.pos.Y = -5F;
                                    zombie.vida--;
                                    zombies._InstZombie[i] = zombie;
                                    if (zombie.vida <= 0)
                                    {
                                        zombies._Zombie.Inst_Delete(zombie.zombie);
                                        zombies._InstZombie.Remove(zombie);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
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
                guisante.Render(true);
                SuperGuisante.Render(true);
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_LanzaguisantesInstancia> _InstLanzaguisantes;
        public bool Is_Personal = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Lanzaguisantes(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA_PLANTA)
        {
            _game = game;

            _Planta.Set_Transform(0, 2.1F, 0,
                                            0.06F, 0.06F, 0.06F,
                                            0, GameModel.PI, 0);

            _InstLanzaguisantes = new List<t_LanzaguisantesInstancia>();
        }

        public static t_Lanzaguisantes Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Lanzaguisantes(game, n);
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
                _InstLanzaguisantes[_iPersonal].super();
            }

            for (int i=0; i<_InstLanzaguisantes.Count; i++)
            {
                _InstLanzaguisantes[i].update(ShowBoundingBoxWithKey);
            }

            if (LanzaguisanteCreado == 2)
            {
                // Lanzaguisante ubicado
                t_LanzaguisantesInstancia Lanzaguisantes = new t_LanzaguisantesInstancia(_game, _InstPlanta[_InstPlanta.Count - 1], _Planta._instancias[_Planta._instancias.Count - 1]);
                _InstLanzaguisantes.Add(Lanzaguisantes);
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();

            for (int i = 0; i < _InstLanzaguisantes.Count; i++)
            {
                _InstLanzaguisantes[i].render();
            }
        }
    }
}
