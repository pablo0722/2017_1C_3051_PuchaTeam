using System.Collections.Generic;


namespace TGC.Group.Model
{
    public class t_Girasol : t_Planta
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_SolInstancia
        {
            public float TiempoComienzo;   // Tiempo (del _game._TiempoTranscurrido) que se creo una instancia de girasol
            public int SolN;   // Numero de sol creado
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ =         "..\\..\\Media\\Objetos\\Girasol-TgcScene.xml";
        private const string PATH_TEXTURA_ON =  "..\\..\\Media\\Texturas\\HUD_Girasol_sel.jpg";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_Girasol.jpg";
        private const int PLANTA_VALOR = 50;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private GameModel _game;
        private List<t_SolInstancia> _InstSol;
        bool _CrearGirasol;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Girasol(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR)
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

            _InstSol = new List<t_SolInstancia>();
            _CrearGirasol = false;
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
            bool GirasolCreado = base.Update(ShowBoundingBoxWithKey);

            if(_CrearGirasol && _game._mouse.ClickIzq_RisingDown() && _game._camara.Modo_Is_CamaraAerea())
            {
                // Girasol ubicado
                t_SolInstancia sol = new t_SolInstancia();
                sol.SolN = 0;
                sol.TiempoComienzo = _game._TiempoTranscurrido;
                _InstSol.Add(sol);

                _CrearGirasol = false;
            }

            if (GirasolCreado)
            {
                // El usuario tiene que ubicar el girasol
                _CrearGirasol = true;
            }

            for(int i=0; i< _InstSol.Count; i++)
            {
                if ((_game._TiempoTranscurrido - _InstSol[i].TiempoComienzo) >= CantSegundosSegundosAEsperarParaCrearSol * (_InstSol[i].SolN + 1))
                {
                    _game._Sol.Do_CreateSol();
                    t_SolInstancia sol = _InstSol[i];
                    sol.SolN ++;
                    _InstSol[i] = sol;
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
