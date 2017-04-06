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
         *                                      VARIABLES
         ******************************************************************************************/
        private TgcMesh p_Mesh_zombie { get; set; }             // Zombie










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Zombies()
        {
            p_Mesh_zombie = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshZombie).Meshes[0];
            p_Mesh_zombie.Scale = new Vector3((float)0.25, (float)0.25, (float)0.25);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Zombies()
        {
            Func_MeshRender(p_Mesh_zombie);
        }
    }
}
