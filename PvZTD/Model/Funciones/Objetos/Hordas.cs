using Microsoft.DirectX;
using System;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    public class t_Hordas
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        public const string IMG_CONTORNO_PATH = "..\\..\\Media\\Texturas\\hordas.png";
        public const string IMG_RELLENO_PATH = "..\\..\\Media\\Texturas\\orig_60339.jpg";
        public const string TXT_HORDA_NIVEL = "total"; // Nombre que va a tener el zombie comun dentro del archivo de texto del nivel










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        CustomBitmap HordaContornoBitmap;
        CustomBitmap HordaRellenoBitmap;
        CustomSprite HordaContornoSprite;
        CustomSprite HordaRellenoSprite;
        GameModel _game;
        public string _NivelActual = null;
        float tiempo = 0;
        int img_width;
        int img_height;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_Hordas(GameModel game)
        {
            _game = game;

            HordaContornoBitmap = new CustomBitmap(IMG_CONTORNO_PATH, D3DDevice.Instance.Device);
            HordaRellenoBitmap = new CustomBitmap(IMG_RELLENO_PATH, D3DDevice.Instance.Device);

            HordaContornoSprite = new CustomSprite();
            HordaContornoSprite.Bitmap = HordaContornoBitmap;
            HordaContornoSprite.SrcRect = new Rectangle(0, 0, HordaContornoBitmap.Width, HordaContornoBitmap.Height);
            img_width = D3DDevice.Instance.Device.Viewport.Width/3;
            img_height = D3DDevice.Instance.Device.Viewport.Height/20;
            HordaContornoSprite.Scaling = new Vector2((float)img_width / HordaContornoBitmap.Width, (float)img_height / HordaContornoBitmap.Height);
            HordaContornoSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width - img_width * 1.1F,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);
            HordaContornoSprite.Rotation = 0;

            HordaRellenoSprite = new CustomSprite();
            HordaRellenoSprite.Bitmap = HordaRellenoBitmap;
            HordaRellenoSprite.SrcRect = new Rectangle(0, 0, HordaRellenoBitmap.Width, HordaRellenoBitmap.Height);
            img_width = D3DDevice.Instance.Device.Viewport.Width / 3;
            img_height = D3DDevice.Instance.Device.Viewport.Height / 20;
            //HordaRellenoSprite.Scaling = new Vector2((float)img_width / HordaRellenoBitmap.Width, (float)img_height / HordaRellenoBitmap.Height);
            HordaRellenoSprite.Scaling = new Vector2(0, (float)img_height / HordaRellenoBitmap.Height);
            HordaRellenoSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width - img_width * 1.1F,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);
            HordaRellenoSprite.Rotation = 0;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Renderiza todos los objetos relativos a la clase
        public void Update()
        {
            if (_game._NivelActual != null)
            {
                if (String.Compare(_NivelActual, _game._NivelActual) != 0)
                {
                    _NivelActual = _game._NivelActual;
                    string txt_nivel = System.IO.File.ReadAllText(_game._NivelActual);
                    string[] tags = txt_nivel.Split('<', '>');
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (String.Compare(tags[i], TXT_HORDA_NIVEL) == 0)
                        {
                            var tiempos = tags[i + 1].Split(',');
                            tiempo = new float();
                            tiempo = int.Parse(tiempos[1]);
                            break;
                        }
                    }
                }
            }

            float sx = (float)img_width / HordaRellenoBitmap.Width;
            sx = _game._TiempoTranscurrido * sx / tiempo;
            if(sx > (float)img_width / HordaRellenoBitmap.Width)
                sx = (float)img_width / HordaRellenoBitmap.Width;
            HordaRellenoSprite.Scaling = new Vector2(sx, (float)img_height / HordaRellenoBitmap.Height);

            float x = D3DDevice.Instance.Device.Viewport.Width - (img_width * 1.1F) + (float)img_width - sx * (float)HordaRellenoBitmap.Width;
            HordaRellenoSprite.Position = new Vector2(x,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        // Renderiza todos los objetos relativos a la clase
        public void Render()
        {
            _game._spriteDrawer.BeginDrawSprite();
            _game._spriteDrawer.DrawSprite(HordaRellenoSprite);
            _game._spriteDrawer.DrawSprite(HordaContornoSprite);
            _game._spriteDrawer.EndDrawSprite();
        }
    }
}
