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
         *                                      ESTRUCTURA
         ******************************************************************************************/
        private struct p_s_planta
        {
            public List<TgcMesh> meshes { get; set; }    // Girasol (Lista)
            private List<Vector3> posicion { get; set; }       // Posicion Girasol
        }










        /******************************************************************************************
         *                                      VARIABLES
         ******************************************************************************************/
        private List<TgcMesh> p_Meshes_Girasol { get; set; }    // Girasol (Lista)
        private List<TgcMesh> p_Meshes_Patatapum { get; set; }       // Mina (Lista)

        //      Posicion de las plantas
        private Vector3 p_Pos_PlantaActual { get; set; }       // Posicion Planta Actual
        private List<Vector3> p_Pos_Patatapum { get; set; }       // Posicion Girasol
        private List<Vector3> p_Pos_Girasol { get; set; }       // Posicion Girasol










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Plantas()
        {
            // Meshes
            p_Meshes_Girasol = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshGirasol).Meshes;
            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].Scale = new Vector3((float)0.05, (float)0.05, (float)0.05);
                p_Meshes_Girasol[i].rotateY((float)PI);
                p_Meshes_Girasol[i].Position = new Vector3((float)10, (float)0, (float)-50);
            }

            p_Meshes_Patatapum = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshMina).Meshes;
            for (int i = 0; i < p_Meshes_Patatapum.Count; i++)
            {
                p_Meshes_Patatapum[i].Scale = new Vector3((float)0.15, (float)0.15, (float)0.15);
                p_Meshes_Patatapum[i].rotateY((float)PI);
                p_Meshes_Patatapum[i].Position = new Vector3(-10, 0, -50);
            }

            // Posiciones
            p_Pos_Girasol = new List<Vector3>();
            p_Pos_Patatapum = new List<Vector3>();
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Plantas()
        {
            // Girasol
            for (int i = 0; i < p_Pos_Girasol.Count; i++)
            {
                Func_MeshesPos(p_Meshes_Girasol, p_Pos_Girasol[i].X, p_Pos_Girasol[i].Y, p_Pos_Girasol[i].Z);
                Func_MeshesRender(p_Meshes_Girasol);
            }

            // Patatapum
            for (int i = 0; i < p_Pos_Patatapum.Count; i++)
            {
                Func_MeshesPos(p_Meshes_Patatapum, p_Pos_Patatapum[i].X, p_Pos_Patatapum[i].Y, p_Pos_Patatapum[i].Z);
                Func_MeshesRender(p_Meshes_Patatapum);
            }
        }










        /******************************************************************************************
         *                                      DISPOSE
         ******************************************************************************************/
        private void p_Func_Dispose_Plantas()
        {
            for (int i = 0; i < p_Meshes_Girasol.Count; i++)
            {
                p_Meshes_Girasol[i].dispose();
            }

            for (int i = 0; i < p_Meshes_Patatapum.Count; i++)
            {
                p_Meshes_Patatapum[i].dispose();
            }
        }
    }
}
