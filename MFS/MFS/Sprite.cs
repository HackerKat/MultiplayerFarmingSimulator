using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS {
    public class Sprite
    {
        private Texture2D textureImage;
        private Point sheetSize;
        private int millisecondsPerFrame;
        private Point currentFrame;
        public Point CurrentFrame
        {
            get
            {
                return currentFrame;
            }
            set
            {
                currentFrame = value;
            }
        }
        
        private int timeSinceLastFrame;
        private Point frameSize;
        public Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }

        //for non-animated sprites
        public Sprite(Texture2D textureImage, Point frameSize)
            : this(textureImage, frameSize, new Point(1, 1), 0)
        {
        }

        //for animated sprites
        public Sprite(Texture2D textureImage, Point frameSize, Point sheetSize, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.frameSize = frameSize;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
            timeSinceLastFrame = 0;
            currentFrame = new Point(0, 0);
        }

        public Image GetImage()
        {
            Vector2 size = new Vector2(frameSize.X, frameSize.Y);
            Image img = new Image(textureImage, size);
            return img;
        }

        public void Animate(GameTime gameTime)
        {
            if (sheetSize != new Point(1, 1))
            {
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool ignoreDepth = false)
        {
            float depth = 1 - ((position.Y - frameSize.Y) / 800);

            if (depth < 0)
            {
                depth = 0;
            }
            else if (depth > 1)
            {
                depth = 1;
            }

            spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X,
                frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, ignoreDepth ? 1.0f : depth
                );
        }
        
    }
}