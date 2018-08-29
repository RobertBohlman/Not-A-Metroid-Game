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
            handleInput();
        }

        public override void handleInput()
        {
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Right))
            {
                owner.SetFacing(0);
                owner.changeState("Walk");
            }      
            else if (kstate.IsKeyDown(Keys.Left))
            {
                owner.SetFacing(1);
                owner.changeState("Walk");
            }
            else if (kstate.IsKeyDown(Keys.Up))
            {
                owner.changeState("Jump");
            }
        }
    }
}

