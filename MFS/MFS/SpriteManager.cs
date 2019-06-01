using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MFS
{
    //with XNA it should be Microsoft.Xna.Framework.DrawableGameComponent
    public class SpriteManager : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Player player;
        private List <Prop> props;

        public SpriteManager(Game game)
        : base (game)
        {
            props = new List<Prop>();
        }



        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //player
            Sprite playerSprite = new Sprite(Game.Content.Load<Texture2D>(@"Images/AnimatedSprites/mani-idle-run"), 
                                            new Point(24, 24), new Point(7, 1), 5);

            player = new Player(playerSprite, new Vector2(100, 100));

            //rock
            Sprite rockSprite = new Sprite(Game.Content.Load<Texture2D>(@"Images/generic-rpg-rock04"),
                                            new Point(26, 15));

            props.Add(new Prop(rockSprite, new Vector2(150, 150)));

            //sensei
            //spriteList.Add(new AutomatedSprite(
            //    Game.Content.Load<Texture2D>(@"Images/sensei"), new Vector2(300, 150), new Point(16, 23), 5, new Point(0, 0),
            //    new Point(1, 1), Vector2.Zero));
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            //Update all automated sprites
            foreach(Prop prop in props)
            {
                prop.Update(gameTime, Game.Window.ClientBounds);

                ////check for collision
                //if (s.collisionRect.Intersects(player.collisionRect))
                //{
                //    Game.Exit();
                //}
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            //draw player
            player.Draw(spriteBatch);

            //draw all automated sprites
            foreach (Prop prop in props)
            {
                prop.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}