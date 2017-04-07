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
        private Objeto3D p_Obj_Girasol;
        private Objeto3D p_Obj_Peashooter;
        private Objeto3D p_Obj_Patatapum;

        //      Posicion de las plantas
        private Vector3 p_Pos_PlantaActual { get; set; }       // Posicion Planta Actual










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Plantas()
        {
            p_Obj_Girasol = new Objeto3D(MediaDir + Game.Default.MeshGirasol);
            p_Obj_Girasol.Transform(0, 0, 0,
                                    (float)0.05, (float)0.05, (float)0.05,
                                    0, (float)PI, 0);

            p_Obj_Peashooter = new Objeto3D(MediaDir + Game.Default.MeshPea);
            p_Obj_Peashooter.Transform(0, 3, 0,
                                    (float)0.05, (float)0.05, (float)0.05,
                                    0, (float)PI, 0);

            p_Obj_Patatapum = new Objeto3D(MediaDir + Game.Default.MeshPatatapum);
            p_Obj_Patatapum.Transform(  0, -5, 0,
                                        (float)0.15, (float)0.15, (float)0.15,
                                        0, (float)PI, 0);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Plantas()
        {
            p_Obj_Girasol.Render();
            p_Obj_Peashooter.Render();
            p_Obj_Patatapum.Render();
        }
    }
}
