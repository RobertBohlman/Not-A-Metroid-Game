﻿using System;
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

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            //Init placeholder image
            this.sprite = sprite = content.Load<Texture2D>("rsz_photo_1");
            this.position = new Vector2(10, 380);
            this.velocity = new Vector2(0, 0);
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
