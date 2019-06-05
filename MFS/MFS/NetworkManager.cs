using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class NetworkManager
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;

        public NetworkManager()
        {
            client = new TcpClient("localhost", 4848);

            Stream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
        }

        public void Update()
        {

        }
    }
}
