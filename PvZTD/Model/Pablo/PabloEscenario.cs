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
        private Objeto3D p_Obj_Plano;
        private Objeto3D p_Obj_Mountain;










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Escenario()
        {
            p_Obj_Mountain = new Objeto3D(MediaDir + Game.Default.MeshMountain);

            p_Obj_Mountain.Inst_Create(-122, 0, 100);
            p_Obj_Mountain.Inst_Create(-124, 0, 50);
            p_Obj_Mountain.Inst_Create(-100, 0, 0);
            p_Obj_Mountain.Inst_Create(-121, 0, -50);
            p_Obj_Mountain.Inst_Create(-123, 0, -100);

            p_Obj_Plano = new Objeto3D(MediaDir + Game.Default.MeshPlano);
            p_Obj_Plano.Size(2, 1, 2);

            p_Obj_Plano.Inst_Create(0, 0, 0);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Escenario()
        {
            p_Obj_Plano.Render();
            p_Obj_Mountain.Render();
        }
    }
}
