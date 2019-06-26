using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Net;
using Microsoft.Xna.Framework;
using System.Threading;

namespace MFS
{
    public class NetworkManager
    {
        private NetPeerConfiguration config;
        private NetServer server;
        private bool isHost;
        private NetClient client;
        private NetConnection connectionToHost;
        private bool connected;
        private bool clientInitialized;

        public NetworkManager(int port)
        {
            config = new NetPeerConfiguration("MFS");
            config.Port = port;
            config.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void Host()
        {
            server = new NetServer(config);
            
            server.Start();
            isHost = true;
        }

        public void Connect(string hostname)
        {
            NetPeerConfiguration clientConfig = new NetPeerConfiguration("MFS");
            clientConfig.EnableMessageType(NetIncomingMessageType.Data);
            client = new NetClient(clientConfig);
            client.Start();
            connectionToHost = client.Connect(hostname, config.Port);

            if (connectionToHost == null)
            {
                connected = false;
                return;
            }

            isHost = false;
            connected = true;
            clientInitialized = false;
            Thread.Sleep(500);
            NetOutgoingMessage hello = client.CreateMessage();
            hello.Write("Hello");
            client.SendMessage(hello, NetDeliveryMethod.ReliableOrdered);
        }

        public NetOutgoingMessage positionUpdate(ushort id, Entity entity)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            outMsg.Write("PositionUpdate");
            outMsg.Write(id);
            entity.PackPacket(outMsg);
            return outMsg;
        }

        public void Update()
        {
            NetIncomingMessage msg;
            #region Server
            if (isHost)
            {
                while ((msg = server.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                string type = msg.ReadString();
                                if (type == "PositionUpdate")
                                {
                                    ushort id = msg.ReadUInt16();
                                    Entity entity = EntityManager.Instance.GetEntity(id);
                                    entity.UnpackPacket(msg);
                                    Console.WriteLine(id);
                                    Console.WriteLine("Position Update");

                                    var outMsg = positionUpdate(id, entity);

                                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                                }
                                else if (type == "Hello")
                                {
                                    NetworkPlayer netPlayer = new NetworkPlayer(new Microsoft.Xna.Framework.Vector2(200,200), 0);

                                    NetOutgoingMessage outMsg = server.CreateMessage();
                                    var entities = EntityManager.Instance.GetAllEntities();
                                    outMsg.Write("InitialSetup");
                                    outMsg.Write(entities.Count);
                                    Console.WriteLine(entities.Count);
                                    foreach (var element in entities)
                                    {
                                        ushort id = element.Key;
                                        Entity entity = element.Value;

                                        outMsg.Write(id);
                                        outMsg.Write(entity.GetEntityType());
                                        entity.PackPacket(outMsg);
                                    }
                                    ushort netPlayerID = EntityManager.Instance.AddEntity(netPlayer);
                                    outMsg.Write(netPlayerID);

                                    Console.WriteLine(outMsg);
                                    server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                                    outMsg = server.CreateMessage();
                                    outMsg.Write("AddPlayer");
                                    outMsg.Write(netPlayerID);
                                    netPlayer.PackPacket(outMsg);
                                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                                }
                            }
                            break;
                    }
                    server.Recycle(msg);
                }
                {
                    ushort id = EntityManager.Instance.PlayerID;
                    Entity entity = EntityManager.Instance.GetEntity(id);
                    NetOutgoingMessage outMsg = server.CreateMessage();
                    outMsg.Write("PositionUpdate");
                    outMsg.Write(id);
                    entity.PackPacket(outMsg);

                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                }
            }
            #endregion
            #region Client
            else
            {
                while((msg = client.ReadMessage()) != null)
                {
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            {
                                string type = msg.ReadString();
                                if (type == "PositionUpdate" && clientInitialized)
                                {
                                    ushort id = msg.ReadUInt16();
                                    Entity entity = EntityManager.Instance.GetEntity(id);
                                    entity.UnpackPacket(msg);
                                }
                                if (type == "AddPlayer")
                                {
                                    NetworkPlayer newPlayer = new NetworkPlayer(new Vector2(100, 100), 0);
                                    ushort id = msg.ReadUInt16();
                                    if (id != EntityManager.Instance.PlayerID)
                                    {
                                        newPlayer.UnpackPacket(msg);
                                        EntityManager.Instance.AddEntity(newPlayer, id);
                                    }
                                }
                                else if (type == "InitialSetup")
                                {
                                    Console.WriteLine(msg);
                                    EntityManager.Instance.Clear();
                                    Console.WriteLine("initial setup");
                                    int count = msg.ReadInt32();
                                    Console.WriteLine(count);
                                    for (int i = 0; i < count; i++)
                                    {
                                        ushort id = msg.ReadUInt16();
                                        string entityType = msg.ReadString();
                                        if (entityType == "prop")
                                        {
                                            Console.WriteLine(entityType);
                                            Prop prop = new Prop(Vector2.Zero, 1);
                                            prop.UnpackPacket(msg);
                                            EntityManager.Instance.AddEntity(prop, id);
                                        }
                                        else if (entityType == "player" || entityType == "networkplayer")
                                        {
                                            Console.WriteLine(entityType);
                                            NetworkPlayer netp = new NetworkPlayer(Vector2.Zero, 0);
                                            
                                            netp.UnpackPacket(msg);
                                            EntityManager.Instance.AddEntity(netp, id);
                                        }
                                    }
                                    ushort netid = msg.ReadUInt16();
                                    Player player = new Player(new Vector2(200, 200), 0);
                                    //player.UnpackPacket(msg);
                                    Console.WriteLine(msg);
                                    
                                    EntityManager.Instance.AddEntity(player, netid);
                                    EntityManager.Instance.PlayerID = netid;
                                    clientInitialized = true;
                                }
                            }
                            break;
                    }
                    client.Recycle(msg);
                }
                Entity p = EntityManager.Instance.GetEntity(EntityManager.Instance.PlayerID);
                NetOutgoingMessage outMsg = client.CreateMessage();
                outMsg.Write("PositionUpdate");
                Console.WriteLine("Position changed on client side");
                outMsg.Write(EntityManager.Instance.PlayerID);
                p.PackPacket(outMsg);
                Console.WriteLine(outMsg);
                client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
            }
            #endregion
        }
    }
}
