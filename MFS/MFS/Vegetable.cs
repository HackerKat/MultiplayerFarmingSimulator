using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class Vegetable : Item
    {
        public Vegetable(Vector2 position, ushort spriteID)
            : base(EntityType.VEGETABLE, position, spriteID)
        {
        }

        public override void Use()
        {

        }
    }
}
