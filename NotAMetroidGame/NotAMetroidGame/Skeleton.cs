using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Skeleton : Creature
    {
        //How long this enemy waits before switching patrol paths
        private int pathTimer;

        //Direction the skeleton is patrolling 0 = right, 1 = left
        private int path;

        private int stop;

        private Vector2 RIGHT;
        private Vector2 LEFT;

        Random rng;

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.sprite = sprite = content.Load<Texture2D>("orc_skeleton_single");
            this.position = new Vector2(350, 385);
            this.velocity = new Vector2(0, 0);

            this.speedCap = 150;

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));

            RIGHT = new Vector2(150, 0);
            LEFT = new Vector2(-150, 0);

            pathTimer = 0;
            path = 0;

            speedCap = 80;

            rng = new Random();
        }

        
        public override void Action(GameTime gameTime, Player player)
        {
            path = facing;

            if (Math.Abs(player.position.X - this.position.X) < 100)
            {
                path = 2;
                if (player.position.X < this.position.X)
                {
                    facing = 1;
                }
                else
                {
                    facing = 0;
                }
            }

            if (path == 1)
            {
                facing = 1;
                this.Move(LEFT, gameTime);
            }
            else if (path == 0)
            {
                facing = 0;
                this.Move(RIGHT, gameTime);
            }
            else if (path == 2)
            {
                velocity.X = 0;
            }

           if (position.X <= 10)
                path = 0;

           if (position.X >= 740)
                path = 1;
               
          
        }

        public override void Update(GameTime gameTime, Player player)
        {

            base.Update(gameTime, player);

            //Hard coded floor
            if (this.Grounded())
            {
                //Debug.WriteLine("Grounded");
                this.velocity.Y = 0;
                this.position.Y = 385;
            }

            Action(gameTime, player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            //spriteBatch.Draw(this.sprite, this.position, null, Color.White, 0, Vector2.Zero, 0.23f, SpriteEffects.None, 0f);

            Vector2 scaleVector = new Vector2(0.7f, 0.7f);
            if (facing == 0)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, scaleVector, SpriteEffects.FlipHorizontally, 0f);
            }
            else if (facing == 1)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
            }


        }
    }
}
