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
        /*                 CONSTANTES - Deben comenzar con "p_"
        /******************************************************************************************/
        private const bool P_SHOW_AABB_WITH_KEY = true;










        /******************************************************************************************/
        /*                 VARIABLES - Deben comenzar con "p_"
        /******************************************************************************************/
        private t_Girasol _Girasol;
        private t_Lanzaguisantes _Lanzaguisantes;
        private t_Patatapum _Patatapum;
        private t_SolComun _Sol;
        private t_ZombieComun _zombie;
        private t_Escenario1 _escenario1;










        /******************************************************************************************/
        /*                 INIT - Se ejecuta una vez sola al comienzo
        /******************************************************************************************/

        private void pablo_init()
        {
            _escenario1 = t_Escenario1.Crear(this);
            _EscenarioBase = _escenario1;

            _Sol = t_SolComun.Crear(this);

            _zombie = t_ZombieComun.Crear(this);

            _Girasol = t_Girasol.Crear(this, 0);
            _Lanzaguisantes = t_Lanzaguisantes.Crear(this, 1);
            _Patatapum = t_Patatapum.Crear(this, 2);

            p_Func_Camara_Init();
        }










        /******************************************************************************************/
        /*                 UPDATE - Realiza la lógica del juego
        /******************************************************************************************/

        private void pablo_update()
        {
            _escenario1.Update(true, 4);

            _Sol.Update(true, 2);

            _zombie.Update(true);

            _Girasol.Update(P_SHOW_AABB_WITH_KEY, true, true);
            _Lanzaguisantes.Update(P_SHOW_AABB_WITH_KEY, true, true);
            _Patatapum.Update(P_SHOW_AABB_WITH_KEY, true, true);

            if (Input.keyPressed(Key.H))
            {
                _camara.Modo_Change();
            }
        }










        /******************************************************************************************/
        /*                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
        /******************************************************************************************/

        private void pablo_render()
        {
            _escenario1.Render();

            _Sol.Render();

            _zombie.Render();

            _Girasol.Render();
            _Lanzaguisantes.Render();
            _Patatapum.Render();

            if (_camara.Modo_Is_CamaraAerea())
            {
                Func_Text("H Para cambiar a Camara Primera Persona", 10, 80);
            }
            else
            {
                Func_Text("H Para cambiar a Camara Aérea", 10, 80);
            }
        }










        /******************************************************************************************/
        /*                 DISPOSE - Se ejecuta al finalizar el juego. Libera la memoria
        /******************************************************************************************/
        private void pablo_dispose()
        {
        }
    }
}
