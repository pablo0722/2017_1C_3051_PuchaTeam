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
        private Objeto3D p_Obj_Sol;










        /******************************************************************************************/
        /*                                      INICIALIZACION
        /******************************************************************************************/
        private void p_Func_Init_Soles()
        {
            p_Obj_Sol = new Objeto3D(MediaDir + Game.Default.MeshSol);
            p_Obj_Sol.Transform(0, 0, 0,
                                (float)0.075, (float)0.075, (float)0.075,
                                0, 0, 0);

            p_Obj_Sol.Inst_Create(0, 50, 0);
            p_Obj_Sol.Inst_Create(20, 50, 30);

            p_Obj_Sol.Inst_RotateAll(ElapsedTime / PI, 0, PI / 2);
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        private void p_Func_RotarSoles()
        {
            p_Obj_Sol.Inst_RotateAll(ElapsedTime / PI, 0, 0);
        }










            /******************************************************************************************/
            /*                                      RENDERIZACION
            /******************************************************************************************/
            private void p_Func_Render_Soles()
        {
            p_Obj_Sol.Render();
        }
    }
}
