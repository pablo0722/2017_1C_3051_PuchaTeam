namespace TGC.Group.Model
{
    public class t_Patatapum : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ =         "..\\..\\Media\\Objetos\\mina-TgcScene.xml";
        private const string PATH_TEXTURA_ON =  "..\\..\\Media\\Texturas\\HUD_Patatapum_sel.jpg";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\HUD_Patatapum.jpg";
        private const int PLANTA_VALOR = 125;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Patatapum(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR)
        {
            _Planta.Set_Transform(0, -5.9F, 0,
                                    0.15F, 0.15F, 0.15F,
                                    0, GameModel.PI, 0);

            _Planta.Mesh_Select(0);
            _Planta.Mesh_Color(64, 64, 64);

            _Planta.Mesh_Select(1);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(2);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(3);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(4);
            _Planta.Mesh_Color(95, 40, 0);

            _Planta.Mesh_Select(5);
            _Planta.Mesh_Color(255, 0, 0);

            _Planta.Mesh_Select(6);
            _Planta.Mesh_Color(60, 27, 0);
        }

        public static t_Patatapum Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Patatapum(game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public new void Update(bool ShowBoundingBoxWithKey)
        {
            base.Update(ShowBoundingBoxWithKey);
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
