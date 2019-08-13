﻿using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//TODO: Major refactor of Client/Server (method renaming and delete all unnecessray methods)
namespace MFS
{
    class Client
    {
        private NetClient client;
        private bool connected;
        private int port;
        private bool clientInitialized;

        public Client(int port)
        {
            this.port = port;
            NetPeerConfiguration clientConfig = new NetPeerConfiguration("MFS");
            this.client = new NetClient(clientConfig);
            clientConfig.EnableMessageType(NetIncomingMessageType.Data);
        }

        public void Connect(string hostname)
        {
            client.Start();
            NetConnection connectionToHost = client.Connect(hostname, port);

            if (connectionToHost == null)
            {
                connected = false;
                return;
            }

            connected = true;
            clientInitialized = false;
            Thread.Sleep(500);
            NetOutgoingMessage outMsg = client.CreateMessage();
            outMsg.Write((byte)PacketType.HELLO);
            client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
        }
        
        public void ReceivePosition(NetIncomingMessage msg)
        {
            ushort id = msg.ReadUInt16();
            Entity entity = EntityManager.Instance.GetEntity(id);
            if (id != EntityManager.Instance.PlayerID)
            {
                entity.UnpackPacket(msg);
            }
        }

        public void UpdatePosition()
        {
            ushort id = EntityManager.Instance.PlayerID;
            Entity entity = EntityManager.Instance.GetEntity(id);

            NetOutgoingMessage outMsg = null;
           
            outMsg = client.CreateMessage();

            outMsg.Write((byte)PacketType.POSITION_UPDATE);
            outMsg.Write(id);
            entity.PackPacket(outMsg);
            
            client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void InitialSetup(NetIncomingMessage msg)
        {
            EntityManager.Instance.Clear();
            int count = msg.ReadInt32();
            
            for (int i = 0; i < count; i++)
            {
                ushort id = msg.ReadUInt16();
                Console.WriteLine(id);
                EntityType entityType = (EntityType)msg.ReadByte();
                switch (entityType)
                {
                    case EntityType.PROP:
                        {
                            Prop prop = new Prop(Vector2.Zero, 1);
                            prop.UnpackPacket(msg);
                            EntityManager.Instance.AddEntity(prop, id);
                        }
                        break;
                    case EntityType.PLAYER:
                        {
                            NetworkPlayer netp = new NetworkPlayer(Vector2.Zero, 1);
                            netp.UnpackPacket(msg);
                            EntityManager.Instance.AddEntity(netp, id);
                        }
                        break;
                    case EntityType.AXE:
                        {
                            Axe axe = new Axe(Vector2.Zero, 11);
                            axe.UnpackPacket(msg);
                            EntityManager.Instance.AddEntity(axe, id);
                        }
                        break;
                    case EntityType.VEGETABLE:
                        {
                            Vegetable vegetable = new Vegetable(Vector2.Zero, 11);
                            vegetable.UnpackPacket(msg);
                            EntityManager.Instance.AddEntity(vegetable, id);
                        }
                        break;
                }
            }
            clientInitialized = true;
            AddSelf(msg);
        }

        public void RemoveProp(NetIncomingMessage msg)
        {
            ushort propid = msg.ReadUInt16();
            EntityManager.Instance.RemoveEntity(propid, false);
        }

        public void AddSelf(NetIncomingMessage msg)
        {
            ushort netid = msg.ReadUInt16();
            Player player = new Player(new Vector2(500, 500), 1);

            EntityManager.Instance.AddEntity(player, netid);
            EntityManager.Instance.PlayerID = netid;
            InputManager.Instance.EntityToControlID = netid;
        }

       //TODO: when second player is added, position of first and second player changes!
        public void AddNetworkPlayer(NetIncomingMessage msg)
        {
            NetworkPlayer newPlayer = new NetworkPlayer(new Vector2(100, 100), 1);
            ushort id = msg.ReadUInt16();
            if (id != EntityManager.Instance.PlayerID)
            {
                newPlayer.UnpackPacket(msg);
                EntityManager.Instance.AddEntity(newPlayer, id);
            }
        }

        //TODO: should be public?
        private void RemoveEntity()
        {
            var deletedIDs = EntityManager.Instance.DeletedIDs;

            if (deletedIDs.Count != 0)
            {
                foreach (ushort deletedID in deletedIDs)
                {
                    NetOutgoingMessage outMsg = null;
                    outMsg = client.CreateMessage();

                    outMsg.Write((byte)PacketType.REMOVE_PROP);
                    outMsg.Write(deletedID);

                    client.SendMessage(outMsg, NetDeliveryMethod.ReliableOrdered);
                }
                deletedIDs.Clear();
            }
        }

        public void Update()
        {
            NetIncomingMessage msg;
            while ((msg = client.ReadMessage()) != null)
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
                                    ReceivePosition(msg);
                                }
                                break;
                            case PacketType.REMOVE_PROP:
                                    RemoveProp(msg);
                                    break;
                            case PacketType.ADD_PLAYER:
                                AddNetworkPlayer(msg);
                                break;
                            case PacketType.INITIAL_SETUP:
                                InitialSetup(msg);
                                break;
                        }
                    }
                    break;
                }
                client.Recycle(msg);
            }
            if (clientInitialized)
            {
                UpdatePosition();
                RemoveEntity();
            }
        }
    }
}
