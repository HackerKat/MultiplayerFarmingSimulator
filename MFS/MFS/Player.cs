using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Player
    {
        private Sprite playerSprite;
        private Vector2 position;
        private Rectangle collisionRect;
        public Rectangle CollisionRect
        {
            get
            {
                return collisionRect;
            }
        }

        public Player(Sprite playerSprite, Vector2 position)
        {
            this.playerSprite = playerSprite;
            this.position = position;
            collisionRect = new Rectangle((int)position.X, (int)position.Y,
                             playerSprite.FrameSize.X, playerSprite.FrameSize.Y);
        }

        private void BoundsCheck(Rectangle clientBounds)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - collisionRect.Width)
                position.X = clientBounds.Width - collisionRect.Width;
            if (position.Y > clientBounds.Height - collisionRect.Height)
                position.Y = clientBounds.Height - collisionRect.Height;
        }

        private Vector2 InputHandle()
        {
            Vector2 inputDirection = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                inputDirection.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                inputDirection.X += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                inputDirection.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                inputDirection.Y += 1;

            return inputDirection;
        }

        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += InputHandle();

            //bounds check
            BoundsCheck(clientBounds);

            playerSprite.Animate(gameTime);

            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch, position);
        }
    }
}