using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        EntityManager entityManager;
        SpriteManager spriteManager;

        SpriteFont text;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteManager = SpriteManager.Instance;
            entityManager = EntityManager.Instance;
            spriteManager.Game = this;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            spriteManager.LoadContent();
            Player player = new Player( new Vector2(100, 100), 0);
            Prop rock01 = new Prop(new Vector2(200, 100), 1);
            Prop rock02 = new Prop(new Vector2(50, 50), 2);

            entityManager.AddEntity(player);
            entityManager.AddEntity(rock01);
            entityManager.AddEntity(rock02);

            text = Content.Load<SpriteFont>(@"font\text");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Rectangle clientBounds = this.Window.ClientBounds;

            entityManager.Update(gameTime, clientBounds);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            entityManager.Draw(spriteBatch);

            //Draw font
            spriteBatch.DrawString(text, "Welcome to my game", new Vector2(10, 10), Color.Black,
                0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
