using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    public class Player : Creature
    {
        //Jump modifiers
        protected float fallMult = 2.5f;
        protected float shortJump = 15f;

        Animation walkRight;
        Animation walkLeft;
        Animation jump;
        Animation fall;
        Animation idle;
        Animation hurt;

        //in an attacking state or not
        public bool attacking;

        //keeps track of elapsed time since start of an attack
        private float attackTimer;

        //bounding box of attack
        public BoundingBox hit;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //Init placeholder image
            this.sprite = sprite = content.Load<Texture2D>("sprite_base_addon_2012_12_14");
            this.position = new Vector2(10, 380);
            this.velocity = new Vector2(0, 0);

            bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 37, this.position.Y + 60, 0));

            //Animation setup
            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(20, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(84, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(148, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(212, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(276, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(340, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(404, 88, 20, 30), TimeSpan.FromSeconds(.15));
            walkRight.AddFrame(new Rectangle(468, 88, 20, 30), TimeSpan.FromSeconds(.15));

            //Since we use the same frames for both, no need to assign twice.
            walkLeft = walkRight;

            idle = new Animation();
            idle.AddFrame(new Rectangle(20, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(84, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(148, 24, 20, 30), TimeSpan.FromSeconds(.25));
            idle.AddFrame(new Rectangle(212, 24, 20, 30), TimeSpan.FromSeconds(.25));

            jump = new Animation();
            jump.AddFrame(new Rectangle(148, 149, 20, 30), TimeSpan.FromSeconds(1));

            fall = new Animation();
            fall.AddFrame(new Rectangle(212, 149, 20, 30), TimeSpan.FromSeconds(1));

            hurt = new Animation();
            hurt.AddFrame(new Rectangle(148, 280, 20, 30), TimeSpan.FromSeconds(1));

            currentAnimation = idle;
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
                currentAnimation = jump;
                jump.Update(gameTime);

            }
            else if (this.velocity.Y < 0 && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                this.velocity = Vector2.Add(this.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (shortJump - 1));
                currentAnimation = fall;
                fall.Update(gameTime);
            }

            if (this.position.Y >= 385)
            {
                //Debug.WriteLine("Grounded");
                this.velocity = Vector2.Zero;
                this.position.Y = 385;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && this.position.Y >= 385)
            {
                facing = 0;
                currentAnimation = walkRight;
                walkRight.Update(gameTime);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && this.position.Y >= 385)
            {
                facing = 1;
                currentAnimation = walkLeft;
                walkLeft.Update(gameTime);
            }
            else if (this.position.Y >= 385)
            {
                currentAnimation = idle;
                idle.Update(gameTime);
            }

            if (this.recoil)
            {
                currentAnimation = hurt;
                hurt.Update(gameTime);
            }
        }

        public override void Action(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
