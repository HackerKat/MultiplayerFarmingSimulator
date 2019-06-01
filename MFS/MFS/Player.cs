using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Player
    {
        private Sprite playerSprite;
        private Rectangle collisionRect;
        private Vector2 position;

        public Player(Sprite playerSprite, Vector2 position)
        {
            this.playerSprite = playerSprite;
            this.position = position;
            collisionRect = new Rectangle((int)position.X, (int)position.Y, 
                            playerSprite.FrameSize.X, playerSprite.FrameSize.Y);
        }



        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Vector2 inputDirection = new Vector2(0, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                inputDirection.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                inputDirection.X += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                inputDirection.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                inputDirection.Y += 1;

            position += inputDirection;

            //bounds check
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - collisionRect.Width)
                position.X = clientBounds.Width - collisionRect.Width;
            if (position.Y > clientBounds.Height - collisionRect.Height)
                position.Y = clientBounds.Height - collisionRect.Height;

            playerSprite.Animate(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerSprite.Draw(spriteBatch, position);
        }
    }
}