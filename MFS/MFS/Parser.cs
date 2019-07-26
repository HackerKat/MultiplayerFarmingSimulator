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
        
        //TODO: separate method for parsing and entity adding
        public void ObjectLayer()
        {
            List<JObject> layers = root["layers"].Values<JObject>().ToList();

            foreach (JObject layer in layers)
            {
                string name = layer["name"].Value<string>();
                if (name == "Trees")
                {
                    List<JObject> layerobjects = layer["objects"].Values<JObject>().ToList();
                    foreach (JObject lobject in layerobjects)
                    {
                        ushort gid = lobject["gid"].Value<ushort>();
                        int posX = lobject["x"].Value<int>();
                        int posY = lobject["y"].Value<int>();
                        int height = lobject["height"].Value<int>();
                        EntityManager.Instance.AddEntity(new Prop(new Vector2(posX, posY - height), (ushort)(gid + 1), PropType.SOLID));
                    }
                }
                else if (name == "house")
                {
                    List<JObject> layerobjects = layer["objects"].Values<JObject>().ToList();
                    foreach (JObject lobject in layerobjects)
                    {
                        ushort gid = lobject["gid"].Value<ushort>();
                        int posX = lobject["x"].Value<int>();
                        int posY = lobject["y"].Value<int>();
                        int height = lobject["height"].Value<int>();
                        EntityManager.Instance.AddEntity(new Prop(new Vector2(posX, posY - height), (ushort)(gid + 1), PropType.SOLID));
                    }
                }
                else if (name == "pickup")
                {
                    List<JObject> layerobjects = layer["objects"].Values<JObject>().ToList();
                    foreach (JObject lobject in layerobjects)
                    {
                        //ushort gid = lobject["gid"].Value<ushort>();
                        int posX = lobject["x"].Value<int>();
                        int posY = lobject["y"].Value<int>();
                        int height = lobject["height"].Value<int>();
                        EntityManager.Instance.AddEntity(new Prop(new Vector2(posX, posY - height), 10, PropType.PICKUP));
                    }
                }
            }
        }
    }
}
