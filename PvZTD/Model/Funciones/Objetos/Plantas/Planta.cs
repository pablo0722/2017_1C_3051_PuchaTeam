using Microsoft.DirectX;
using TGC.Core.Utils;
using System.Collections.Generic;
using Microsoft.DirectX.DirectInput;


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
        // VARIABLES ESTATICAS
        static private bool _sCrearPlanta;

        // VARIABLES NO ESTATICAS
        public t_Objeto3D _Planta;
        public t_Objeto3D.t_instancia _instPersonal = null;
        public t_HUDBox _HUDBox;
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

            int ClickSobreHUDBox = _HUDBox.Update(ShowBoundingBoxWithKey, ChangeHUDTextureWhenMouseOver);
            
            _Planta.Update(ShowBoundingBoxWithKey);


            if (_game._camara.Modo_Is_CamaraAerea() && !_sCrearPlanta && _game._mouse.ClickIzq_RisingDown())
            {
                t_Objeto3D.t_instancia inst = _game._colision.MouseMesh(_Planta);
                if (inst != null)
                {
                    _instPersonal = inst;
                    _game._camara.Modo_Personal();
                    _game._camara.Aerea_Posicion(inst.pos.X, inst.pos.Y + 20, inst.pos.Z - 20);
                    _game._camara.Aerea_LookAt(inst.pos.X, inst.pos.Y, inst.pos.Z + 100);
                    _game._camara.Aerea_Up(0, 1, 0);

                    ret = 3;
                }
            }

            if ( (_game.Input.keyPressed(Key.Escape) || _game._mouse.ClickDer_RisingDown()) && _game._camara.Modo_Is_CamaraPersonal())
            {
                _Planta.Inst_ShaderGirar(false);
                _instPersonal = null;
                _game._camara.Modo_Aerea();
            }


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
                _sCrearPlanta = false;
                _Planta.Inst_ShaderGirar(false);
                ret = 2;
            }

            if (_CrearPlanta && _game._mouse.ClickDer_RisingDown() && _game._camara.Modo_Is_CamaraAerea())
            {
                // Planta requiere ubicacion del usuario
                System.Windows.Forms.Cursor.Show();
                _game._soles += _ValorPlanta;
                _Planta.Inst_Delete();

                _CrearPlanta = false;
                _sCrearPlanta = false;
            }

            if (_game._soles >= _ValorPlanta)
            {
                if (ClickSobreHUDBox == 1)
                {
                    // Planta requiere ubicacion del usuario
                    System.Windows.Forms.Cursor.Hide();
                    _game._soles -= _ValorPlanta;
                    _Planta.Inst_CreateAndSelect();

                    _CrearPlanta = true;
                    _sCrearPlanta = true;
                    _Planta.Inst_ShaderGirar(true);

                    ret = 1;
                }
            }
            if(ClickSobreHUDBox == 2)
            {
                ret = 4;
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
            _Planta.Render(true);
        }
    }
}
