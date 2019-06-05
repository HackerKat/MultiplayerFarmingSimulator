using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MFS
{
    //with XNA it should be Microsoft.Xna.Framework.DrawableGameComponent
    public class SpriteManager
    {
        public static SpriteManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new SpriteManager();
                }
                return instance;
            }
        }
        private static SpriteManager instance;

        private SpriteBatch spriteBatch;
        private List <Sprite> sprites;
        public Game Game
        {
            get;
            set;
        }

        private SpriteManager()
        {
            sprites = new List<Sprite>();
        }
        
        public void LoadContent()
        {
            //playerID0
            Sprite playerSprite = new Sprite(Game.Content.Load<Texture2D>(@"Images/AnimatedSprites/mani-idle-run"),
                                             new Point(24, 24), new Point(7, 1), 5);

            //rock04ID1
            Sprite rockSprite01 = new Sprite(Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock04"),
                                            new Point(26, 15));

            //rock05ID2
            Sprite rockSprite02 = new Sprite(Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock05"),
                                            new Point(26, 15));
            
            //backgroundID3
            Sprite bckgr = new Sprite (Game.Content.Load<Texture2D>(@"Images/Background/backgroundSheet2"), new Point(11, 2), new Point(16, 16), 0);


            sprites.Add(playerSprite);
            sprites.Add(rockSprite01);
            sprites.Add(rockSprite02);
            sprites.Add(bckgr);
        }

        public Sprite GetSprite (ushort spriteID)
        {
            return sprites[spriteID];
        }
    }
}