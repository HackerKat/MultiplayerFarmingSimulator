using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Player : Entity
    {
        private InputManager inputManager;

        public Player(Vector2 position, ushort spriteID) 
            : base (position, spriteID)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            
            inputManager = InputManager.Instance;
        }

        private void BoundsCheck(Rectangle clientBounds)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - CollisionRect.Width)
                position.X = clientBounds.Width - CollisionRect.Width;
            if (position.Y > clientBounds.Height - CollisionRect.Height)
                position.Y = clientBounds.Height - CollisionRect.Height;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            
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