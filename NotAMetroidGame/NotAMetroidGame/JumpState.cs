using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NotAMetroidGame
{
    public class JumpState : State
    {
        //Jump modifiers
        protected readonly float JUMPMULT = 2.5f;
        

        //Movement constants
        private static readonly Vector2 RIGHT = new Vector2(250, 0);
        private static readonly Vector2 LEFT = new Vector2(-250, 0);
        public static readonly Vector2 JUMP = new Vector2(0, -600);

        private enum Direction {LEFT, RIGHT, NONE};
        private Direction jumpAngle;

        public JumpState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(208, 0, 16, 32), TimeSpan.FromSeconds(1), "jump");
        }

        public override void Enter()
        {
            base.Enter();
            owner.Move(JUMP);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (jumpAngle)
            {
                case Direction.NONE:
                    owner.velocity.X = 0;
                    break;

                case Direction.RIGHT:
                    owner.Move(RIGHT);
                    break;

                case Direction.LEFT:
                    owner.Move(LEFT);
                    break;

                default:
                    break;
            }

            if (owner.velocity.Y > 0 || Keyboard.GetState().IsKeyUp(Keys.Up))
                owner.changeState("Fall");
            else if (owner.Grounded())
                owner.changeState("Idle");
        }

        public override void handleInput()
        {
            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Right))
                jumpAngle = Direction.RIGHT;
            else if (kstate.IsKeyDown(Keys.Left))
                jumpAngle = Direction.LEFT;
            else
                jumpAngle = Direction.NONE;
        }
    }
}

