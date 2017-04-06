using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

using System.Collections.Generic;

namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************
         *                                      ESTRUCTURAS
         ******************************************************************************************/
        private struct p_s_HUDPlanta
        {
            public int n;
            public TgcBox Mesh_box;
            public TgcTexture TexturaOn;
            public TgcTexture TexturaOff;
        };










        /******************************************************************************************
         *                                      CONSTANTES
         ******************************************************************************************/
        // Tamaños y posiciones
        private const float P_HUD_BOX_SIZE = 5;
        private const float P_HUD_BOX_POS_X = 20;
        private const float P_HUD_BOX_POS_Y = 62;
        private const float P_HUD_BOX_POS_Z = -55;
        private const float P_HUD_BOX_ROT = PI * (float)0.17;

        // Numero de cajas HUD (de plantas)
        // Numero (desde 0 en adelante) de caja para setear posicion de izquierda a derecha
        private const int P_HUD_N_GIRASOL = 0;
        private const int P_HUD_N_PEASHOOTER = 1;
        private const int P_HUD_N_PATATAPUM = 2;












        /******************************************************************************************
         *                                      VARIABLES HUD (Head-Up Display)
         ******************************************************************************************/
        private p_s_HUDPlanta p_HUDPlanta_Girasol;
        private p_s_HUDPlanta p_HUDPlanta_Peashooter;
        private p_s_HUDPlanta p_HUDPlanta_Patatapum;

        private Vector3 HUDSize;










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_HUD()
        {
            HUDSize = new Vector3(P_HUD_BOX_SIZE, P_HUD_BOX_SIZE, P_HUD_BOX_SIZE);

            p_HUDPlanta_Girasol.n = 0;
            p_HUDPlanta_Peashooter.n = 1;
            p_HUDPlanta_Patatapum.n = 2;

            p_HUDPlanta_Girasol.TexturaOff = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDGirasolOff);
            p_HUDPlanta_Peashooter.TexturaOff = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDPeashooterOff);
            p_HUDPlanta_Patatapum.TexturaOff = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDPatatapumOff);

            p_HUDPlanta_Girasol.TexturaOn = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDGirasolOn);
            p_HUDPlanta_Peashooter.TexturaOn = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDPeashooterOn);
            p_HUDPlanta_Patatapum.TexturaOn = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDPatatapumOn);

            p_HUDPlanta_Girasol.Mesh_box = new TgcBox();
            p_HUDPlanta_Peashooter.Mesh_box = new TgcBox();
            p_HUDPlanta_Patatapum.Mesh_box = new TgcBox();

            p_Func_Init_HUDBoxes();
        }

        // Inicializa todas las texturas
        private void p_Func_Init_HUDBoxes()
        {
            p_Func_HUDBoxTexturaOff(ref p_HUDPlanta_Girasol);
            p_Func_HUDBoxTexturaOff(ref p_HUDPlanta_Peashooter);
            p_Func_HUDBoxTexturaOff(ref p_HUDPlanta_Patatapum);
        }










        /******************************************************************************************
         *                                      TEXTURAS
         ******************************************************************************************/
        private void p_Func_HUDBoxTexturaOn(ref p_s_HUDPlanta box)
        {
            bool change_actual = false;
            bool change_prev = false;

            if (Mesh_BoxPicked == box.Mesh_box)
            {
                change_actual = true;
            }

            if (Mesh_BoxPickedPrev == box.Mesh_box)
            {
                change_prev = true;
            }

            box.Mesh_box = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + P_HUD_BOX_SIZE * box.n), HUDSize, box.TexturaOn);
            box.Mesh_box.rotateZ(P_HUD_BOX_ROT);

            if(change_actual)
            {
                Mesh_BoxPicked = box.Mesh_box;
            }

            if (change_prev)
            {
                Mesh_BoxPickedPrev = box.Mesh_box;
            }
        }

        private void p_Func_HUDBoxTexturaOff(ref p_s_HUDPlanta box)
        {
            bool change_actual = false;
            bool change_prev = false;

            if (Mesh_BoxPicked == box.Mesh_box)
            {
                change_actual = true;
            }

            if (Mesh_BoxPickedPrev == box.Mesh_box)
            {
                change_prev = true;
            }

            box.Mesh_box = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + P_HUD_BOX_SIZE * box.n), HUDSize, box.TexturaOff);
            box.Mesh_box.rotateZ(P_HUD_BOX_ROT);

            if (change_actual)
            {
                Mesh_BoxPicked = box.Mesh_box;
            }

            if (change_prev)
            {
                Mesh_BoxPickedPrev = box.Mesh_box;
            }
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_HUD()
        {
            Func_BoxRender(p_HUDPlanta_Girasol.Mesh_box);
            Func_BoxRender(p_HUDPlanta_Peashooter.Mesh_box);
            Func_BoxRender(p_HUDPlanta_Patatapum.Mesh_box);
        }










        /******************************************************************************************
         *                                      DISPOSE
         ******************************************************************************************/
        private void p_Func_Dispose_HUD()
        {
            p_HUDPlanta_Girasol.Mesh_box.dispose();
            p_HUDPlanta_Peashooter.Mesh_box.dispose();
            p_HUDPlanta_Patatapum.Mesh_box.dispose();
        }
    }
}
