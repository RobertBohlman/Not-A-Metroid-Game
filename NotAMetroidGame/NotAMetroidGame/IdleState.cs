using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class IdleState : State
    {
        public static readonly Vector2 JUMP = new Vector2(0, -600);

        public IdleState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(16, 0, 16, 32), TimeSpan.FromSeconds(.25), "idle");
        }

        public override void Enter()
        {
            base.Enter();
            Debug.WriteLine("Resetting Velocity");
            owner.velocity.X = 0;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            stateChange = handleInput();

            if (stateChange != null)
                owner.changeState(stateChange);
        }

        public override String handleInput()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Right))
            {
                owner.SetFacing(0);
                return "Walk";
            }      
            else if (kstate.IsKeyDown(Keys.Left))
            {
                owner.SetFacing(1);
                return "Walk";
            }
            else if (kstate.IsKeyDown(Keys.Up))
            {
                return "Jump";
            }
            else if (kstate.IsKeyDown(Keys.Space))
            {
                return "Attack";
            }
            else if (owner.velocity.Y > 0)
            {
                return "Fall";
            }
            else
            {
                return null;
            }
        }
    }
}

