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
        Animation hurt;
        Animation attack;

        //in an attacking state or not
        public bool attacking;

        //keeps track of elapsed time since start of an attack
        private float attackTimer;

        public Texture2D swordSprite;

        //bounding box of attack
        public BoundingBox hit;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //Init placeholder image

            this.position = new Vector2(10, 10);
            this.sprite = content.Load<Texture2D>("sprite_base_addon_2012_12_14");
            this.swordSprite = content.Load<Texture2D>("imageedit_1_2417391721");
            this.velocity = new Vector2(0, 0);

            attacking = false;

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));

            //Animation setup
            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(20, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(84, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(148, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(212, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(276, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(340, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(404, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(468, 88, 20, 30), TimeSpan.FromSeconds(.15), "walking");

            //Since we use the same frames for both, no need to assign twice.
            walkLeft = walkRight;

            idle = new Animation();
            idle.AddFrame(new Rectangle(20, 24, 20, 30), TimeSpan.FromSeconds(.25), "idle");
            idle.AddFrame(new Rectangle(84, 24, 20, 30), TimeSpan.FromSeconds(.25), "idle");
            idle.AddFrame(new Rectangle(148, 24, 20, 30), TimeSpan.FromSeconds(.25), "idle");
            idle.AddFrame(new Rectangle(212, 24, 20, 30), TimeSpan.FromSeconds(.25), "idle");

            jump = new Animation();
            jump.AddFrame(new Rectangle(148, 149, 20, 30), TimeSpan.FromSeconds(1), "jump");

            fall = new Animation();
            fall.AddFrame(new Rectangle(212, 149, 20, 30), TimeSpan.FromSeconds(1), "fall");

            hurt = new Animation();
            hurt.AddFrame(new Rectangle(148, 280, 20, 30), TimeSpan.FromSeconds(1), "hurt");

            attack = new Animation();
            attack.AddFrame(new Rectangle(20, 728, 27, 30), TimeSpan.FromSeconds(0.05), "attack");
            attack.AddFrame(new Rectangle(84, 728, 27, 30), TimeSpan.FromSeconds(0.15), "attack");
            attack.AddFrame(new Rectangle(148, 728, 27, 30), TimeSpan.FromSeconds(0.25), "attack");
            attack.AddFrame(new Rectangle(212, 728, 27, 30), TimeSpan.FromSeconds(0.25), "attack");
            attack.AddFrame(new Rectangle(276, 728, 27, 30), TimeSpan.FromSeconds(0.15), "attack");
            attack.AddFrame(new Rectangle(340, 728, 27, 30), TimeSpan.FromSeconds(0.05), "attack");

            currentAnimation = idle;

            this.speedCap = 250;

            scaleVector = new Vector2(2.0f, 2.0f);

            tint = Color.White;
        }

        /**
        * (Likely to be altered or removed.  Used for boundingbox testing)
        * Attack updates the attack bounding box.
        * Returns whether or not the player is attacking.
        */
        public void Attack(GameTime gameTime)
        {
            if (!attacking)
            {
                attacking = true;
                
            }
        }

        public override void Update(GameTime gameTime, Level map, Player player)
        {
            base.Update(gameTime, player);
            
            feet = new BoundingBox(new Vector3(this.position.X, this.position.Y + 64, 0),
                new Vector3(this.position.X + 16, this.position.Y + 70, 0));
                
            if (velocity.Y > 0)
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (fallMult - 1));
                currentAnimation = fall;
            }
            else if (this.velocity.Y < 0 && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (shortJump - 1));
                currentAnimation = fall;
            }


            //check for collisions
            Collision(map);

            //Recoil and invulnerability timers
            if (recoil)
            {
                if (this.position.Y >= 385)
                {
                    this.velocity.X = 0;
                }

                this.hitTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (hitTimer > 300 && this.position.Y >= 385)
                {
                    this.recoil = false;
                    this.hitTimer = 0;

                }
            }

            if (invuln)
            {
                this.invulnTimer += gameTime.ElapsedGameTime.Milliseconds;
                this.tint = Color.Red;

                if (invulnTimer > 800)
                {
                    //Debug.WriteLine("Invuln ended");
                    this.tint = Color.White;
                    this.invuln = false;
                    this.invulnTimer = 0;
                }
            }

            //Attack code
            if (attacking)
            {
                //Default behavior is for the player to stand still while attacking if they're on the ground
                if (position.Y >= 385)
                    velocity.X = 0;

                attackTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (facing == 0)
                {
                    hit = new BoundingBox(new Vector3(this.position.X + 37, this.position.Y + 20, 0),
                        new Vector3(this.position.X + 106, this.position.Y + 40, 0));
                }
                else
                {
                    hit = new BoundingBox(new Vector3(this.position.X - 62, this.position.Y + 20, 0),
                        new Vector3(this.position.X, this.position.Y + 40, 0));
                }

                if (attackTimer > 800)
                {
                    attacking = false;
                    attackTimer = 0;
                }

            }

            //Animations
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && grounded)
            {
                currentAnimation = walkRight;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && grounded)
            {
                currentAnimation = walkLeft;
            }
            else if (grounded)
            {
                currentAnimation = idle;
            }
            
            if (velocity.Y < 0)
            {
                currentAnimation = jump;
            }

            if (attacking)
            {
                currentAnimation = attack;
            }
            else
            {
                attack.Reset();
            }

            if (this.recoil)
                currentAnimation = hurt;

            currentAnimation.Update(gameTime);
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

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch);

            if (attacking)
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

        public void SetFacing(int facing)
        {
            this.facing = facing;
        }

        /**
         * Override from Creature.Damage 
         **/
        public override bool Damage(long damage, bool knockback)
        {
            if (knockback)
            {
                recoil = true;
                invuln = true;
                Vector2 newVel = Vector2.Zero;  

                if (velocity.Y > 0)
                {
                    if (this.getFacing() == 0)
                        newVel.X = -450;
                    else if (this.getFacing() == 1)
                        newVel.X = 450;
                    newVel.Y = -800;
                }
                velocity = newVel;
            }

            //Damage numerical calculation happens here

            return false;
        }

        public override void Action(GameTime gameTime, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
