using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;

namespace TGC.Group.Model.Funciones.Objetos
{
    public class Menu
    {

        public CustomBitmap mi_Bitmap; // es donde se carga la imagen
        public CustomSprite mi_sprite; // Es como una instancia de la imagen. es para cargar la imagen una sola vez y poder dibujarla varias veces.
        private GameModel _game;

        public CustomBitmap BoxBitmapBase;
        public CustomBitmap BoxBitmapIP;
        public CustomBitmap BoxBitmapSalida;
        public CustomBitmap BoxBitmapOpciones;
        public CustomBitmap BoxBitmapComoJugar;

        public CustomSprite BoxSprite;
        public int x, y, sx, sy;
        public const int POSICION_MENU = 0;

        public const string PathTexturaMenuBase = "..\\..\\Media\\Texturas\\MenuPvZ.png";
        public const string PathTexturaMenuIniciar = "..\\..\\Media\\Texturas\\MenuPvZIP.png";
        public const string PathTexturaMenuSalida = "..\\..\\Media\\Texturas\\MenuPvZSalida.png";
        public const string PathTexturaMenuOpciones = "..\\..\\Media\\Texturas\\MenuPvZOpciones.png";
        public const string PathTexturaMenuComoJugar = "..\\..\\Media\\Texturas\\MenuPvZCJ.png";

        public static bool IniciarJuego = false;

        public struct Coordenadas
        {

            public int InicialX;
            public int InicialY;
            public int FinalX;
            public int FinalY;
        }
        Coordenadas CoordenadasIP;
        Coordenadas CoordenadasSalida;
        Coordenadas CoordenadasOpciones;
        Coordenadas CoordenadasComoJugar;



        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private Menu( GameModel game)
        {
            _game = game;

            CoordenadasIP.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.33F);
            CoordenadasIP.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.38F);
            CoordenadasIP.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.57F);
            CoordenadasIP.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.78F);

            CoordenadasSalida.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.84F);
            CoordenadasSalida.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.02);
            CoordenadasSalida.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.96F);
            CoordenadasSalida.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.11);

            CoordenadasOpciones.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.77F);
            CoordenadasOpciones.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.46F);
            CoordenadasOpciones.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.96F);
            CoordenadasOpciones.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.58F);

            CoordenadasComoJugar.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.77F);
            CoordenadasComoJugar.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.60F);
            CoordenadasComoJugar.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.96F);
            CoordenadasComoJugar.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.72F);


            BoxBitmapBase = new CustomBitmap(PathTexturaMenuBase, D3DDevice.Instance.Device);
            BoxBitmapIP = new CustomBitmap(PathTexturaMenuIniciar, D3DDevice.Instance.Device);
            BoxBitmapSalida = new CustomBitmap(PathTexturaMenuSalida, D3DDevice.Instance.Device);
            BoxBitmapOpciones = new CustomBitmap(PathTexturaMenuOpciones, D3DDevice.Instance.Device);
            BoxBitmapComoJugar = new CustomBitmap(PathTexturaMenuComoJugar, D3DDevice.Instance.Device);

            BoxSprite = new CustomSprite();
            BoxSprite.Bitmap = BoxBitmapIP;
            sx = D3DDevice.Instance.Device.Viewport.Width;
            sy = D3DDevice.Instance.Device.Viewport.Height;
            x = 0;
            y = 0;
            BoxSprite.SrcRect = new Rectangle(0, 0, BoxBitmapBase.Size.Width, BoxBitmapBase.Size.Height);
            BoxSprite.Scaling = new Vector2((float)sx / BoxBitmapBase.Size.Width, (float)sy / BoxBitmapBase.Size.Height);
            BoxSprite.Position = new Vector2(x, y);
            BoxSprite.Rotation = 0;
        }

        public static Menu Crear( GameModel game)
        {
                return new Menu(game);
        }

        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~Menu()
        {
            BoxBitmapBase.D3dTexture.Dispose();
            BoxBitmapIP.D3dTexture.Dispose();
        }

        /******************************************************************************************/
        /*                                      TEXTURAS
        /******************************************************************************************/
        public void Set_Textura_IP()
        {
            BoxSprite.Bitmap = BoxBitmapIP;
        }

        public void Set_Textura_Base()
        {
            BoxSprite.Bitmap = BoxBitmapBase;
        }

        public void Set_Textura_Salida()
        {
            BoxSprite.Bitmap = BoxBitmapSalida;
        }

        public void Set_Textura_Opcionese()
        {
            BoxSprite.Bitmap = BoxBitmapOpciones;
        }

        public void Set_Textura_ComoJugar()
        {
            BoxSprite.Bitmap = BoxBitmapComoJugar;
        }
        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/
        public int Is_MouseOver()
        {
            //INICIAR
            if (_game._mouse.Is_Position(CoordenadasIP.InicialX, CoordenadasIP.FinalX, CoordenadasIP.InicialY, CoordenadasIP.FinalY))
            {
                return 1;
            }
            //SALIDA
            if (_game._mouse.Is_Position(CoordenadasSalida.InicialX, CoordenadasSalida.FinalX, CoordenadasSalida.InicialY, CoordenadasSalida.FinalY))
            {
                return 2;
            }
            //Opciones
            if (_game._mouse.Is_Position(CoordenadasOpciones.InicialX, CoordenadasOpciones.FinalX, CoordenadasOpciones.InicialY, CoordenadasOpciones.FinalY))
            {
                return 3;
            }
            //Como Jugar
            if (_game._mouse.Is_Position(CoordenadasComoJugar.InicialX, CoordenadasComoJugar.FinalX, CoordenadasComoJugar.InicialY, CoordenadasComoJugar.FinalY))
            {
                return 4;
            }

            return 0;
        }

        public int Is_MouseClicker()
        {
            if (_game._mouse.ClickIzq_RisingDown())
            {
                //INICIAR
                if (_game._mouse.Is_Position(CoordenadasIP.InicialX, CoordenadasIP.FinalX, CoordenadasIP.InicialY, CoordenadasIP.FinalY))
                {
                    return 1;
                }
                //SALIDA
                if (_game._mouse.Is_Position(CoordenadasSalida.InicialX, CoordenadasSalida.FinalX, CoordenadasSalida.InicialY, CoordenadasSalida.FinalY))
                {
                    return 2;
                }
                //Opciones
                if (_game._mouse.Is_Position(CoordenadasOpciones.InicialX, CoordenadasOpciones.FinalX, CoordenadasOpciones.InicialY, CoordenadasOpciones.FinalY))
                {
                    return 3;
                }
                //Como Jugar
                if (_game._mouse.Is_Position(CoordenadasComoJugar.InicialX, CoordenadasComoJugar.FinalX, CoordenadasComoJugar.InicialY, CoordenadasComoJugar.FinalY))
                {
                    return 4;
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


            int sectorMenu = Is_MouseOver();
            switch (sectorMenu)
            {
                case 0:
                    Set_Textura_Base();
                    break;

                case 1:
                    Set_Textura_IP();
                    break;

                case 2:
                    Set_Textura_Salida();
                    break;

                case 3:
                    Set_Textura_Opcionese();
                    break;

                case 4:
                    Set_Textura_ComoJugar();
                    break;

                default: break;
            }
            int sectorMenuClick = Is_MouseClicker();
            switch (sectorMenuClick)
            {
                case 0:
                    break;

                case 1:
                    IniciarJuego = true;
                    _game._TiempoTranscurrido = 0;
                    _game._camara.Aerea_Reset();
                    break;

                case 2:
                    System.Environment.Exit(1);
                    break;

                case 3:
                    break;

                case 4:
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
