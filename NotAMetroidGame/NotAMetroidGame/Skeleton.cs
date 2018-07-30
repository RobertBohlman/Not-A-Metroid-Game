using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Skeleton : Creature
    {
        //How long this enemy waits before switching patrol paths
        private int pathTimer;

        //Direction the skeleton is patrolling 0 = right, 1 = left
        private int path;

        private int stop;

        private Animation stopped;
        private Animation walking;
        private Animation swing;

        private bool attacking;

        private float recoveryTimer;

        public Texture2D swordSprite;

        private Vector2 RIGHT;
        private Vector2 LEFT;

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.sprite = sprite = content.Load<Texture2D>("orc_skeleton_single");
            this.swordSprite = content.Load<Texture2D>("imageedit_1_2417391721");
            this.position = new Vector2(350, 385);
            this.velocity = new Vector2(0, 0);

            this.speedCap = 150;

            stopped = new Animation();
            stopped.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(1), "stopped");

            walking = stopped;

            swing = new Animation();
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(0.70), "windup");
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(0.33), "swing");
            swing.AddFrame(new Rectangle(0, 0, 66, 96), TimeSpan.FromSeconds(1), "recovery");

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));

            RIGHT = new Vector2(150, 0);
            LEFT = new Vector2(-150, 0);

            pathTimer = 0;
            path = 0;

            speedCap = 80;

            tint = Color.White;

            scaleVector = new Vector2(0.7f, 0.7f);
        }

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

        public override void Update(GameTime gameTime, Player player)
        {

            base.Update(gameTime, player);

            //Hard coded floor
            if (this.Grounded())
            {
                //Debug.WriteLine("Grounded");
                this.velocity.Y = 0;
                this.position.Y = 385;
            }

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
                        bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                                     new Vector3(this.position.X + 117, this.position.Y + 60, 0));
                    }
                    else
                    {
                        bound = new BoundingBox(new Vector3(this.position.X - 80, this.position.Y, 0),
                                    new Vector3(this.position.X + 37, this.position.Y + 60, 0));
                    }
                }
                else if (String.Equals(currentAnimation.getFrameName(), "recovery"))
                {
                    bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                            new Vector3(this.position.X + 37, this.position.Y + 60, 0));
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

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(this.sprite, this.position, null, Color.White, 0, Vector2.Zero, 0.23f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);

            if (String.Equals(currentAnimation.getFrameName(), "swing"))
            {
                if (this.facing == 0)
                {
                    spriteBatch.Draw(swordSprite, new Vector2(this.position.X + 37, this.position.Y + 20), null, Color.White,
                        0, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(swordSprite, new Vector2(this.position.X - 62, this.position.Y + 20), null, Color.White,
                        0, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.FlipHorizontally, 0);
                }
            }
        }
    }
}
