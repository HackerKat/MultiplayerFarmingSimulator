using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Prop : Entity
    {
        private Rectangle collisionRect;
        public Rectangle CollisionRect
        {
            get
            {
                return collisionRect;
            }
        }

        public Prop (Vector2 position, ushort spriteID)
            : base (position, spriteID)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);

            collisionRect = new Rectangle((int)position.X, (int)position.Y,
                            propSprite.FrameSize.X, propSprite.FrameSize.Y);
        }
        
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);
            //bounds check
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - collisionRect.Width)
                position.X = clientBounds.Width - collisionRect.Width;
            if (position.Y > clientBounds.Height - collisionRect.Height)
                position.Y = clientBounds.Height - collisionRect.Height;

            propSprite.Animate(gameTime);

            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);

            propSprite.Draw(spriteBatch, position);
        }
    }
}