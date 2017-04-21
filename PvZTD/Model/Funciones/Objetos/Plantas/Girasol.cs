namespace TGC.Group.Model
{
    public class t_Girasol : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ =         "..\\..\\Media\\Objetos\\Girasol-TgcScene.xml";
        private const string PATH_TEXTURA_ON =  "..\\..\\Media\\Texturas\\HUD_Girasol_sel.jpg";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_Girasol.jpg";
        private const int PLANTA_VALOR = -1000000;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Girasol(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n)
        {
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
        public void Update(bool ShowBoundingBoxWithKey)
        {
            base.Update(ShowBoundingBoxWithKey, PLANTA_VALOR);
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
