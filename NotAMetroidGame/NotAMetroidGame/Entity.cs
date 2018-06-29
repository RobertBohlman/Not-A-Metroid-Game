using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NotAMetroidGame
{
    public class Entity
    {
        //The entity's current position
        public Vector2 position;

        //See note for draw function
        public Texture2D sprite;

        //Boudning Box for collision
        public BoundingBox bound;

        //Entity's current velocity
        public Vector2 velocity;

        //Tint applied to the entity's sprite
        public Color tint;

        public Entity()
        {
            tint = Color.White;
        }

        /**[NOTE]: This is likely to be removed 
         * 
         * Draw the entity on screen.
         * 
         *  The draw function will use the sprite field 
         *  and other necessary calls to draw this entity 
         *  on screen at its current position
         *  
         *  [NOTE]: While using a Texture2D allows us to draw a static image to the screen, 
         *  I'm not sure if there's a better way to draw frames of animation (there probably is)
         *  As a result, this will probably get revised slightly.
         **/
        public void Draw()
        {

        }

    }
}
