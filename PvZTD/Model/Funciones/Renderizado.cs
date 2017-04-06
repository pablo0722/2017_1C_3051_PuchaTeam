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
         *                                  RENDERIZA MESHES
         ******************************************************************************************/
        private void Func_MeshRender(TgcMesh mesh)
        {
            //Cuando tenemos modelos mesh podemos utilizar un método que hace la matriz de transformación estándar.
            //Es útil cuando tenemos transformaciones simples, pero OJO cuando tenemos transformaciones jerárquicas o complicadas.
            mesh.UpdateMeshTransform();
            mesh.render();
        }

        private void Func_MeshesRender(List<TgcMesh> meshes)
        {
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].UpdateMeshTransform();
                meshes[i].render();
            }
        }










        /******************************************************************************************
         *                                  RENDERIZA BOXES
         ******************************************************************************************/
        private void Func_BoxRender(TgcBox box)
        {
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            box.Transform = Matrix.Scaling(box.Scale) *
                            Matrix.RotationYawPitchRoll(box.Rotation.Y, box.Rotation.X, box.Rotation.Z) *
                            Matrix.Translation(box.Position);

            box.render();
        }










        /******************************************************************************************
         *                                  TEXTO
         ******************************************************************************************/
        private void Func_Text(string text, int x, int y)
        {
            DrawText.drawText(text, x, y, Color.White);
            DrawText.drawText(text, x, y + 10, Color.Black);
        }
    }
}
