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
        private const float TIEMPO_GUISANTE = 2;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_LanzaguisantesInstancia
        {
            public float x, y, z;
            public float tiempo;
            public t_Objeto3D guisante { get; set; }
            public t_Objeto3D.t_instancia Lanzaguisante;
            public GameModel game;

            public t_LanzaguisantesInstancia(GameModel game, t_Objeto3D.t_instancia Lanzaguisante)
            {
                this.game = game;

                this.Lanzaguisante = Lanzaguisante;

                tiempo = TIEMPO_GUISANTE;

                x = Lanzaguisante.pos.X;
                y = Lanzaguisante.pos.Y + 5;
                z = Lanzaguisante.pos.Z;

                guisante = t_Objeto3D.Crear(game, PATH_GUISANTE_OBJ);
                guisante.Set_Size(0.04F, 0.04F, 0.04F);
                guisante.Inst_CreateAndSelect(x, y, z);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                guisante.Update(ShowBoundingBoxWithKey);

                if (tiempo > 0)
                {
                    tiempo -= game.ElapsedTime;
                    guisante._instancias[0].pos.Z += game.ElapsedTime * 50;
                }
                else
                {
                    tiempo = TIEMPO_GUISANTE;
                    guisante._instancias[0].pos.Z = z;
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
                t_LanzaguisantesInstancia Lanzaguisantes = new t_LanzaguisantesInstancia(_game, this._Planta._instancias[_Planta._instancias.Count - 1]);
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
