using TGC.Core.Example;
using TGC.Core.Input;
using Microsoft.DirectX;


namespace TGC.Group.Model
{
    public class t_Mouse
    {
        /******************************************************************************************/
        /*                                  VARIABLES
        /******************************************************************************************/
        private TgcExample _example;










        /******************************************************************************************/
        /*                                  CONSTRUCTOR
        /******************************************************************************************/
        public t_Mouse(TgcExample example)
        {
            _example = example;
        }










        /******************************************************************************************/
        /*                              ESTADO DE POSICION
        /******************************************************************************************/
        public Vector2 Position()
        {
            return new Vector2(_example.Input.Xpos, _example.Input.Ypos);
        }

        public bool Is_Position(int x1, int x2, int y1, int y2)
        {
            float x = _example.Input.Xpos;
            float y = _example.Input.Ypos;

            if(x > x1 && x < x2 && y > y1 && y < y2)
            {
                return true;
            }

            return false;
        }










        /******************************************************************************************/
        /*                              ESTADOS DE BOTONES
        /******************************************************************************************/
        // BOTON IZQUIERDO
        public bool ClickIzq_Down()
        {
            return _example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT);
        }
        public bool ClickIzq_Up()
        {
            return !_example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_LEFT);
        }
        public bool ClickIzq_RisingDown()
        {
            return _example.Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT);
        }
        public bool ClickIzq_RisingUp()
        {
            return _example.Input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT);
        }

        // BOTON DERECHO
        public bool ClickDer_Down()
        {
            return _example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_RIGHT);
        }
        public bool ClickDer_Up()
        {
            return !_example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_RIGHT);
        }
        public bool ClickDer_RisingDown()
        {
            return _example.Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_RIGHT);
        }
        public bool ClickDer_RisingUp()
        {
            return _example.Input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_RIGHT);
        }

        // BOTON MEDIO (RUEDA)
        public bool ClickMid_Down()
        {
            return _example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_MIDDLE);
        }
        public bool ClickMid_Up()
        {
            return !_example.Input.buttonDown(TgcD3dInput.MouseButtons.BUTTON_MIDDLE);
        }
        public bool ClickMid_RisingDown()
        {
            return _example.Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_MIDDLE);
        }
        public bool ClickMid_RisingUp()
        {
            return _example.Input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_MIDDLE);
        }
    }
}
