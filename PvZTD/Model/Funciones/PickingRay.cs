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
         *                                  DETECTA SELECCION DE MESH
         ******************************************************************************************/
        private bool Func_IsMeshPicked(TgcBox mesh)
        {
            //Actualizar Ray de colision en base a posicion del mouse
            PickingRay.updateRay();

            Mesh_BoxPicked = Mesh_BoxCollision;
            Mesh_BoxPickedPrev = Mesh_BoxCollision;

            var aabb = mesh.BoundingBox;

            //Ejecutar test, si devuelve true se carga el punto de colision collisionPoint
            var selected = TGC.Core.Collision.TgcCollisionUtils.intersectRayAABB(PickingRay.Ray, aabb, out PickRay_Pos);
            if (selected)
            {
                Mesh_BoxPicked = mesh;

                return true;
            }

            return false;
        }










        /******************************************************************************************
         *                                  INICIALIZA PICKING-RAY
         ******************************************************************************************/
        private void Func_Init_PickingRay()
        {
            PickingRay = new TgcPickingRay(Input);
            Mesh_BoxCollision = TgcBox.fromSize(new Vector3((float)0.5, (float)0.5, (float)0.5), Color.Red);
            Mesh_BoxCollision.AutoTransformEnable = true;
        }










        /******************************************************************************************
         *                                  DISPOSE PICKING-RAY
         ******************************************************************************************/
        private void Func_Dispose_PickingRay()
        {
            Mesh_BoxCollision.dispose();
        }
    }
}
