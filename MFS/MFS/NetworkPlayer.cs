using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MFS
{
    public class NetworkPlayer : Entity
    {
        public NetworkPlayer(Vector2 position, ushort spriteID)
            : base(position, spriteID)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            playerSprite.Draw(spriteBatch, position);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            
        }

        public override string GetEntityType()
        {
            return "networkplayer";
        }

    }
}
