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
        private float posY = -13;
        private float posZ = INICIAL_Z;
        private const float RAZON_PASTO = 13;
        private const float INICIAL_X = 50;
        private const float INICIAL_Z = -70;
        public t_Objeto3D valla { get; set; }
        /******************************************************************************************
         *                 INIT - Se ejecuta una vez sola al comienzo
         ******************************************************************************************/

        private void jose_init()
        {
            valla = t_Objeto3D.Crear(this, "..\\..\\Media\\Objetos\\valla-TgcScene.xml");
            valla.Set_Rotation(0, PI / 2, 0);
            valla.Set_Position(65, 0, 90);
            valla.Set_Size(0.5F, 0.5F, 0.5F);

            for (int mover = -20; mover > -180; mover -= 20)
            {
                valla.Inst_CreateAndSelect();
                valla.Inst_Move(0, 0, mover);

            }
            valla.Set_Rotation(0, -PI / 2, 0);
            valla.Set_Position(-50, 0, 90);
            for (int mover = -20; mover > -180; mover -= 20)
            {
                valla.Inst_CreateAndSelect();
                valla.Inst_Move(0, 0, mover);

            }
            valla.Set_Rotation(0, 0, 0);
            valla.Inst_CreateAndSelect();
            valla.Inst_Move(10, 0, -170);
            valla.Inst_CreateAndSelect();
            valla.Inst_Move(30, 0, -170);
            valla.Inst_CreateAndSelect();
            valla.Inst_Move(85, 0, -170);
            valla.Inst_CreateAndSelect();
            valla.Inst_Move(105, 0, -170);

            
            initSkyBox();
            initPasto();
            var PathBrain= MediaDir + Game.Default.MeshBrain;
            j_meshBrain = new TgcSceneLoader().loadSceneFromFile(PathBrain).Meshes;

            for (int i = 0; i < j_meshBrain.Count; i++)
            {
                j_meshBrain[i].Position = new Vector3((float)INICIAL_X, (float)5, (float)INICIAL_Z);
                j_meshBrain[i].Scale = new Vector3((float)0.03, (float)0.03, (float)0.03);
            }

            var PathCasa = MediaDir + Game.Default.CasitaDave;
            j_meshCasa = new TgcSceneLoader().loadSceneFromFile(PathCasa).Meshes;

            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].Position = new Vector3((float)7.5, (float)-7, (float)-115);
                j_meshCasa[i].Scale = new Vector3((float)0.5, (float)0.5, (float)0.5);
                j_meshCasa[i].rotateY(PI/2);
            }
            
        }










        /******************************************************************************************
         *                 UPDATE - Realiza la lógica del juego
         ******************************************************************************************/

        private void jose_update()
        {
            /*
            for (int i = 0; i < j_meshBrain.Count; i++)
            {
                
                j_meshBrain[i].rotateY(0.01F * PI);
            }
            */
        }










        /******************************************************************************************
         *                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
         ******************************************************************************************/

        private void jose_render()
        {
            valla.Render(true);
            skyBox.render();
            /*
            renderPasto();
            for (int i = 0; i < j_meshCasa.Count; i++) 
            {
                j_meshCasa[i].render();
            }

            for (int j = 0; j < 5; j++)
            {
                for (int i = 0; i < j_meshBrain.Count; i++)
                {
                    j_meshBrain[i].render();
                    j_meshBrain[i].move(-RAZON_PASTO*1.6F, 0,0);
                }
            }
            j_meshBrain[0].move(RAZON_PASTO * 1.6F * 5, 0, 0);
            j_meshBrain[1].move(RAZON_PASTO * 1.6F * 5, 0, 0);

            
            */
        }
        /******************************************************************************************
         *                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
         ******************************************************************************************/

        private void jose_dispose()
        {
            skyBox.dispose();
            /*
            for (int i = 0; i < j_meshBrain.Count; i++)
            {

                j_meshBrain[i].dispose();
            }
            for (int i = 0; i < j_meshCasa.Count; i++)
            {
                j_meshCasa[i].dispose();

            }

            j_meshsPastoClaro.dispose();
            j_meshsPastoMedio.dispose();
            j_meshsPastoOscuro.dispose();
            */
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

