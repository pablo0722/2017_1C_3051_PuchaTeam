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
        class Objeto3D
        {
            /******************************************************************************************
             *                                      VARIABLES
             ******************************************************************************************/
            private List<TgcMesh> _meshes { get; set; }
            private List<Vector3> _position { get; set; }










            /******************************************************************************************
             *                                      CONSTRUCTOR
             ******************************************************************************************/
            Objeto3D(string path)
            {
                _meshes = new TgcSceneLoader().loadSceneFromFile(path).Meshes;
            }










            /******************************************************************************************
             *                                      DESTRUCTOR
             ******************************************************************************************/
            ~Objeto3D()
            {
                for (int i = 0; i < _meshes.Count; i++)
                {
                    _meshes[i].dispose();
                }
            }










            /******************************************************************************************
             *                                      TRANSFORMACIONES
             ******************************************************************************************/
            private void Position(float X, float Y, float Z)
            {
                for (int i = 0; i < _meshes.Count; i++)
                {
                    _meshes[i].Position = new Vector3(X, Y, Z);
                }
            }

            private void Size(float X, float Y, float Z)
            {
                for (int i = 0; i < _meshes.Count; i++)
                {
                    _meshes[i].Scale = new Vector3(X, Y, Z);
                }
            }

            private void Rotation(float X, float Y, float Z)
            {
                for (int i = 0; i < _meshes.Count; i++)
                {
                    _meshes[i].Rotation = new Vector3(X, Y, Z);
                }
            }










            /******************************************************************************************
             *                                      RENDER
             ******************************************************************************************/
            private void Render()
            {
                for (int i = 0; i < _position.Count; i++)
                {
                    for (int j = 0; j < _meshes.Count; j++)
                    {
                        _meshes[j].Position = _position[i];
                        _meshes[j].UpdateMeshTransform();
                        _meshes[j].render();
                    }
                }

            }
        }
    }
}
