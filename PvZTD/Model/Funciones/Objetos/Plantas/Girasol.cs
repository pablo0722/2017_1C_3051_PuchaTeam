using System.Collections.Generic;


namespace TGC.Group.Model
{
    public class t_Girasol : t_Planta
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_GirasolInstancia
        {
            public float TiempoComienzo;   // Tiempo (del _game._TiempoTranscurrido) que se creo una instancia de girasol
            public int SolN;   // Numero de sol creado
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string    PATH_OBJ =          "..\\..\\Media\\Objetos\\Girasol-TgcScene.xml";
        private const string    PATH_TEXTURA_ON =   "..\\..\\Media\\Texturas\\HUD_Girasol_sel.jpg";
        private const string    PATH_TEXTURA_OFF =  "..\\..\\Media\\Texturas\\HUD_Girasol.jpg";
        private const int       PLANTA_VALOR =      50;
        private const float     VIDA =              3;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_GirasolInstancia> _InstGirasol;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Girasol(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA)
        {
            _game = game;

            _Planta.Set_Transform(  0, 0, 0,
                                    0.05F, 0.05F, 0.05F,
                                    0, GameModel.PI, 0);

            _Planta.Mesh_Select(0);
            _Planta.Mesh_Color(153, 228, 153);

            _Planta.Mesh_Select(1);
            _Planta.Mesh_Color(27, 177, 27);

            _Planta.Mesh_Select(2);
            _Planta.Mesh_Color(8, 8, 136);

            _Planta.Mesh_Select(3);
            _Planta.Mesh_Color(255, 217, 7);

            _Planta.Mesh_Select(4);
            _Planta.Mesh_Color(27, 177, 27);

            _Planta.Mesh_Select(5);
            _Planta.Mesh_Color(255, 217, 7);

            _InstGirasol = new List<t_GirasolInstancia>();
        }

        public static t_Girasol Crear(GameModel game, byte n)
        {
            if(t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Girasol(game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, int CantSegundosSegundosAEsperarParaCrearSol)
        {
            int GirasolCreado = base.Update(ShowBoundingBoxWithKey);

            if(GirasolCreado == 2)
            {
                // Girasol ubicado
                t_GirasolInstancia Girasol = new t_GirasolInstancia();
                Girasol.SolN = 0;
                Girasol.TiempoComienzo = _game._TiempoTranscurrido;
                _InstGirasol.Add(Girasol);
            }

            for(int i=0; i< _InstGirasol.Count; i++)
            {
                if ((_game._TiempoTranscurrido - _InstGirasol[i].TiempoComienzo) >= CantSegundosSegundosAEsperarParaCrearSol * (_InstGirasol[i].SolN + 1))
                {
                    _game._Sol.Do_CreateSol();
                    t_GirasolInstancia sol = _InstGirasol[i];
                    sol.SolN ++;
                    _InstGirasol[i] = sol;
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();
        }
    }
}
