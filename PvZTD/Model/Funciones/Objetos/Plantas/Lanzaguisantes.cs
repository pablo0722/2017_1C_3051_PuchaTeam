using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_Lanzaguisantes : t_Planta
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_LanzaguisantesInstancia
        {
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string    PATH_OBJ =          "..\\..\\Media\\Objetos\\Pea-TgcScene.xml";
        private const string    PATH_TEXTURA_ON =   "..\\..\\Media\\Texturas\\HUD_Peashooter_sel.jpg";
        private const string    PATH_TEXTURA_OFF =  "..\\..\\Media\\Texturas\\HUD_Peashooter.jpg";
        private const int       PLANTA_VALOR =      100;
        private const float     VIDA_PLANTA =       3;










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
            int PatatapumCreado = base.Update(ShowBoundingBoxWithKey);

            if (PatatapumCreado == 2)
            {
                // Girasol ubicado
                t_LanzaguisantesInstancia Lanzaguisantes = new t_LanzaguisantesInstancia();
                _InstLanzaguisantes.Add(Lanzaguisantes);
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
