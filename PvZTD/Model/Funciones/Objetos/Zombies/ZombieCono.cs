using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.Funciones.Objetos.Zombies
{
    public class t_ZombieCono : t_ZombieComun
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombieCono-TgcScene.xml";
        private const int VIDA_ZOMBIE_CONO = 6;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public t_ZombieCono(GameModel game) : base(game, PATH_OBJ)
        {
            base.vida_zombie_comun = VIDA_ZOMBIE_CONO;
        }

        public new static t_ZombieCono Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_ZombieCono(game);
            }

            return null;
        }

    }
}