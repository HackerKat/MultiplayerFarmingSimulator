using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class BackgroundSprite
    {
        private Texture2D textureImage;
        private Point sheetSize;
        private Point frameSize;
        private Point currentFrame;

        public BackgroundSprite(Texture2D textureImage, Point sheetSize, Point frameSize)
        {
            this.textureImage = textureImage;
            this.sheetSize = sheetSize;
            this.frameSize = frameSize;
            currentFrame = GetRandomTile();
        }

        private Point GetRandomTile()
        {
            Point rndTile = new Point(1, 1);
            Random rnd = new Random();
            if (sheetSize != new Point(1, 1))
            {
                rndTile.X = rnd.Next(sheetSize.X);
            }
            return rndTile;
        }

        public void FillBackground(Rectangle clientBounds, SpriteBatch spriteBatch)
        {

            int xFrames = clientBounds.Width / frameSize.X;
            int yFrames = clientBounds.Height / frameSize.Y;

            for (int x = 0; x < xFrames; x++)
            {
                for (int y = 0; y < yFrames; y++)
                {
                    spriteBatch.Draw(textureImage, new Vector2(x * frameSize.X, y * frameSize.Y), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                                    Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                   
                }
                
            }
        }
    }
}
