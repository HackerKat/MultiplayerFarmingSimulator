using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Button
    {
        public delegate void ButtonClickFunc();

        private int buttonX;
        private int buttonY;
        private int width;
        private int height;
        private string title;
        private MouseState oldState;
        private ButtonClickFunc func;
        private Texture2D texture;

        public Button (string title, int buttonX, int buttonY, int width, int height, ButtonClickFunc func, Texture2D texture)
        {
            this.title = title;
            this.width = width;
            this.height = height;
            this.buttonX = buttonX;
            this.buttonY = buttonY;
            this.func = func;
            this.texture = texture;
            Rectangle button = new Rectangle (buttonX, buttonY, width, height);
        }

        public bool EnterButton()
        {
            MouseState state = Mouse.GetState();
            if (state.X < buttonX + width &&
                state.X > buttonX &&
                state.Y < buttonY + height &&
                state.Y > buttonY)
            {
                return true;
            }
            return false;
        }

        public void Update()
        {
            MouseState newState = Mouse.GetState();

            if (EnterButton() && newState.LeftButton == ButtonState.Pressed && 
                oldState.LeftButton == ButtonState.Released)
            {
                func();
            }

            oldState = newState;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont text)
        {
            spriteBatch.Draw(texture, new Vector2(buttonX, buttonY), Color.White);

            //Draw font
            int margin = 10;

            spriteBatch.DrawString(text, title, new Vector2(buttonX + margin, buttonY + margin), Color.Black,
                0, Vector2.Zero, 0.9f, SpriteEffects.None, 1);
        }
    }
}
