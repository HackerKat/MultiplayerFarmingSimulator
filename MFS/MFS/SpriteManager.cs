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

            ////rock04ID1
            Sprite rockSprite01 = new Sprite(Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock04"),
                                            new Point(26, 15));

            ////rock05ID2
            Sprite rockSprite02 = new Sprite(Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock05"),
                                            new Point(26, 15));
            
            //backgroundID3
            Sprite bckgr = new Sprite (Game.Content.Load<Texture2D>(@"Images/Background/grasstiles2"), new Point(32, 32), new Point(2, 1), 0);

            //treealive1ID4
            Sprite treealive1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive1"), new Point(56,64));

            //treealive2ID5
            Sprite treealive2 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive2"), new Point(56, 72));

            //treealive3ID6
            Sprite treealive3 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive3"), new Point(64, 72));

            //treestump1ID7
            Sprite treestump1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treestump1"), new Point(32, 32));

            //treedead1ID8
            Sprite treedead1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treedead1"), new Point(56, 64));

            //houseID9
            Sprite house = new Sprite(Game.Content.Load<Texture2D>(@"Images/House/House"), new Point(100, 100));

            //tomatoID10
            Sprite tomato = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/tomato"), new Point(32, 25));


            sprites.Add(playerSprite);
            sprites.Add(rockSprite01);
            sprites.Add(rockSprite02);
            sprites.Add(bckgr);
            sprites.Add(treealive1);
            sprites.Add(treealive2);
            sprites.Add(treealive3);
            sprites.Add(treestump1);
            sprites.Add(treedead1);
            sprites.Add(house);
            sprites.Add(tomato);
        }

        public Sprite GetSprite (ushort spriteID)
        {
            return sprites[spriteID];
        }
    }
}