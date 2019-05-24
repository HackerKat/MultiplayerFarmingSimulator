using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MFS
{
    class AutomatedSprite : Sprite
    {
        public override Vector2 direction
        {
            get
            {
                return speed;
            }
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
       : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += direction;

            base.Update(gameTime, clientBounds);
        }
    }
}