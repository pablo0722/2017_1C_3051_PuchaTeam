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
        private t_Objeto3D p_Obj_Girasol;
        private t_Objeto3D p_Obj_Peashooter;
        private t_Objeto3D p_Obj_Patatapum;

        //      Posicion de las plantas
        private Vector3 p_Pos_PlantaActual { get; set; }       // Posicion Planta Actual










        /******************************************************************************************/
        /*                                      INICIALIZACION
        /******************************************************************************************/
        private void p_Func_Plantas_Init()
        {
            p_Obj_Girasol = t_Objeto3D.CrearObjeto3D(MediaDir + Game.Default.MeshGirasol);
            p_Obj_Girasol.Set_Transform(0, 0, 0,
                                        0.05F, 0.05F, 0.05F,
                                        0, PI, 0);

            p_Obj_Peashooter = t_Objeto3D.CrearObjeto3D(MediaDir + Game.Default.MeshPea);
            p_Obj_Peashooter.Set_Transform( 0, 2.1F, 0,
                                            0.06F, 0.06F, 0.06F,
                                            0, PI, 0);

            p_Obj_Patatapum = t_Objeto3D.CrearObjeto3D(MediaDir + Game.Default.MeshPatatapum);
            p_Obj_Patatapum.Set_Transform(  0, -5.9F, 0,
                                            0.15F, 0.15F, 0.15F,
                                            0, PI, 0);
        }










        /******************************************************************************************/
        /*                                      UPDATES
        /******************************************************************************************/
        private void p_Func_Plantas_Update_PosMouse2DToPlanta3D()
        {
            p_Pos_PlantaActual = new Vector3(Input.Ypos / P_HEIGHT * 110 - 40, 0, Input.Xpos / P_WIDTH * 150 - 75);
        }

        private void p_Func_Plantas_Update_CreatePlantaAndSelect(p_s_HUDPlanta PlantaBox, t_Objeto3D PlantaObj)
        {
            _Mesh_BoxPicked = PlantaBox.Mesh_box;
            PlantaObj.Inst_CreateAndSelect();
            PlantaObj.Inst_Set_PositionX(p_Pos_PlantaActual.X);
            PlantaObj.Inst_Set_PositionZ(p_Pos_PlantaActual.Z);
        }










        /******************************************************************************************/
        /*                                      RENDERIZACION
        /******************************************************************************************/
        private void p_Func_Plantas_Render()
        {
            p_Obj_Girasol.Render();
            p_Obj_Peashooter.Render();
            p_Obj_Patatapum.Render();
        }
    }
}
