﻿using System;
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

        Animation walkRight;
        Animation walkLeft;
        Animation jump;
        Animation fall;
        Animation idle;
        Animation hurt;
        Animation attack;

        public Texture2D swordSprite;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 pos) : base(pos)
        {
            //Init placeholder image
    
            this.sprite = content.Load<Texture2D>("sprite_base_addon_2012_12_14");
            this.swordSprite = content.Load<Texture2D>("imageedit_1_2417391721");
            this.velocity = new Vector2(0, 0);
            this.size = new Vector2(37, 60);

            attacking = false;

            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));
            prevBound = bound;

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

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, camera);

            if (attacking)
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
