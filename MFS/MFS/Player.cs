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

        public Player(Vector2 position, ushort spriteID) 
            : base (position, spriteID)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);

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

        public string Serialize()
        {
            string plyrPacket;

            plyrPacket = position.X + ":" + position.Y;

            return plyrPacket;
        }

        public void Deserialize(string data)
        {
            string[] elements = data.Split(':');
            //id = short.Parse(elements[0]);
            position.X = float.Parse(elements[1]);
            position.Y = float.Parse(elements[2]);
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            position += InputHandle();

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
    }
}