using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Player : Entity
    {
        private Rectangle collisionRect;
        public Rectangle CollisionRect
        {
            get
            {
                return collisionRect;
            }
        }
        private InputManager inputManager;

        public Player(Vector2 position, ushort spriteID) 
            : base (position, spriteID)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);

            collisionRect = new Rectangle((int)position.X, (int)position.Y,
                             playerSprite.FrameSize.X, playerSprite.FrameSize.Y);

            inputManager = InputManager.Instance;
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

        private Vector2 Input()
        {
            return inputManager.InputHandle();
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            position += Input();

            //bounds check
            BoundsCheck(clientBounds);

            playerSprite.Animate(gameTime);

            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            playerSprite.Draw(spriteBatch, position);
        }

        public override string GetEntityType()
        {
            return "player";
        }
    }
}