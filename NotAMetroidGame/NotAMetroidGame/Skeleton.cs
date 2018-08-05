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

        public Texture2D swordSprite;

        //Speed constants
        private readonly Vector2 RIGHT = new Vector2(150, 0);
        private readonly Vector2 LEFT = new Vector2(-150, 0);

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 pos) : base(pos)
        {
            //Variable setup
            sprite = sprite = content.Load<Texture2D>("orc_skeleton_single");
            swordSprite = content.Load<Texture2D>("imageedit_1_2417391721");
            velocity = new Vector2(0, 0);
            size = new Vector2(45, 55);
            speedCap = 80;

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));
            prevBound = bound;

            path = 0;

            tint = Color.White;

            //Animation setup
            stopped = new Animation();
            stopped.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(1), "stopped");

            walking = stopped;

            swing = new Animation();
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(0.70), "windup");
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(0.33), "swing");
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(1), "recovery");

            scaleVector = new Vector2(0.7f, 0.7f);
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
                    this.Move(LEFT, gameTime);
                }
                else if (path == 0)
                {
                    facing = 0;
                    this.Move(RIGHT, gameTime);
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

            if (attacking)
            {
                currentAnimation = swing;

                if (String.Equals(currentAnimation.getFrameName(), "windup"))
                {
                    tint = Color.Blue;
                }
                else if (String.Equals(currentAnimation.getFrameName(), "swing"))
                {
                    if (facing == 0)
                    {
                        hit = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                                     new Vector3(this.position.X + 117, this.position.Y + 60, 0));
                    }
                    else
                    {
                        hit = new BoundingBox(new Vector3(this.position.X - 80, this.position.Y, 0),
                                    new Vector3(this.position.X + 37, this.position.Y + 60, 0));
                    }
                }
                else if (String.Equals(currentAnimation.getFrameName(), "recovery"))
                {
                    hit = new BoundingBox(new Vector3(0, 0, 0),
                                    new Vector3(0, 0, 0));
                    tint = Color.Yellow;
                    recoveryTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (recoveryTimer >= 700)
                    {
                        recoveryTimer = 0;
                        attacking = false;
                        tint = Color.White;
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

            if (String.Equals(currentAnimation.getFrameName(), "swing"))
            {
                if (this.facing == 0)
                {
                    spriteBatch.Draw(swordSprite, Vector2.Subtract(new Vector2(this.position.X + 37, this.position.Y + 20), camera.position), null, Color.White,
                        0, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(swordSprite, Vector2.Subtract(new Vector2(this.position.X - 62, this.position.Y + 20), camera.position), null, Color.White,
                        0, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.FlipHorizontally, 0);
                }
            }
        }
    }
}
