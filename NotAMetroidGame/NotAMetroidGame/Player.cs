using System;
using System.Collections.Generic;
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

        public float width = 16;
        public float height = 64;

        //Jump modifiers
        protected float fallMult = 2.5f;
        protected float shortJump = 15f;

        private Weapon equippedWeapon;

        Animation walkRight;
        Animation walkLeft;
        Animation jump;
        Animation fall;
        Animation idle;
        Animation hurt;
        Animation attack;
        private int recoveryTimer;

        //These need to be changed to a less messy solution
        private Rectangle swingFront;
        private Rectangle swingRear;
        private Rectangle recovFront;
        private Rectangle recovRear;

        //public Texture2D swordSprite;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 pos) : base(pos)
        {
            //Init placeholder image
    
            this.sprite = content.Load<Texture2D>("player");
            this.velocity = new Vector2(0, 0);
            this.size = new Vector2(37, 60);

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X, this.position.Y + 60, 0));
            prevBound = bound;

            this.speedCap = 250;
            attacking = false;
            equippedWeapon = new Weapon("Longsword", 450, 5);

            //Animation setup
            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(64, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(113, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            walkRight.AddFrame(new Rectangle(160, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");


            //Since we use the same frames for both, no need to assign twice.
            walkLeft = walkRight;

            idle = new Animation();
            idle.AddFrame(new Rectangle(16, 0, 16, 32), TimeSpan.FromSeconds(.25), "idle");


            jump = new Animation();
            jump.AddFrame(new Rectangle(208, 0, 16, 32), TimeSpan.FromSeconds(1), "jump");

            fall = jump;

            hurt = new Animation();
            hurt.AddFrame(new Rectangle(384, 0, 16, 32), TimeSpan.FromSeconds(1), "hurt");

            attack = new Animation();
            attack.AddFrame(new Rectangle(255, 0, 16, 32), TimeSpan.FromSeconds(0.15), "windup");
            attack.AddFrame(new Rectangle(305, 0, 16, 32), TimeSpan.FromSeconds(0.15), "swing");
            attack.AddFrame(new Rectangle(352, 0, 16, 32), TimeSpan.FromSeconds(0.15), "recovery");

            swingFront = new Rectangle(320, 0, 16, 32);
            swingRear = new Rectangle(289, 0, 16, 32);
            recovFront = new Rectangle(367, 0, 16, 32);
            recovRear = new Rectangle(336, 0, 16, 32);

            currentAnimation = idle;

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
            base.Update(gameTime, map, player);
            Collision(map);

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

            //Recoil and invulnerability timers
            if (recoil)
            {
                if (this.position.Y >= 385)
                {
                    this.velocity.X = 0;
                }

                this.hitTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (hitTimer > 300 && this.grounded)
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
                if (this.Grounded())
                    velocity.X = 0;

                currentAnimation = attack;

                if (String.Equals(currentAnimation.getFrameName(), "swing"))
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
                    recoveryTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (recoveryTimer >= 150)
                    {
                        recoveryTimer = 0;
                        attacking = false;
                        currentAnimation.Reset();
                    }
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

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, camera);

            //Draw the supplemental attack sprites for animation.
            if (String.Equals(currentAnimation.getFrameName(), "swing"))
            {
                if (facing == 0)
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(16, 0), Vector2.Subtract(position, camera.position)), swingFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-16, 0), Vector2.Subtract(position, camera.position)), swingRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-16, 0), Vector2.Subtract(position, camera.position)), swingFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(16, 0), Vector2.Subtract(position, camera.position)), swingRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                }
            }
            else if (String.Equals(currentAnimation.getFrameName(), "recovery"))
            {
                if (facing == 0)
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(16, 0), Vector2.Subtract(position, camera.position)), recovFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-16, 0), Vector2.Subtract(position, camera.position)), recovRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(-16, 0), Vector2.Subtract(position, camera.position)), recovFront, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.Draw(sprite, Vector2.Add(new Vector2(16, 0), Vector2.Subtract(position, camera.position)), recovRear, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
                }
            }

        }

        /**
         * Override from Creature.Damage 
         **/
        public override bool Damage(Weapon source, bool knockback)
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

        //Getters and Setters

        public Weapon getWeapon()
        {
            return this.equippedWeapon;
        }

        public void SetFacing(int facing)
        {
            this.facing = facing;
        }

        public override void Action(GameTime gameTime, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
