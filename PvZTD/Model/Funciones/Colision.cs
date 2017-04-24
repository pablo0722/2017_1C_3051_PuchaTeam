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
    public class t_Colision
    {
        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private TgcExample _example;

        // RAY PICKING
        private TgcPickingRay _PickingRay;
        private Vector3 _PickRay_Pos;

        // MESHES
        private TgcBox _Mesh_BoxCollision;  // Punto rojo colision










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_Colision(TgcExample example)
        {
            _example = example;

            _PickingRay = new TgcPickingRay(_example.Input);
            _Mesh_BoxCollision = TgcBox.fromSize(new Vector3((float)0.5, (float)0.5, (float)0.5), Color.Red);
            _Mesh_BoxCollision.AutoTransformEnable = true;
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~t_Colision()
        {
            _Mesh_BoxCollision.dispose();
        }










        /******************************************************************************************/
        /*                                  DETECTA COLISION DE MESH
        /******************************************************************************************/
        public bool MouseBox(TgcBox mesh)
        {
            //Actualizar Ray de colision en base a posicion del mouse
            _PickingRay.updateRay();

            var aabb = mesh.BoundingBox;

            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
            var selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(_PickingRay.Ray, aabb, out _PickRay_Pos);
            if (selected)
            {
                return true;
            }

            return false;
        }

        public t_Objeto3D.t_instancia MouseMesh(t_Objeto3D obj)
        {
            //Actualizar Ray de colision en base a posicion del mouse
            _PickingRay.updateRay();

            for (int i = 0; i < obj._instancias.Count; i++)
            {
                for (int j = 0; j < obj._meshes.mesh.Count; j++)
                {
                    obj._meshes.mesh[j].Position = obj._instancias[i].pos;
                    obj._meshes.mesh[j].Rotation = obj._instancias[i].rot;
                    obj._meshes.mesh[j].Scale = obj._instancias[i].size;
                    obj._meshes.mesh[j].UpdateMeshTransform();

                    var aabb = obj._meshes.mesh[j].BoundingBox;

                    //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
                    var selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(_PickingRay.Ray, aabb, out _PickRay_Pos);
                    if (selected)
                    {
                        return obj._instancias[i];
                    }
                }
            }

            return null;
        }
    }
}
