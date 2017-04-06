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

        //      MESHES VARIOS
        private const float P_BOLA_RAZIEL_ROT = 1;
        private const float P_CIELO_ROT = (float)0.005;

        //      HUD
        private const float P_BOX_HUD_SIZE = 5;
        private const float P_BOX_HUD_POS_X = 20;
        private const float P_BOX_HUD_POS_Y = 62;
        private const float P_BOX_HUD_POS_Z = -55;
        private const float P_BOX_HUD_ROT = PI * (float)0.17;

        //      CAMARAS
        // Camara Libre
        private const float P_CAM_POS_X_INIT = 20;
        private const float P_CAM_POS_Y_INIT = 20;
        private const float P_CAM_POS_Z_INIT = 0;
        private const float P_CAM_VEL = 50;
        private const float P_CAM_JUMP = 50;
        private const float P_CAM_ROT = (float)0.02;

        // Camara Plano Picado
        private const float P_CAM_PICADO_X = 100;
        private const float P_CAM_PICADO_Y = 100;
        private const float P_CAM_PICADO_Z = 0;
        private const float P_CAM_UP_X = -1;
        private const float P_CAM_UP_Y = 1;
        private const float P_CAM_UP_Z = 0;







        /******************************************************************************************
         *                 VARIABLES - Deben comenzar con "p_"
         ******************************************************************************************/

        //      CAMARAS
        // Camara Libre
        private MyCamara1Persona p_Camara1Persona;
        // Camara Plano Picado
        private Core.Camara.TgcCamera p_CamaraPicado;

        //      MESHES
        private TgcMesh p_Mesh_BolaRaziel { get; set; }         // Bola Raziel
        private TgcMesh p_Mesh_Cielo { get; set; }              // TgcLogo
        private TgcMesh p_Mesh_plano { get; set; }              // Escenario
        private TgcMesh p_Mesh_zombie { get; set; }             // Zombie
        private TgcMesh p_Mesh_mountain { get; set; }           // Mountain

        private TgcBox p_Mesh_BoxCollision;                    // Punto rojo colision
        private TgcBox p_Mesh_HUDPlant1 { get; set; }          // Caja Planta HUD 1
        private TgcBox p_Mesh_HUDPlant2 { get; set; }          // Caja Planta HUD 2
        private TgcBox p_Mesh_HUDPlant3 { get; set; }          // Caja Planta HUD 3

        private List<TgcMesh> p_Meshes_Girasol { get; set; }    // Girasol (Lista)
        private List<TgcMesh> p_Meshes_Mina { get; set; }       // Mina (Lista)

        //      POSICION DE LOS MESHES
        private Vector3 p_Pos_PlantaActual { get; set; }       // Posicion Planta Actual
        private List<Vector3> p_Pos_Girasol { get; set; }       // Posicion Girasol

        //      RAY PICKING
        private TgcPickingRay p_PickingRay;

        //      OTRAS VARIABLES
        private bool p_Is_CamPicado = false;
        private TgcBox p_Mesh_BoxPicked;
        private TgcBox p_Mesh_BoxPickedPrev;
        private Vector3 p_PickRay_Pos;












        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void pablo_init()
        {
            //*

            //          PICKING RAY
            p_PickingRay = new TgcPickingRay(Input);
            p_Mesh_BoxCollision = TgcBox.fromSize(new Vector3((float)0.5, (float)0.5, (float)0.5), Color.Red);
            p_Mesh_BoxCollision.AutoTransformEnable = true;

            //          MESHES
            var texture = TgcTexture.createTexture(MediaDir + Game.Default.TexturaHUDGirasol);

            var size = new Vector3(P_BOX_HUD_SIZE, P_BOX_HUD_SIZE, P_BOX_HUD_SIZE);
            p_Mesh_HUDPlant1 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z), size, texture);
            p_Mesh_HUDPlant1.rotateZ(P_BOX_HUD_ROT);
            p_Mesh_HUDPlant2 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z + P_BOX_HUD_SIZE), size, texture);
            p_Mesh_HUDPlant2.rotateZ(P_BOX_HUD_ROT);
            p_Mesh_HUDPlant3 = TgcBox.fromSize(new Vector3(P_BOX_HUD_POS_X, P_BOX_HUD_POS_Y, P_BOX_HUD_POS_Z + P_BOX_HUD_SIZE * 2), size, texture);
            p_Mesh_HUDPlant3.rotateZ(P_BOX_HUD_ROT);
            
            var pathMeshTgc = MediaDir + Game.Default.MeshRaziel;
            p_Mesh_BolaRaziel = new TgcSceneLoader().loadSceneFromFile(pathMeshTgc).Meshes[0];
            p_Mesh_BolaRaziel.Scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
            p_Mesh_BolaRaziel.move(0, 12, -3);
            
            //p_Mesh_mountain = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshMountain).Meshes[0];
            //p_Mesh_mountain.rotateX(PI / 2);
            //p_Mesh_mountain.Scale = new Vector3(20, 20, 20);

            var PathMeshCielo = MediaDir + Game.Default.MeshCielo;
            p_Mesh_Cielo = new TgcSceneLoader().loadSceneFromFile(PathMeshCielo).Meshes[0];
            p_Mesh_Cielo.rotateZ((float)-PI / 2);

            var PathMeshPlano = MediaDir + Game.Default.MeshPlano;
            p_Mesh_plano = new TgcSceneLoader().loadSceneFromFile(PathMeshPlano).Meshes[0];

            var PathMeshZombie = MediaDir + Game.Default.MeshZombie;
            p_Mesh_zombie = new TgcSceneLoader().loadSceneFromFile(PathMeshZombie).Meshes[0];
            p_Mesh_zombie.Scale = new Vector3((float)0.25, (float)0.25, (float)0.25);

            p_Meshes_Girasol = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshGirasol).Meshes;
            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].Scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
                p_Meshes_Girasol[i].rotateY((float)PI);
                p_Meshes_Girasol[i].Position = new Vector3((float)10, (float)0, (float)-50);
            }

            var PathMeshMina = MediaDir + Game.Default.MeshMina;
            p_Meshes_Mina = new TgcSceneLoader().loadSceneFromFile(PathMeshMina).Meshes;
            for (int i = 0; i < p_Meshes_Mina.Count; i++)
            {
                p_Meshes_Mina[i].Scale = new Vector3((float)0.15, (float)0.15, (float)0.15);
                p_Meshes_Mina[i].rotateY((float)PI);
                p_Meshes_Mina[i].Position = new Vector3(-10, 0, -50);
            }

            p_Pos_Girasol = new List<Vector3>();


            //          CAMARAS
            // Camara Picado
            p_CamaraPicado = new Core.Camara.TgcCamera();
            p_CamaraPicado.SetCamera(new Vector3(P_CAM_PICADO_X, P_CAM_PICADO_Y, P_CAM_PICADO_Z),
                Vector3.Empty, new Vector3(P_CAM_UP_X, P_CAM_UP_Y, P_CAM_UP_Z));

            // Camara Primera Persona
            p_Camara1Persona = new MyCamara1Persona(new Vector3(P_CAM_POS_X_INIT, P_CAM_POS_Y_INIT, P_CAM_POS_Z_INIT),
                P_CAM_VEL, P_CAM_JUMP, P_CAM_ROT, Input);
            Camara = p_Camara1Persona;
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void pablo_update()
        {
            //*
            p_Func_CamUpdate();

            p_Mesh_BolaRaziel.rotateY(P_BOLA_RAZIEL_ROT * ElapsedTime);

            p_Mesh_Cielo.Position = Camara.Position;
            p_Mesh_Cielo.rotateY(P_CIELO_ROT * ElapsedTime);

            if (p_Is_CamPicado)
            {
                if (Input.buttonDown(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    p_Pos_PlantaActual = new Vector3(Input.Ypos / P_HEIGHT * 100 - 50, 0, Input.Xpos / P_WIDTH * 100 - 50);
                }

                if (Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    if (!p_Func_IsMeshPicked(p_Mesh_HUDPlant3))
                    {
                        if (!p_Func_IsMeshPicked(p_Mesh_HUDPlant2))
                        {
                            if (!p_Func_IsMeshPicked(p_Mesh_HUDPlant1))
                            {
                            }
                        }
                    }
                }

                if (Input.buttonUp(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    p_Mesh_BoxPickedPrev = p_Mesh_BoxPicked;
                    p_Mesh_BoxPicked = p_Mesh_BoxCollision;
                }
            }
            //*/
        }










        /******************************************************************************************
         *                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
         ******************************************************************************************/

        private void pablo_render()
        {
            //*
            for (int i = 0; i < p_Pos_Girasol.Count; i++)
            {
                p_Func_MeshesPos(p_Meshes_Girasol, p_Pos_Girasol[i].X, p_Pos_Girasol[i].Y, p_Pos_Girasol[i].Z);
                p_Func_MeshesRender(p_Meshes_Girasol);
            }
            
            if (p_Is_CamPicado)
            {
                p_Func_Text("H Para cambiar a Camara Primera Persona", 10, 80);

                p_Func_BoxRender(p_Mesh_HUDPlant1);
                p_Func_BoxRender(p_Mesh_HUDPlant2);
                p_Func_BoxRender(p_Mesh_HUDPlant3);

                if(p_Mesh_BoxPicked == p_Mesh_BoxCollision)
                {
                    if (p_Mesh_BoxPickedPrev == p_Mesh_HUDPlant1)
                    {
                        p_Pos_Girasol.Add(p_Pos_PlantaActual);
                    }

                    p_Mesh_BoxPickedPrev = p_Mesh_BoxCollision;
                }
                else if (p_Mesh_BoxPicked == p_Mesh_HUDPlant1)
                {
                    p_Func_MeshesPos(p_Meshes_Girasol, p_Pos_PlantaActual.X, p_Pos_PlantaActual.Y, p_Pos_PlantaActual.Z);
                    p_Func_MeshesRender(p_Meshes_Girasol);
                }
                else if (p_Mesh_BoxPicked == p_Mesh_HUDPlant2)
                {
                }
                else if (p_Mesh_BoxPicked == p_Mesh_HUDPlant3)
                {
                }
            }
            else
            {
                p_Func_Text("H Para cambiar a Camara Aérea", 10, 80);
            }

            p_Func_MeshRender(p_Mesh_BolaRaziel);
            p_Func_MeshRender(p_Mesh_plano);
            //p_Func_MeshRender(p_Mesh_Cielo);
            p_Func_MeshRender(p_Mesh_zombie);
            //p_Func_MeshRender(p_Mesh_mountain);


            p_Func_MeshesRender(p_Meshes_Mina);
        }










        /******************************************************************************************
         *                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
         ******************************************************************************************/

        private void pablo_dispose()
        {
            //*

            //Dispose de los meshes
            p_Mesh_BolaRaziel.dispose();
            p_Mesh_Cielo.dispose();
            p_Mesh_plano.dispose();
            p_Mesh_zombie.dispose();
            p_Mesh_BoxCollision.dispose();
            p_Mesh_HUDPlant1.dispose();
            p_Mesh_HUDPlant2.dispose();
            p_Mesh_HUDPlant3.dispose();

            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].dispose();
            }

            for (int i = 0; i < p_Meshes_Mina.Count; i++)
            {
                p_Meshes_Mina[i].dispose();
            }

            //*/
        }










        /******************************************************************************************
         *                                  FUNCIONES AUXILIARES
         ******************************************************************************************/

        private bool p_Func_IsMeshPicked(TgcBox mesh)
        {
            //Actualizar Ray de colision en base a posicion del mouse
            p_PickingRay.updateRay();

            p_Mesh_BoxPicked = p_Mesh_BoxCollision;
            p_Mesh_BoxPickedPrev = p_Mesh_BoxCollision;

            var aabb = mesh.BoundingBox;

            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
            var selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(p_PickingRay.Ray, aabb, out p_PickRay_Pos);
            if (selected)
            {
                p_Mesh_BoxPicked = mesh;

                return true;
            }

            return false;
        }

        private void p_Func_CamUpdate()
        {
            if (!p_Is_CamPicado)
                p_Camara1Persona.UpdateCamera(ElapsedTime);

            if (Input.keyPressed(Key.H))
            {
                p_Is_CamPicado = !p_Is_CamPicado;

                if (p_Is_CamPicado)
                {
                    Camara = p_CamaraPicado;
                }
                else
                {
                    Camara = p_Camara1Persona;
                }
            }
        }

        private void p_Func_BoxRender(TgcBox box)
        {
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            box.Transform = Matrix.Scaling(box.Scale) *
                            Matrix.RotationYawPitchRoll(box.Rotation.Y, box.Rotation.X, box.Rotation.Z) *
                            Matrix.Translation(box.Position);

            box.render();
        }

        private void p_Func_MeshRender(TgcMesh mesh)
        {
            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            mesh.UpdateMeshTransform();
            mesh.render();
        }

        private void p_Func_MeshesRender(List<TgcMesh> meshes)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
                //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
                meshes[i].UpdateMeshTransform();
                meshes[i].render();
            }
        }
        private void p_Func_MeshesPos(List<TgcMesh> meshes, float X, float Y, float Z)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                p_Meshes_Girasol[i].Position = new Vector3(X, Y, Z);
            }
        }

        private void p_Func_Text(string text, int x, int y)
        {
            DrawText.drawText(text, x, y, Color.White);
            DrawText.drawText(text, x, y+10, Color.Black);
        }
    }
}
