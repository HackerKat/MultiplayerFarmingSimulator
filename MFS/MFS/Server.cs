using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace MFS
{
    public class Server
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

        private void ReceiveAndSendPositionUpdate(NetIncomingMessage msg)
        {
            ushort id = msg.ReadUInt16();
            Entity entity = EntityManager.Instance.GetEntity(id);

            entity.UnpackPacket(msg);

            var outMsg = PackPositionUpdate(id, entity);
            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        private NetOutgoingMessage PackPositionUpdate(ushort id, Entity entity)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            outMsg.Write((byte)PacketType.POSITION_UPDATE);
            outMsg.Write(id);
            entity.PackPacket(outMsg);
            return outMsg;
        }

        private void SendInitialSetup(NetIncomingMessage helloMsg)
        {
            Player newPlayer = new Player(new Vector2(500, 500), 1);
            NetOutgoingMessage outMsg = server.CreateMessage();
            var entities = EntityManager.Instance.GetAllEntities();
            outMsg.Write((byte)PacketType.INITIAL_SETUP);
            outMsg.Write(entities.Count);

            foreach (var element in entities)
            {
                ushort id = element.Key;
                Entity entity = element.Value;

                outMsg.Write(id);
                outMsg.Write((Byte)entity.EntityType);
                entity.PackPacket(outMsg);
            }

            ushort netPlayerID = EntityManager.Instance.AddEntity(newPlayer);
            outMsg.Write(netPlayerID);
            server.SendMessage(outMsg, helloMsg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
            AddPlayer(netPlayerID);
        }
        
        private void AddPlayer(ushort netPlayerID)
        {
            NetOutgoingMessage outMsg = server.CreateMessage();
            Player newPlayer = (Player)EntityManager.Instance.GetEntity(netPlayerID);

            outMsg.Write((byte)PacketType.ADD_PLAYER);
            outMsg.Write(netPlayerID);
            newPlayer.PackPacket(outMsg);
            server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
        }

        private void SendOwnPositionUpdate()
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

        private void SendOwnRemoveEntity()
        {
            var deletedIDs = EntityManager.Instance.DeletedIDs;
            
            if (deletedIDs.Count != 0)
            {
                foreach (ushort deletedID in deletedIDs)
                {
                    NetOutgoingMessage outMsg = null;
                    outMsg = server.CreateMessage();

                    outMsg.Write((byte)PacketType.REMOVE_ENTITY);
                    outMsg.Write(deletedID);
                    server.SendToAll(outMsg, NetDeliveryMethod.ReliableOrdered);
                }
                deletedIDs.Clear();
            }
        }

        private void ReceiveAndSendRemoveEntity(NetIncomingMessage msg)
        {
            ushort propid = msg.ReadUInt16();
            EntityManager.Instance.RemoveEntity(propid, false);

            NetOutgoingMessage outMsg = null;
            outMsg = server.CreateMessage();

            outMsg.Write((byte)PacketType.REMOVE_ENTITY);
            outMsg.Write(propid);
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
                                    ReceiveAndSendPositionUpdate(msg);
                                    break;
                                case PacketType.REMOVE_ENTITY:
                                    ReceiveAndSendRemoveEntity(msg);
                                    break;
                                case PacketType.HELLO:
                                    SendInitialSetup(msg);
                                    break;
                            }
                        }
                        break;
                }
                server.Recycle(msg);
            }
            SendOwnPositionUpdate();
            SendOwnRemoveEntity();
        }
    }
}
