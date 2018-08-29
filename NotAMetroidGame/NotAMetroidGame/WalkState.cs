using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class WalkState : State
    {
        //Movement constants
        private static readonly Vector2 RIGHT = new Vector2(250, 0);
        private static readonly Vector2 LEFT = new Vector2(-250, 0);
        public static readonly Vector2 JUMP = new Vector2(0, -600);

        //Which direction we were holding when we entered this state
        private KeyboardState entryKState;

        public WalkState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(64, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            animation.AddFrame(new Rectangle(112, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
            animation.AddFrame(new Rectangle(160, 0, 16, 32), TimeSpan.FromSeconds(.15), "walking");
        }

        public override void Enter()
        {
            base.Enter();
            entryKState = Keyboard.GetState();
        }

        public override void Exit()
        {
            base.Exit();
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (owner.getFacing() == 0)
                owner.Move(RIGHT);
            else
                owner.Move(LEFT);

            stateChange = handleInput();

            if (stateChange != null)
                owner.changeState(stateChange);
        }

        public override String handleInput()
        {
            KeyboardState kstate = Keyboard.GetState();

            if (entryKState.IsKeyDown(Keys.Right) && kstate.IsKeyUp(Keys.Right))
            {
                return "Idle";
            }
            else if (entryKState.IsKeyDown(Keys.Left) && kstate.IsKeyUp(Keys.Left))
            {
                return "Idle";
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

