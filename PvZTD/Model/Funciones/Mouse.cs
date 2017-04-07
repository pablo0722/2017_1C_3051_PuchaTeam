using TGC.Core.Example;
using TGC.Core.Input;


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
