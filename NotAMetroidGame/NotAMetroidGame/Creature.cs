using Microsoft.Xna.Framework;
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

        private float fallMult = 2.5f;

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
         * For this instance, that enemy can override this function.
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

        public void Jump()
        {
            this.velocity = Vector2.Add(this.velocity, new Vector2(0, -50));
        }

        public void Update(GameTime gameTime)
        {
            Debug.WriteLine("V: " + this.velocity);
            Debug.WriteLine("P: " + this.position);
            this.position = Vector2.Add(this.position, (this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds));
            this.velocity = Vector2.Add(this.velocity, (Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds));
            this.velocity.X = 0;

            if (this.velocity.Y > 0)
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (fallMult - 1));
            }

            if (this.position.Y >= 385)
            {
                Debug.WriteLine("Grounded");
                this.velocity = Vector2.Zero;
                this.position.Y = 385;
            }
            
            
        }

        /**Logic for enemy AI
         * 
         * Called every update, determine if move/attack, etc.
         **/
        public abstract void Action(GameTime gameTime);
    }

}
