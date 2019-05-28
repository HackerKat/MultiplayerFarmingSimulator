using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    class Player
    {
        Sprite playerSprite;
        Rectangle collisionRect;

        public Player(Sprite playerSprite, Rectangle collisionRect)
        {
            playerSprite = this.playerSprite;
            collisionRect = this.collisionRect;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //bounds check
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }
    }
}