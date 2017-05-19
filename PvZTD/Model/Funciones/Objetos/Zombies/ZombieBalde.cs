using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.Funciones.Objetos.Zombies
{
    public class t_ZombieBalde : t_ZombieComun
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombieBalde-TgcScene.xml";
        private const int VIDA_ZOMBIE_BALDE = 30;


        public t_ZombieBalde(GameModel game) : base(game, PATH_OBJ)
        {
            base.vida_zombie_comun = VIDA_ZOMBIE_BALDE;
            base._TxtZombieNivel = "Zbalde";
        }

        public new static t_ZombieBalde Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_ZombieBalde(game);
            }

            return null;
        }
    }
}
