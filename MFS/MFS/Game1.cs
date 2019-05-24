using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MFS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 50;

        Texture2D rock;
        Vector2 rockPosition = new Vector2(100, 100);
        Point rockFrameSize = new Point(26, 15);
        int rockCollisionOffset = 5;

        Texture2D character;
        Point charFrameSize = new Point(24, 24);
        Point charCurrentFrame = new Point(0, 0);
        Point charSheetSize = new Point(7, 1);
        Vector2 charPosition = Vector2.Zero;
        const float charSpeed = 6;
        int charCollisionOffset = 5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            texture = Content.Load<Texture2D>(@"Images/sensei");
            //each tile is  24x24 7 images total; 
            character = Content.Load<Texture2D>(@"Images/AnimatedSprites/Mani-idle-run");
            //load rcok sprite
            rock = Content.Load<Texture2D>(@"Images/generic-rpg-rock04");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;

                ++charCurrentFrame.X;

                if (charCurrentFrame.X >= charSheetSize.X)
                {
                    charCurrentFrame.X = 0;
                    ++charCurrentFrame.Y;
                    if (charCurrentFrame.Y >= charSheetSize.Y)
                        charCurrentFrame.Y = 0;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left))
                charPosition.X -= charSpeed;
            if (keyboardState.IsKeyDown(Keys.Right))
                charPosition.X += charSpeed;
            if (keyboardState.IsKeyDown(Keys.Up))
                charPosition.Y -= charSpeed;
            if (keyboardState.IsKeyDown(Keys.Down))
                charPosition.Y += charSpeed;

            if (charPosition.X < 0)
                charPosition.X = 0;
            if (charPosition.Y < 0)
                charPosition.Y = 0;
            if (charPosition.X > Window.ClientBounds.Width - charFrameSize.X)
                charPosition.X = Window.ClientBounds.Width - charFrameSize.X;
            if (charPosition.Y > Window.ClientBounds.Height - charFrameSize.Y)
                charPosition.Y = Window.ClientBounds.Height - charFrameSize.Y;

            if (Collide())
                Exit();

            base.Update(gameTime);
        }

        protected bool Collide()
        {
            Rectangle charRect = new Rectangle((int)charPosition.X + charCollisionOffset, (int)charPosition.Y + charCollisionOffset, 
                                                charFrameSize.X - (charCollisionOffset * 2), charFrameSize.Y - (charCollisionOffset * 2));
            Rectangle rockRect = new Rectangle((int)rockPosition.X + rockCollisionOffset, (int)rockPosition.Y + rockCollisionOffset, 
                                                rockFrameSize.X - (rockCollisionOffset * 2), rockFrameSize.Y - (rockCollisionOffset * 2));

            return charRect.Intersects(rockRect);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            spriteBatch.Draw(character, charPosition,
                new Rectangle(charCurrentFrame.X * charFrameSize.X,
                charCurrentFrame.Y * charFrameSize.Y,
                charFrameSize.X,
                charFrameSize.Y),
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 1
                );

            spriteBatch.Draw(rock, rockPosition, null,
                Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 0
                );
            spriteBatch.End();


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
