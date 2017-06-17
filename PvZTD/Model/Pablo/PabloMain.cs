using Microsoft.DirectX.DirectInput;
using TGC.Core.Example;
using TGC.Group.Model.Funciones.Objetos.Zombies;
using TGC.Group.Model.Funciones.Objetos.Plantas;
using TGC.Group.Model.Funciones.Objetos;

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
        public t_Jalapenio _Jalapenio;
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
            _Jalapenio = t_Jalapenio.Crear(this, 5);

            _NivelActual = TXT_NIVEL_1;
        }










        /******************************************************************************************/
        /*                 UPDATE - Realiza la lógica del juego
        /******************************************************************************************/
    
        private void pablo_update()
        {
            if (_Hordas.FinDeNivel == false && !t_ZombieComun.gameover)
            {
                _escenario1.Update(true, 4);

                _Sol.Update(true, 20);

                _zombie.Update(true, true);
                _zombieCono.Update(true, true);
                _zombieBalde.Update(true, true);

                _Girasol.Update(P_SHOW_AABB_WITH_KEY, 21);
                _Lanzaguisantes.Update(P_SHOW_AABB_WITH_KEY);
                _Patatapum.Update(P_SHOW_AABB_WITH_KEY);
                _repetidor.Update(P_SHOW_AABB_WITH_KEY);
                _Jalapenio.Update(P_SHOW_AABB_WITH_KEY);

                if (Input.keyPressed(Key.H))
                {
                    _camara.Modo_Change();
                }
            }
        }










        /******************************************************************************************/
        /*                 RENDER - Se ejecuta aprox 60 veces por segundo. Dibuja en pantalla
        /******************************************************************************************/

        private void pablo_render()
        {
            _escenario1.Render();

            if(Menu.IniciarJuego)
            {
            
                _Sol.Render();

                _zombie.Render();
                _zombieBalde.Render();
                _zombieCono.Render();


                _Girasol.Render();
                _Lanzaguisantes.Render();
                _repetidor.Render();
                _Patatapum.Render();
                _Jalapenio.Render();
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
