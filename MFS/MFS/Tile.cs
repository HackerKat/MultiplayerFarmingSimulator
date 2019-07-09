using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class Tile
    {
        private ushort spriteID;
        private Point frame;

        public Tile(ushort spriteID, Point frame)
        {
            this.spriteID = spriteID;
            this.frame = frame;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteManager spriteManager = SpriteManager.Instance;
            Sprite sprite = spriteManager.GetSprite(spriteID);
            sprite.CurrentFrame = frame;
            sprite.Draw(spriteBatch, position);
        }
    }
}
