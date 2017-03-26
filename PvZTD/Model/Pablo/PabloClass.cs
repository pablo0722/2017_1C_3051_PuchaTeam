using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Core.SkeletalAnimation;

namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************
         *                 CONSTANTES - Deben comenzar con "p_"
         ******************************************************************************************/

        private const float vel = 10;










        /******************************************************************************************
         *                 VARIABLES - Deben comenzar con "p_"
         ******************************************************************************************/

        //      CAMARAS
        // Camara con movimiento
        private MyCamara camaraInterna;

        //      MESHES
        private TgcMesh p_Mesh { get; set; }        // Bola Raziel
        private TgcMesh p_MeshCielo { get; set; }   // TgcLogo

        private System.Collections.Generic.List<TgcMesh> p_MeshGirasol { get; set; } // Girasol
        private System.Collections.Generic.List<TgcMesh> p_MeshMina { get; set; }    // Mina

        //      OBJETOS
        private TgcBox p_Box { get; set; }          //Caja que se muestra en el ejemplo












        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void pablo_init()
        {
            //*
            // Cargar una textura
            var pathTexturaCaja = MediaDir + Game.Default.TexturaCaja;
            var texture = TgcTexture.createTexture(pathTexturaCaja);

            //Creamos una caja 3D ubicada de dimensiones (5, 10, 5) y la textura como color.
            var size = new Vector3(5, 10, 5);
            var pos = new Vector3(25, 0, 0);
            //Construimos una caja según los parámetros, por defecto la misma se crea con centro en el origen y se recomienda así para facilitar las transformaciones.
            p_Box = TgcBox.fromSize(pos, size, texture);
            

            //Cargo el unico mesh que tiene la escena.
            var pathMeshTgc = MediaDir + Game.Default.MeshRaziel;
            p_Mesh = new TgcSceneLoader().loadSceneFromFile(pathMeshTgc).Meshes[0];
            p_Mesh.Scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
            p_Mesh.move(-2, 7, 0);

            var PathMeshCielo = MediaDir + Game.Default.MeshCielo;
            p_MeshCielo = new TgcSceneLoader().loadSceneFromFile(PathMeshCielo).Meshes[0];
            p_MeshCielo.Scale = new Vector3((float) 1, (float) 1, (float) 1);
            p_MeshCielo.rotateZ((float) -PI/2);

            var PathMeshGirasol = MediaDir + Game.Default.MeshGirasol;
            p_MeshGirasol = new TgcSceneLoader().loadSceneFromFile(PathMeshGirasol).Meshes;
            for (int i = 0; i < p_MeshGirasol.Count; i++)
            {
                p_MeshGirasol[i].Scale = new Vector3((float) 0.05, (float) 0.05, (float) 0.05);
                p_MeshGirasol[i].rotateY((float)PI);
                p_MeshGirasol[i].Position = new Vector3((float)10, (float)0, (float)-50);
            }

            var PathMeshMina = MediaDir + Game.Default.MeshMina;
            p_MeshMina = new TgcSceneLoader().loadSceneFromFile(PathMeshMina).Meshes;
            for(int i=0; i<p_MeshMina.Count; i++)
            {
                p_MeshMina[i].Scale = new Vector3((float)0.15, (float)0.15, (float)0.15);
                p_MeshMina[i].rotateY((float)PI);
                p_MeshMina[i].Position = new Vector3((float)-10, (float)0, (float)-50);
            }


            //Camara
            camaraInterna = new MyCamara(p_Mesh.Position, (float)10, (float)50);
            Camara = camaraInterna;
            //*/
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void pablo_update()
        {
            //*
            p_Mesh.rotateY((float) 0.01);

            if (Input.keyDown(Key.W))
            {
                p_Mesh.move(0, 0, -vel * ElapsedTime);

                camaraInterna.Target = p_Mesh.Position;
            }

            if (Input.keyDown(Key.S))
            {
                p_Mesh.move(0, 0, vel * ElapsedTime);

                camaraInterna.Target = p_Mesh.Position;
            }

            p_MeshCielo.Position = Camara.Position;

            p_MeshCielo.rotateY((float) 0.00005);
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
            p_Box.Transform = Matrix.Scaling(p_Box.Scale) *
                            Matrix.RotationYawPitchRoll(p_Box.Rotation.Y, p_Box.Rotation.X, p_Box.Rotation.Z) *
                            Matrix.Translation(p_Box.Position);

            //A modo ejemplo realizamos toda las multiplicaciones, pero aquí solo nos hacia falta la traslación.
            //Finalmente invocamos al render de la caja
            p_Box.render();

            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            p_Mesh.UpdateMeshTransform();
            //Render del mesh
            p_Mesh.render();

            p_MeshCielo.UpdateMeshTransform();
            p_MeshCielo.render();

            for (int i = 0; i < p_MeshGirasol.Count; i++)
            {
                p_MeshGirasol[i].UpdateMeshTransform();
                p_MeshGirasol[i].render();
            }

            for (int i = 0; i < p_MeshMina.Count; i++)
            {
                p_MeshMina[i].UpdateMeshTransform();
                p_MeshMina[i].render();
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
            //Dispose de la caja.
            p_Box.dispose();

            //Dispose del mesh.
            p_Mesh.dispose();
            //Dispose del mesh.

            p_MeshCielo.dispose();

            for (int i = 0; i < p_MeshGirasol.Count; i++)
            {
                p_MeshGirasol[i].dispose();
            }

            for (int i = 0; i < p_MeshMina.Count; i++)
            {
                p_MeshMina[i].dispose();
            }
            
            //*/
        }
    }
}
