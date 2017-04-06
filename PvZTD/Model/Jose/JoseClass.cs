using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using System.Collections.Generic;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Core.Terrain;

namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************
         *                 VARIABLES - Deben comenzar con "j_"
         ******************************************************************************************/
        //      MESHES
        private List<TgcMesh> j_meshsPasto { get; set; }    // Suelo con pasto(Lista)
        private TgcMesh j_meshMountain { get; set; }
        private TgcSkyBox skyBox;




        private float posX = 15;
        private float posY = -10;
        private float posZ = 35;

        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void jose_init()
        {
            initSkyBox();
            initPasto();
            //   var PathMeshMountain = MediaDir + Game.Default.MeshMountain;
            //  j_meshMountain = new TgcSceneLoader().loadSceneFromFile(PathMeshMountain).Meshes[0];
            //  j_meshMountain.Position = new Vector3((float)45, (float)-30, (float)35);
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void jose_update()
        {




        }










        /******************************************************************************************
         *                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
         ******************************************************************************************/

        private void jose_render()
        {
            skyBox.render();
            //enderPasto();
            //j_meshMountain.UpdateMeshTransform();
            //j_meshMountain.render();




        }
        /******************************************************************************************
         *                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
         ******************************************************************************************/

        private void jose_dispose()
        {
            skyBox.dispose();
            //j_meshMountain.dispose();
            for (int i = 0; i < j_meshsPasto.Count; i++)
            {
                j_meshsPasto[i].dispose();
            }
        }

        private void initPasto()
        {
            var PathMeshsPasto = MediaDir + Game.Default.MeshPasto;
            j_meshsPasto = new TgcSceneLoader().loadSceneFromFile(PathMeshsPasto).Meshes;

            for (int i = 0; i < j_meshsPasto.Count; i++)
            {
                j_meshsPasto[i].Scale = new Vector3((float)1, (float)1, (float)1);
                j_meshsPasto[i].Position = new Vector3((float)posX, (float)posY, (float)posZ);
            }
        }
        private void renderPasto()
        {
            posZ = 35;
            for (int cVertical = 0; cVertical < 10; cVertical++, posZ -= 7.5F)
            {
                posX = 15;
                for (int j = 0; j < 5; j++, posX -= 7.5F)
                {
                    for (int i = 0; i < j_meshsPasto.Count; i++)
                    {
                        j_meshsPasto[i].UpdateMeshTransform();
                        j_meshsPasto[i].render();
                        j_meshsPasto[i].Position = new Vector3((float)posX, (float)posY, (float)posZ);
                    }
                }

            }
        }


        private void initSkyBox()
        {

            //Crear SkyBox
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 500, 0);
            skyBox.Size = new Vector3(8000, 8000, 8000);

            var texturesPath = MediaDir + Game.Default.TexturasSkyBox;

            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "up.tga");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "down.tga");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "left.tga");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "rigth.tga");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "front.tga");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "back.tga");

            skyBox.Init();
        }















    }
    }

