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
        private TgcBox _Mesh_BoxPicked;
        private TgcBox _Mesh_BoxPickedPrev;










        /******************************************************************************************/
        /*                 INIT - Se ejecuta una vez sola al comienzo
        /******************************************************************************************/

        private void pablo_init()
        {
            p_Func_Camara_Init();
            p_Func_Colision_Init();
            p_Func_Escenario_Init();
            p_Func_HUD_Init();
            p_Func_Zombies_Init();
            p_Func_Plantas_Init();
            p_Func_Soles_Init();
        }










        /******************************************************************************************/
        /*                 UPDATE - Realiza la lógica del juego
        /******************************************************************************************/

        private void pablo_update()
        {
            p_Func_Soles_Update_Rotation();

            if (Input.keyPressed(Key.H))
            {
                _camara.Modo_Change();
            }

            if (_camara.Modo_Is_CamaraAerea())
            {
                if (_mouse.ClickIzq_Down())
                {
                    p_Func_Plantas_Update_PosMouse2DToPlanta3D();
                }
                if (_mouse.ClickIzq_Up())
                {
                    _Mesh_BoxPickedPrev = _Mesh_BoxPicked;
                    _Mesh_BoxPicked = null;

                    p_Func_HUD_Update_BoxesTextures();
                }

                if (_mouse.ClickIzq_RisingDown())
                {
                    _Mesh_BoxPicked = null;
                    _Mesh_BoxPickedPrev = null;

                    if (_colision.MouseBox(p_HUDPlanta_Patatapum.Mesh_box))
                    {
                        p_Func_Plantas_Update_CreatePlantaAndSelect(p_HUDPlanta_Patatapum, p_Obj_Patatapum);
                    }
                    else if (_colision.MouseBox(p_HUDPlanta_Peashooter.Mesh_box))
                    {
                        p_Func_Plantas_Update_CreatePlantaAndSelect(p_HUDPlanta_Peashooter, p_Obj_Peashooter);
                    }
                    else if (_colision.MouseBox(p_HUDPlanta_Girasol.Mesh_box))
                    {
                        p_Func_Plantas_Update_CreatePlantaAndSelect(p_HUDPlanta_Girasol, p_Obj_Girasol);
                    }
                }
            }
        }










        /******************************************************************************************/
        /*                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
        /******************************************************************************************/

        private void pablo_render()
        {
            p_Func_Escenario_Render();
            p_Func_Plantas_Render();
            p_Func_Zombies_Render();
            p_Func_Soles_Render();

            if (_camara.Modo_Is_CamaraAerea())
            {
                Func_Text("H Para cambiar a Camara Primera Persona", 10, 80);

                p_Func_HUD_Render();

                if (_Mesh_BoxPicked == null)
                {
                    _Mesh_BoxPickedPrev = null;
                }
                else if (_Mesh_BoxPicked == p_HUDPlanta_Girasol.Mesh_box)
                {
                    p_Obj_Girasol.Inst_Set_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Girasol.Inst_Set_PositionZ(p_Pos_PlantaActual.Z);
                    p_Obj_Girasol.Render();
                }
                else if (_Mesh_BoxPicked == p_HUDPlanta_Peashooter.Mesh_box)
                {
                    p_Obj_Peashooter.Inst_Set_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Peashooter.Inst_Set_PositionZ(p_Pos_PlantaActual.Z);
                    p_Obj_Peashooter.Render();
                }
                else if (_Mesh_BoxPicked == p_HUDPlanta_Patatapum.Mesh_box)
                {
                    p_Obj_Patatapum.Inst_Set_PositionX(p_Pos_PlantaActual.X);
                    p_Obj_Patatapum.Inst_Set_PositionZ(p_Pos_PlantaActual.Z);
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
