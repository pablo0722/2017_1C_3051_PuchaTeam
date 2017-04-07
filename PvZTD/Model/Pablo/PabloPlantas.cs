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
                                    0.05F, 0.05F, 0.05F,
                                    0, PI, 0);

            p_Obj_Peashooter = new Objeto3D(MediaDir + Game.Default.MeshPea);
            p_Obj_Peashooter.Transform(0, 2.1F, 0,
                                    0.06F, 0.06F, 0.06F,
                                    0, PI, 0);

            p_Obj_Patatapum = new Objeto3D(MediaDir + Game.Default.MeshPatatapum);
            p_Obj_Patatapum.Transform(  0, -5.9F, 0,
                                        0.15F, 0.15F, 0.15F,
                                        0, PI, 0);
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
