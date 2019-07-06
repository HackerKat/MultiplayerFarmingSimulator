using System;
using System.Collections.Generic;
using Lidgren.Network;
using System.Net;
using Microsoft.Xna.Framework;
using System.Threading;

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
            NetOutgoingMessage outMsg = client.CreateMessage();
            outMsg.Write((byte)PacketType.HELLO);
            client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        //create positionUpdate packet
        public NetOutgoingMessage PositionUpdatePacket(ushort id, Entity entity)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            outMsg.Write((byte)PacketType.POSITION_UPDATE);
            outMsg.Write(id);
            entity.PackPacket(outMsg);
            return outMsg;
        }

        //should work for server an client
        public void PositionUpdate(NetIncomingMessage msg)
        {
            ushort id = msg.ReadUInt16();
            Entity entity = EntityManager.Instance.GetEntity(id);
            if (id != EntityManager.Instance.PlayerID)
            {
                entity.UnpackPacket(msg);
            }

            if (isHost)
            {
                var outMsg = PositionUpdatePacket(id, entity);
                server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
            }
        }

        #region Server InitialSetup
        public void InitialSetup(NetIncomingMessage msg)
        {
            NetworkPlayer netPlayer = new NetworkPlayer(Vector2.Zero, 0);
            NetOutgoingMessage outMsg = server.CreateMessage();
            var entities = EntityManager.Instance.GetAllEntities();
            outMsg.Write((byte)PacketType.INITIAL_SETUP);
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
            AddPlayer(netPlayerID);
        }
        #endregion

        #region Server AddPlayer
        public void AddPlayer(ushort netPlayerID)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            NetworkPlayer netPlayer = (NetworkPlayer)EntityManager.Instance.GetEntity(netPlayerID);

            outMsg.Write((byte)PacketType.ADD_PLAYER);
            outMsg.Write(netPlayerID);
            netPlayer.PackPacket(outMsg);
            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }
        #endregion

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
                                PacketType type = (PacketType)msg.ReadByte();
                                switch (type)
                                {
                                    case PacketType.POSITION_UPDATE:
                                        PositionUpdate(msg);
                                        break;
                                    case PacketType.HELLO:
                                        InitialSetup(msg);
                                        break;
                                }
                            } 
                            break;
                        }
                        server.Recycle(msg);
                    }
                    #region HostPositionUpdate
                    ushort id = EntityManager.Instance.PlayerID;
                    Entity entity = EntityManager.Instance.GetEntity(id);
                    NetOutgoingMessage outMsg = server.CreateMessage();
                    outMsg.Write((byte)PacketType.POSITION_UPDATE);
                    outMsg.Write(id);
                    entity.PackPacket(outMsg);

                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                    #endregion
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
                                    PacketType type = (PacketType)msg.ReadByte();
                                    switch (type)
                                    {
                                        case PacketType.POSITION_UPDATE:
                                            if (clientInitialized)
                                            {
                                                PositionUpdate(msg);
                                            }
                                            break;
                                        case PacketType.ADD_PLAYER:
                                        {
                                            //TODO: Refactor AddPlayer to it's own method (together with server-side AddPlayer?)
                                            NetworkPlayer newPlayer = new NetworkPlayer(new Vector2(100, 100), 0);
                                            ushort id = msg.ReadUInt16();
                                            if (id != EntityManager.Instance.PlayerID)
                                            {
                                                newPlayer.UnpackPacket(msg);
                                                EntityManager.Instance.AddEntity(newPlayer, id);
                                            }
                                        }
                                        break;
                                        case PacketType.INITIAL_SETUP:
                                        {
                                            //TODO: Refactor InitialSetup to it's own method (together with server-side InitialSetup?)
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
                                                    EntityManager.Instance.AddEntity(prop, id);
                                                }
                                                else if (entityType == "player" || entityType == "networkplayer")
                                                {
                                                    NetworkPlayer netp = new NetworkPlayer(Vector2.Zero, 0);
                                                    netp.UnpackPacket(msg);
                                                    EntityManager.Instance.AddEntity(netp, id);
                                                }
                                            }
                                            ushort netid = msg.ReadUInt16();
                                            Player player = new Player(new Vector2(200, 200), 0);

                                            EntityManager.Instance.AddEntity(player, netid);
                                            EntityManager.Instance.PlayerID = netid;
                                            InputManager.Instance.EntityToControlID = netid;
                                            clientInitialized = true;
                                        }
                                        break;
                                    }
                                
                                //    string type = msg.ReadString();
                                //    if (type == "PositionUpdate" && clientInitialized)
                                //    {
                                //        PositionUpdate(msg);
                                //    }
                                //    if (type == "AddPlayer")
                                //    {
                                //    //TODO: Refactor AddPlayer to it's own method (together with server-side AddPlayer?)
                                //        NetworkPlayer newPlayer = new NetworkPlayer(new Vector2(100, 100), 0);
                                //        ushort id = msg.ReadUInt16();
                                //        if (id != EntityManager.Instance.PlayerID)
                                //        {
                                //            newPlayer.UnpackPacket(msg);
                                //            EntityManager.Instance.AddEntity(newPlayer, id);
                                //        }
                                //    }
                                //    else if (type == "InitialSetup")
                                //    {
                                //    //TODO: Refactor InitialSetup to it's own method (together with server-side InitialSetup?)
                                //    EntityManager.Instance.Clear();
                                //    int count = msg.ReadInt32();

                                //    for (int i = 0; i < count; i++)
                                //    {
                                //        ushort id = msg.ReadUInt16();
                                //        string entityType = msg.ReadString();
                                //        if (entityType == "prop")
                                //        {
                                //            Prop prop = new Prop(Vector2.Zero, 1);
                                //            prop.UnpackPacket(msg);
                                //            EntityManager.Instance.AddEntity(prop, id);
                                //        }
                                //        else if (entityType == "player" || entityType == "networkplayer")
                                //        {
                                //            NetworkPlayer netp = new NetworkPlayer(Vector2.Zero, 0);
                                //            netp.UnpackPacket(msg);
                                //            EntityManager.Instance.AddEntity(netp, id);
                                //        }
                                //    }
                                //    ushort netid = msg.ReadUInt16();
                                //    Player player = new Player(new Vector2(200, 200), 0);

                                //    EntityManager.Instance.AddEntity(player, netid);
                                //    EntityManager.Instance.PlayerID = netid;
                                //    clientInitialized = true;
                                //}
                                }
                                break;
                        }
                        client.Recycle(msg);
                    }
                    if (clientInitialized)
                    {
                        Entity p = EntityManager.Instance.GetEntity(EntityManager.Instance.PlayerID);
                        NetOutgoingMessage outMsg = client.CreateMessage();
                        outMsg.Write((byte)PacketType.POSITION_UPDATE);
                        outMsg.Write(EntityManager.Instance.PlayerID);
                        p.PackPacket(outMsg);
                        client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
                    }
                }
                #endregion
            }
        }
}
