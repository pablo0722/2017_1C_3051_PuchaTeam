using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model.Funciones.Objetos.Zombies
{
    public class t_ZombieBalde : t_ZombieComun
    {

        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\zombieBalde-TgcScene.xml";


        public t_ZombieBalde(GameModel game) : base(game, PATH_OBJ)
        {
            base.velocidad_zombie = -2F;
            base.vida_zombie_comun = 30;
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
