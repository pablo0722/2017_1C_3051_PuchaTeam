using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using System.Collections.Generic;
using TGC.Examples.Lights;


namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************
         *                 CONSTANTES - Deben comenzar con "p_"
         ******************************************************************************************/

        private const float p_CharVel = 10;
        private const float p_CharRot = 1;
        private const float p_CieloRot = (float)0.005;

        private const float p_CamPosXInit = 20;
        private const float p_CamPosYInit = 20;
        private const float p_CamPosZInit = 0;
        private const float p_CamVel = 50;
        private const float p_CamJump = 50;
        private const float p_CamRot = (float)0.02;







        /******************************************************************************************
         *                 VARIABLES - Deben comenzar con "p_"
         ******************************************************************************************/

        //      CAMARAS
        // Camara con movimiento
        private MyCamara1Persona p_CamaraInterna;

        //      MESHES
        private TgcMesh p_Mesh_BolaRaziel { get; set; }         // Bola Raziel
        private TgcMesh p_Mesh_Cielo { get; set; }              // TgcLogo
        private TgcMesh p_Mesh_plano { get; set; }              // Escenario
        private TgcMesh p_Mesh_zombie { get; set; }             // Zombie
        private TgcBox  p_Mesh_BoxCollision;                    // Punto rojo colision
        private TgcBox  p_Mesh_BoxHUD1 { get; set; }            // Caja HUD 1
        private TgcBox  p_Mesh_BoxHUD2 { get; set; }            // Caja HUD 2
        private TgcBox  p_Mesh_BoxHUD3 { get; set; }            // Caja HUD 3

        private List<TgcMesh> p_Meshes_Girasol { get; set; }    // Girasol (Lista)
        private List<TgcMesh> p_Meshes_Mina { get; set; }       // Mina (Lista)

        //      ESPECIALES
        private TgcPickingRay p_PickingRay;

        //      OTRAS VARIABLES
        private bool p_is_CamPicado = false;
        private TgcBox  p_Mesh_BoxPicked;
        private Vector3 p_PickPos;












        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void pablo_init()
        {
            //*

            //Iniciarlizar PickingRay
            p_PickingRay = new TgcPickingRay(Input);
            p_Mesh_BoxCollision = TgcBox.fromSize(new Vector3((float)0.5, (float)0.5, (float)0.5), Color.Red);
            p_Mesh_BoxCollision.AutoTransformEnable = true;

            // Cargar una textura
            var pathTexturaCaja = MediaDir + Game.Default.TexturaCaja;
            var texture = TgcTexture.createTexture(pathTexturaCaja);

            //Creamos una caja 3D ubicada de dimensiones (5, 10, 5) y la textura como color.
            var size = new Vector3(5, 5, 5);
            //Construimos una caja según los parámetros, por defecto la misma se crea con centro en el origen y se recomienda así para facilitar las transformaciones.
            p_Mesh_BoxHUD1 = TgcBox.fromSize(new Vector3(-50, 50, 20), size, texture);
            p_Mesh_BoxHUD2 = TgcBox.fromSize(new Vector3(-60, 50, 20), size, texture);
            p_Mesh_BoxHUD3 = TgcBox.fromSize(new Vector3(-70, 50, 20), size, texture);

            //Cargo el unico mesh que tiene la escena.
            var pathMeshTgc = MediaDir + Game.Default.MeshRaziel;
            p_Mesh_BolaRaziel = new TgcSceneLoader().loadSceneFromFile(pathMeshTgc).Meshes[0];
            p_Mesh_BolaRaziel.Scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
            p_Mesh_BolaRaziel.move(0, 12, -3);

            var PathMeshCielo = MediaDir + Game.Default.MeshCielo;
            p_Mesh_Cielo = new TgcSceneLoader().loadSceneFromFile(PathMeshCielo).Meshes[0];
            p_Mesh_Cielo.rotateZ((float) -PI/2);

            var PathMeshPlano = MediaDir + Game.Default.MeshPlano;
            p_Mesh_plano = new TgcSceneLoader().loadSceneFromFile(PathMeshPlano).Meshes[0];

            var PathMeshZombie = MediaDir + Game.Default.MeshZombie;
            p_Mesh_zombie = new TgcSceneLoader().loadSceneFromFile(PathMeshZombie).Meshes[0];
            p_Mesh_zombie.Scale = new Vector3((float)0.25, (float)0.25, (float)0.25);

            var PathMeshGirasol = MediaDir + Game.Default.MeshGirasol;
            p_Meshes_Girasol = new TgcSceneLoader().loadSceneFromFile(PathMeshGirasol).Meshes;
            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].Scale = new Vector3((float) 0.05, (float) 0.05, (float) 0.05);
                p_Meshes_Girasol[i].rotateY((float)PI);
                p_Meshes_Girasol[i].Position = new Vector3((float)10, (float)0, (float)-50);
            }

            var PathMeshMina = MediaDir + Game.Default.MeshMina;
            p_Meshes_Mina = new TgcSceneLoader().loadSceneFromFile(PathMeshMina).Meshes;
            for(int i=0; i< p_Meshes_Mina.Count; i++)
            {
                p_Meshes_Mina[i].Scale = new Vector3((float)0.15, (float)0.15, (float)0.15);
                p_Meshes_Mina[i].rotateY((float)PI);
                p_Meshes_Mina[i].Position = new Vector3(-10, 0, -50);
            }


            //Camara
            p_CamaraInterna = new MyCamara1Persona(new Vector3(p_CamPosXInit, p_CamPosYInit, p_CamPosZInit), 
                p_CamVel, p_CamJump, p_CamRot, Input);
            Camara = p_CamaraInterna;
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void pablo_update()
        {
            //*
            if (!p_is_CamPicado)
                p_CamaraInterna.UpdateCamera(ElapsedTime);

            p_Mesh_BolaRaziel.rotateY(p_CharRot* ElapsedTime);
            p_Mesh_Cielo.Position = Camara.Position;
            p_Mesh_Cielo.rotateY(p_CieloRot * ElapsedTime);

            if (Input.keyPressed(Key.H))
            {
                p_is_CamPicado = !p_is_CamPicado;

                if (p_is_CamPicado)
                {
                    Camara = new Core.Camara.TgcCamera();
                    Camara.SetCamera(new Vector3(-100, 100, 0), Vector3.Empty, new Vector3(1, (float)1, 0));
                }
                else
                {
                    Camara = p_CamaraInterna;
                }
            }

            if (p_is_CamPicado)
            {
                if (Input.buttonPressed(TGC.Core.Input.TgcD3dInput.MouseButtons.BUTTON_LEFT))
                {
                    //Actualizar Ray de colision en base a posicion del mouse
                    p_PickingRay.updateRay();

                    p_Mesh_BoxPicked = p_Mesh_BoxCollision;

                    var aabb = p_Mesh_BoxHUD1.BoundingBox;
                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    var selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(p_PickingRay.Ray, aabb, out p_PickPos);
                    if (selected)
                    {
                        p_Mesh_BoxPicked = p_Mesh_BoxHUD1;
                    }
                    else
                    {

                        aabb = p_Mesh_BoxHUD2.BoundingBox;
                        //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                        selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(p_PickingRay.Ray, aabb, out p_PickPos);
                        if (selected)
                        {
                            p_Mesh_BoxPicked = p_Mesh_BoxHUD2;
                        }
                        else
                        {

                            aabb = p_Mesh_BoxHUD3.BoundingBox;
                            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                            selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(p_PickingRay.Ray, aabb, out p_PickPos);
                            if (selected)
                            {
                                p_Mesh_BoxPicked = p_Mesh_BoxHUD3;
                            }
                        }
                    }
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
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            p_Mesh_BoxHUD1.Transform = Matrix.Scaling(p_Mesh_BoxHUD1.Scale) *
                            Matrix.RotationYawPitchRoll(p_Mesh_BoxHUD1.Rotation.Y, p_Mesh_BoxHUD1.Rotation.X, p_Mesh_BoxHUD1.Rotation.Z) *
                            Matrix.Translation(p_Mesh_BoxHUD1.Position);

            p_Mesh_BoxHUD2.Transform = Matrix.Scaling(p_Mesh_BoxHUD2.Scale) *
                            Matrix.RotationYawPitchRoll(p_Mesh_BoxHUD2.Rotation.Y, p_Mesh_BoxHUD2.Rotation.X, p_Mesh_BoxHUD2.Rotation.Z) *
                            Matrix.Translation(p_Mesh_BoxHUD2.Position);

            p_Mesh_BoxHUD3.Transform = Matrix.Scaling(p_Mesh_BoxHUD3.Scale) *
                            Matrix.RotationYawPitchRoll(p_Mesh_BoxHUD3.Rotation.Y, p_Mesh_BoxHUD3.Rotation.X, p_Mesh_BoxHUD3.Rotation.Z) *
                            Matrix.Translation(p_Mesh_BoxHUD3.Position);

            if (p_is_CamPicado)
            {
                p_Mesh_BoxHUD1.render();
                p_Mesh_BoxHUD2.render();
                p_Mesh_BoxHUD3.render();

                if (p_Mesh_BoxPicked == p_Mesh_BoxHUD1)
                {
                    p_Mesh_BoxHUD1.BoundingBox.render();

                    //Dibujar caja que representa el punto de colision
                    p_Mesh_BoxCollision.render();

                    p_Mesh_BoxCollision.Position = p_PickPos;
                }
                else if (p_Mesh_BoxPicked == p_Mesh_BoxHUD2)
                {
                    p_Mesh_BoxHUD2.BoundingBox.render();

                    //Dibujar caja que representa el punto de colision
                    p_Mesh_BoxCollision.render();

                    p_Mesh_BoxCollision.Position = p_PickPos;
                }
                else if (p_Mesh_BoxPicked == p_Mesh_BoxHUD3)
                {
                    p_Mesh_BoxHUD3.BoundingBox.render();

                    //Dibujar caja que representa el punto de colision
                    p_Mesh_BoxCollision.render();

                    p_Mesh_BoxCollision.Position = p_PickPos;
                }
            }

            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            p_Mesh_BolaRaziel.UpdateMeshTransform();
            //Render del mesh
            p_Mesh_BolaRaziel.render();

            p_Mesh_plano.UpdateMeshTransform();
            p_Mesh_plano.render();

            p_Mesh_Cielo.UpdateMeshTransform();
            p_Mesh_Cielo.render();

            p_Mesh_zombie.UpdateMeshTransform();
            p_Mesh_zombie.render();

            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].UpdateMeshTransform();
                p_Meshes_Girasol[i].render();
            }

            for (int i = 0; i < p_Meshes_Mina.Count; i++)
            {
                p_Meshes_Mina[i].UpdateMeshTransform();
                p_Meshes_Mina[i].render();
            }


            //Render de BoundingBox, muy útil para debug de colisiones.
            //p_Box.BoundingBox.render();
            //p_Mesh.BoundingBox.render();
            //p_MeshCielo.BoundingBox.render();
            //*/
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
            p_Mesh_BoxHUD1.dispose();
            p_Mesh_BoxHUD2.dispose();
            p_Mesh_BoxHUD3.dispose();

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
    }
}
