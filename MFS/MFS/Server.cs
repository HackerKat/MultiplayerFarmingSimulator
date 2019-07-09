using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace MFS
{
    class Server
    {
        private NetServer server;
        private NetPeerConfiguration config;
       

        public Server(NetPeerConfiguration config)
        {
            this.config = config;
            this.server = new NetServer(config);
        }

        public void Host()
        {
            server.Start();
        }

        public void SendPositionUpdate(NetIncomingMessage msg)
        {
            ushort id = msg.ReadUInt16();
            Entity entity = EntityManager.Instance.GetEntity(id);

            entity.UnpackPacket(msg);

            var outMsg = PacketPositionUpdate(id, entity);
            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public NetOutgoingMessage PacketPositionUpdate(ushort id, Entity entity)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            outMsg.Write((byte)PacketType.POSITION_UPDATE);
            outMsg.Write(id);
            entity.PackPacket(outMsg);
            return outMsg;
        }

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
        
        public void AddPlayer(ushort netPlayerID)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            NetworkPlayer netPlayer = (NetworkPlayer)EntityManager.Instance.GetEntity(netPlayerID);

            outMsg.Write((byte)PacketType.ADD_PLAYER);
            outMsg.Write(netPlayerID);
            netPlayer.PackPacket(outMsg);
            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void UpdatePosition()
        {
            ushort id = EntityManager.Instance.PlayerID;
            Entity entity = EntityManager.Instance.GetEntity(id);

            NetOutgoingMessage outMsg = null;
            outMsg = server.CreateMessage();

            outMsg.Write((byte)PacketType.POSITION_UPDATE);
            outMsg.Write(id);
            entity.PackPacket(outMsg);

            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        public void Update()
        {
            NetIncomingMessage msg;

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
                                    SendPositionUpdate(msg);
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
            UpdatePosition();
        }
    }
}
