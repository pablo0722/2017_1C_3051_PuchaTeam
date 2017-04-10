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
        /******************************************************************************************/
        /*                                  RENDERIZA BOXES
        /******************************************************************************************/
        private void Func_BoxRender(TgcBox box)
        {
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            box.Transform = Matrix.Scaling(box.Scale) *
                            Matrix.RotationYawPitchRoll(box.Rotation.Y, box.Rotation.X, box.Rotation.Z) *
                            Matrix.Translation(box.Position);

            box.render();
        }










        /******************************************************************************************/
        /*                                  TEXTO
        /******************************************************************************************/
        public void Func_Text(string text, int x, int y)
        {
            DrawText.drawText(text, x, y, Color.White);
            DrawText.drawText(text, x, y + 10, Color.Black);
        }
    }
}
