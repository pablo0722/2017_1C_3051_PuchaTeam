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
         *                                      CONSTANTES
         ******************************************************************************************/
        private const float P_BOX_HUD_SIZE = 5;
        private const float P_BOX_HUD_POS_X = 20;
        private const float P_BOX_HUD_POS_Y = 62;
        private const float P_BOX_HUD_POS_Z = -55;
        private const float P_BOX_HUD_ROT = PI * (float)0.17;










        /******************************************************************************************
         *                                      VARIABLES HUD (Head-Up Display)
         ******************************************************************************************/
        private TgcBox p_Mesh_HUDPlant1 { get; set; }          // Caja Planta HUD 1
        private TgcBox p_Mesh_HUDPlant2 { get; set; }          // Caja Planta HUD 2
        private TgcBox p_Mesh_HUDPlant3 { get; set; }          // Caja Planta HUD 3










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_HUD()
        {
            var texture = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDGirasol);

            var size = new Vector3(P_BOX_HUD_SIZE, P_BOX_HUD_SIZE, P_BOX_HUD_SIZE);

            p_Mesh_HUDPlant1 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z), size, texture);
            p_Mesh_HUDPlant1.rotateZ(P_BOX_HUD_ROT);

            p_Mesh_HUDPlant2 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z + P_BOX_HUD_SIZE), size, texture);
            p_Mesh_HUDPlant2.rotateZ(P_BOX_HUD_ROT);

            p_Mesh_HUDPlant3 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z + P_BOX_HUD_SIZE * 2), size, texture);
            p_Mesh_HUDPlant3.rotateZ(P_BOX_HUD_ROT);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_HUD()
        {
            Func_BoxRender(p_Mesh_HUDPlant1);
            Func_BoxRender(p_Mesh_HUDPlant2);
            Func_BoxRender(p_Mesh_HUDPlant3);
        }
    }
}
