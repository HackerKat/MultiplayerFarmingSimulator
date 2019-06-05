﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        protected ushort spriteID;

        public Entity(Vector2 position, ushort spriteID)
        {
            this.position = position;
            this.spriteID = spriteID;
        }
        
        public abstract void Update(GameTime gameTime, Rectangle clientBounds);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
