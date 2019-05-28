using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS
{
    class BackgroundSprite : Sprite
    {
        public override Vector2 direction
        {
            get
            {
                return Vector2.Zero;
            }
        }

        public BackgroundSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
        }

        public BackgroundSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
       : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            base.Update(gameTime, clientBounds);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        //public void RandomizeTiles()
        //{
        //    List<Sprite> backgroundTiles = new List<Sprite>();

        //    int xSprite = 0;
        //    int ySprite = 0;


        //    while (xSprite < sheetSize.X && ySprite < sheetSize.Y)
        //    {
        //        backgroundTiles.Add();
        //    }

        //    if (timeSinceLastFrame > millisecondsPerFrame)
        //    {
        //        timeSinceLastFrame = 0;
        //        ++currentFrame.X;
        //        if (currentFrame.X >= sheetSize.X)
        //        {
        //            currentFrame.X = 0;
        //            ++currentFrame.Y;
        //            if (currentFrame.Y >= sheetSize.Y)
        //                currentFrame.Y = 0;
        //        }
        //    }
        //}
    }
}