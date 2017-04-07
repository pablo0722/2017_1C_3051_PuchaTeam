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
         *                                      VARIABLES
         ******************************************************************************************/
        private Objeto3D p_Obj_Zombie;










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Zombies()
        {
            p_Obj_Zombie = new Objeto3D(MediaDir + Game.Default.MeshZombie);
            p_Obj_Zombie.Size((float)0.25, (float)0.25, (float)0.25);

            p_Obj_Zombie.Inst_Create(-32, 0, 70);
            p_Obj_Zombie.Inst_Create(-32 + 21, 0, 70);
            p_Obj_Zombie.Inst_Create(-32 + 21 * 2, 0, 70);
            p_Obj_Zombie.Inst_Create(-32 + 21 * 3, 0, 70);
            p_Obj_Zombie.Inst_Create(-32 + 21 * 4, 0, 70);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Zombies()
        {
            p_Obj_Zombie.Render();
        }
    }
}
