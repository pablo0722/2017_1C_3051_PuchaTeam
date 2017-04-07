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
        private TgcMesh j_meshsPastoClaro { get; set; }
        private TgcMesh j_meshsPastoMedio { get; set; }
        private TgcMesh j_meshsPastoOscuro { get; set; }
        private TgcSkyBox skyBox;
        private List<TgcMesh> j_meshCasa { get; set; }
        private List<TgcMesh> j_meshBrain { get; set; }
        private List<TgcMesh> j_meshPea { get; set; }


        private float posX = INICIAL_X;
        private float posY = -10;
        private float posZ = INICIAL_Z;
        private const float RAZON_PASTO = 12;
        private const float INICIAL_X = 70;
        private const float INICIAL_Z = -90;
        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void jose_init()
        {
            initSkyBox();
            initPasto();
            var PathBrain= MediaDir + Game.Default.MeshBrain;
            j_meshBrain = new TgcSceneLoader().loadSceneFromFile(PathBrain).Meshes;

            var PathPea = MediaDir + Game.Default.MeshPea;
            j_meshPea = new TgcSceneLoader().loadSceneFromFile(PathPea).Meshes;

            for (int i = 0; i < j_meshPea.Count; i++)
            {
                j_meshPea[i].Position = new Vector3((float)15, (float)120, (float)0);
                j_meshPea[i].Scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
                j_meshPea[i].rotateY(PI);
            }

            for (int i = 0; i < j_meshBrain.Count; i++)
            {
                j_meshBrain[i].Position = new Vector3((float)15, (float)5, (float)-35);
                j_meshBrain[i].Scale = new Vector3((float)0.03, (float)0.03, (float)0.03);
            }

            var PathCasa = MediaDir + Game.Default.CasitaDave;
            j_meshCasa = new TgcSceneLoader().loadSceneFromFile(PathCasa).Meshes;

            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].Position = new Vector3((float)15, (float)-7, (float)-140);
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
            renderPasto();
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


            j_meshsPastoClaro.dispose();
            j_meshsPastoMedio.dispose();
            j_meshsPastoOscuro.dispose();

        }

        private void initPasto()
        {

            var PathPastoClaro = MediaDir + Game.Default.MeshPastoClaro;
            j_meshsPastoClaro = new TgcSceneLoader().loadSceneFromFile(PathPastoClaro).Meshes[0];
            j_meshsPastoClaro.Scale = new Vector3((RAZON_PASTO/10)*1.6F, (float)RAZON_PASTO/10, (float)RAZON_PASTO/10);

            var PathPastoMedio = MediaDir + Game.Default.MeshPastoMedio;
            j_meshsPastoMedio = new TgcSceneLoader().loadSceneFromFile(PathPastoMedio).Meshes[0];
            j_meshsPastoMedio.Scale = new Vector3((RAZON_PASTO / 10) * 1.6F, (float)RAZON_PASTO / 10, (float)RAZON_PASTO / 10);


            var PathPastoOscuro = MediaDir + Game.Default.MeshPastoOscuro;
            j_meshsPastoOscuro = new TgcSceneLoader().loadSceneFromFile(PathPastoOscuro).Meshes[0];
            j_meshsPastoOscuro.Scale = new Vector3((RAZON_PASTO / 10) * 1.6F, (float)RAZON_PASTO / 10, (float)RAZON_PASTO / 10);

        }

        private void renderPasto()
        {
            posZ = INICIAL_Z;
            for (int i = 0; i < 6 ; i++)
            {
                renderFilaMedioClara();
                posZ = posZ + RAZON_PASTO;
                renderFilaMedioOscura();
                posZ = posZ + RAZON_PASTO;
            }
        }

        private void renderFilaMedioClara()
        {
            j_meshsPastoMedio.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoMedio.render();
            posX = posX - (RAZON_PASTO*1.6F);
            j_meshsPastoClaro.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoClaro.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoMedio.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoMedio.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoClaro.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoClaro.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoMedio.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoMedio.render();
            posX = INICIAL_X;
        }
        private void renderFilaMedioOscura()
        {
            j_meshsPastoOscuro.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoOscuro.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoMedio.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoMedio.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoOscuro.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoOscuro.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoMedio.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoMedio.render();
            posX = posX - (RAZON_PASTO * 1.6F);
            j_meshsPastoOscuro.Position = new Vector3((float)posX, (float)posY, (float)posZ);
            j_meshsPastoOscuro.render();
            posX = INICIAL_X;
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

