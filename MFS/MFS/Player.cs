using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MFS
{
    public class Player : Entity
    {
        private InputManager inputManager;
        private Inventory inventory;
        public Vector2 Direction
        {
            get; set;
        }

        public Player(Vector2 position, ushort spriteID) 
            : base (EntityType.PLAYER, position, spriteID)
        {
            Sprite playerSprite = SpriteManager.Instance.GetSprite(spriteID);
            
            inputManager = InputManager.Instance;
            inventory = new Inventory();
        }

        public Inventory GetInventory()
        {
            return inventory;
        }
    }
}