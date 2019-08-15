using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MFS
{
    //TODO: Player in Network player, and player should be deleted
    public class NetworkPlayer : Entity
    {
        public NetworkPlayer(Vector2 position, ushort spriteID)
            : base(EntityType.PLAYER, position, spriteID)
        {

        }
    }
}
