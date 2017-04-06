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
         *                 CONSTANTES - Deben comenzar con "p_"
         ******************************************************************************************/

        //      SCREEN
        private const float P_WIDTH = 1360;
        private const float P_HEIGHT = 768;







        /******************************************************************************************
         *                 VARIABLES - Deben comenzar con "p_"
         ******************************************************************************************/
            












        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void pablo_init()
        {
            p_Func_Init_Escenario();
            p_Func_Init_Zombies();
            p_Func_Init_Plantas();
            p_Func_Init_HUD();
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void pablo_update()
        {
            if (Is_CamPicado)
            {
                if (Input.buttonDown(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    p_Pos_PlantaActual = new Vector3(Input.Ypos / P_HEIGHT * 100 - 50, 0, Input.Xpos / P_WIDTH * 100 - 50);
                }

                if (Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    if (!Func_IsMeshPicked(p_Mesh_HUDPlant3))
                    {
                        if (!Func_IsMeshPicked(p_Mesh_HUDPlant2))
                        {
                            if (!Func_IsMeshPicked(p_Mesh_HUDPlant1))
                            {
                            }
                        }
                    }
                }

                if (Input.buttonUp(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    Mesh_BoxPickedPrev = Mesh_BoxPicked;
                    Mesh_BoxPicked = Mesh_BoxCollision;
                }
            }
        }










        /******************************************************************************************
         *                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
         ******************************************************************************************/

        private void pablo_render()
        {
            p_Func_Render_Escenario();
            p_Func_Render_Plantas();
            p_Func_Render_Zombies();

            if (Is_CamPicado)
            {
                Func_Text("H Para cambiar a Camara Primera Persona", 10, 80);

                p_Func_Render_HUD();

                if (Mesh_BoxPicked == Mesh_BoxCollision)
                {
                    if (Mesh_BoxPickedPrev == p_Mesh_HUDPlant1)
                    {
                        p_Pos_Girasol.Add(p_Pos_PlantaActual);
                    }

                    Mesh_BoxPickedPrev = Mesh_BoxCollision;
                }
                else if (Mesh_BoxPicked == p_Mesh_HUDPlant1)
                {
                    Func_MeshesPos(p_Meshes_Girasol, p_Pos_PlantaActual.X, p_Pos_PlantaActual.Y, p_Pos_PlantaActual.Z);
                    Func_MeshesRender(p_Meshes_Girasol);
                }
                else if (Mesh_BoxPicked == p_Mesh_HUDPlant2)
                {
                }
                else if (Mesh_BoxPicked == p_Mesh_HUDPlant3)
                {
                }
            }
            else
            {
                Func_Text("H Para cambiar a Camara Aérea", 10, 80);
            }
        }










        /******************************************************************************************
         *                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
         ******************************************************************************************/

        private void pablo_dispose()
        {
            //Dispose de los meshes
            p_Mesh_plano.dispose();
            p_Mesh_zombie.dispose();
            p_Mesh_HUDPlant1.dispose();
            p_Mesh_HUDPlant2.dispose();
            p_Mesh_HUDPlant3.dispose();
            p_Mesh_mountain.dispose();

            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].dispose();
            }

            for (int i = 0; i < p_Meshes_Mina.Count; i++)
            {
                p_Meshes_Mina[i].dispose();
            }
        }
    }
}
