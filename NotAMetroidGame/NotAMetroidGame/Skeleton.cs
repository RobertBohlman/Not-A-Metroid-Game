using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Skeleton : Creature
    {

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.sprite = sprite = content.Load<Texture2D>("orc_skeleton_single");
            this.position = new Vector2(150, 385);
            this.velocity = new Vector2(0, 0);

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));
        }

        
        public override void Action(GameTime gameTime)
        {
            //if (this.position.Y == 380)
            //{
               // Debug.WriteLine("Grounded");
                //this.Move(new Vector2(0, -200), gameTime);
                //this.Move(new Vector2(5, 0), gameTime);

            //}
          
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.position.Y >= 385)
            {
                //Debug.WriteLine("Grounded");
                this.velocity = Vector2.Zero;
                this.position.Y = 385;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(this.sprite, this.position, null, Color.White, 0, Vector2.Zero, 0.23f, SpriteEffects.None, 0f);

            Vector2 scaleVector = new Vector2(0.7f, 0.7f);
            if (facing == 0)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
            }
            else if (facing == 1)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
            }


        }
    }
}
