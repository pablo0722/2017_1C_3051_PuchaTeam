using Microsoft.DirectX;
using TGC.Core.Utils;
using System.Collections.Generic;


namespace TGC.Group.Model
{
    public class t_Planta
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_PlantaInstancia
        {
            public float vida;
            public int fila;
            public int columna;
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public t_Objeto3D _Planta;
        protected t_HUDBox _HUDBox;
        private GameModel _game;
        private Vector3 _Pos_PlantaActual;       // Posicion Planta Actual
        private int _ValorPlanta;
        public List<t_PlantaInstancia> _InstPlanta;
        private bool _CrearPlanta;
        private float _vida;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_Planta(string PathObj, string PathTexturaOn, string PathTexturaOff, GameModel game, byte n, int ValorPlanta, float vida)
        {
            _game = game;

            _HUDBox = t_HUDBox.Crear(PathTexturaOn, PathTexturaOff, game, n, ValorPlanta);

            _Planta = t_Objeto3D.Crear(_game, PathObj);

            _ValorPlanta = ValorPlanta;
            _CrearPlanta = false;

            _vida = vida;

            _InstPlanta = new List<t_PlantaInstancia>();
        }

        public static t_Planta Crear(string PathObj, string PathTexturaOn, string PathTexturaOff, GameModel game, byte n, int ValorPlanta, float vida)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Planta(PathObj, PathTexturaOn, PathTexturaOff, game, n, ValorPlanta, vida);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public int Update(bool ShowBoundingBoxWithKey)
        {
            int ret = 0;
            bool ChangeHUDTextureWhenMouseOver = false;

            if (_game._soles >= _ValorPlanta)
            {
                ChangeHUDTextureWhenMouseOver = true;
            }

            bool ClickSobreHUDBox = _HUDBox.Update(ShowBoundingBoxWithKey, ChangeHUDTextureWhenMouseOver);
            
            _Planta.Update(ShowBoundingBoxWithKey);


            if (_CrearPlanta && _game._mouse.ClickIzq_RisingDown() && _game._camara.Modo_Is_CamaraAerea() && !t_EscenarioBase.Is_PastoOcupado(t_EscenarioBase.MouseY, t_EscenarioBase.MouseX))
            {
                // Planta ubicada
                System.Windows.Forms.Cursor.Show();
                t_PlantaInstancia planta = new t_PlantaInstancia();
                planta.vida = _vida;
                planta.fila = t_EscenarioBase.MouseY;
                planta.columna = t_EscenarioBase.MouseX;
                _InstPlanta.Add(planta);
                
                _CrearPlanta = false;
                ret = 2;
            }

            if (_game._soles >= _ValorPlanta)
            {
                if (ClickSobreHUDBox)
                {
                    // Planta requiere ubicacion del usuario
                    System.Windows.Forms.Cursor.Hide();
                    _game._soles -= _ValorPlanta;
                    _Planta.Inst_CreateAndSelect();

                    _CrearPlanta = true;
                    ret = 1;
                }
            }
            if (_HUDBox.Is_BoxPicked())
            {
                _Pos_PlantaActual = new Vector3(
                                (t_EscenarioBase.MouseY) * t_EscenarioBase.PASTO_RAZON * t_EscenarioBase.PASTO_AJUSTE -
                                                FastMath.Abs(t_EscenarioBase.PASTO_POS_X_INICIAL),
                                0,
                                t_EscenarioBase.MouseX * t_EscenarioBase.PASTO_RAZON - FastMath.Abs(t_EscenarioBase.PASTO_POS_Z_INICIAL));

                _Planta.Inst_Set_PositionX(_Pos_PlantaActual.X);
                _Planta.Inst_Set_PositionZ(_Pos_PlantaActual.Z);
            }

            return ret;
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
