using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public enum Kind
    {
        TREE,
        HOUSE
    }

    public class Prop : Entity
    {
        public Kind Kind
        {
            get; set;
        }

        public Prop (Vector2 position, ushort spriteID)
            : base (EntityType.PROP, position, spriteID)
        {
            Sprite propSprite = SpriteManager.Instance.GetSprite(spriteID);
        }

        public override void PackPacket(NetOutgoingMessage msgToFill)
        {
            base.PackPacket(msgToFill);
            msgToFill.Write((byte)Kind);
        }

        public override void UnpackPacket(NetIncomingMessage msgToRead)
        {
            base.UnpackPacket(msgToRead);
            Kind = (Kind)msgToRead.ReadByte();
        }
    }
}