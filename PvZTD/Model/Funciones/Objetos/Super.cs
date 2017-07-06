using Microsoft.DirectX;
using System;
using TGC.Group.Model.Funciones.Objetos;
using System.Drawing;
using TGC.Core.Direct3D;

namespace TGC.Group.Model
{
    public class t_Super
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        public const string IMG_CONTORNO_PATH = "..\\..\\Media\\Texturas\\Super.png";
        public const string IMG_RELLENO_PATH = "..\\..\\Media\\Texturas\\Sfondo_verde.jpg";
        public const string IMG_RELLENO_COMPLETO_PATH = "..\\..\\Media\\Texturas\\Verde_claro.jpg";
        public const string IMG_INDICADOR_PATH = "..\\..\\Media\\Texturas\\frijolito.png";
        public const string IMG_INDICADOR_FINISH_PATH = "..\\..\\Media\\Texturas\\atomoVerde.png";
        public const string TXT_HORDA_NIVEL = "total"; // Nombre que va a tener el zombie comun dentro del archivo de texto del nivel
        public const int TIEMPO = 30;//30;
        public const float ROTATION = 1F;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        CustomBitmap SuperContornoBitmap;
        CustomBitmap SuperRellenoBitmap;
        CustomBitmap SuperRellenoCompletoBitmap;
        CustomBitmap SuperIndicadorBitmap;
        CustomBitmap SuperIndicadorFinishBitmap;
        CustomSprite SuperContornoSprite;
        CustomSprite SuperRellenoSprite;
        CustomSprite SuperIndicadorSprite;
        GameModel _game;
        public static float _TiempoTranscurrido;
        int img_width;
        int img_height;
        bool Finished;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_Super(GameModel game)
        {
            _game = game;

            Finished = false;

            _TiempoTranscurrido = 0;

            SuperContornoBitmap = new CustomBitmap(IMG_CONTORNO_PATH, D3DDevice.Instance.Device);
            SuperRellenoBitmap = new CustomBitmap(IMG_RELLENO_PATH, D3DDevice.Instance.Device);
            SuperRellenoCompletoBitmap = new CustomBitmap(IMG_RELLENO_COMPLETO_PATH, D3DDevice.Instance.Device);
            SuperIndicadorBitmap = new CustomBitmap(IMG_INDICADOR_PATH, D3DDevice.Instance.Device);
            SuperIndicadorFinishBitmap = new CustomBitmap(IMG_INDICADOR_FINISH_PATH, D3DDevice.Instance.Device);

            img_width = GameModel._ResolucionPantalla.Width / 30;
            img_height = GameModel._ResolucionPantalla.Height / 2;

            SuperContornoSprite = new CustomSprite();
            SuperContornoSprite.Bitmap = SuperContornoBitmap;
            SuperContornoSprite.SrcRect = new Rectangle(0, 0, SuperContornoBitmap.Width, SuperContornoBitmap.Height);
            SuperContornoSprite.Scaling = new Vector2((float)img_width / SuperContornoBitmap.Width, (float)img_height / SuperContornoBitmap.Height);
            SuperContornoSprite.Position = new Vector2(img_width/2,
                    GameModel._ResolucionPantalla.Height - img_height * 1.2F );
            SuperContornoSprite.Rotation = 0;

            SuperRellenoSprite = new CustomSprite();
            SuperRellenoSprite.Bitmap = SuperRellenoBitmap;
            SuperRellenoSprite.SrcRect = new Rectangle(0, 0, SuperRellenoBitmap.Width, SuperRellenoBitmap.Height);
            SuperRellenoSprite.Scaling = new Vector2((float)img_width / SuperRellenoBitmap.Width, 0);
            SuperRellenoSprite.Position = new Vector2(img_width / 2,
                    GameModel._ResolucionPantalla.Height - img_height * 0.2F);
            SuperRellenoSprite.Rotation = 0;

            SuperIndicadorSprite = new CustomSprite();
            SuperIndicadorSprite.Bitmap = SuperIndicadorBitmap;
            SuperIndicadorSprite.SrcRect = new Rectangle(0, 0, SuperIndicadorBitmap.Width, SuperIndicadorBitmap.Height);
            SuperIndicadorSprite.Scaling = new Vector2((float)img_width / SuperIndicadorBitmap.Width, (float)img_width / SuperIndicadorBitmap.Width);
            SuperIndicadorSprite.Position = new Vector2(img_width / 2,
                    GameModel._ResolucionPantalla.Height - img_height * 0.2F - img_width/2);
            SuperIndicadorSprite.Rotation = 0;
            SuperIndicadorSprite.RotationCenter = new Vector2((float)img_width/2, (float)img_width/2);
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public bool Is_Finished()
        {
            return Finished;
        }

        public void FinishReset()
        {
            _TiempoTranscurrido = 0;
            Finished = false;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Renderiza todos los objetos relativos a la clase
        public void Update()
        {
            float sy;
            float y;

            _TiempoTranscurrido += _game.ElapsedTime;


            sy = _TiempoTranscurrido / TIEMPO;
            if (sy > 1)
            {
                sy = (float)img_height / SuperRellenoCompletoBitmap.Height;
                Finished = true;
                SuperIndicadorSprite.Bitmap = SuperIndicadorFinishBitmap;
                SuperIndicadorSprite.Rotation = _TiempoTranscurrido * ROTATION;

                SuperRellenoSprite.Bitmap = SuperRellenoCompletoBitmap;
                SuperRellenoSprite.SrcRect = new Rectangle(0, 0, SuperRellenoCompletoBitmap.Width, SuperRellenoCompletoBitmap.Height);
                SuperRellenoSprite.Scaling = new Vector2((float)img_width / SuperRellenoCompletoBitmap.Width, sy);
                y = GameModel._ResolucionPantalla.Height - (img_height * 0.2F) - sy * SuperRellenoCompletoBitmap.Height;
                SuperIndicadorSprite.Position = new Vector2(img_width / 2, y - img_width / 2);
            }
            else
            {
                sy = sy * img_height / SuperRellenoBitmap.Height;
                Finished = false;
                SuperIndicadorSprite.Bitmap = SuperIndicadorBitmap;
                SuperIndicadorSprite.Rotation = 0;

                SuperRellenoSprite.Bitmap = SuperRellenoBitmap;
                SuperRellenoSprite.SrcRect = new Rectangle(0, 0, SuperRellenoBitmap.Width, SuperRellenoBitmap.Height);
                SuperRellenoSprite.Scaling = new Vector2((float)img_width / SuperRellenoBitmap.Width, sy);
                y = GameModel._ResolucionPantalla.Height - (img_height * 0.2F) - sy * SuperRellenoBitmap.Height;
                SuperIndicadorSprite.Position = new Vector2(img_width / 2, y - img_width / 2);
            }

            SuperRellenoSprite.Position = new Vector2(img_width / 2, y);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        // Renderiza todos los objetos relativos a la clase
        public void Render()
        {
            _game._spriteDrawer.BeginDrawSprite();
            _game._spriteDrawer.DrawSprite(SuperRellenoSprite);
            _game._spriteDrawer.DrawSprite(SuperContornoSprite);
            _game._spriteDrawer.DrawSprite(SuperIndicadorSprite);
            _game._spriteDrawer.EndDrawSprite();
        }
    }
}
