using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Prop
    {
        private Sprite propSprite;
        private Rectangle collisionRect;
        private Vector2 position;
        public Rectangle CollisionRect
        {
            get
            {
                return collisionRect;
            }
        }

        public Prop(Sprite propSprite, Vector2 position)
        {
            this.propSprite = propSprite;
            this.position = position;
            collisionRect = new Rectangle((int)position.X, (int)position.Y,
                            propSprite.FrameSize.X, propSprite.FrameSize.Y);
        }



        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
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

        public void Draw(SpriteBatch spriteBatch)
        {
            propSprite.Draw(spriteBatch, position);
        }
    }
}