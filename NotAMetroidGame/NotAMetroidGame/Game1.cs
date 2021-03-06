﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Creature player;

        //Should probably have a better place for these,
        //maybe an enum class?
        public static Vector2 GRAV_CONSTANT;
        public static Vector2 RIGHT;
        public static Vector2 LEFT;
        public static Vector2 JUMP;

        KeyboardState OldKeyState;

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
            GRAV_CONSTANT = new Vector2(0, 900);
            RIGHT = new Vector2(250, 0);
            LEFT = new Vector2(-250, 0);
            JUMP = new Vector2(0, -600);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(Content);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up) && OldKeyState.IsKeyUp(Keys.Up) && player.position.Y >= 385)
                player.Move(JUMP, gameTime);

            if (kstate.IsKeyDown(Keys.Right))
                player.Move(RIGHT, gameTime);

            if (kstate.IsKeyDown(Keys.Left))
                player.Move(LEFT, gameTime);

            OldKeyState = kstate;
            player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.Draw(player.sprite, player.position, null, Color.White, 0, Vector2.Zero, 0.23f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
