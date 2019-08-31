using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class Axe : Item
    {
        private const float MIN_DISTANCE = 64f;

        public Axe (Vector2 position, ushort spriteID)
            :base (EntityType.AXE, position, spriteID)
        {

        }

        public override void Use()
        {
            EntityManager entityManager = EntityManager.Instance;
            Player player = entityManager.GetEntity(entityManager.PlayerID) as Player;
            var entities = entityManager.GetAllEntities();

            ushort closestTree = ushort.MaxValue;
            float shortestDistance = float.MaxValue;

            foreach(var entity in entities)
            {
                if (entity.Value is Prop)
                {
                    Prop prop = entity.Value as Prop;
                    if (prop.Kind == Kind.TREE)
                    {
                        float distance = (player.Position - prop.Position).Length();
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            closestTree = entity.Key;
                        }
                    }
                }
            }

            if (shortestDistance <= MIN_DISTANCE)
            {
                player.GetInventory().AddItem(new Resource(Vector2.Zero, 251));

                entityManager.RemoveEntity(closestTree, true);
            }
        }
    }
}
