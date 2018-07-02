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
        Camera camera;
        Creature testEnemy;
        Player player;
        Map map;


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
            camera = new Camera();
            testEnemy = new Skeleton(Content);
            player = new Player(Content);
            map = new Map();
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

            // Used to test attacking
            if (kstate.IsKeyDown(Keys.Z))
                player.attacking = true;

            OldKeyState = kstate;
            player.Update(gameTime);

            // If attacking and attack boundingbox is intersecting the enemy
            if (player.Attack(gameTime) && player.hit.Intersects(testEnemy.bound))
            {
                Debug.WriteLine("HIT");
                testEnemy.Die();
            }

            /* Collision detection testing.  Does not prevent horizontal movement.
             * Vertical movement is halted when the player lands on the enemy to an
             * extent. Eventually vertical velocity will overcome the enemy boundingbox.
             */
            if (testEnemy.bound.Intersects(player.bound))
            {
                Debug.WriteLine("Collision");
                Vector2 newPosition = player.position;
                if (player.velocity.X > 0)
                    newPosition.X = testEnemy.position.X - 66;
                else if (player.velocity.X < 0)
                    newPosition.X = testEnemy.position.X + 66;
                if (player.velocity.Y > 0)
                    newPosition.Y = testEnemy.position.Y - 96;
                player.position = newPosition;
            }

            if (player.position.X < map.left)
            {
                player.position.X = map.left;
            }
            if (player.position.X > map.right - 66)
            {
                player.position.X = map.right - 66;
            }

            camera.Update(player.position);

            if (camera.position.X < map.left)
            {
                camera.position.X = map.left;
            }
            if (camera.position.X > map.right - 800)
            {
                camera.position.X = map.right - 800;
            }
            if (camera.position.Y < map.top)
            {
                camera.position.Y = map.top;
            }
            if (camera.position.Y > map.bottom - 600)
            {
                camera.position.Y = map.bottom - 600;
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

            spriteBatch.Draw(testEnemy.sprite, Vector2.Subtract(testEnemy.position, camera.position), testEnemy.tint);
            spriteBatch.Draw(player.sprite, Vector2.Subtract(player.position, camera.position), null, Color.White, 0, Vector2.Zero, 0.23f, SpriteEffects.None, 0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
