using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Skeleton : Creature
    {
        //Direction the skeleton is patrolling 0 = right, 1 = left
        private int path;

        private Weapon sword;

        //Animations
        private Animation stopped;
        private Animation walking;
        private Animation swing;

        //Skeleton's attack state
        private bool attacking;

        //Determines if the skeleton has walked into a wall
        private bool collided;

        //How long the skeleton spends recovering after a swing
        private float recoveryTimer;

        public float width = 16;
        public float height = 64;

        //Speed constants
        private readonly Vector2 RIGHT = new Vector2(150, 0);
        private readonly Vector2 LEFT = new Vector2(-150, 0);

        //These need to be changed to a less messy solution
        private Rectangle windupRear;
        private Rectangle swingFront;
        private Rectangle swingRear;
        private Rectangle recovFront;
        private Rectangle recovRear;

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 pos) : base(pos)
        {
            //Variable setup
            this.sprite = content.Load<Texture2D>("player");
            velocity = new Vector2(0, 0);
            size = new Vector2(45, 55);
            speedCap = 80;

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X, this.position.Y + 60, 0));
            prevBound = bound;

            sword = new Weapon("Skeleton sword", -1, 3);
            this.body = new Weapon("Skeleton body", -1, 1);

            path = 0;

            //Animation setup
            stopped = new Animation();
            stopped.AddFrame(new Rectangle(16, 0, 16, 32), TimeSpan.FromSeconds(.25), "stopped");

            walking = new Animation();
            walking.AddFrame(new Rectangle(64, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            walking.AddFrame(new Rectangle(112, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            walking.AddFrame(new Rectangle(160, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");

            swing = new Animation();
            swing.AddFrame(new Rectangle(448, 0, 16, 32), TimeSpan.FromSeconds(0.70), "windup");
            swing.AddFrame(new Rectangle(496, 0, 16, 32), TimeSpan.FromSeconds(0.15), "swing");
            swing.AddFrame(new Rectangle(544, 0, 16, 32), TimeSpan.FromSeconds(1), "recovery");

            windupRear = new Rectangle(432, 0, 16, 32);
            swingFront = new Rectangle(512, 0, 16, 32);
            swingRear = new Rectangle(480, 0, 16, 32);
            recovFront = new Rectangle(560, 0, 16, 32);
            recovRear = new Rectangle(528, 0, 16, 32);

            scaleVector = new Vector2(2.4f, 2.0f);

            tint = Color.Brown;
        }

        /**The skeleton's basic attack
         * Swings weapon directly in front of the skeleton
         * 
         * Changes state to attacking
         **/
        public void Swing()
        {
            if (!attacking)
            {
                attacking = true;
            }
        }

        public override void Action(GameTime gameTime, Player player)
        {
            if (!attacking)
            {
                path = facing;

                if (Math.Abs(player.position.X - this.position.X) < 130)
                {
                    path = 2;
                    if (player.position.X < this.position.X)
                    {
                        facing = 1;
                    }
                    else
                    {
                        facing = 0;
                    }

                    if (Math.Abs(player.position.X - this.position.X) < 80)
                    {
                        Swing();
                    }
                }

                if (position.X <= 10)
                    path = 0;

                if (position.X >= 740)
                    path = 1;

                if (collided)
                {
                    path = (path + 1) % 2;
                }

                if (path == 1)
                {
                    facing = 1;
                    this.Move(LEFT);
                }
                else if (path == 0)
                {
                    facing = 0;
                    this.Move(RIGHT);
                }
                else if (path == 2)
                {
                    velocity.X = 0;
                }

            }   

        }

        public override void Update(GameTime gameTime, Level map, Player player)
        {

            base.Update(gameTime, map, player);
            collided = Collision(map);

            this.tint = Color.Brown;

            if (attacking)
            {
                currentAnimation = swing;
                if (String.Equals(currentAnimation.getFrameName(), "swing"))
                {
                    if (facing == 0)
                    {
                        hit = new BoundingBox(new Vector3(this.position.X + 38, this.position.Y, 0),
                                     new Vector3(this.position.X + 76, this.position.Y + 30, 0));
                    }
                    else
                    {
                        hit = new BoundingBox(new Vector3(this.position.X - 36, this.position.Y, 0),
                                    new Vector3(this.position.X, this.position.Y + 30, 0));
                    }
                }
                else if (String.Equals(currentAnimation.getFrameName(), "recovery"))
                {
                    hit = new BoundingBox(new Vector3(0, 0, 0),
                                    new Vector3(0, 0, 0));
                    recoveryTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (recoveryTimer >= 700)
                    {
                        recoveryTimer = 0;
                        this.tint = Color.Brown;
                        attacking = false;
                        currentAnimation.Reset();
                        currentAnimation = stopped;
                    }
                }
            }
            else if (path == 0 || path == 1)
            {
                currentAnimation = walking;
            }
            else if (!attacking && path == 2)
            {
                currentAnimation = stopped;
            }
            currentAnimation.Update(gameTime);
            Action(gameTime, player);
            
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, camera);

            //Draw the supplemental attack sprites for animation.
            if (String.Equals(currentAnimation.getFrameName(), "windup"))
            {
                if (facing == 0)
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-38, 0), Vector2.Subtract(position, camera.position)), windupRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(38, 0), Vector2.Subtract(position, camera.position)), windupRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
            }
            else if (String.Equals(currentAnimation.getFrameName(), "swing"))
            {
                if (facing == 0)
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(38, 0), Vector2.Subtract(position, camera.position)), swingFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-38, 0), Vector2.Subtract(position, camera.position)), swingRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-38, 0), Vector2.Subtract(position, camera.position)), swingFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(38, 0), Vector2.Subtract(position, camera.position)), swingRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            else if (String.Equals(currentAnimation.getFrameName(), "recovery"))
            {
                if (facing == 0)
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(38, 0), Vector2.Subtract(position, camera.position)), recovFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-38, 0), Vector2.Subtract(position, camera.position)), recovRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-38, 0), Vector2.Subtract(position, camera.position)), recovFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(38, 0), Vector2.Subtract(position, camera.position)), recovRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                }
            }
        }
    }
}
