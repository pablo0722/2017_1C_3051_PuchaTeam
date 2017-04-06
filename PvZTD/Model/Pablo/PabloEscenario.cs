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
        private TgcMesh p_Mesh_plano { get; set; }              // Escenario
        private TgcMesh p_Mesh_mountain { get; set; }           // Mountain










        /******************************************************************************************
         *                                      INICIALIZACION
         ******************************************************************************************/
        private void p_Func_Init_Escenario()
        {
            p_Mesh_mountain = new TgcSceneLoader().loadSceneFromFile(MediaDir + Game.Default.MeshMountain).Meshes[0];
            p_Mesh_mountain.move(-100, 0, 0);

            var PathMeshPlano = MediaDir + Game.Default.MeshPlano;
            p_Mesh_plano = new TgcSceneLoader().loadSceneFromFile(PathMeshPlano).Meshes[0];
            p_Mesh_plano.Scale = new Vector3(2, 1, 1);
        }










        /******************************************************************************************
         *                                      RENDERIZACION
         ******************************************************************************************/
        private void p_Func_Render_Escenario()
        {
            Func_MeshRender(p_Mesh_plano);
            Func_MeshRender(p_Mesh_mountain);
        }
    }
}
