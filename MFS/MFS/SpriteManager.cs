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

            //potatooID152
            Sprite potato = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/potato"), new Point(32, 32));

            //artischokeID153
            Sprite artischoke = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/artischoke"), new Point(32, 32));

            //carrotID154
            Sprite carrot = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/carrot"), new Point(32, 32));

            //cornID155
            Sprite corn = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/corn"), new Point(32, 32));

            //paprikaID156
            Sprite paprika = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/paprika"), new Point(32, 32));

            //zucchiniID157
            Sprite zucchini = new Sprite(Game.Content.Load<Texture2D>(@"Images/Plants/zucchini"), new Point(32, 32));

            //swordID201
            Sprite axe = new Sprite(Game.Content.Load<Texture2D>(@"Images/Items/sword"), new Point(32, 32));

            //woodlogID251
            Sprite woodlog = new Sprite(Game.Content.Load<Texture2D>(@"Images/Items/woodlog"), new Point(32, 32));

            //missingSpriteID65535
            Sprite missingSprite = new Sprite(Game.Content.Load<Texture2D>(@"TESTCUBE"), new Point(32, 32));

            //Player Sprites
            sprites.Add(1, playerSprite);
            lookupSpriteTable.Add("playerSprite", 1);
            //Background sprite
            sprites.Add(0, background);
            lookupSpriteTable.Add("background", 0);
            //Tree sprites
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
            //house sprite
            sprites.Add(10, house);
            lookupSpriteTable.Add("house", 10);
            //Vegetables sprites
            sprites.Add(151, tomato);
            lookupSpriteTable.Add("tomato", 151);
            sprites.Add(152, potato);
            lookupSpriteTable.Add("potato", 152);
            sprites.Add(153, artischoke);
            lookupSpriteTable.Add("artischoke", 153);
            sprites.Add(154, carrot);
            lookupSpriteTable.Add("carrot", 154);
            sprites.Add(155, corn);
            lookupSpriteTable.Add("corn", 155);
            sprites.Add(156, paprika);
            lookupSpriteTable.Add("paprika", 156);
            sprites.Add(157, zucchini);
            lookupSpriteTable.Add("zucchini", 157);
            sprites.Add(201, axe);
            //Tools sprite
            lookupSpriteTable.Add("axe", 201);
            //Resources sprite
            sprites.Add(251, woodlog);
            lookupSpriteTable.Add("woodlog", 251);
            //Debug sprites
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