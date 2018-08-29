using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    public class JumpState : State
    {
        //Jump modifiers
        protected readonly float JUMPMULT = 2.5f;
        protected readonly float FALLMULT = 15f;

        //Movement constants
        private static readonly Vector2 RIGHT = new Vector2(250, 0);
        private static readonly Vector2 LEFT = new Vector2(-250, 0);

        public JumpState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(208, 0, 16, 32), TimeSpan.FromSeconds(1), "jump");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Right))
                owner.Move(RIGHT);
            else if (kstate.IsKeyDown(Keys.Left))
                owner.Move(LEFT);
            else
                owner.velocity.X = 0;

            if (kstate.IsKeyUp(Keys.Up))
                owner.velocity = Vector2.Add(owner.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (FALLMULT - 1));

            stateChange = handleInput();

            if (stateChange != null)
                owner.changeState(stateChange);
        }

        public override String handleInput()
        {  
            if (owner.velocity.Y > 0)
                return "Fall";
            else if (owner.Grounded())
                return "Idle";
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                return "Attack";
            else
                return null;
        }
    }
}

