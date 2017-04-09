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
        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private t_Objeto3D p_Obj_Sol;










        /******************************************************************************************/
        /*                                      INICIALIZACION
        /******************************************************************************************/
        private void p_Func_Soles_Init()
        {
            p_Obj_Sol = t_Objeto3D.CrearObjeto3D(MediaDir + Game.Default.MeshSol);
            p_Obj_Sol.Set_Transform(0, 0, 0,
                                (float)0.075, (float)0.075, (float)0.075,
                                0, 0, 0);

            p_Obj_Sol.Inst_Create(0, 50, 0);
            p_Obj_Sol.Inst_Create(20, 50, 30);

            p_Obj_Sol.Inst_RotateAll(ElapsedTime / PI, 0, PI / 2);
        }










        /******************************************************************************************/
        /*                                      UPDATES
        /******************************************************************************************/
        private void p_Func_Soles_Update_Rotation()
        {
            p_Obj_Sol.Inst_RotateAll(ElapsedTime / PI, 0, 0);
        }










        /******************************************************************************************/
        /*                                      RENDERIZACION
        /******************************************************************************************/
        private void p_Func_Soles_Render()
        {
            p_Obj_Sol.Render();
        }
    }
}
