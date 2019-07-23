using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFS
{
    public class InputManager
    {
        public ushort EntityToControlID
        {
            get;
            set;
        }
        //private KeyboardState keyState;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }
        private static InputManager instance;

        private InputManager()
        {
        }

        public void HandleInput()
        {
            Entity entity = EntityManager.Instance.GetEntity(EntityToControlID);
            
            Vector2 inputDirection = Vector2.Zero;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                inputDirection.X -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                inputDirection.X += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                inputDirection.Y -= 1;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                inputDirection.Y += 1;

            EntityManager.Instance.MoveEntity(EntityToControlID, inputDirection);

            //entity.Position += inputDirection;
        }

    }
}
