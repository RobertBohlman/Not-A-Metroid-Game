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
        //(6/26/2018 : Changed the tint of the sprite to visually show that the creature died.
        public void Die()
        {
            tint = Color.Red;
        }

        /**Update Creature's position
         * 
         * This method contains the basic info for adding velocity to any creature's position
         * 
         * Player and other enemies with abnormal movement will override this method
         **/
        public virtual void Update(GameTime gameTime)
        {
            //Debug.WriteLine("V: " + this.velocity);
            //Debug.WriteLine("P: " + this.position);
            //Debug.WriteLine("P: " + this.position);

            this.prevPosition = new Vector2(this.position.X, this.position.Y);
            this.position = Vector2.Add(this.position, (this.velocity * (float)gameTime.ElapsedGameTime.TotalSeconds));
            this.velocity = Vector2.Add(this.velocity, (Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds));

            // Updating bound.  Hard-coded values need to be removed.
            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 66, this.position.Y + 96, 0));

            //This prevents acceleration/deceleration for crisp movement
            this.velocity.X = 0;
            

        }

        /**Logic for enemy AI
         * 
         * Called every update, determine if move/attack, etc.
         **/
        public abstract void Action(GameTime gameTime);
    }

}
