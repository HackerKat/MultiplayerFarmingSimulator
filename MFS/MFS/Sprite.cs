using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS {
    public abstract class Sprite
    {
        private Texture2D textureImage;
        protected Point frameSize;
        private Point currentFrame;
        protected Point sheetSize;
        private int collisionOffset;
        private int timeSinceLastFrame = 0;
        private int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 100;
        protected Vector2 speed;
        protected Vector2 position;

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

        public Sprite (Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed) 
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }


        public List<Texture2D> CropTiles()
        {
            List<Texture2D> textureList = new List<Texture2D>();
            int xSprite = 0;
            int ySprite = 0;
            Rectangle newBounds = new Rectangle(xSprite, ySprite, frameSize.X, frameSize.Y);

            while (xSprite < textureImage.Width && ySprite < textureImage.Height)
            {
                Texture2D croppedTexture = new Texture2D(textureImage.GraphicsDevice, frameSize.X, frameSize.Y);
                Color[] data = new Color[frameSize.X * frameSize.Y];
                textureImage.GetData(0, newBounds, data, 0, newBounds.Width * newBounds.Width);
                croppedTexture.SetData(data);

                textureList.Add(croppedTexture);

                xSprite += frameSize.X;
                ySprite += frameSize.Y;
            }
            return textureList;
        }

        public virtual void Update (GameTime gameTime, Rectangle clientBounds)
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

        public virtual void Draw (GameTime gameTime, SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(textureImage, position,
                new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y,
                frameSize.X,
                frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0
                );
        }
    }
}