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

        public Weapon equippedWeapon;

        //Animation walkRight;
        //Animation walkLeft;
        Animation fall;
        Animation hurt;
        Animation attack;
        

        //These need to be changed to a less messy solution
        private Rectangle swingFront;
        private Rectangle swingRear;
        private Rectangle recovFront;
        private Rectangle recovRear;

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
            equippedWeapon = new Weapon("Longsword", 150, 5);


            //Setup dictionary for states
            stateList = new Dictionary<String, State>();
            stateList.Add("Idle", new IdleState(this));
            stateList.Add("Walk", new WalkState(this));
            stateList.Add("Jump", new JumpState(this));
            stateList.Add("Fall", new FallState(this));
            stateList.Add("Attack", new AttackState(this));

            swingFront = new Rectangle(320, 0, 16, 32);
            swingRear = new Rectangle(288, 0, 16, 32);
            recovFront = new Rectangle(368, 0, 16, 32);
            recovRear = new Rectangle(336, 0, 16, 32);

            changeState("Idle");

            scaleVector = new Vector2(2.4f, 2.0f);

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
                this.tint = Color.Red;
            else
                this.tint = Color.White;

        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            base.Draw(spriteBatch, camera);

            //Draw the supplemental attack sprites for animation.
            if (String.Equals(currentAnimation.getFrameName(), "swing"))
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

        /**
         * Override from Creature.Damage 
         **/
        public override bool Damage(Weapon source, bool knockback)
        {
            if (!invuln)
            {
                if (knockback)
                {
                    lastDamaged = source;
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
            }
            return false;
        }

        //Getters and Setters

        public Weapon getWeapon()
        {
            return this.equippedWeapon;
        }

        public override void Action(GameTime gameTime, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
