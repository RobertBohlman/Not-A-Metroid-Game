using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Entity
    {
        //The entity's current position
        public Vector2 position;

        //The entity's previous position
        public Vector2 prevPosition;

        //See note for draw function
        public Texture2D sprite;

        //Boudning Box for collision
        public BoundingBox bound;

        //Entity's current velocity
        public Vector2 velocity;

        //Temp facing. 0 = right, 1 = left
        protected int facing;

        protected Animation currentAnimation;

        //Tint applied to the entity's sprite
        protected Color tint;

        protected Vector2 scaleVector;

        public Entity()
        {
            tint = Color.White;
        }

        public int getFacing()
        {
            return this.facing;
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
        public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
        {

            var sourceRectangle = currentAnimation.CurrentRectangle;
            if (facing == 0)
            {
                spriteBatch.Draw(sprite, Vector2.Subtract(position,camera.position), sourceRectangle, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
            }
            else if (facing == 1)
            {
                spriteBatch.Draw(sprite, Vector2.Subtract(position,camera.position), sourceRectangle, tint, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);

            }
            

        }

    }
}
