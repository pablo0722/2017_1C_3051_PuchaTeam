using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

using System.Collections.Generic;
using TGC.Group.Model.Funciones.Objetos.Zombies;
using TGC.Group.Model.Funciones.Objetos.Plantas;

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
        public t_Girasol _Girasol;
        public t_Lanzaguisantes _Lanzaguisantes;
        public t_Repetidor _repetidor;
        public t_Patatapum _Patatapum;
        public t_SolComun _Sol;
        public t_ZombieComun _zombie;
        public t_ZombieCono _zombieCono;
        public t_ZombieBalde _zombieBalde;
        public t_Escenario1 _escenario1;










        /******************************************************************************************/
        /*                 INIT - Se ejecuta una vez sola al comienzo
        /******************************************************************************************/

        private void pablo_init()
        {
            _escenario1 = t_Escenario1.Crear(this);
            _EscenarioBase = _escenario1;

            _Sol = t_SolComun.Crear(this);

            _zombie = t_ZombieComun.Crear(this);
            _zombieCono = t_ZombieCono.Crear(this);
            _zombieBalde = t_ZombieBalde.Crear(this);



            _Girasol = t_Girasol.Crear(this, 0);
            _Lanzaguisantes = t_Lanzaguisantes.Crear(this, 1);
            _Patatapum = t_Patatapum.Crear(this, 2);
            _repetidor = t_Repetidor.Crear(this, 3);

            p_Func_Camara_Init();
        }










        /******************************************************************************************/
        /*                 UPDATE - Realiza la lógica del juego
        /******************************************************************************************/
    
        private void pablo_update()
        {
            _escenario1.Update(true, 4);

            _Sol.Update(true, 20);
            
            _zombie.Update(true, new List<int>{ 5 }, true);
            _zombieCono.Update(true, new List<int> { 16 }, true);
            _zombieBalde.Update(true, new List<int> { 33 }, true);

            _Girasol.Update(P_SHOW_AABB_WITH_KEY, 21);
            _Lanzaguisantes.Update(P_SHOW_AABB_WITH_KEY);
            _Patatapum.Update(P_SHOW_AABB_WITH_KEY);
            _repetidor.Update(P_SHOW_AABB_WITH_KEY);

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
            _zombieBalde.Render();
            _zombieCono.Render();


            _Girasol.Render();
            _Lanzaguisantes.Render();
            _Patatapum.Render();
            _repetidor.Render();

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
