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
        private List<TgcMesh> j_meshCasa { get; set; }
        private List<TgcMesh> j_meshBrain { get; set; }
        private List<TgcMesh> j_meshPea { get; set; }


        private float posX = 15;
        private float posY = -10;
        private float posZ = 35;
        private const float RAZON_PASTO = 7.5F;
        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void jose_init()
        {
            initSkyBox();

            var PathBrain= MediaDir + Game.Default.MeshBrain;
            j_meshBrain = new TgcSceneLoader().loadSceneFromFile(PathBrain).Meshes;

            var PathPea = MediaDir + Game.Default.MeshPea;
            j_meshPea = new TgcSceneLoader().loadSceneFromFile(PathPea).Meshes;

            for (int i = 0; i < j_meshPea.Count; i++)
            {
                j_meshPea[i].Position = new Vector3((float)15, (float)0, (float)0);
                j_meshPea[i].Scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
            }

            for (int i = 0; i < j_meshBrain.Count; i++)
            {
                j_meshBrain[i].Position = new Vector3((float)15, (float)0, (float)-35);
                j_meshBrain[i].Scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
            }

            var PathCasa = MediaDir + Game.Default.CasitaDave;
            j_meshCasa = new TgcSceneLoader().loadSceneFromFile(PathCasa).Meshes;

            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].Position = new Vector3((float)0, (float)0, (float)-75);
                j_meshCasa[i].Scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
                j_meshCasa[i].rotateY(PI/2);
            }
            
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void jose_update()
        {
            for (int i = 0; i < j_meshBrain.Count; i++)
            {
                
                j_meshBrain[i].rotateY(0.01F * PI);
            }


        }










        /******************************************************************************************
         *                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
         ******************************************************************************************/

        private void jose_render()
        {

            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].render();
            }
            for (int i = 0; i < j_meshPea.Count; i++)
            {
                j_meshPea[i].render();
            }
            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < j_meshBrain.Count; i++)
                {
                    j_meshBrain[i].render();
                    j_meshBrain[i].move(-RAZON_PASTO, 0,0);
                }

            }
            j_meshBrain[0].move(RAZON_PASTO * 5, 0, 0);
            j_meshBrain[1].move(RAZON_PASTO * 5, 0, 0);

            skyBox.render();




        }
        /******************************************************************************************
         *                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
         ******************************************************************************************/

        private void jose_dispose()
        {
            for (int i = 0; i < j_meshPea.Count; i++)
            {

                j_meshPea[i].dispose();
            }
            for (int i = 0; i < j_meshBrain.Count; i++)
            {

                j_meshBrain[i].dispose();
            }
            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].dispose();

            }
            skyBox.dispose();

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
            for (int cVertical = 0; cVertical < 10; cVertical++, posZ -= RAZON_PASTO)
            {
                posX = 15;
                for (int j = 0; j < 5; j++, posX -= RAZON_PASTO)
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

