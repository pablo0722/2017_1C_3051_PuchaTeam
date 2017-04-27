using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_Lanzaguisantes : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_GUISANTE_OBJ = "..\\..\\Media\\Objetos\\guisante-TgcScene.xml";
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
            public float x, y, z, zActual;
            public int fila;
            public float tiempo;
            public t_Objeto3D guisante { get; set; }
            public t_Objeto3D.t_instancia Lanzaguisante;
            public t_PlantaInstancia planta;
            public GameModel game;
            public bool choca;

            public t_LanzaguisantesInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Lanzaguisante)
            {
                this.game = game;

                this.Lanzaguisante = Lanzaguisante;

                tiempo = TIEMPO_GUISANTE;

                x = Lanzaguisante.pos.X;
                y = Lanzaguisante.pos.Y + 5;
                zActual = z = Lanzaguisante.pos.Z;

                this.planta = planta;

                fila = planta.fila;

                guisante = t_Objeto3D.Crear(game, PATH_GUISANTE_OBJ);
                guisante.Set_Size(0.04F, 0.04F, 0.04F);
                guisante.Inst_CreateAndSelect(x, y, z);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                guisante.Update(ShowBoundingBoxWithKey);

                choca = false;
                for(int i= game._zombie._InstZombie.Count-1; i>=0; i--)
                {
                    if (guisante._instanciaActual.pos.Y != -1.5F)
                    {
                        //El guisante esta en camino
                        t_ZombieComun.t_ZombieInstancia zombie = game._zombie._InstZombie[i];
                        if (fila == zombie.fila)
                        {
                            // Si estan en la misma fila, pueden chocar
                            if ((zActual > zombie.zombie.pos.Z - 1) && (zActual < zombie.zombie.pos.Z + 1))
                            {
                                // Choca
                                guisante._instanciaActual.pos.Y = -1.5F;
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
                }

                if (tiempo > 0)
                {
                    tiempo -= game.ElapsedTime;
                    zActual = guisante._instanciaActual.pos.Z += game.ElapsedTime * 50;
                }
                else
                {
                    tiempo = TIEMPO_GUISANTE;
                    zActual = guisante._instanciaActual.pos.X = x;
                    zActual = guisante._instanciaActual.pos.Y = y;
                    zActual = guisante._instanciaActual.pos.Z = z;
                }
            }

            public void render()
            {
                guisante.Render();
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_LanzaguisantesInstancia> _InstLanzaguisantes;










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

            for(int i=0; i<_InstLanzaguisantes.Count; i++)
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
