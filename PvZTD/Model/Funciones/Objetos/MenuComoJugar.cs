using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;
using Microsoft.DirectX.DirectInput;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Text;
using static TGC.Group.Model.Funciones.Objetos.Menu;
using System.Collections.Generic;

namespace TGC.Group.Model.Funciones.Objetos
{
    class MenuComoJugar
    {
        private GameModel _game;
        public CustomSprite BoxSprite;
        public int x, y, sx, sy;

        public static bool SalirComoJugar = false;

        public List<CustomBitmap> listaBitmap;

        public CustomBitmap BoxBitmapPagina1;
        public CustomBitmap BoxBitmapPagina2;
        public CustomBitmap BoxBitmapPagina3;
        public CustomBitmap BoxBitmapPagina4;
        public CustomBitmap BoxBitmapPagina5;

        public const string PathTexturaPagina1 = "..\\..\\Media\\Texturas\\Pagina1.jpg";
        public const string PathTexturaPagina2 = "..\\..\\Media\\Texturas\\Pagina2.jpg";
        public const string PathTexturaPagina3 = "..\\..\\Media\\Texturas\\Pagina3.jpg";
        public const string PathTexturaPagina4 = "..\\..\\Media\\Texturas\\Pagina4.jpg";
        public const string PathTexturaPagina5 = "..\\..\\Media\\Texturas\\Pagina5.jpg";

        public Coordenadas Atras;
        public Coordenadas Siguiente;

        int paginador = 1;
        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private MenuComoJugar(GameModel game)
        {
            _game = game;
            listaBitmap = new List<CustomBitmap>();
            BoxBitmapPagina1 = new CustomBitmap(PathTexturaPagina1, D3DDevice.Instance.Device);
            BoxBitmapPagina2 = new CustomBitmap(PathTexturaPagina2, D3DDevice.Instance.Device);
            BoxBitmapPagina3 = new CustomBitmap(PathTexturaPagina3, D3DDevice.Instance.Device);
            BoxBitmapPagina4 = new CustomBitmap(PathTexturaPagina4, D3DDevice.Instance.Device);
            BoxBitmapPagina5 = new CustomBitmap(PathTexturaPagina5, D3DDevice.Instance.Device);
            listaBitmap.Add(BoxBitmapPagina1);
            listaBitmap.Add(BoxBitmapPagina2);
            listaBitmap.Add(BoxBitmapPagina3);
            listaBitmap.Add(BoxBitmapPagina4);
            listaBitmap.Add(BoxBitmapPagina5);


            Atras.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.03F);
            Atras.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.41F);
            Atras.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.80F);
            Atras.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.95F);

            Siguiente.InicialX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.59F);
            Siguiente.FinalX = (int)(D3DDevice.Instance.Device.Viewport.Width * 0.97F);
            Siguiente.InicialY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.80F);
            Siguiente.FinalY = (int)(D3DDevice.Instance.Device.Viewport.Height * 0.95F);

            BoxSprite = new CustomSprite();
            BoxSprite.Bitmap = BoxBitmapPagina1;
            sx = D3DDevice.Instance.Device.Viewport.Width;
            sy = D3DDevice.Instance.Device.Viewport.Height;
            x = 0;
            y = 0;
            BoxSprite.SrcRect = new Rectangle(0, 0, BoxBitmapPagina1.Size.Width, BoxBitmapPagina1.Size.Height);
            BoxSprite.Scaling = new Vector2((float)sx / BoxBitmapPagina1.Size.Width, (float)sy / BoxBitmapPagina1.Size.Height);
            BoxSprite.Position = new Vector2(x, y);
            BoxSprite.Rotation = 0;
        }


        public static MenuComoJugar Crear(GameModel game)
        {
            return new MenuComoJugar(game);
        }
        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~MenuComoJugar()
        {
            BoxBitmapPagina1.D3dTexture.Dispose();
            BoxBitmapPagina2.D3dTexture.Dispose();
            BoxBitmapPagina3.D3dTexture.Dispose();
            BoxBitmapPagina4.D3dTexture.Dispose();
            BoxBitmapPagina5.D3dTexture.Dispose();

        }

        /******************************************************************************************/
        /*                                      TEXTURAS
        /******************************************************************************************/
        public void Set_Textura_Pagina()
        {
            BoxSprite.Bitmap = listaBitmap[(paginador-1)];
        }

        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/

        public int Is_MouseClicker()
        {
            if (_game._mouse.ClickIzq_RisingDown())
            {
                //ATRAS
                if (_game._mouse.Is_Position(Atras.InicialX, Atras.FinalX, Atras.InicialY, Atras.FinalY))
                {
                    return 1;
                }
                if (_game._mouse.Is_Position(Siguiente.InicialX, Siguiente.FinalX, Siguiente.InicialY, Siguiente.FinalY))
                {
                    return 2;
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
                    if(paginador > 1)
                    {
                        paginador--;
                        Set_Textura_Pagina();
                    }
                    else
                    {
                        SalirComoJugar = false;
                    }
                    break;

                case 2:
                    if (paginador <5)
                    {
                        paginador++;
                        Set_Textura_Pagina();
                    }
                    break;
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
