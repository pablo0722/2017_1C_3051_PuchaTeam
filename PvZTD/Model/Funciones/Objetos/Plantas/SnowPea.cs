using Microsoft.DirectX;
using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_SnowPea : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\guisanteCongelado-TgcScene.xml";
        private const string PATH_SUPER_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\SuperGuisante-TgcScene.xml";
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\snowPea-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "..\\..\\Media\\Texturas\\SnowPea_hudSelect.png";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\SnowPea_hud.png";
        private const int PLANTA_VALOR = 150;
        private const float VIDA_PLANTA = 3;
        private const float TIEMPO_GUISANTE = 3;
        private const float TIME_ENFRIAR = 2.5F;
        private const float TIME_CONGELAR = 10F;
        public const float VEL_ENFRIAR = -0.7F;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_SnowPeaInstancia
        {
            public float x, y, z;
            public int fila;
            public float tiempo;
            public t_Objeto3D guisante { get; set; }
            public t_Objeto3D SuperGuisante { get; set; }
            public t_Objeto3D.t_instancia Lanzaguisante;
            public t_PlantaInstancia planta;
            public GameModel game;
            public bool flagSuper = false;

            public t_SnowPeaInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Lanzaguisante)
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
                guisante.Set_Size(0.05F, 0.05F, 0.05F);
                guisante.Inst_CreateAndSelect(x, y, z);

                // Guisantes para la super
                SuperGuisante = t_Objeto3D.Crear(game, PATH_SUPER_GUISANTE_OBJ);
                SuperGuisante.Set_Size(0.1F, 0.1F, 0.1F);
                SuperGuisante.Inst_CreateAndSelect(x, -100F, z);

                SuperGuisante.Inst_ShaderBolaDeHielo(true);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                guisante.Update(ShowBoundingBoxWithKey);
                SuperGuisante.Update(ShowBoundingBoxWithKey);

                flagSuper = false;
                colision(guisante, game._zombie);
                colision(guisante, game._zombieCono);
                colision(guisante, game._zombieBalde);
                
                flagSuper = true;
                colision(SuperGuisante, game._zombie);
                colision(SuperGuisante, game._zombieCono);
                colision(SuperGuisante, game._zombieBalde);


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
                        SuperGuisante._instanciaActual.pos.Z += game.ElapsedTime * 50;
                    }
                }

                if (tiempo > 0)
                {
                    tiempo -= game.ElapsedTime;

                    if (guisante._instanciaActual.pos.Y != -100F)
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
                        if (peas._instanciaActual.pos.Y != -100F)
                        {
                            //El guisante esta en camino
                            t_ZombieComun.t_ZombieInstancia zombie = zombies._InstZombie[i];
                            if (fila == zombie.fila)
                            {
                                // Si estan en la misma fila, pueden chocar
                                if ((peas._instanciaActual.pos.Z > zombie.zombie.pos.Z - 1) && (peas._instanciaActual.pos.Z < zombie.zombie.pos.Z + 3))
                                {
                                    // Choca
                                    if(flagSuper)
                                    {
                                        t_ZombieComun zombiesk = null;
                                        for (int l = 0; l < 3; l++)
                                        {
                                            if(l==0)
                                            {
                                                zombiesk = game._zombie;
                                            }
                                            else if (l == 1)
                                            {
                                                zombiesk = game._zombieCono;
                                            }
                                            else if (l == 2)
                                            {
                                                zombiesk = game._zombieBalde;
                                            }

                                            for (int k = zombiesk._InstZombie.Count - 1; k >= 0; k--)
                                            {
                                                t_ZombieComun.t_ZombieInstancia zombiek = zombiesk._InstZombie[k];

                                                zombiek.timeCongelar = TIME_CONGELAR;
                                                peas._instanciaActual.pos.Y = -100F;
                                                peas._instanciaActual.pos.Z = 0F;
                                                zombiesk._InstZombie[k] = zombiek;
                                                zombiesk._Zombie.Inst_Select(zombiesk._InstZombie[k].zombie);
                                                zombiesk._Zombie.Inst_ShaderZombieCongelado(true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        zombie.vida--;
                                        zombie.timeEnfriar = TIME_ENFRIAR;
                                        peas._instanciaActual.pos.Y = -100F;
                                        peas._instanciaActual.pos.Z = 0F;
                                        zombies._InstZombie[i] = zombie;
                                        zombies._Zombie.Inst_Select(zombies._InstZombie[i].zombie);
                                        zombies._Zombie.Inst_ShaderZombieEnfriar(true);
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
        public List<t_SnowPeaInstancia> _InstLanzaguisantes;
        public bool Is_Personal = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_SnowPea(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA_PLANTA)
        {
            _game = game;

            _Planta.Set_Transform(0, 2.1F, 0,
                                            0.1F, 0.1F, 0.1F,
                                            0, GameModel.PI, 0);

            _InstLanzaguisantes = new List<t_SnowPeaInstancia>();
        }

        public static t_SnowPea Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_SnowPea(game, n);
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
                for (i = 0; i < _Planta._instancias.Count; i++)
                {
                    if (_Planta._instancias[i] == _instPersonal)
                    {
                        break;
                    }
                }
                _InstLanzaguisantes[i].super();
            }

            for (int i=0; i<_InstLanzaguisantes.Count; i++)
            {
                _InstLanzaguisantes[i].update(ShowBoundingBoxWithKey);
            }

            if (LanzaguisanteCreado == 2)
            {
                // Lanzaguisante ubicado
                t_SnowPeaInstancia Lanzaguisantes = new t_SnowPeaInstancia(_game, _InstPlanta[_InstPlanta.Count - 1], _Planta._instancias[_Planta._instancias.Count - 1]);
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
