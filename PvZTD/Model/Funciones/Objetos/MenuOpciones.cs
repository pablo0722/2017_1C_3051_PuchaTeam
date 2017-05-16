using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;

namespace TGC.Group.Model.Funciones.Objetos
{
    class MenuOpciones
    { 
    public CustomBitmap mi_Bitmap; // es donde se carga la imagen
    public CustomSprite mi_sprite; // Es como una instancia de la imagen. es para cargar la imagen una sola vez y poder dibujarla varias veces.
    private GameModel _game;

    public CustomBitmap BoxBitmapMenuOpciones;
    public CustomBitmap BoxBitmapIPMenuOpcionesLuz;
    public CustomBitmap BoxBitmapMenuOpcionesSombra;
    public CustomBitmap BoxBitmapMenuOpcionesSombraLuz;

    public CustomSprite BoxSprite;
    public int x, y, sx, sy;
    public const int POSICION_MENU = 0;

    public const string PathTexturaMenuOpciones = "..\\..\\Media\\Texturas\\MenuOpciones.png";
    public const string PathTexturaMenuOpcionesLuz = "..\\..\\Media\\Texturas\\MenuOpcionesLuz.png";
    public const string PathTexturaMenuOpcionesSombra = "..\\..\\Media\\Texturas\\MenuOpcionesSombra.png";
    public const string PathTexturaMenuOpcionesSombraLuz = "..\\..\\Media\\Texturas\\MenuOpcionesSombraLuz.png";

    public static bool SalirOpciones = false;

    public struct Coordenadas
    {
        public int InicialX;
        public int InicialY;
        public int FinalX;
        public int FinalY;
    }
        public Coordenadas CoordenadasOpcionesAtras;
        public Coordenadas CoordenadasOpcionesIluminacion;
        public Coordenadas CoordenadasOpcionesSombra;
        public Coordenadas CoordenadasOpcionesMusicaUP;
        public Coordenadas CoordenadasOpcionesMusicaDOWN;

        public bool EstadoLuz = false;
        public bool EstadoSombra = false;
        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private MenuOpciones(GameModel game)
    {
        _game = game;

        CoordenadasOpcionesAtras.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.03F);
        CoordenadasOpcionesAtras.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.41F);
        CoordenadasOpcionesAtras.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.74F);
        CoordenadasOpcionesAtras.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.90F);

        CoordenadasOpcionesIluminacion.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.27F);
        CoordenadasOpcionesIluminacion.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.32F);
        CoordenadasOpcionesIluminacion.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.53F);
        CoordenadasOpcionesIluminacion.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.62F);

        CoordenadasOpcionesSombra.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.20F);
        CoordenadasOpcionesSombra.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.24F);
        CoordenadasOpcionesSombra.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.38F);
        CoordenadasOpcionesSombra.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.46F);

        CoordenadasOpcionesMusicaUP.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.20F);
        CoordenadasOpcionesMusicaUP.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.24F);
        CoordenadasOpcionesMusicaUP.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.23F);
        CoordenadasOpcionesMusicaUP.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.27F);

        CoordenadasOpcionesMusicaDOWN.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.20F);
        CoordenadasOpcionesMusicaDOWN.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.24F);
        CoordenadasOpcionesMusicaDOWN.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.28F);
        CoordenadasOpcionesMusicaDOWN.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.32F);

        BoxBitmapMenuOpciones = new CustomBitmap(PathTexturaMenuOpciones, D3DDevice.Instance.Device);
        BoxBitmapIPMenuOpcionesLuz = new CustomBitmap(PathTexturaMenuOpcionesLuz, D3DDevice.Instance.Device);
        BoxBitmapMenuOpcionesSombra = new CustomBitmap(PathTexturaMenuOpcionesSombra, D3DDevice.Instance.Device);
        BoxBitmapMenuOpcionesSombraLuz = new CustomBitmap(PathTexturaMenuOpcionesSombraLuz, D3DDevice.Instance.Device);

        BoxSprite = new CustomSprite();
        BoxSprite.Bitmap = BoxBitmapMenuOpciones;
        sx = D3DDevice.Instance.Device.Viewport.Width;
        sy = D3DDevice.Instance.Device.Viewport.Height;
        x = 0;
        y = 0;
        BoxSprite.SrcRect = new Rectangle(0, 0, BoxBitmapMenuOpciones.Size.Width, BoxBitmapMenuOpciones.Size.Height);
        BoxSprite.Scaling = new Vector2((float)sx / BoxBitmapMenuOpciones.Size.Width, (float)sy / BoxBitmapMenuOpciones.Size.Height);
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
        BoxBitmapMenuOpciones.D3dTexture.Dispose();
       
    }

    /******************************************************************************************/
    /*                                      TEXTURAS
    /******************************************************************************************/
    public void Set_Textura_MenuOpciones()
    {
        BoxSprite.Bitmap = BoxBitmapMenuOpciones;
    }

    public void Set_Textura_MenuOpcionesLuz()
    {
        BoxSprite.Bitmap = BoxBitmapIPMenuOpcionesLuz;
    }

    public void Set_Textura_MenuOpcionesSombra()
    {
        BoxSprite.Bitmap = BoxBitmapMenuOpcionesSombra;
    }

    public void Set_Textura_MenuOpcionesSombraLuz()
    {
        BoxSprite.Bitmap = BoxBitmapMenuOpcionesSombraLuz;
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
            if (_game._mouse.Is_Position(CoordenadasOpcionesIluminacion.InicialX, CoordenadasOpcionesIluminacion.FinalX, CoordenadasOpcionesIluminacion.InicialY, CoordenadasOpcionesIluminacion.FinalY))
            {
                return 2;
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

            case 2:
                    EstadoLuz = !EstadoLuz;
                    if (EstadoSombra && EstadoLuz)
                    {
                        Set_Textura_MenuOpcionesSombraLuz();
                    }
                    else if (EstadoLuz)
                    {
                        Set_Textura_MenuOpcionesLuz();
                    }
                    else if (EstadoSombra)
                    {
                        Set_Textura_MenuOpcionesSombra();
                    }
                    else
                    {
                        Set_Textura_MenuOpciones();
                    }
                break;

            case 3:
                EstadoSombra = !EstadoSombra;
                if (EstadoSombra&&EstadoLuz)
                {
                    Set_Textura_MenuOpcionesSombraLuz();
                }
                else if(EstadoSombra)
                {
                    Set_Textura_MenuOpcionesSombra();
                }
                else if (EstadoLuz)
                {
                    Set_Textura_MenuOpcionesLuz();
                }
                else 
                {
                    Set_Textura_MenuOpciones();
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
    }

}




}
