using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Net;
using Microsoft.Xna.Framework;

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

        public NetworkManager(int port)
        {
            config = new NetPeerConfiguration("MFS");
            config.Port = port;            
        }

        public void Host()
        {
            server = new NetServer(config);
            server.Start();
            isHost = true;
        }

        public void Connect()
        {
            client = new NetClient(new NetPeerConfiguration("MFS"));
            client.Start();
            connectionToHost = client.Connect("127.0.0.1", config.Port);

            if (connectionToHost == null)
            {
                connected = false;
                return;
            }

            isHost = false;
            connected = true;

            NetOutgoingMessage hello = client.CreateMessage();
            hello.Write("Hello");
            client.SendMessage(hello, NetDeliveryMethod.ReliableOrdered);
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
                                    
                                    NetOutgoingMessage outMsg = server.CreateMessage();
                                    outMsg.Write("PositionUpdate");
                                    outMsg.Write(id);
                                    entity.PackPacket(outMsg);

                                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                                }
                                else if (type == "Hello")
                                {
                                    NetworkPlayer netPlayer = new NetworkPlayer(new Microsoft.Xna.Framework.Vector2(200,200), 0);
                                    
                                    NetOutgoingMessage outMsg = server.CreateMessage();
                                    var entities = EntityManager.Instance.GetAllEntities();
                                    outMsg.Write("InitialSetup");
                                    outMsg.Write(entities.Count);
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

                                    server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                                }
                            }
                            break;
                    }
                    server.Recycle(msg);
                }
                //foreach (var element in EntityManager.Instance.GetAllEntities())
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
                                if (type == "PositionUpdate")
                                {
                                    ushort id = msg.ReadUInt16();
                                    Entity entity = EntityManager.Instance.GetEntity(id);
                                    entity.UnpackPacket(msg);
                                }
                                else if (type == "InitialSetup")
                                {
                                    EntityManager.Instance.Clear();

                                    int count = msg.ReadInt32();
                                    for (int i = 0; i < count; i++)
                                    {
                                        ushort id = msg.ReadUInt16();
                                        string entityType = msg.ReadString();
                                        if (entityType == "prop")
                                        {
                                            Prop prop = new Prop(Vector2.Zero, 1);
                                            prop.UnpackPacket(msg);
                                            EntityManager.Instance.AddEntity(prop);
                                        }
                                        else if (entityType == "player" || entityType == "networkplayer")
                                        {
                                            NetworkPlayer netp = new NetworkPlayer(Vector2.Zero, 0);
                                            netp.UnpackPacket(msg);
                                            EntityManager.Instance.AddEntity(netp, id);
                                        }
                                        ushort netid = msg.ReadUInt16();
                                        Player player = new Player(Vector2.Zero, 0);
                                        player.UnpackPacket(msg);
                                        EntityManager.Instance.AddEntity(player, netid);
                                        EntityManager.Instance.PlayerID = netid;
                                    }
                                }
                            }
                            break;
                    }
                    client.Recycle(msg);
                }
                Entity p = EntityManager.Instance.GetEntity(EntityManager.Instance.PlayerID);
                NetOutgoingMessage outMsg = client.CreateMessage();
                outMsg.Write("PositionUpdate");
                outMsg.Write(EntityManager.Instance.PlayerID);
                p.PackPacket(outMsg);
            }
            #endregion
        }

    }
}
