using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class World
    {
        private Tile[,] tiles;
        private const int TILESIZE = 32;
        private List<Tile> availableTiles;

        public World(Rectangle clientBounds)
        {
            int numX = clientBounds.Width / TILESIZE;
            int numY = clientBounds.Height / TILESIZE;

            tiles = new Tile[numX, numY];

            availableTiles = new List<Tile>();
        }

        public void LoadTiles()
        {
            ushort spriteID = SpriteManager.Instance.GetSpriteIDByName("background");

            Point sheetSize = SpriteManager.Instance.GetSprite(spriteID).SheetSize;

            for (int x = 0; x < sheetSize.X; x++)
            {
                for (int y = 0; y < sheetSize.Y; y++)
                {
                    Point frame = new Point(x, y);
                    Tile tile = new Tile(spriteID, frame);
                    availableTiles.Add(tile);
                }
            }
        }

        public void GenerateWorld()
        {
            Random rnd = new Random();
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int rndTile = rnd.Next(availableTiles.Count);
                    tiles[x, y] = availableTiles[rndTile];
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    Vector2 pos = new Vector2(x * TILESIZE, y * TILESIZE);
                    tiles[x, y].Draw(spriteBatch, pos);
                }
            }
        }

    }
}
