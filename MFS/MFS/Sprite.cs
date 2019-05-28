using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS {
    public abstract class Sprite
    {
        private Texture2D textureImage;
        protected Point frameSize;
        protected Point sheetSize;
        protected Vector2 position;
        private int millisecondsPerFrame;

        //for non-animated sprites
        public Sprite (Texture2D textureImage, Vector2 position, Point frameSize) 
            : this(textureImage, position, frameSize, new Point(1, 1), 0)
        {
        }

        //for animated sprites
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point sheetSize, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public void Animate(GameTime gameTime)
        {
            if (sheetSize != new Point(1, 1))
            {
                int timeSinceLastFrame = 0;
                Point currentFrame = new Point(0, 0);
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        ++currentFrame.Y;
                        if (currentFrame.Y >= sheetSize.Y)
                            currentFrame.Y = 0;
                    }
                }
            }
        }

    }
}