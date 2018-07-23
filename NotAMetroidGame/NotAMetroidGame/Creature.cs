using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
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

        public Creature()
        {
        }

        /** Adjust this creature's velocity by adding the movement vector.
         * 
         * This function will likely be where the logic for collision is implemented.
         * 
         **/
        public void Move(Vector2 movement, GameTime gameTime)
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
         **/
        public bool Damage(long damage)
        {
            return false;

        }

        //*Insert Zelda CD-I reference here*
        public void Die()
        {

        }

        /**Update Creature's position
         * 
         * This method contains the basic info for adding velocity to any creature's position
         * 
         * Player and other enemies with abnormal movement will override this method
         **/
        public virtual void Update(GameTime gameTime, Player player)
        {
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

            // Updating bound.  Hard-coded values need to be removed.
            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));
        }

        //Checks if the creature is standing on something. Hard coded for now.
        public bool Grounded()
        {
            return this.position.Y >= 385;
        }

        /**Logic for enemy AI
         * 
         * Called every update, determine if move/attack, etc.
         **/
        public abstract void Action(GameTime gameTime, Player player);
        //internal abstract bool Attack();
    }

}
