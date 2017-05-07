﻿using Microsoft.DirectX;
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
        private const float P_HUD_BOX_SIZE_X = 0.1F;
        private const float P_HUD_BOX_SIZE_Y = 2;
        private const float P_HUD_BOX_SIZE_Z = 2;
        private const float P_HUD_BOX_POS_X = 65;
        private const float P_HUD_BOX_POS_Y = 83;
        private const float P_HUD_BOX_POS_Z = -24;
        private const float P_HUD_BOX_ROT = GameModel.PI * (float)0.17;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private static int s_n = 0; // Se usa como campo de bits.
                                    // cada bit es una posicion distinta del BoxHUD de cada planta.
                                    // Si un bit esta en '1', significa que esa posicion esta ocupada.
                                    // Si un bit esta en '0', significa que esa posicion esta libre.
        private static Vector3 _HUDSize = new Vector3(P_HUD_BOX_SIZE_X, P_HUD_BOX_SIZE_Y, P_HUD_BOX_SIZE_Z);
        private static bool _ShowBoundingBox = false;
        private static bool _Is_AnyBoxPicked = false;

        // NO ESTATICAS
        private int _n;         // Numero (posicion) del BoxHUD
        private TgcBox _Mesh_HUDBoxOn;
        private TgcBox _Mesh_HUDBoxOff;
        private TgcBox _Mesh_HUDBoxActual;
        private GameModel _game;
        private bool _Is_BoxPicked;
        private int _ValorPlanta;
        CustomBitmap BoxBitmapOn;
        CustomBitmap BoxBitmapOff;
        CustomSprite BoxSprite;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_HUDBox(string PathTexturaOn, string PathTexturaOff, GameModel game, byte n, int ValorPlanta)
        {
            s_n = s_n | (1 << n);   // Reserva lugar en el campo de bits
            _n = n;

            _game = game;

            TgcTexture TexturaOn = TgcTexture.createTexture(PathTexturaOn);

            _Mesh_HUDBoxOn = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + (P_HUD_BOX_SIZE_Z + P_HUD_BOX_SIZE_X) * _n), _HUDSize, TexturaOn);
            _Mesh_HUDBoxOn.rotateZ(P_HUD_BOX_ROT);

            TgcTexture TexturaOff = TgcTexture.createTexture(PathTexturaOff);

            _Mesh_HUDBoxOff = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + (P_HUD_BOX_SIZE_Z + P_HUD_BOX_SIZE_X) * _n), _HUDSize, TexturaOff);
            _Mesh_HUDBoxOff.rotateZ(P_HUD_BOX_ROT);

            _Mesh_HUDBoxActual = _Mesh_HUDBoxOff;

            _Is_BoxPicked = false;

            _ValorPlanta = ValorPlanta;

            BoxBitmapOn = new CustomBitmap(PathTexturaOn, D3DDevice.Instance.Device);
            BoxBitmapOff = new CustomBitmap(PathTexturaOff, D3DDevice.Instance.Device);
            BoxSprite = new CustomSprite();
            BoxSprite.Bitmap = BoxBitmapOff;
            BoxSprite.SrcRect = new Rectangle(0, 0, 250, 250);
            BoxSprite.Scaling = new Vector2(1, 1);
            BoxSprite.Position = new Vector2(250 * n, 50);
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
            _Mesh_HUDBoxOn.dispose();
            _Mesh_HUDBoxOff.dispose();
            BoxBitmapOn.D3dTexture.Dispose();
            BoxBitmapOff.D3dTexture.Dispose();
        }










        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/
        public bool Is_MouseOver()
        {
            if(_game._colision.MouseBox(_Mesh_HUDBoxOff) || _game._colision.MouseBox(_Mesh_HUDBoxOn))
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
            _Mesh_HUDBoxActual = _Mesh_HUDBoxOn;
        }

        public void Set_Textura_Off()
        {
            _Mesh_HUDBoxActual = _Mesh_HUDBoxOff;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        // Devuelve verdadero si se acaba de clickear sobre el HUDBox
        public bool Update(bool ShowBoundingBoxWithKey, bool ChangeHUDTextureWhenMouseOver)
        {
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
            _game.spriteDrawer.BeginDrawSprite();
            _game.spriteDrawer.DrawSprite(BoxSprite);
            _game.spriteDrawer.EndDrawSprite();

            if (_game._camara.Modo_Is_CamaraAerea())
            {
                if (_game._soles >= _ValorPlanta)
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), 110 + 50 * _n, 50, System.Drawing.Color.Yellow);
                }
                else
                {
                    _game.DrawText.drawText(_ValorPlanta.ToString(), 110 + 50 * _n, 50, System.Drawing.Color.Red);
                }
            }

            Func_BoxRender(_Mesh_HUDBoxActual);
        }
    }
}
