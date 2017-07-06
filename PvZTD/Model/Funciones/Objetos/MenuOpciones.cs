using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Text;

namespace TGC.Group.Model.Funciones.Objetos
{
    class MenuOpciones
    { 
    //public CustomBitmap mi_Bitmap = null; // es donde se carga la imagen
    //public CustomSprite mi_sprite = null; // Es como una instancia de la imagen. es para cargar la imagen una sola vez y poder dibujarla varias veces.
    private GameModel _game;

    public CustomBitmap MenuOpcionesSinSombra;
    public CustomBitmap MenuOpcionesConSombra;


    public CustomSprite BoxSprite;
    public int x, y, sx, sy;
    public const int POSICION_MENU = 0;

    public const string PathTexturaMenuOpcionesSinSombra = "..\\..\\Media\\Texturas\\MenuOpcionesSinSombra.png";
    public const string PathTexturaMenuOpcionesConSombra = "..\\..\\Media\\Texturas\\MenuOpcionesConSombra.png";

    public static bool SalirOpciones = false;

    public struct Coordenadas
    {
        public int InicialX;
        public int InicialY;
        public int FinalX;
        public int FinalY;
    }
        public Coordenadas CoordenadasOpcionesAtras;
        public Coordenadas CoordenadasOpcionesSombra;
        public Coordenadas CoordenadasOpcionesMusicaUP;
        public Coordenadas CoordenadasOpcionesMusicaDOWN;
        public Coordenadas CoordenadasOpcionesEfectoUP;
        public Coordenadas CoordenadasOpcionesEfectoDOWN;

        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private MenuOpciones(GameModel game)
    {
        _game = game;
            

        CoordenadasOpcionesAtras.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.03F);
        CoordenadasOpcionesAtras.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.41F);
        CoordenadasOpcionesAtras.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.80F);
        CoordenadasOpcionesAtras.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.95F);

        CoordenadasOpcionesSombra.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.19F);
        CoordenadasOpcionesSombra.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.25F);
        CoordenadasOpcionesSombra.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.40F);
        CoordenadasOpcionesSombra.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.50F);

        CoordenadasOpcionesMusicaUP.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.20F);
        CoordenadasOpcionesMusicaUP.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.24F);
        CoordenadasOpcionesMusicaUP.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.23F);
        CoordenadasOpcionesMusicaUP.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.275F);

        CoordenadasOpcionesMusicaDOWN.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.20F);
        CoordenadasOpcionesMusicaDOWN.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.24F);
        CoordenadasOpcionesMusicaDOWN.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28F);
        CoordenadasOpcionesMusicaDOWN.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.325F);

        CoordenadasOpcionesEfectoUP.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.48F);
        CoordenadasOpcionesEfectoUP.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.52F);
        CoordenadasOpcionesEfectoUP.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.23F);
        CoordenadasOpcionesEfectoUP.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.275F);

        MenuOpcionesConSombra = new CustomBitmap(PathTexturaMenuOpcionesConSombra, D3DDevice.Instance.Device);
        MenuOpcionesSinSombra = new CustomBitmap(PathTexturaMenuOpcionesSinSombra, D3DDevice.Instance.Device);

        BoxSprite = new CustomSprite();

        BoxSprite.Bitmap = MenuOpcionesConSombra;
        sx = D3DDevice.Instance.Device.Viewport.Width;
        sy = D3DDevice.Instance.Device.Viewport.Height;
        x = 0;
        y = 0;
        BoxSprite.SrcRect = new Rectangle(0, 0, MenuOpcionesConSombra.Size.Width, MenuOpcionesConSombra.Size.Height);
        BoxSprite.Scaling = new Vector2((float)sx / MenuOpcionesConSombra.Size.Width, (float)sy / MenuOpcionesConSombra.Size.Height);
        BoxSprite.Position = new Vector2(x, y);
        BoxSprite.Rotation = 0;

        }

    public static MenuOpciones Crear(GameModel game)
    {
        return new MenuOpciones(game);
    }

    /******************************************************************************************/
    /*                                      DESTRUCTOR
    /******************************************************************************************/
    ~MenuOpciones()
    {
            MenuOpcionesSinSombra.D3dTexture.Dispose();
            MenuOpcionesConSombra.D3dTexture.Dispose();
     }

    /******************************************************************************************/
    /*                                      TEXTURAS
    /******************************************************************************************/
    public void Set_Textura_MenuOpcionesConSombra()
    {
        BoxSprite.Bitmap = MenuOpcionesConSombra;
    }

    public void Set_Textura_MenuOpcionesSinSombra()
    {
        BoxSprite.Bitmap = MenuOpcionesSinSombra;
    }

    /******************************************************************************************/
    /*                                      COLISION
    /******************************************************************************************/

    public int Is_MouseClicker()
    {
        if (_game._mouse.ClickIzq_RisingDown())
        {
            //ATRAS
            if (_game._mouse.Is_Position(CoordenadasOpcionesAtras.InicialX, CoordenadasOpcionesAtras.FinalX, CoordenadasOpcionesAtras.InicialY, CoordenadasOpcionesAtras.FinalY))
            {
                return 1;
            }
            if (_game._mouse.Is_Position(CoordenadasOpcionesSombra.InicialX, CoordenadasOpcionesSombra.FinalX, CoordenadasOpcionesSombra.InicialY, CoordenadasOpcionesSombra.FinalY))
            {
                return 3;
            }
            if (_game._mouse.Is_Position(CoordenadasOpcionesMusicaUP.InicialX, CoordenadasOpcionesMusicaUP.FinalX, CoordenadasOpcionesMusicaUP.InicialY, CoordenadasOpcionesMusicaUP.FinalY))
            {
                return 4;
            }
            if (_game._mouse.Is_Position(CoordenadasOpcionesMusicaDOWN.InicialX, CoordenadasOpcionesMusicaDOWN.FinalX, CoordenadasOpcionesMusicaDOWN.InicialY, CoordenadasOpcionesMusicaDOWN.FinalY))
            {
                return 5;
            }
            if (_game._mouse.Is_Position(CoordenadasOpcionesEfectoUP.InicialX, CoordenadasOpcionesEfectoUP.FinalX, CoordenadasOpcionesEfectoUP.InicialY, CoordenadasOpcionesEfectoUP.FinalY))
            {
                return 6;
            }
            if (_game._mouse.Is_Position(CoordenadasOpcionesEfectoDOWN.InicialX, CoordenadasOpcionesEfectoDOWN.FinalX, CoordenadasOpcionesEfectoDOWN.InicialY, CoordenadasOpcionesEfectoDOWN.FinalY))
            {
                return 7;
            }
            }
            if (_game._mouse.ClickIzq_Down())
            {
                if (_game._mouse.Is_Position(CoordenadasOpcionesMusicaUP.InicialX, CoordenadasOpcionesMusicaUP.FinalX, CoordenadasOpcionesMusicaUP.InicialY, CoordenadasOpcionesMusicaUP.FinalY))
                {
                    return 4;
                }
                if (_game._mouse.Is_Position(CoordenadasOpcionesMusicaDOWN.InicialX, CoordenadasOpcionesMusicaDOWN.FinalX, CoordenadasOpcionesMusicaDOWN.InicialY, CoordenadasOpcionesMusicaDOWN.FinalY))
                {
                    return 5;
                }
                if (_game._mouse.Is_Position(CoordenadasOpcionesEfectoUP.InicialX, CoordenadasOpcionesEfectoUP.FinalX, CoordenadasOpcionesEfectoUP.InicialY, CoordenadasOpcionesEfectoUP.FinalY))
                {
                    return 6;
                }
                if (_game._mouse.Is_Position(CoordenadasOpcionesEfectoDOWN.InicialX, CoordenadasOpcionesEfectoDOWN.FinalX, CoordenadasOpcionesEfectoDOWN.InicialY, CoordenadasOpcionesEfectoDOWN.FinalY))
                {
                    return 7;
                }
            }
                return 0;
    }


    /******************************************************************************************/
    /*                                      UPDATE
    /******************************************************************************************/
    // Devuelve verdadero si se acaba de clickear sobre el HUDBox
    public void Update()
    {

        int sectorMenuClick = Is_MouseClicker();
        switch (sectorMenuClick)
        {

            case 1:
                SalirOpciones = false;
                break;

            case 3:
                _game.shader.SombraEnable = !_game.shader.SombraEnable;
                if (_game.shader.SombraEnable)
                {
                    Set_Textura_MenuOpcionesConSombra();
                }
                else 
                if(!_game.shader.SombraEnable)
                {
                    Set_Textura_MenuOpcionesSinSombra();
                }
                break;
            case 4:
                    if (_game._sonidos.musicvolume < 100)
                    {
                        if (_game._sonidos.musicvolume < 0)
                        {
                            _game._sonidos.musicvolume = 2;
                        }
                        else
                        {
                            _game._sonidos.musicvolume += 2;
                        }
                    }
                        break;
            case 5:
                    if (_game._sonidos.musicvolume > 0)
                    {
                        _game._sonidos.musicvolume -= 2;
                    }
                    break;
            case 6:
                if ((_game._sonidos.fxvolume * 2) < 100)
                {
                    if (_game._sonidos.fxvolume < 0)
                    {
                        _game._sonidos.fxvolume = 1;
                    }
                    else
                    {
                        _game._sonidos.fxvolume++;
                    }
                }
                break;

            case 7:
                if ((_game._sonidos.fxvolume * 2) > 0)
                {
                    _game._sonidos.fxvolume--;
                }
                break;
                default: break;
        }

    }


    /******************************************************************************************/
    /*                                      RENDER
    /******************************************************************************************/

    // Renderiza todos los objetos relativos a la clase
    public void Render()
    {
        _game._spriteDrawer.BeginDrawSprite();
        _game._spriteDrawer.DrawSprite(BoxSprite);
        _game._spriteDrawer.EndDrawSprite();
        if(_game._sonidos.musicvolume>0)
            _game.DrawText.drawText(_game._sonidos.musicvolume.ToString(), (int)(D3DDevice.Instance.Device.Viewport.Width * 0.175), (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28), Color.Black);
        else
            _game.DrawText.drawText("0", (int)(D3DDevice.Instance.Device.Viewport.Width * 0.175), (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28), Color.Black);

        if ((_game._sonidos.fxvolume * 2) > 0)
            _game.DrawText.drawText((_game._sonidos.fxvolume * 2).ToString(), (int)(D3DDevice.Instance.Device.Viewport.Width * 0.45), (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28), Color.Black);
        else
            _game.DrawText.drawText("0", (int)(D3DDevice.Instance.Device.Viewport.Width * 0.45), (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28), Color.Black);

        }

    }




}
