using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.Funciones.Objetos.Zombies
{
    public class t_ZombieCono : t_ZombieComun
    {

        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombieCono-TgcScene.xml";

        public t_ZombieCono(GameModel game) : base(game, PATH_OBJ)
        {

            velocidad_zombie = -2F;
            vida_zombie_comun = 20;
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