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
        protected Point size;
        public Point Size
        {
            get
            {
                return size;
            }
            private set
            {
                size = value;
                CollisionRect = new Rectangle((int)position.X, (int)position.Y, size.X, size.Y);
            }
        }
        protected Rectangle collisionRect;
        public Rectangle CollisionRect
        {
            get
            {
                return collisionRect;
            }
            set
            {
                collisionRect = value;
            }
        }


        public Entity(Vector2 position, ushort spriteID)
        {
            this.position = position;
            this.spriteID = spriteID;

            Size = SpriteManager.Instance.GetSprite(spriteID).FrameSize;
            
            //TODO: add offset
            CollisionRect = new Rectangle((int)position.X, (int)position.Y, size.X, size.Y);
        }
        
        public abstract void Update(GameTime gameTime, Rectangle clientBounds);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract string GetEntityType();

        public virtual void PackPacket(NetOutgoingMessage msgToFill)
        {
            msgToFill.Write(position);
            msgToFill.Write(spriteID);
            msgToFill.Write(size);
        }

        public virtual void UnpackPacket(NetIncomingMessage msgToRead)
        {
            position = msgToRead.ReadVector2();
            spriteID = msgToRead.ReadUInt16();
            Size = msgToRead.ReadPoint();
        }
    }
}
