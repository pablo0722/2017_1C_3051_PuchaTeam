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
        private List<TgcMesh> p_Mesh_zombie { get; set; }             // Zombie










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Zombies()
        {
            p_Mesh_zombie = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshZombie).Meshes;
  

            for (int i = 0; i < p_Mesh_zombie.Count; i++)
            {
                p_Mesh_zombie[i].Scale = new Vector3((float)0.25, (float)0.25, (float)0.25);
            }
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Zombies()
        {
            Func_MeshesRender(p_Mesh_zombie);
        }
    }
}
