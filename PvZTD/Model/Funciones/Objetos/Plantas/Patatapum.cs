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










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Patatapum(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n)
        {
            _Planta.Set_Transform(0, -5.9F, 0,
                                    0.15F, 0.15F, 0.15F,
                                    0, GameModel.PI, 0);
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
        public new void Update(bool ShowBoundingBoxWithKey, bool ChangeHUDTextureWhenMouseOver, bool CrearPlantaWhenClickOverHUDBox)
        {
            base.Update(ShowBoundingBoxWithKey, ChangeHUDTextureWhenMouseOver, CrearPlantaWhenClickOverHUDBox);
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
