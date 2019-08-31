using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MFS
{
    public class Resource : Item
    {
        public Resource(Vector2 position, ushort spriteID)
            :base(EntityType.RESOURCE, position, spriteID)
        {
        }

        public override void Use()
        {
        }
    }
}
