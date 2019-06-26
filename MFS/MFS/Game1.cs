using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    public class Game1 : Game
    {
        public enum GameState
        {
            MAINMENU,
            GAMEPLAY
        };

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private EntityManager entityManager;
        private SpriteManager spriteManager;
        private World world;
        private NetworkManager networkManager;
        private SpriteFont text;
        private GameState state;
        private string hostname;
        
        public Game1(string hostname)
        {
            this.hostname = hostname;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteManager = SpriteManager.Instance;
            entityManager = EntityManager.Instance;
            spriteManager.Game = this;
            world = new World(this.Window.ClientBounds);
            networkManager = new NetworkManager(666);
            state = GameState.MAINMENU;

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

            world.LoadTiles();
            world.GenerateWorld();

            text = Content.Load<SpriteFont>(@"font\text");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void updateMainMenu()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                networkManager.Host();
                state = GameState.GAMEPLAY;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                networkManager.Connect(hostname);
                state = GameState.GAMEPLAY;
            }
        }

        private void drawMainMenu()
        {

        }

        private void updateGameplay(GameTime gameTime)
        {
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Rectangle clientBounds = this.Window.ClientBounds;

            entityManager.Update(gameTime, clientBounds);
            networkManager.Update();
        }

        private void drawGameplay()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            world.Draw(spriteBatch);

            entityManager.Draw(spriteBatch);

            //Draw font
            spriteBatch.DrawString(text, "Welcome to my game", new Vector2(10, 10), Color.Black,
                0, Vector2.Zero, 1, SpriteEffects.None, 1);
            spriteBatch.End();
        }


        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.MAINMENU:
                    updateMainMenu();
                    break;
                case GameState.GAMEPLAY:
                    updateGameplay(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.MAINMENU:
                    drawMainMenu();
                    break;
                case GameState.GAMEPLAY:
                    drawGameplay();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
