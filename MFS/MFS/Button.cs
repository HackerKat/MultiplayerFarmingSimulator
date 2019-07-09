using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Button
    {
        private int buttonX;
        private int buttonY;
        private int size;
        private string title;

        public Button (string title, int buttonX, int buttonY, int size)
        {
            this.title = title;
            this.size = size;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
            Rectangle button = new Rectangle (buttonX, buttonY, size, size);
        }

        public bool entersButton()
        {
            MouseState state = Mouse.GetState();
            if (state.X < buttonX + size &&
                state.X > buttonX &&
                state.Y < buttonY + size &&
                state.Y > buttonY)
            {
                return true;
            }

            return false;
        }

        public void Update()
        {
            MouseState newState = Mouse.GetState();
            MouseState oldState = Mouse.GetState();

            if (entersButton() && newState.LeftButton == ButtonState.Pressed &&
                oldState.LeftButton == ButtonState.Released)
            {

            }
        }

       
    }
}
