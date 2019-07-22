using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network.Xna;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public abstract class Entity
    {
        protected Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        protected ushort spriteID;

        public Entity(Vector2 position, ushort spriteID)
        {
            this.position = position;
            this.spriteID = spriteID;
        }
        
        public abstract void Update(GameTime gameTime, Rectangle clientBounds);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract string GetEntityType();

        public virtual void PackPacket(NetOutgoingMessage msgToFill)
        {
            msgToFill.Write(position);
            msgToFill.Write(spriteID);
        }

        public virtual void UnpackPacket(NetIncomingMessage msgToRead)
        {
            position = msgToRead.ReadVector2();
            spriteID = msgToRead.ReadUInt16();
        }
    }
}
