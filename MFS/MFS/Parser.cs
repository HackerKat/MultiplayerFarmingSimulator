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
        //private static readonly string path;

        public Parser(string path)
        {
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                Console.WriteLine("path exists");
            }
        }

        public int GetMapHeight()
        {
            JObject o = JObject.Parse(json);

            //TODO: rename SelectToken in more consistent version
            int tileheight = (int)o.SelectToken("[ 'tileheight' ]");
            int height = (int)o.SelectToken("[ 'height' ]");

            return height * tileheight;
        }

        public int GetMapWidth()
        {
            JObject o = JObject.Parse(json);

            //TODO: rename SelectToken in more consistent version
            int tilewidth = (int)o.SelectToken("[ 'tilewidth' ]");
            int width = (int)o.SelectToken("[ 'width' ]");
            
            return width * tilewidth;
        }

        public void TreeLayer()
        {
            JObject o = JObject.Parse(json);
            int obj = o["height"].Value<int>();
            //Console.WriteLine(obj);

            List<JObject> layers = o["layers"].Values<JObject>().ToList();

            foreach (JObject l in layers)
            {
                string s = l["name"].Value<string>();
                if (s == "Trees")
                {
                    List<JObject> objects = l["objects"].Values<JObject>().ToList();
                    foreach (JObject ob in objects)
                    {
                        ushort gid = ob["gid"].Value<ushort>();
                        //Console.WriteLine(gid);
                        int posX = ob["x"].Value<int>();
                        int posY = ob["y"].Value<int>();
                        EntityManager.Instance.AddEntity(new Prop(new Vector2(posX, posY), (ushort)(gid + 1)));
                    }
                }
                //Console.WriteLine(s);
            }
        

            //IEnumerable <JToken> objects = o.SelectTokens("layers[0].name[1].objects");

            


            //foreach(JToken obj in objects)
            //{
            //    string gid = (string)o.SelectToken();
            //}
        }
    }
}
