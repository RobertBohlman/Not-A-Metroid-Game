using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    public class Player : Creature
    {
        //using skeleton sprite for now
        private static string SPRITE = "orc_skeleton_single";

        //keeps track of elapsed time since start of an attack
        private float attackTimer;

        //in an attacking state or not
        public bool attacking;

        //bounding box of attack
        public BoundingBox hit;

        //Jump modifiers
        protected float fallMult = 2.5f;
        protected float shortJump = 15f;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //Init placeholder image
            attacking = false;
            this.sprite = sprite = content.Load<Texture2D>("rsz_photo_1");
            this.position = new Vector2(10, 380);
            this.velocity = new Vector2(0, 0);
        }

        /**
         * (Likely to be altered or removed.  Used for boundingbox testing)
         * Attack updates the attack bounding box.
         * Returns whether or not the player is attacking.
         */
        public bool Attack(GameTime gameTime)
        {
            if (attacking)
            {
                attackTimer += gameTime.ElapsedGameTime.Milliseconds;
                hit = new BoundingBox(new Vector3(this.position.X - 100, this.position.Y + 30, 0),
                    new Vector3(this.position.X, this.position.Y + 40, 0));
                if (attackTimer > 100)
                {
                    attacking = false;
                    attackTimer = 0;
                }
                return true;

            }
            else
            {
                return false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.velocity.Y > 0)
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (fallMult - 1));

            }
            else if (this.velocity.Y < 0 && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (shortJump - 1));
            }

            if (this.position.Y >= 385)
            {
                Debug.WriteLine("Grounded");
                this.velocity = Vector2.Zero;
                this.position.Y = 385;
            }
        }

        public override void Action(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
