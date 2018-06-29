using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace NotAMetroidGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Creature testEnemy;
        Player testPlayer;

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
            GRAV_CONSTANT = new Vector2(0, 800);
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
            testPlayer = new Player(Content);
            testEnemy = new Skeleton(Content);
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

            if (kstate.IsKeyDown(Keys.Up) && OldKeyState.IsKeyUp(Keys.Up) && testPlayer.position.Y >= 385)
                testPlayer.Move(JUMP, gameTime);

            if (kstate.IsKeyDown(Keys.Right))
                testPlayer.Move(RIGHT, gameTime);

            if (kstate.IsKeyDown(Keys.Left))
                testPlayer.Move(LEFT, gameTime);

            // Used to test attacking
            if (kstate.IsKeyDown(Keys.Z))
                testPlayer.attacking = true;

            OldKeyState = kstate;
            testEnemy.Update(gameTime);
            testPlayer.Update(gameTime);

            // If attacking and attack boundingbox is intersecting the enemy
            if (testPlayer.Attack(gameTime) && testPlayer.hit.Intersects(testEnemy.bound))
            {
                Debug.WriteLine("HIT");
                testEnemy.Die();
            }

            /* Collision detection testing.  Does not prevent horizontal movement.
             * Vertical movement is halted when the player lands on the enemy to an
             * extent. Eventually vertical velocity will overcome the enemy boundingbox.
             */
            if (testEnemy.bound.Intersects(testPlayer.bound))
            {
                Debug.WriteLine("Collision");
                Vector2 newPosition = testPlayer.position;
                if (testPlayer.velocity.X > 0)
                    newPosition.X = testEnemy.position.X - 66;
                else if (testPlayer.velocity.X < 0)
                    newPosition.X = testEnemy.position.X + 66;
                if (testPlayer.velocity.Y > 0)
                    newPosition.Y = testEnemy.position.Y - 96;
                testPlayer.position = newPosition;
            }


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
            spriteBatch.Draw(testPlayer.sprite, testPlayer.position, testPlayer.tint);
            spriteBatch.Draw(testEnemy.sprite, testEnemy.position, testEnemy.tint);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
