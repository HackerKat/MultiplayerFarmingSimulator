using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Net;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Input;
using Server = MFS.Server;

namespace MFS
{
    public enum PacketType
    {
        HELLO,
        INITIAL_SETUP,
        ADD_PLAYER,
        POSITION_UPDATE,
        REMOVE_PLAYER
    };

    public class NetworkManager
    {
        private NetPeerConfiguration config;
        private Server server;
        private Client client;

        public static NetworkManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NetworkManager(666);
                }
                return instance;
            }
        }

        private static NetworkManager instance;

        private NetworkManager(int port)
        {
            this.config = new NetPeerConfiguration("MFS");
            config.Port = port;
            config.EnableMessageType(NetIncomingMessageType.Data);
        }
        
        public void StartHost()
        {
            server = new Server(config);
            server.Host();
        }

        public void StartClient(string hostname)
        {
            client = new Client(config.Port);
            client.Connect(hostname);
        }


        //public bool StartNetwork(string hostname)
        //{
        //    if (Keyboard.GetState().IsKeyDown(Keys.H))
        //    {
        //        server = new Server(config);
        //        server.Host();
        //        return true;
        //    }
        //    else if (Keyboard.GetState().IsKeyDown(Keys.C))
        //    {
        //        client = new Client(config.Port);
        //        client.Connect(hostname);
        //        return true;
        //    }
        //    return false;
        //}

        public void Update()
        {
            server?.Update();
            client?.Update();
        }
    }
}
