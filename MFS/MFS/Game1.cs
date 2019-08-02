using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

// using GeonBit UI elements
using GeonBit.UI;
using GeonBit.UI.Entities;

using GBBtutton = GeonBit.UI.Entities.Button;
using GBEntity = GeonBit.UI.Entities.Entity;
using System.Collections.Generic;

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
        private GBBtutton button;
        private Panel inventoryPanel;

        public Game1(string hostname)
        {
            this.hostname = hostname;
            string path = @"map_1.json";
            //string path = @"D:\Documents\University\Bachelor\MultiplayerFarmingSimulator\MFS\MFS\testmap.json";
            parser = new Parser(path);
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = parser.GetMapWidth();  // set this value to the desired width of your window
            Console.WriteLine(parser.GetMapWidth());
            Console.WriteLine(parser.GetMapHeight());
            graphics.PreferredBackBufferHeight = parser.GetMapHeight();   // set this value to the desired height of your window
            graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            UserInterface.Initialize(Content, BuiltinThemes.hd);

            button = new GBBtutton("Inventory", anchor: Anchor.BottomRight,  size: new Vector2(300, 50));
            button.Visible = false;
            button.ToggleMode = true;
            button.OnValueChange = OpenInventory;
            UserInterface.Active.AddEntity(button);

            inventoryPanel = new Panel(new Vector2(300, 300));
            inventoryPanel.Visible = false;
            UserInterface.Active.AddEntity(inventoryPanel);

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
            parser.ObjectLayer();
            Player player = new Player( new Vector2(300, 300), 0);
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

        private void OpenInventory(GBEntity entity)
        {
            if (button.Checked)
            {
                inventoryPanel.ClearChildren();
                ushort playerid = entityManager.PlayerID;
                Player player = (Player)entityManager.GetEntity(playerid);

                List<Prop> inventory = player.GetInventory();

                foreach (Prop item in inventory)
                {
                    ushort spriteid = item.GetSpriteID();
                    Sprite sprite = spriteManager.GetSprite(spriteid);

                    inventoryPanel.AddChild(sprite.GetImage());
                }
                inventoryPanel.Visible = true;
            }
            else
            {
                inventoryPanel.Visible = false;
            }
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


            button.Visible = true;
        }

        private void StartClient()
        {
            networkManager.StartClient(hostname);
            state = GameState.GAMEPLAY;


            button.Visible = true;
        }

        private void DrawMainMenu()
        {
            this.IsMouseVisible = true;
            startHost.Draw(spriteBatch, text);
            startClient.Draw(spriteBatch, text);
        }

        private void UpdateGameplay(GameTime gameTime)
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

            //button.Visible = true;

            ////Draw font
            //spriteBatch.DrawString(text, "Welcome to my game", new Vector2(10, 10), Color.Red,
            //    0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }


        protected override void Update(GameTime gameTime)
        {
            // GeonBit.UIL update UI manager
            UserInterface.Active.Update(gameTime);
            switch (state)
            {
                case GameState.MAINMENU:
                    UpdateMainMenu();
                    break;
                case GameState.GAMEPLAY:
                    UpdateGameplay(gameTime);
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

            // GeonBit.UI: draw UI using the spriteBatch you created above
            UserInterface.Active.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
