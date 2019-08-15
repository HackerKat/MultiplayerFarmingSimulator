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
        private Button startHost;
        private Button startClient;
        private Parser parser;
        private GBBtutton inventoryButton;
        private Panel inventoryPanel;
        private UI ui;

        public Game1()
        {
            string path = @"map_2.json";
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
            parser.ParseJson();
            Player player = new Player( new Vector2(300, 300), 1);

            ushort entityID;

            entityID = entityManager.AddEntity(player);
            EntityManager.Instance.PlayerID = entityID;
            InputManager.Instance.EntityToControlID = entityID;

            world.LoadTiles();
            world.GenerateWorld();

            ui = new UI(Content);
            ui.OnHost += StartHost;
            ui.OnConnect += StartClient;
            ui.OnInventory += OpenInventory;
            ui.EnableStartScreen();
        }

        protected override void UnloadContent()
        {
           
        }

        private void OpenInventory(Panel inventoryPanel)
        {
            inventoryPanel.ClearChildren();
            ushort playerid = entityManager.PlayerID;
            Player player = (Player)entityManager.GetEntity(playerid);

            List<Item> inventory = player.GetInventory().GetAllItems();

            foreach (Item item in inventory)
            {
                ushort spriteid = item.SpriteID;
                Sprite sprite = spriteManager.GetSprite(spriteid);
                Image img = sprite.GetImage();

                img.OnClick += (GBEntity ent) => item.Use();

                inventoryPanel.AddChild(img);
            }
            
        }

        private void UpdateMainMenu()
        {
            
        }

        private void StartHost()
        {
            networkManager.StartHost();
            state = GameState.GAMEPLAY;
            ui.DissableStartScreen();
            ui.EnableInGameUI();
        }

        private void StartClient(string hostname)
        {
            networkManager.StartClient(hostname);
            state = GameState.GAMEPLAY;
            ui.DissableStartScreen();
            ui.EnableInGameUI();
        }

        private void DrawMainMenu()
        {
            this.IsMouseVisible = false;
            //startHost.Draw(spriteBatch, text);
            //startClient.Draw(spriteBatch, text);
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
            ui.BeginDraw(spriteBatch);

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
            ui.EndDraw(spriteBatch);
           // UserInterface.Active.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
