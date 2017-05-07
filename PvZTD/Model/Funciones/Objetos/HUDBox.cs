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










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private static int s_n = 0; // Se usa como campo de bits.
                                    // cada bit es una posicion distinta del BoxHUD de cada planta.
                                    // Si un bit esta en '1', significa que esa posicion esta ocupada.
                                    // Si un bit esta en '0', significa que esa posicion esta libre.
        private static bool _ShowBoundingBox = false;
        private static bool _Is_AnyBoxPicked = false;

        // NO ESTATICAS
        private int _n;         // Numero (posicion) del BoxHUD
        private GameModel _game;
        private bool _Is_BoxPicked;
        private int _ValorPlanta;
        CustomBitmap BoxBitmapOn;
        CustomBitmap BoxBitmapOff;
        CustomSprite BoxSprite;
        int x, y, sx, sy;










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

            BoxBitmapOn = new CustomBitmap(PathTexturaOn, D3DDevice.Instance.Device);
            BoxBitmapOff = new CustomBitmap(PathTexturaOff, D3DDevice.Instance.Device);
            BoxSprite = new CustomSprite();
            BoxSprite.Bitmap = BoxBitmapOff;
            sx = (D3DDevice.Instance.Device.Viewport.Width/3)/8;
            sy = sx;
            x = sx * (n+1);
            y = sy/2;
            BoxSprite.SrcRect = new Rectangle(0, 0, BoxBitmapOff.Size.Width, BoxBitmapOff.Size.Height);
            BoxSprite.Scaling = new Vector2((float)sx/ BoxBitmapOff.Size.Width, (float)sy / BoxBitmapOff.Size.Height);
            BoxSprite.Position = new Vector2(x, y);
            BoxSprite.Rotation = 0;
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










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Devuelve verdadero si se acaba de clickear sobre el HUDBox
        public bool Update(bool ShowBoundingBoxWithKey, bool ChangeHUDTextureWhenMouseOver)
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

            bool ret = false;
            if (_game._camara.Modo_Is_CamaraAerea())
            {
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

                        ret = true;
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
            _game._spriteDrawer.BeginDrawSprite();
            _game._spriteDrawer.DrawSprite(BoxSprite);
            _game._spriteDrawer.EndDrawSprite();

            if (_game._camara.Modo_Is_CamaraAerea())
            {
                if (_game._soles >= _ValorPlanta)
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), x+sx/4, y+sy/4, System.Drawing.Color.Yellow);
                }
                else
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), x + sx / 4, y + sy / 4, System.Drawing.Color.Red);
                }
            }
        }
    }
}
