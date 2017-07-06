using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    public class t_HUDBox
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_TEXTURA_SUPER_ON = "..\\..\\Media\\Texturas\\SuperHojaOn.png";
        private const string PATH_TEXTURA_SUPER_OVER = "..\\..\\Media\\Texturas\\SuperHojaOver.png";
        private const string PATH_TEXTURA_SUPER_OFF = "..\\..\\Media\\Texturas\\SuperHojaOff.png";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private static int s_n = 0; // Se usa como campo de bits.
                                    // cada bit es una posicion distinta del BoxHUD de cada planta.
                                    // Si un bit esta en '1', significa que esa posicion esta ocupada.
                                    // Si un bit esta en '0', significa que esa posicion esta libre.
        public static bool _ShowBoundingBox = false;
        public static bool _Is_AnyBoxPicked = false;

        // NO ESTATICAS
        private int _n;         // Numero (posicion) del BoxHUD
        private GameModel _game;
        public bool _Is_BoxPicked;
        private int _ValorPlanta;
        public bool _super;
        CustomBitmap BoxBitmapOn;
        CustomBitmap BoxBitmapOff;
        CustomSprite BoxSprite;
        CustomBitmap SuperBitmapOn;
        CustomBitmap SuperBitmapOver;
        CustomBitmap SuperBitmapOff;
        CustomSprite SuperSprite;
        int x, y, sx, sy;
        int super_x, super_y, super_sx, super_sy;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_HUDBox(string PathTexturaOn, string PathTexturaOff, GameModel game, byte n, int ValorPlanta)
        {
            s_n = s_n | (1 << n);   // Reserva lugar en el campo de bits
            _n = n;

            _game = game;

            _Is_BoxPicked = false;

            _ValorPlanta = ValorPlanta;

            _super = false;

            BoxBitmapOn = new CustomBitmap(PathTexturaOn, D3DDevice.Instance.Device);
            BoxBitmapOff = new CustomBitmap(PathTexturaOff, D3DDevice.Instance.Device);
            BoxSprite = new CustomSprite();
            BoxSprite.Bitmap = BoxBitmapOff;
            sx = (GameModel._ResolucionPantalla.Width/3)/8;
            sy = sx;
            x = sx * (n+1);
            y = sy/2;
            BoxSprite.SrcRect = new Rectangle(0, 0, BoxBitmapOff.Size.Width, BoxBitmapOff.Size.Height);
            BoxSprite.Scaling = new Vector2((float)sx/ BoxBitmapOff.Size.Width, (float)sy / BoxBitmapOff.Size.Height);
            BoxSprite.Position = new Vector2(x, y);
            BoxSprite.Rotation = 0;

            SuperBitmapOn = new CustomBitmap(PATH_TEXTURA_SUPER_ON, D3DDevice.Instance.Device);
            SuperBitmapOver = new CustomBitmap(PATH_TEXTURA_SUPER_OVER, D3DDevice.Instance.Device);
            SuperBitmapOff = new CustomBitmap(PATH_TEXTURA_SUPER_OFF, D3DDevice.Instance.Device);
            SuperSprite = new CustomSprite();
            SuperSprite.Bitmap = SuperBitmapOff;
            super_sx = (GameModel._ResolucionPantalla.Width / 2) / 8;
            super_sy = super_sx;
            super_x = GameModel._ResolucionPantalla.Width - (int)(super_sx * 1.5F);
            super_y = super_sy;
            SuperSprite.SrcRect = new Rectangle(0, 0, SuperBitmapOff.Size.Width, SuperBitmapOff.Size.Height);
            SuperSprite.Scaling = new Vector2((float)super_sx / SuperBitmapOff.Size.Width, (float)super_sy / SuperBitmapOff.Size.Height);
            SuperSprite.Position = new Vector2(super_x, super_y);
            SuperSprite.Rotation = 0;
        }

        public static t_HUDBox Crear(string PathTexturaOn, string PathTexturaOff, GameModel game, byte n, int ValorPlanta)
        {
            if((PathTexturaOn != null) && (PathTexturaOff != null) && (game != null) && Is_Libre(n))
            {
                return new t_HUDBox(PathTexturaOn, PathTexturaOff, game, n, ValorPlanta);
            }

            return null;
        }
        public static bool Is_Libre(byte n)
        {
            return ((s_n & (1 << n)) == 0); // Verifica lugar en el campo de bits)
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~t_HUDBox()
        {
            s_n = s_n & (~(1 << _n));   // Se borra del campo de bits
            BoxBitmapOn.D3dTexture.Dispose();
            BoxBitmapOff.D3dTexture.Dispose();
            SuperBitmapOn.D3dTexture.Dispose();
            SuperBitmapOff.D3dTexture.Dispose();
        }










        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/
        public bool Is_MouseOver()
        {
            if(_game._mouse.Is_Position(x, x+sx, y, y+sy))
            {
                return true;
            }

            return false;
        }

        public bool Is_MouseSuperOver()
        {
            if (_game._mouse.Is_Position(super_x, super_x + super_sx, super_y, super_y + super_sy))
            {
                return true;
            }

            return false;
        }










        /******************************************************************************************/
        /*                                      PICKED
        /******************************************************************************************/
        public static bool Is_AnyBoxPicked()
        {
            return _Is_AnyBoxPicked;
        }

        public bool Is_BoxPicked()
        {
            return _Is_BoxPicked;
        }










        /******************************************************************************************/
        /*                                      TEXTURAS
        /******************************************************************************************/
        public void Set_Textura_On()
        {
            BoxSprite.Bitmap = BoxBitmapOn;
        }

        public void Set_Textura_Off()
        {
            BoxSprite.Bitmap = BoxBitmapOff;
        }

        public void Set_Textura_Super_On()
        {
            SuperSprite.Bitmap = SuperBitmapOn;
        }

        public void Set_Textura_Super_Over()
        {
            SuperSprite.Bitmap = SuperBitmapOver;
        }

        public void Set_Textura_Super_Off()
        {
            SuperSprite.Bitmap = SuperBitmapOff;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Devuelve verdadero si se acaba de clickear sobre el HUDBox
        public int Update(bool ShowBoundingBoxWithKey, bool ChangeHUDTextureWhenMouseOver)
        { 
            sx = (D3DDevice.Instance.Device.DisplayMode.Width / 3) / 8;
            sy = sx;
            x = sx * (_n + 1);
            y = sy / 2;

            _ShowBoundingBox = false;

            if (ShowBoundingBoxWithKey)
            {
                if (_game.Input.keyDown(Key.F))
                {
                    _ShowBoundingBox = true;
                }
            }

            int ret = 0;
            if (_game._camara.Modo_Is_CamaraAerea())
            {
                if (_game._mouse.ClickDer_RisingDown())
                {
                    _Is_AnyBoxPicked = false;
                    _Is_BoxPicked = false;
                }
                if (_game._mouse.ClickIzq_RisingDown())
                {
                    if (_Is_BoxPicked)
                    {
                        if (!t_EscenarioBase.Is_PastoOcupado(t_EscenarioBase.MouseY, t_EscenarioBase.MouseX))
                        {
                            _Is_AnyBoxPicked = false;
                            _Is_BoxPicked = false;
                        }
                    }
                    else if (Is_MouseOver() && !_Is_AnyBoxPicked && ChangeHUDTextureWhenMouseOver)
                    {
                        _Is_AnyBoxPicked = true;
                        _Is_BoxPicked = true;

                        ret = 1;
                    }
                }
                else if (_game._mouse.ClickIzq_Up())
                {
                    if (!_Is_AnyBoxPicked)
                    {
                        Set_Textura_Off();

                        if (ChangeHUDTextureWhenMouseOver)
                        {
                            if (Is_MouseOver())
                            {
                                Set_Textura_On();
                            }
                        }
                    }
                }
            }
            else if (_game._camara.Modo_Is_CamaraPersonal())
            {
                if(_game._Super.Is_Finished())
                {
                    _super = true;
                }

                if (_super)
                {

                    if (_game._mouse.ClickIzq_RisingDown() && Is_MouseSuperOver())
                    {
                        _super = false;
                        _game._Super.FinishReset();

                        ret = 2;
                    }

                    if (Is_MouseSuperOver())
                    {
                        Set_Textura_Super_Over();
                    }
                    else
                    {
                        Set_Textura_Super_On();
                    }
                }
                else
                {
                    Set_Textura_Super_Off();
                }
            }

            return ret;
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        private void Func_BoxRender(TgcBox box)
        {
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            box.Transform = Matrix.Scaling(box.Scale) *
                            Matrix.RotationYawPitchRoll(box.Rotation.Y, box.Rotation.X, box.Rotation.Z) *
                            Matrix.Translation(box.Position);

            box.render();

            if(_ShowBoundingBox)
            {
                box.BoundingBox.render();
            }
        }

        // Renderiza todos los objetos relativos a la clase
        public void Render()
        {
            if (_game._camara.Modo_Is_CamaraAerea())
            {
                _game._spriteDrawer.BeginDrawSprite();
                _game._spriteDrawer.DrawSprite(BoxSprite);
                _game._spriteDrawer.EndDrawSprite();

                if (_game._soles >= _ValorPlanta)
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), x+sx/4, y+sy/4, System.Drawing.Color.Yellow);
                }
                else
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), x + sx / 4, y + sy / 4, System.Drawing.Color.Red);
                }
            }
            else if(_game._camara.Modo_Is_CamaraPersonal())
            {
                _game._spriteDrawer.BeginDrawSprite();
                _game._spriteDrawer.DrawSprite(SuperSprite);
                _game._spriteDrawer.EndDrawSprite();
            }
        }
    }
}
