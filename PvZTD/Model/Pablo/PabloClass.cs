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
        /*                 CONSTANTES - Deben comenzar con "p_"
        /******************************************************************************************/

        //      SCREEN
        private const float P_WIDTH = 1360;
        private const float P_HEIGHT = 768;







        /******************************************************************************************/
        /*                 VARIABLES - Deben comenzar con "p_"
        /******************************************************************************************/
            












        /******************************************************************************************/
        /*                 INIT - Se ejecuta una vez sola al comienzo
        /******************************************************************************************/

        private void pablo_init()
        {
            p_Func_Init_Escenario();
            p_Func_Init_Zombies();
            p_Func_Init_Plantas();
            p_Func_Init_HUD();
            p_Func_Init_Soles();
        }










        /******************************************************************************************/
        /*                 UPDATE - Realiza la lógica del juego
        /******************************************************************************************/

        private void pablo_update()
        {
            p_Func_RotarSoles();

            if (Is_CamPicado)
            {
                if (Input.buttonDown(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    p_Pos_PlantaActual = new Vector3(Input.Ypos / P_HEIGHT * 100 - 50, 0, Input.Xpos / P_WIDTH * 100 - 50);
                }
                else
                {
                    p_Func_Init_HUDBoxes();

                    if (Func_IsMeshPicked(p_HUDPlanta_Patatapum.Mesh_box))
                    {
                        p_Func_HUDBoxTexturaOn(ref p_HUDPlanta_Patatapum);
                    }
                    else if (Func_IsMeshPicked(p_HUDPlanta_Peashooter.Mesh_box))
                    {
                        p_Func_HUDBoxTexturaOn(ref p_HUDPlanta_Peashooter);
                    }
                    else if (Func_IsMeshPicked(p_HUDPlanta_Girasol.Mesh_box))
                    {
                        p_Func_HUDBoxTexturaOn(ref p_HUDPlanta_Girasol);
                    }
                }

                if (Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    Mesh_BoxPicked = Mesh_BoxCollision;
                    Mesh_BoxPickedPrev = Mesh_BoxCollision;

                    if (Func_IsMeshPicked(p_HUDPlanta_Patatapum.Mesh_box))
                    {
                        Mesh_BoxPicked = p_HUDPlanta_Patatapum.Mesh_box;
                        p_Obj_Patatapum.Inst_CreateAndSelect();
                        p_Obj_Patatapum.Inst_PositionX(p_Pos_PlantaActual.X);
                        p_Obj_Patatapum.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    }
                    else if (Func_IsMeshPicked(p_HUDPlanta_Peashooter.Mesh_box))
                    {
                        Mesh_BoxPicked = p_HUDPlanta_Peashooter.Mesh_box;
                        p_Obj_Peashooter.Inst_CreateAndSelect();
                        p_Obj_Peashooter.Inst_PositionX(p_Pos_PlantaActual.X);
                        p_Obj_Peashooter.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    }
                    else if (Func_IsMeshPicked(p_HUDPlanta_Girasol.Mesh_box))
                    {
                        Mesh_BoxPicked = p_HUDPlanta_Girasol.Mesh_box;
                        p_Obj_Girasol.Inst_CreateAndSelect();
                        p_Obj_Girasol.Inst_PositionX(p_Pos_PlantaActual.X);
                        p_Obj_Girasol.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    }
                }

                if (Input.buttonUp(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    p_Func_Init_HUDBoxes();

                    Mesh_BoxPickedPrev = Mesh_BoxPicked;
                    Mesh_BoxPicked = Mesh_BoxCollision;
                }
            }
        }










        /******************************************************************************************/
        /*                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
        /******************************************************************************************/

        private void pablo_render()
        {
            p_Func_Render_Escenario();
            p_Func_Render_Plantas();
            p_Func_Render_Zombies();
            p_Func_Render_Soles();

            if (Is_CamPicado)
            {
                Func_Text("H Para cambiar a Camara Primera Persona", 10, 80);

                p_Func_Render_HUD();

                if (Mesh_BoxPicked == Mesh_BoxCollision)
                {

                    Mesh_BoxPickedPrev = Mesh_BoxCollision;
                }
                else if (Mesh_BoxPicked == p_HUDPlanta_Girasol.Mesh_box)
                {
                    p_Obj_Girasol.Inst_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Girasol.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    p_Obj_Girasol.Render();
                }
                else if (Mesh_BoxPicked == p_HUDPlanta_Peashooter.Mesh_box)
                {
                    p_Obj_Peashooter.Inst_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Peashooter.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    p_Obj_Peashooter.Render();
                }
                else if (Mesh_BoxPicked == p_HUDPlanta_Patatapum.Mesh_box)
                {
                    p_Obj_Patatapum.Inst_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Patatapum.Inst_PositionZ(p_Pos_PlantaActual.Z);
                    p_Obj_Patatapum.Render();
                }
            }
            else
            {
                Func_Text("H Para cambiar a Camara Aérea", 10, 80);
            }
        }










        /******************************************************************************************/
        /*                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
        /******************************************************************************************/

        private void pablo_dispose()
        {
            p_Func_Dispose_HUD();
        }
    }
}
