using Microsoft.DirectX;


namespace TGC.Group.Model
{
    public class t_Planta
    {
        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        protected t_Objeto3D _Planta;
        protected t_HUDBox _HUDBox;
        private GameModel _game;
        private Vector3 _Pos_PlantaActual;       // Posicion Planta Actual










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_Planta(string PathObj, string PathTexturaOn, string PathTexturaOff, GameModel game, byte n)
        {
            _game = game;

            _HUDBox = t_HUDBox.Crear(PathTexturaOn, PathTexturaOff, game, n);

            _Planta = t_Objeto3D.Crear(_game, PathObj);
        }

        public static t_Planta Crear(string PathObj, string PathTexturaOn, string PathTexturaOff, GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Planta(PathObj, PathTexturaOn, PathTexturaOff, game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, bool ChangeHUDTextureWhenMouseOver, bool CrearPlantaWhenClickOverHUDBox)
        {
            bool ClickSobreHUDBox = _HUDBox.Update(ShowBoundingBoxWithKey, ChangeHUDTextureWhenMouseOver);
            
            _Planta.Update(ShowBoundingBoxWithKey);

            _Pos_PlantaActual = new Vector3(_game.Input.Ypos / GameModel.P_HEIGHT * 110 - 40, 0, _game.Input.Xpos / GameModel.P_WIDTH * 150 - 75);

            if (CrearPlantaWhenClickOverHUDBox)
            {
                if (ClickSobreHUDBox)
                {
                    _Planta.Inst_CreateAndSelect();
                }
                if (_HUDBox.Is_BoxPicked())
                {
                    _Planta.Inst_Set_PositionX(_Pos_PlantaActual.X);
                    _Planta.Inst_Set_PositionZ(_Pos_PlantaActual.Z);
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _HUDBox.Render();
            _Planta.Render();
        }
    }
}
