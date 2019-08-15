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
    public enum EntityType
    {
        PROP,
        PLAYER,
        ITEM,
        AXE,
        VEGETABLE,
        RESOURCE
    }

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
        public ushort SpriteID
        {
            get
            {
                return spriteID;
            }
            private set
            {
                spriteID = value;
            }
        }
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
                CollisionRect = new Rectangle((int)position.X, (int)position.Y, size.X - OFFSET, size.Y - OFFSET);
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
        protected EntityType entityType;
        public EntityType EntityType
        {
            get
            {
                return entityType;
            }
        }
        private const int OFFSET = 10;

        public Entity(EntityType entityType, Vector2 position, ushort spriteID)
        {
            this.entityType = entityType;
            this.position = position;
            this.spriteID = spriteID;

            Size = SpriteManager.Instance.GetSprite(spriteID).FrameSize;

            CollisionRect = new Rectangle((int)position.X, (int)position.Y, size.X - OFFSET, size.Y - OFFSET);
        }

        public virtual void BoundsCheck(Rectangle clientBounds)
        {
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - CollisionRect.Width)
                position.X = clientBounds.Width - CollisionRect.Width;
            if (position.Y > clientBounds.Height - CollisionRect.Height)
                position.Y = clientBounds.Height - CollisionRect.Height;
        }

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

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            Sprite entitySprite = SpriteManager.Instance.GetSprite(spriteID);

            BoundsCheck(clientBounds);

            entitySprite.Animate(gameTime);

            collisionRect.X = (int)position.X;
            collisionRect.Y = (int)position.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite entitySprite = SpriteManager.Instance.GetSprite(spriteID);
            entitySprite.Draw(spriteBatch, position);
        }
    }
}
