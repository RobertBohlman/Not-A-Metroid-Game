using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public abstract class Creature : Entity
    {
        //This may or may not stay as an int
        private int HP;

        //Reduces damage taken
        private int armor;

        //recoil state after taking damage
        public bool recoil;

        //Time since last knocked by a hit
        public int hitTimer;

        //invulnerability state after recoil
        public bool invuln;

        //Time since invulnerability period started
        public int invulnTimer;

        //Cap for horizontal speed.
        public int speedCap;

        protected Weapon body;
        
        protected bool grounded;

        public BoundingBox feet;

        public BoundingBox prevBound;

        //The last thing this creature took damage from.
        protected Weapon lastDamaged;

        //bounding box of attack
        public BoundingBox hit;

        //Size of the creature
        protected Vector2 size;

        //Creature's current state
        private State currentState;

        protected IDictionary<String, State> stateList;

        public Creature(Vector2 pos)
        {
            this.position = pos;
        }

        /// <summary>
        /// Adjust this creature's velocity by adding the movement vector.
        /// </summary>
        /// <param name="movement"></param>
        /// <param name="gameTime"></param>
        public void Move(Vector2 movement)
        {
            this.velocity = Vector2.Add(this.velocity, movement);
        }

        /** Deals X damage to this creature's HP, returns true if this damage is lethal.
         * 
         * Example: Player lands a hit on enemy x, code checks the stats on the 
         * player's weapon, charatcer, etc and calls x.Damage(howevermuch damage should be done).
         * This function takes into account armor and other stats we decide to consider.
         * 
         * Some enemies may have special effects or resistances when they take damage.
         * For this instance, that enemy can override this method.
         * 
         * Takes the numerical damage as a long value, and a bool to
         * determine if this causes the knockback/stun behavior (For player, enemies will knock them back
         * but certain environmental hazards may not).
         * 
         **/
        public virtual bool Damage(Weapon source, bool knockback)
        {
            if (!invuln)
            {
                lastDamaged = source;
                this.tint = Color.Green;
                invuln = true;
            }
            return false;

        }

        //*Insert Zelda CD-I reference here*
        //(6/26/2018 : Changed the tint of the sprite to visually show that the creature died.
        public void Die()
        {
            tint = Color.Red;
        }

        /// <summary>
        /// Periodic update method for creatures
        /// Handles adjustment of positon by velocity, gravity, collision and physics
        /// Also changes creature state depending on attack, idle, etc
        /// Overriden by each creature and player
        /// </summary>
        /// <param name="gameTime">Game Time object</param>
        /// <param name="map">Current level</param>
        /// <param name="player">Player reference, used for enemy AI</param>
        public virtual void Update(GameTime gameTime, Level map, Player player)
        {
            currentState.Update(gameTime);
            Debug.WriteLine("X: " + this.velocity.X);
            Debug.WriteLine("Y: " + this.velocity.Y);
            //Debug.WriteLine(this.currentState);

            this.prevPosition = new Vector2(this.position.X, this.position.Y);
            
            if (Math.Abs(this.velocity.X) > this.speedCap)
            {
                if (this.velocity.X > 0)
                {
                    this.velocity.X = speedCap;
                }
                else
                {
                    this.velocity.X = speedCap * -1;
                }
            }
            this.position = Vector2.Add(this.position, (this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds));
            this.velocity = Vector2.Add(this.velocity, (Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds));

            prevBound = bound;
            UpdateBounds();

            //invuln timer
            if (invuln)
            {
                this.invulnTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (invulnTimer > lastDamaged.invulnTime)
                {
                    //Debug.WriteLine("Invuln ended");
                    this.invuln = false;
                    this.invulnTimer = 0;
                }
            }
            
        }

        /// <summary>
        /// Checks for any collisions with Structures on the current level.
        /// </summary>
        /// <param name="level"></param>
        protected bool Collision(Level level)
        {
            List<Structure> structures = level.GetStructures();
            //The player is falling unless a floor is detected
            bool wallCollide = false;
            grounded = false;
            structures.ForEach((s) =>
            {
                BoundingBox obj = s.GetBounds();
                if (obj.Intersects(bound))
                {
                    //Checking if the player landed on the object
                    if (prevPosition.Y < position.Y && prevBound.Max.Y < obj.Min.Y)
                    {
                        position.Y = obj.Min.Y + bound.Min.Y - bound.Max.Y;
                        velocity.Y = 0;
                    }
                    //Colliding with the bottom of the structure
                    else if (prevPosition.Y > position.Y && prevBound.Min.Y > obj.Max.Y)
                    {
                        position.Y = obj.Max.Y;
                        velocity.Y = 0;
                    }
                    //Colliding with the left side of the structure
                    else if (prevPosition.X < position.X && Math.Abs(obj.Min.Y - bound.Max.Y) > 0.001)
                    {
                        position.X = obj.Min.X + bound.Min.X - bound.Max.X;
                        velocity.X = 0;
                        wallCollide = true;
                    }
                    //Colliding with the right side of the structure
                    else if (prevPosition.X > position.X && Math.Abs(obj.Min.Y - bound.Max.Y) > 0.001)
                    {
                        position.X = obj.Max.X;
                        velocity.X = 0;
                        wallCollide = true;
                    }
                    // Updating the bounds after every position change
                    UpdateBounds();
                    // Checking for any Structures that are right below the player
                    if (obj.Intersects(feet) && !(Math.Abs(feet.Min.X - obj.Max.X) < 0.001 || Math.Abs(feet.Max.X - obj.Min.X) < 0.001))
                    {
                        grounded = true;
                        if (velocity.Y > 0)
                            velocity.Y = 0;
                    }
                }

            });

            // For Testing purposes: The player will be moved back to the top of the level if they fall out of bounds.
            while (position.Y > level.GetHeight())
            {
                position.Y -= level.GetHeight();
            }
            return wallCollide;
        }

        /// <summary>
        /// Updates the BoundingBoxes to the creature's position.
        /// </summary>
        private void UpdateBounds()
        {
            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(Vector2.Add(position, size), 0));
            feet = new BoundingBox(new Vector3(position.X, position.Y + size.Y, 0),
                new Vector3(position.X + size.X, position.Y + size.Y + 1, 0));
        }

        //Checks if the creature is standing on something.
        public bool Grounded()
        {
            return this.grounded;
        }

        public Weapon getBody()
        {
            return this.body;
        }

        public void setTint(Color tint)
        {
            this.tint = tint;
        }

        public void SetAnimation(Animation newAnimation)
        {
            currentAnimation = newAnimation;
        }

        public Animation GetAnimation()
        {
            return currentAnimation;
        }

        public void changeState(String stateName)
        {
            State newState = stateList[stateName];

            if (currentState != null)
                currentState.Exit();

            currentState = newState;
            currentState.Enter();
        }

        public void SetFacing(int newFacing)
        {
            this.facing = newFacing;
        }


        /// <summary>
        /// Logic for enemy AI
        /// Called every update, determine if move/attack, etc.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public abstract void Action(GameTime gameTime, Player player);
        //internal abstract bool Attack();
    }

}
