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
        public const string IMG_CONTORNO_PATH =         "..\\..\\Media\\Texturas\\hordas.png";
        public const string IMG_RELLENO_PATH =          "..\\..\\Media\\Texturas\\orig_60339.jpg";
        public const string IMG_INDICADOR_PATH =        "..\\..\\Media\\Texturas\\CabezaZombie.png";
        public const string IMG_FIN_NIVEL_PATH =        "..\\..\\Media\\Texturas\\Victory.jpg";
        public const string IMG_FIN_NIVEL_OVER_PATH =   "..\\..\\Media\\Texturas\\Victory.jpg";
        public const string IMG_HORDA_LLEGADA_1_PATH =  "..\\..\\Media\\Texturas\\hordas1.png";
        public const string IMG_HORDA_LLEGADA_2_PATH =  "..\\..\\Media\\Texturas\\hordas2.png";
        public const string IMG_GAMEOVER_PATH =         "..\\..\\Media\\Texturas\\gameover.png";
        public const string TXT_HORDA_NIVEL =           "total"; // Nombre que va a tener la duracion del nivel dentro del archivo de texto del nivel
        public const float ROTATION = (GameModel.PI * 3 / 2)/ROTATION_TIME;
        public const float ROTATION_TIME = 3;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        CustomBitmap HordaContornoBitmap;
        CustomBitmap HordaRellenoBitmap;
        CustomBitmap HordaIndicadorBitmap;
        CustomBitmap HordaLlegada1Bitmap;
        CustomBitmap HordaLlegada2Bitmap;
        CustomBitmap FinNivelBitmap;
        CustomBitmap FinNivelOverBitmap;
        CustomBitmap GameOverBitmap;
        CustomSprite HordaContornoSprite;
        CustomSprite HordaRellenoSprite;
        CustomSprite HordaIndicadorSprite;
        CustomSprite MensajeSprite;
        GameModel _game;
        public string _NivelActual = null;
        float tiempo = 0;
        int img_width;
        int img_height;
        public bool FinDeNivel = false;
        bool LlegadaHorda = false;

        float PrimeraHordaSx;
        float PrimeraHordaSy;
        float PrimeraHordaX;
        float PrimeraHordaY;

        float SegundaHordaSx;
        float SegundaHordaSy;
        float SegundaHordaX;
        float SegundaHordaY;

        float GameOverSx;
        float GameOverSy;
        float GameOverX;
        float GameOverY;

        float _TiempoTranscurrido;
        bool gameover = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_Hordas(GameModel game)
        {
            _game = game;

            HordaContornoBitmap = new CustomBitmap(IMG_CONTORNO_PATH, D3DDevice.Instance.Device);
            HordaRellenoBitmap = new CustomBitmap(IMG_RELLENO_PATH, D3DDevice.Instance.Device);
            HordaIndicadorBitmap = new CustomBitmap(IMG_INDICADOR_PATH, D3DDevice.Instance.Device);
            FinNivelBitmap = new CustomBitmap(IMG_FIN_NIVEL_PATH, D3DDevice.Instance.Device);
            FinNivelOverBitmap = new CustomBitmap(IMG_FIN_NIVEL_OVER_PATH, D3DDevice.Instance.Device);
            HordaLlegada1Bitmap = new CustomBitmap(IMG_HORDA_LLEGADA_1_PATH, D3DDevice.Instance.Device);
            HordaLlegada2Bitmap = new CustomBitmap(IMG_HORDA_LLEGADA_2_PATH, D3DDevice.Instance.Device);
            GameOverBitmap = new CustomBitmap(IMG_GAMEOVER_PATH, D3DDevice.Instance.Device);

            img_width = D3DDevice.Instance.Device.Viewport.Width / 3;
            img_height = D3DDevice.Instance.Device.Viewport.Height / 20;


            // Sprites de Barra de Hordas
            HordaContornoSprite = new CustomSprite();
            HordaContornoSprite.Bitmap = HordaContornoBitmap;
            HordaContornoSprite.SrcRect = new Rectangle(0, 0, HordaContornoBitmap.Width, HordaContornoBitmap.Height);
            HordaContornoSprite.Scaling = new Vector2((float)img_width / HordaContornoBitmap.Width, (float)img_height*5 / HordaContornoBitmap.Height);
            HordaContornoSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width - img_width * 1.1F,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 5.2F);
            HordaContornoSprite.Rotation = 0;

            HordaRellenoSprite = new CustomSprite();
            HordaRellenoSprite.Bitmap = HordaRellenoBitmap;
            HordaRellenoSprite.SrcRect = new Rectangle(0, 0, HordaRellenoBitmap.Width, HordaRellenoBitmap.Height);
            HordaRellenoSprite.Scaling = new Vector2(0, (float)img_height / HordaRellenoBitmap.Height);
            HordaRellenoSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width - img_width * 1.1F,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);
            HordaRellenoSprite.Rotation = 0;
            
            HordaIndicadorSprite = new CustomSprite();
            HordaIndicadorSprite.Bitmap = HordaIndicadorBitmap;
            HordaIndicadorSprite.SrcRect = new Rectangle(0, 0, HordaIndicadorBitmap.Width, HordaIndicadorBitmap.Height);
            HordaIndicadorSprite.Scaling = new Vector2((float)img_height / HordaIndicadorBitmap.Height, (float)img_height / HordaIndicadorBitmap.Height);
            HordaIndicadorSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width - img_height * 1.1F,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);
            HordaIndicadorSprite.Rotation = 0;


            // Sprite de Fin de Nivel
            MensajeSprite = new CustomSprite();
            MensajeSprite.Bitmap = FinNivelBitmap;
            MensajeSprite.SrcRect = new Rectangle(0, 0, FinNivelBitmap.Width, FinNivelBitmap.Height);
            MensajeSprite.Scaling = new Vector2((float)D3DDevice.Instance.Device.Viewport.Width/3 / FinNivelBitmap.Height, (float)D3DDevice.Instance.Device.Viewport.Height/3 / FinNivelBitmap.Height);
            MensajeSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width/3,
                    D3DDevice.Instance.Device.Viewport.Height/3);
            MensajeSprite.Rotation = 0;

            // Otras posiciones y tamaños
            PrimeraHordaSx = ((float)D3DDevice.Instance.Device.Viewport.Width - 100) / HordaLlegada1Bitmap.Width;
            PrimeraHordaSy = PrimeraHordaSx;
            PrimeraHordaX = 50;
            PrimeraHordaY = (D3DDevice.Instance.Device.Viewport.Height - PrimeraHordaSx * HordaLlegada1Bitmap.Height) / 2;

            SegundaHordaSx = ((float)D3DDevice.Instance.Device.Viewport.Width * 1.5F / 2) / HordaLlegada2Bitmap.Width;
            SegundaHordaSy = SegundaHordaSx;
            SegundaHordaX = ((float)D3DDevice.Instance.Device.Viewport.Width - SegundaHordaSx * HordaLlegada2Bitmap.Width) / 2;
            SegundaHordaY = (D3DDevice.Instance.Device.Viewport.Height - SegundaHordaSy * HordaLlegada2Bitmap.Height) / 2;

            GameOverSx = ((float)D3DDevice.Instance.Device.Viewport.Width /4) / GameOverBitmap.Width;
            GameOverSy = GameOverSx;
            GameOverX = ((float)D3DDevice.Instance.Device.Viewport.Width - GameOverSx * GameOverBitmap.Width) / 2;
            GameOverY = (D3DDevice.Instance.Device.Viewport.Height - GameOverSy * GameOverBitmap.Height) / 2;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void ClearScene()
        {
            _game._Lanzaguisantes._InstLanzaguisantes.Clear();
            _game._Lanzaguisantes._InstPlanta.Clear();
            _game._Lanzaguisantes._Planta.Inst_DeleteAll();
            _game._Lanzaguisantes._HUDBox._Is_BoxPicked = false;

            _game._repetidor._InstRepetidor.Clear();
            _game._repetidor._InstPlanta.Clear();
            _game._repetidor._Planta.Inst_DeleteAll();
            _game._repetidor._HUDBox._Is_BoxPicked = false;

            _game._Patatapum._InstPatatapum.Clear();
            _game._Patatapum._InstPlanta.Clear();
            _game._Patatapum._Planta.Inst_DeleteAll();
            _game._Patatapum._HUDBox._Is_BoxPicked = false;

            _game._Girasol._InstGirasol.Clear();
            _game._Girasol._InstPlanta.Clear();
            _game._Girasol._Planta.Inst_DeleteAll();
            _game._Girasol._HUDBox._Is_BoxPicked = false;

            t_HUDBox._Is_AnyBoxPicked = false;

            _game._zombie._InstZombie.Clear();
            _game._zombie._Zombie.Inst_DeleteAll();
            _game._zombie._NivelActual = null;

            _game._zombieCono._InstZombie.Clear();
            _game._zombieCono._Zombie.Inst_DeleteAll();
            _game._zombieCono._NivelActual = null;

            _game._zombieBalde._InstZombie.Clear();
            _game._zombieBalde._Zombie.Inst_DeleteAll();
            _game._zombieBalde._NivelActual = null;

            _game._EscenarioBase.Set_PastoDesocupadoAll();

            _game._TiempoTranscurrido = 0;
            FinDeNivel = false;
            gameover = false;
            t_ZombieComun.gameover = false;
            _TiempoTranscurrido = 0;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Renderiza todos los objetos relativos a la clase
        public void Update()
        {
            LlegadaHorda = false;
            if (String.Compare(_NivelActual, _game._NivelActual) != 0)
            {
                gameover = false;
                FinDeNivel = false;
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

            float sx = (float)img_width / HordaRellenoBitmap.Width;
            sx = _game._TiempoTranscurrido * sx / tiempo;
            if(sx > (float)img_width / HordaRellenoBitmap.Width)
                sx = (float)img_width / HordaRellenoBitmap.Width;
            HordaRellenoSprite.Scaling = new Vector2(sx, (float)img_height / HordaRellenoBitmap.Height);

            float x = D3DDevice.Instance.Device.Viewport.Width - (img_width * 1.1F) + (float)img_width - sx * (float)HordaRellenoBitmap.Width;
            HordaRellenoSprite.Position = new Vector2(x,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);

            HordaIndicadorSprite.Position = new Vector2(x - img_height / 2,
                    D3DDevice.Instance.Device.Viewport.Height - img_height * 1.5F);


            if (gameover)
            {
                // GAMEOVER
                _game._TiempoTranscurrido -= _game.ElapsedTime;
                _TiempoTranscurrido += _game.ElapsedTime;
                if (_TiempoTranscurrido < ROTATION_TIME)
                {
                    MensajeSprite.SrcRect = new Rectangle(0, 0, GameOverBitmap.Width, GameOverBitmap.Height);
                    MensajeSprite.Scaling = new Vector2(GameOverSx, GameOverSy);
                    MensajeSprite.Position = new Vector2(GameOverX, GameOverY);
                    MensajeSprite.Bitmap = GameOverBitmap;
                    MensajeSprite.RotationCenter = new Vector2(GameOverSx * GameOverBitmap.Width / 2, GameOverSy * GameOverBitmap.Height / 2);

                    MensajeSprite.Rotation = -_TiempoTranscurrido * ROTATION;
                }
                else if (_TiempoTranscurrido > ROTATION_TIME * 3F)
                {
                    ClearScene();
                    _NivelActual = null;
                    Menu.IniciarJuego = false;
                    MensajeSprite.Rotation = 0;
                }
            }
            if (t_ZombieComun.gameover && !gameover)
            {
                // GAMEOVER
                System.Windows.Forms.Cursor.Show();
                _game._TiempoTranscurrido -= _game.ElapsedTime;

                MensajeSprite.SrcRect = new Rectangle(0, 0, GameOverBitmap.Width, GameOverBitmap.Height);
                MensajeSprite.Scaling = new Vector2(GameOverSx, GameOverSy);
                MensajeSprite.Position = new Vector2(GameOverX, GameOverY);
                MensajeSprite.Bitmap = GameOverBitmap;
                MensajeSprite.RotationCenter = new Vector2(GameOverSx * GameOverBitmap.Width / 2, GameOverSy * GameOverBitmap.Height / 2);
                _TiempoTranscurrido = 0;
                gameover = true;
                _game._camara.Modo_Aerea();
            }

            if (!gameover && _game._TiempoTranscurrido > tiempo / 2 - 5 && _game._TiempoTranscurrido < tiempo/2)
            {
                // PRIMERA HORDA

                MensajeSprite.SrcRect = new Rectangle(0, 0, HordaLlegada1Bitmap.Width, HordaLlegada1Bitmap.Height);
                MensajeSprite.Scaling = new Vector2(PrimeraHordaSx, PrimeraHordaSy);
                MensajeSprite.Position = new Vector2(PrimeraHordaX, PrimeraHordaY);
                MensajeSprite.Bitmap = HordaLlegada1Bitmap;
                LlegadaHorda = true;
            }

            if (!gameover && _game._TiempoTranscurrido > tiempo - 5 && _game._TiempoTranscurrido < tiempo)
            {
                // SEGUNDA HORDA

                MensajeSprite.SrcRect = new Rectangle(0, 0, HordaLlegada2Bitmap.Width, HordaLlegada2Bitmap.Height);
                MensajeSprite.Scaling = new Vector2(SegundaHordaSx, SegundaHordaSy);
                MensajeSprite.Position = new Vector2(SegundaHordaX, SegundaHordaY);
                MensajeSprite.Bitmap = HordaLlegada2Bitmap;
                LlegadaHorda = true;
            }

            if (!gameover && FinDeNivel)
            {
                // FIN DEL NIVEL

                FinDeNivel = true;
                _game._TiempoTranscurrido -= _game.ElapsedTime;

                if (_game._mouse.Is_Position(D3DDevice.Instance.Device.Viewport.Width/3, D3DDevice.Instance.Device.Viewport.Width*2/3,
                                             D3DDevice.Instance.Device.Viewport.Height/3, D3DDevice.Instance.Device.Viewport.Height*2/3))
                {
                    MensajeSprite.SrcRect = new Rectangle(0, 0, FinNivelBitmap.Width, FinNivelBitmap.Height);
                    MensajeSprite.Scaling = new Vector2((float)D3DDevice.Instance.Device.Viewport.Width / 3 / FinNivelBitmap.Height, (float)D3DDevice.Instance.Device.Viewport.Height / 3 / FinNivelBitmap.Height);
                    MensajeSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width / 3,
                            D3DDevice.Instance.Device.Viewport.Height / 3);
                    MensajeSprite.Bitmap = FinNivelOverBitmap;

                    if (_game._mouse.ClickIzq_RisingDown())
                    {
                        if (String.Compare(_game._NivelActual, GameModel.TXT_NIVEL_1) == 0)
                        {
                            ClearScene();
                            _game._NivelActual = GameModel.TXT_NIVEL_2;
                        }
                    }
                }
                else
                {
                    MensajeSprite.SrcRect = new Rectangle(0, 0, FinNivelBitmap.Width, FinNivelBitmap.Height);
                    MensajeSprite.Scaling = new Vector2((float)D3DDevice.Instance.Device.Viewport.Width / 3 / FinNivelBitmap.Height, (float)D3DDevice.Instance.Device.Viewport.Height / 3 / FinNivelBitmap.Height);
                    MensajeSprite.Position = new Vector2(D3DDevice.Instance.Device.Viewport.Width / 3,
                            D3DDevice.Instance.Device.Viewport.Height / 3);
                    MensajeSprite.Bitmap = FinNivelBitmap;
                }
            }
            if (!gameover &&
                _game._TiempoTranscurrido > tiempo &&
                _game._zombie._InstZombie.Count == 0 &&
                _game._zombieCono._InstZombie.Count == 0 &&
                _game._zombieBalde._InstZombie.Count == 0)
            {
                // FIN DEL NIVEL

                System.Windows.Forms.Cursor.Show();
                _game._camara.Modo_Aerea();
                FinDeNivel = true;
            }
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
            _game._spriteDrawer.DrawSprite(HordaIndicadorSprite);

            if(LlegadaHorda || FinDeNivel || t_ZombieComun.gameover)
            {
                _game._spriteDrawer.DrawSprite(MensajeSprite);
            }

            _game._spriteDrawer.EndDrawSprite();
        }
    }
}
