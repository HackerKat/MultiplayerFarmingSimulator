using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS
{
    //with XNA it should be Microsoft.Xna.Framework.DrawableGameComponent
    public class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<Sprite> spriteList = new List<Sprite>();

        public SpriteManager(Game game)
        : base (game)
        {
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //player
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/AnimatedSprites/mani-idle-run"), Vector2.Zero,
                new Point(24, 24), 5, new Point(0, 0), new Point(7, 1), new Vector2(6, 6));

            //rock
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock04"), new Vector2(150, 150), new Point(26, 15), 5, new Point(0, 0),
                new Point(1, 1), Vector2.Zero));

            //sensei
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/sensei"), new Vector2(300, 150), new Point(16, 23), 5, new Point(0, 0),
                new Point(1, 1), Vector2.Zero));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            //Update all automated sprites
            foreach(Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);

                //check for collision
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    Game.Exit();
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //draw player
            player.Draw(gameTime, spriteBatch);

            //draw all automated sprites
            foreach (Sprite s in spriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}