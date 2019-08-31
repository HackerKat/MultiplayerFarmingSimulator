using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public abstract class Item : Entity
    {
        public Item (EntityType type, Vector2 position, ushort spriteID)
            : base(type, position, spriteID)
        {
        }

        public abstract void Use();
    }
}
