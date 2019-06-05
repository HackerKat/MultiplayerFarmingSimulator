using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class Tile
    {
        private ushort spriteID;
        private Point frame;

        public Tile(ushort spriteID, Point frame)
        {
            this.spriteID = spriteID;
            this.frame = frame;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteManager spriteManager = SpriteManager.Instance;
            Sprite sprite = spriteManager.GetSprite(spriteID);
            sprite.CurrentFrame = frame;
            sprite.Draw(spriteBatch, position);
        }


        //private Point GetRandomTile()
        //{
        //    Point rndTile = new Point(1, 1);
        //    Random rnd = new Random();
        //    if (sheetSize != new Point(1, 1))
        //    {
        //        rndTile.X = rnd.Next(sheetSize.X);
        //    }
        //    return rndTile;
        //}

        //public void FillBackground(Rectangle clientBounds, SpriteBatch spriteBatch)
        //{

        //    int xFrames = clientBounds.Width / frameSize.X;
        //    int yFrames = clientBounds.Height / frameSize.Y;

        //    for (int x = 0; x < xFrames; x++)
        //    {
        //        for (int y = 0; y < yFrames; y++)
        //        {
        //            spriteBatch.Draw(textureImage, new Vector2(x * frameSize.X, y * frameSize.Y), new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
        //                            Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                   
        //        }
                
        //    }
        //}
    }
}
