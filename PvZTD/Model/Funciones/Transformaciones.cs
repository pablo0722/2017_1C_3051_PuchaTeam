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
         *                                  POSICION DE MESHES
         ******************************************************************************************/
        private void Func_MeshesPos(List<TgcMesh> meshes, float X, float Y, float Z)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].Position = new Vector3(X, Y, Z);
            }
        }










        /******************************************************************************************
         *                                  ESCALA DE MESHES
         ******************************************************************************************/
        private void Func_MeshesScale(List<TgcMesh> meshes, float X, float Y, float Z)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].Scale = new Vector3(X, Y, Z);
            }
        }










        /******************************************************************************************
         *                                  ROTACION DE MESHES
         ******************************************************************************************/
        private void Func_MeshesRotate(List<TgcMesh> meshes, float X, float Y, float Z)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].Rotation = new Vector3(X, Y, Z);
            }
        }

        private void Func_MeshesRotateX(List<TgcMesh> meshes, float angulo)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].rotateX(angulo);
            }
        }

        private void Func_MeshesRotateY(List<TgcMesh> meshes, float angulo)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].rotateY(angulo);
            }
        }

        private void Func_MeshesRotateZ(List<TgcMesh> meshes, float angulo)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].rotateZ(angulo);
            }
        }
    }
}
