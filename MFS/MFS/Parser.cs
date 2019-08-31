using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MFS
{
    public class Parser
    {
        private string json;
        private JObject root;

        public Parser(string path)
        {
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                root = JObject.Parse(json);
            }
        }

        public int GetMapHeight()
        {
            int tileheight = root["tileheight"].Value<int>();
            int height = root["height"].Value<int>();
            
            return height * tileheight;
        }

        public int GetMapWidth()
        {
            int tilewidth = root["tilewidth"].Value<int>();
            int width = root["width"].Value<int>();
            
            return width * tilewidth;
        }

        public void ParseJson()
        {
            List<JObject> layers = root["layers"].Values<JObject>().ToList();

            foreach (JObject layer in layers)
            {
                string name = layer["name"].Value<string>();
                if (name == "Objects")
                {
                    List<JObject> layerobjects = layer["objects"].Values<JObject>().ToList();
                    foreach (JObject layerobject in layerobjects)
                    {
                        string spriteName = layerobject["name"].Value<string>();
                        ushort spriteID = SpriteManager.Instance.GetSpriteIDByName(spriteName);
                        int posX = layerobject["x"].Value<int>();
                        int posY = layerobject["y"].Value<int>();
                        int height = layerobject["height"].Value<int>();
                        EntityType entityType = (EntityType)Enum.Parse(typeof (EntityType), layerobject["type"].Value<string>());
                        switch (entityType)
                        {
                            case EntityType.PROP:
                                {
                                    Prop prop = new Prop(new Vector2(posX, posY - height), spriteID);
                                    string kind = layerobject["properties"].Values<JObject>().ToList()[0].Value<JObject>()["value"].Value<string>();
                                    Kind propKind = (Kind) Enum.Parse(typeof(Kind), kind);
                                    prop.Kind = propKind;
                                    EntityManager.Instance.AddEntity(prop);
                                }
                                break;
                            case EntityType.PLAYER:
                                {
                                    EntityManager.Instance.AddEntity(new Player(new Vector2(posX, posY - height), spriteID));
                                }
                                break;
                            case EntityType.AXE:
                                {
                                    EntityManager.Instance.AddEntity(new Axe(new Vector2(posX, posY - height), spriteID));
                                }
                                break;
                            case EntityType.VEGETABLE:
                                {
                                    EntityManager.Instance.AddEntity(new Vegetable(new Vector2(posX, posY - height), spriteID));
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
