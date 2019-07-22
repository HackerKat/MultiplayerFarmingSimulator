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

        public void TreeLayer()
        {
            List<JObject> layers = root["layers"].Values<JObject>().ToList();

            foreach (JObject layer in layers)
            {
                string layername = layer["name"].Value<string>();
                if (layername == "Trees")
                {
                    List<JObject> layerobjects = layer["objects"].Values<JObject>().ToList();
                    foreach (JObject lobject in layerobjects)
                    {
                        ushort gid = lobject["gid"].Value<ushort>();
                        int posX = lobject["x"].Value<int>();
                        int posY = lobject["y"].Value<int>();
                        EntityManager.Instance.AddEntity(new Prop(new Vector2(posX, posY), (ushort)(gid + 1)));
                    }
                }
            }
        }
    }
}
