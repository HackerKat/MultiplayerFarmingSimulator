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
        private Button startHost;
        private Button startClient;
        private Parser parser;

        public Game1(string hostname)
        {
            this.hostname = hostname;
            string path = @"D:\Documents\University\Bachelor\MultiplayerFarmingSimulator\MFS\MFS\map_1.json";
            parser = new Parser(path);
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = parser.GetMapWidth();  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = parser.GetMapHeight();   // set this value to the desired height of your window
            graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            spriteManager = SpriteManager.Instance;
            entityManager = EntityManager.Instance;
            spriteManager.Game = this;
            world = new World(this.Window.ClientBounds);
            networkManager = NetworkManager.Instance;
            state = GameState.MAINMENU;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            spriteManager.LoadContent();
            parser.TreeLayer();
            Player player = new Player( new Vector2(100, 100), 0);
            //Prop rock01 = new Prop(new Vector2(200, 100), 1);
            //Prop rock02 = new Prop(new Vector2(50, 50), 2);

            Texture2D buttonTexture = Content.Load<Texture2D>(@"Images\UI\button3");

            ushort entityID;

            entityID = entityManager.AddEntity(player);
            EntityManager.Instance.PlayerID = entityID;
            InputManager.Instance.EntityToControlID = entityID;

            //entityManager.AddEntity(rock01);
            //entityManager.AddEntity(rock02);

            world.LoadTiles();
            world.GenerateWorld();

            int padding = 10;
            int centerX = Window.ClientBounds.Width / 2;
            int centerY = Window.ClientBounds.Height / 2;


            startHost = new Button("Host game", centerX - 50, centerY - 40 - padding, 100, 40, StartHost, buttonTexture);
            startClient = new Button("Connect", centerX - 50, centerY - padding, 100, 40, StartClient, buttonTexture);

            text = Content.Load<SpriteFont>(@"font\text");
        }

        protected override void UnloadContent()
        {
           
        }

        private void UpdateMainMenu()
        {
            startHost.Update();
            startClient.Update();
        }

        private void StartHost()
        {
            networkManager.StartHost();
            state = GameState.GAMEPLAY;
        }

        private void StartClient()
        {
            networkManager.StartClient(hostname);
            state = GameState.GAMEPLAY;
        }

        private void DrawMainMenu()
        {
            this.IsMouseVisible = true;
            startHost.Draw(spriteBatch, text);
            startClient.Draw(spriteBatch, text);
        }

        private void updateGameplay(GameTime gameTime)
        {
            InputManager.Instance.HandleInput();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Rectangle clientBounds = this.Window.ClientBounds;

            entityManager.Update(gameTime, clientBounds);
            networkManager.Update();
        }

        private void DrawGameplay()
        {
            world.Draw(spriteBatch);

            entityManager.Draw(spriteBatch);

            //Draw font
            spriteBatch.DrawString(text, "Welcome to my game", new Vector2(10, 10), Color.Black,
                0, Vector2.Zero, 1, SpriteEffects.None, 1);
        }


        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.MAINMENU:
                    UpdateMainMenu();
                    break;
                case GameState.GAMEPLAY:
                    updateGameplay(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (state)
            {
                case GameState.MAINMENU:
                    DrawMainMenu();
                    break;
                case GameState.GAMEPLAY:
                    DrawGameplay();
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
