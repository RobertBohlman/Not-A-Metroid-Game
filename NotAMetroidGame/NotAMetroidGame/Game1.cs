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
        Level level;
        Player player;

        //Should probably have a better place for these,
        //maybe an enum class?
        public static readonly Vector2 GRAV_CONSTANT = new Vector2(0, 900);
        public static readonly Vector2 RIGHT = new Vector2(250, 0);
        public static readonly Vector2 LEFT = new Vector2(-250, 0);
        public static readonly Vector2 JUMP = new Vector2(0, -600);

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
            level = new Test_00();
            player = new Player(Content, level.GetSpawnLocation());
            player.position = level.GetSpawnLocation();
            level.InitMap(Content);

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
            if (level.nextLevel != null)
            {
                level = level.nextLevel;
                level.InitMap(Content);
            }
            //Player controls
            var kstate = Keyboard.GetState();
            
            if (kstate.IsKeyDown(Keys.Right) && !player.recoil && (!player.Grounded() || !player.attacking))
            {
                player.SetFacing(0);
                player.Move(RIGHT, gameTime);
            }

            if (kstate.IsKeyDown(Keys.Left) && !player.recoil && (!player.Grounded() || !player.attacking))
            {
                player.SetFacing(1);
                player.Move(LEFT, gameTime);
            }
                

            if (!player.attacking && !player.recoil)
            {
                if (kstate.IsKeyDown(Keys.Up) && OldKeyState.IsKeyUp(Keys.Up) && player.Grounded())
                    player.Move(JUMP, gameTime);

                if (kstate.IsKeyDown(Keys.Space))
                    player.Attack(gameTime);

                if (kstate.IsKeyUp(Keys.Left) && OldKeyState.IsKeyDown(Keys.Left))
                    player.velocity.X = 0;

                if (kstate.IsKeyUp(Keys.Right) && OldKeyState.IsKeyDown(Keys.Right))
                    player.velocity.X = 0;
            }
            OldKeyState = kstate;
            player.Update(gameTime, level, player);
            camera.Update(player.position, graphics);
            level.Update(gameTime, player, camera);

            //Player to enemy hit detection
            if (player.attacking)
            {
                foreach (Creature enemy in level.GetCreatures())
                {
                    if (player.hit.Intersects(enemy.bound))
                        Debug.WriteLine("HIT");
                }   
            }

            //Enemy to player hit detection.
            foreach (Creature enemy in level.GetCreatures())
            {
                if ((enemy.bound.Intersects(player.bound) || enemy.hit.Intersects(player.bound)) && !player.invuln)
                    player.Damage(enemy.getBody(), true);
            }
            
            Debug.WriteLine("");
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
            level.Draw(spriteBatch, camera);
            player.Draw(spriteBatch, camera);
            foreach (Creature enemy in level.GetCreatures())
                enemy.Draw(spriteBatch, camera);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
