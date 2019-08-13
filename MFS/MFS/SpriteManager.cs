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

        private Dictionary<string, ushort> lookupSpriteTable;
        private Dictionary <ushort, Sprite> sprites;
        //TODO: shouldn't it be private?
        public Game Game
        {
            get;
            set;
        }

        private SpriteManager()
        {
            sprites = new Dictionary<ushort, Sprite>();
            lookupSpriteTable = new Dictionary<string, ushort>();
        }
       
        public void LoadContent()
        {
            //playerID1
            Sprite playerSprite = new Sprite(Game.Content.Load<Texture2D>(@"Images/AnimatedSprites/mani-idle-run"),
                                             new Point(24, 24), new Point(7, 1), 5);
            
            //backgroundID0
            Sprite background = new Sprite (Game.Content.Load<Texture2D>(@"Images/Background/grasstiles2"), new Point(32, 32), new Point(2, 1), 0);

            //treealive1ID11
            Sprite treealive1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive1"), new Point(56,64));

            //treealive2ID12
            Sprite treealive2 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive2"), new Point(56, 72));

            //treealive3ID13
            Sprite treealive3 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treealive3"), new Point(64, 72));

            //treestump1ID14
            Sprite treestump1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treestump1"), new Point(32, 32));

            //treedead1ID15
            Sprite treedead1 = new Sprite(Game.Content.Load<Texture2D>(@"Images/Trees/treedead1"), new Point(56, 64));

            //houseID10
            Sprite house = new Sprite(Game.Content.Load<Texture2D>(@"Images/House/House"), new Point(100, 100));

            //TODO: add more vegetables and tools and adjust their IDs
            //tomatoID151
            Sprite tomato = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/tomato"), new Point(32, 25));

            //swordID201
            Sprite axe = new Sprite(Game.Content.Load<Texture2D>(@"Images/Items/sword"), new Point(32, 32));

            //woodlogID251
            Sprite woodlog = new Sprite(Game.Content.Load<Texture2D>(@"Images/Items/woodlog"), new Point(32, 32));

            //missingSpriteID65535
            Sprite missingSprite = new Sprite(Game.Content.Load<Texture2D>(@"TESTCUBE"), new Point(32, 32));

            sprites.Add(1, playerSprite);
            lookupSpriteTable.Add("playerSprite", 1);
            sprites.Add(0, background);
            lookupSpriteTable.Add("background", 0);
            sprites.Add(11, treealive1);
            lookupSpriteTable.Add("treealive1", 11);
            sprites.Add(12, treealive2);
            lookupSpriteTable.Add("treealive2", 12);
            sprites.Add(13, treealive3);
            lookupSpriteTable.Add("treealive3", 13);
            sprites.Add(14, treestump1);
            lookupSpriteTable.Add("treestump1", 14);
            sprites.Add(15, treedead1);
            lookupSpriteTable.Add("treedead1", 15);
            sprites.Add(10, house);
            lookupSpriteTable.Add("house", 10);
            sprites.Add(101, tomato);
            lookupSpriteTable.Add("tomato", 101);
            sprites.Add(201, axe);
            lookupSpriteTable.Add("axe", 201);
            sprites.Add(251, woodlog);
            lookupSpriteTable.Add("woodlog", 251);
            sprites.Add(ushort.MaxValue, missingSprite);
            lookupSpriteTable.Add("missingSprite", ushort.MaxValue);
        }

        public Sprite GetSprite (ushort spriteID)
        {
            if (sprites.ContainsKey(spriteID))
            {
                return sprites[spriteID];
            }
            return sprites[ushort.MaxValue];
        }

        public ushort GetSpriteIDByName(string spriteName)
        {
            if (lookupSpriteTable.ContainsKey(spriteName))
            {
                return lookupSpriteTable[spriteName];
            }
            return ushort.MaxValue;
        }
    }
}