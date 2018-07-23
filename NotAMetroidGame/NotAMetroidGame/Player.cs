using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    public class Player : Creature
    {
        //keeps track of elapsed time since start of an attack
        private float attackTimer;

        //in an attacking state or not
        public bool attacking;

        public bool grounded;

        //bounding box of attack
        public BoundingBox hit;

        public BoundingBox feet;

        public float width = 16;
        public float height = 64;

        //Jump modifiers
        protected float fallMult = 2.5f;
        protected float shortJump = 15f;

        Animation walkRight;
        Animation walkLeft;
        Animation jump;
        Animation fall;
        Animation idle;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //Init placeholder image
            this.sprite = sprite = content.Load<Texture2D>("sprite_base_addon_2012_12_14");
            attacking = false;
            this.position = new Vector2(10, 10);
            this.velocity = new Vector2(0, 0);

            //Animation setup
            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(20, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(84, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(148, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(212, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(276, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(340, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(404, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(468, 88, 20, 30), TimeSpan.FromSeconds(.15));

            //Since we use the same frames for both, no need to assign twice.
            walkLeft = walkRight;

            idle = new Animation();
            idle.AddFrame(new Rectangle(20, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(84, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(148, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(212, 24, 20, 30), TimeSpan.FromSeconds(.25));

            jump = new Animation();
            jump.AddFrame(new Rectangle(148, 149, 20, 30), TimeSpan.FromSeconds(1));

            fall = new Animation();
            fall.AddFrame(new Rectangle(212, 149, 20, 30), TimeSpan.FromSeconds(1));

            currentAnimation = idle;
        }

        /**
         * (Likely to be altered or removed.  Used for boundingbox testing)
         * Attack updates the attack bounding box.
         * Returns whether or not the player is attacking.
         */
        public bool Attack(GameTime gameTime)
        {
            if (attacking)
            {
                attackTimer += gameTime.ElapsedGameTime.Milliseconds;
                hit = new BoundingBox(new Vector3(this.position.X - 100, this.position.Y + 30, 0),
                    new Vector3(this.position.X, this.position.Y + 40, 0));
                if (attackTimer > 100)
                {
                    attacking = false;
                    attackTimer = 0;
                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public void Update(GameTime gameTime, Level map)
        {
            base.Update(gameTime);

            feet = new BoundingBox(new Vector3(this.position.X, this.position.Y + 64, 0),
                new Vector3(this.position.X + 16, this.position.Y + 70, 0));

            if (this.velocity.Y > 0)
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (fallMult - 1));
                currentAnimation = jump;
                jump.Update(gameTime);

            }
            else if (this.velocity.Y < 0 && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (shortJump - 1));
                currentAnimation = fall;
                fall.Update(gameTime);
            }

            //check for collisions
            Collision(map);

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && grounded)
            {
                facing = 0;
                currentAnimation = walkRight;
                walkRight.Update(gameTime);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && grounded)
            {
                facing = 1;
                currentAnimation = walkLeft;
                walkLeft.Update(gameTime);
            }
            else if (grounded)
            {
                currentAnimation = idle;
                idle.Update(gameTime);
            }
            //Debug.WriteLine(position.X + "," + position.Y);
            Debug.WriteLine(this.velocity.Y);

            //This was originally in the Creature code.  Moved here for testing.
            this.velocity = Vector2.Add(this.velocity, (Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds));
        }

        /*
         * This function handles the complexities related to collisions. 
         */
        private void Collision(Level map)
        {
            // Keeps the player within the level boundaries
            if (this.position.X < map.left)
            {
                this.position.X = map.left;
            }
            if (this.position.X > map.right - width)
            {
                this.position.X = map.right - width;
            }


            Rectangle collisionArea = new Rectangle();
            collisionArea.X = Math.Min((int)this.position.X, (int)this.prevPosition.X) - 64;
            collisionArea.Y = Math.Min((int)this.position.Y, (int)this.prevPosition.Y) - 64;
            collisionArea.Width = Math.Abs((int)(this.position.X - this.prevPosition.X)) + (int)this.width + 64;
            collisionArea.Height = Math.Abs((int)(this.position.Y - this.prevPosition.Y)) + (int)this.height + 64;
            //Debug.WriteLine(collisionArea.X + " " + collisionArea.Y + " ");
            Object[] obstacles = map.GetTiles(collisionArea);

            int xDirection = 0;
            if (this.position.X < this.prevPosition.X)
            {
                xDirection = -1;
            }
            else if (this.position.X > this.prevPosition.X)
            {
                xDirection = 1;
            }
            int yDirection = 0;
            if (this.position.Y < this.prevPosition.Y)
            {
                yDirection = -1;
            }
            else if (this.position.Y > this.prevPosition.Y)
            {
                yDirection = 1;
            }
            bool prevState = grounded;
            grounded = false;
            for (int i = 0; i < obstacles.GetLength(0) && obstacles[i] != null; i++)
            {
                //Debug.WriteLine(i);
                Structure s = (Structure)obstacles[i];
                if (this.bound.Intersects(s.bound))
                {
                    if (yDirection > 0
                        && (this.prevPosition.Y + 50 < s.position.Y || prevState))
                    {
                        this.position.Y = (float)(s.position.Y - 60);
                        this.velocity = Vector2.Zero;
                        grounded = true;
                    }

                    else if (xDirection > 0)
                        this.position.X = s.position.X - this.width;
                    else if (xDirection < 0)
                        this.position.X = s.position.X + 64;
                }

            }
            /*
            if (this.position.Y >= 500)
            {
                //Debug.WriteLine("Grounded");
                this.velocity = Vector2.Zero;
                this.position.Y = 500f;
            }
            */
        }

        public override void Action(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
