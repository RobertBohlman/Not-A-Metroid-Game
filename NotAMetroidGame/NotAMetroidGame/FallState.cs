using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace NotAMetroidGame
{
    public class FallState : State
    {
        protected readonly float JUMPMULT = 2.5f;
        protected readonly float FALLMULT = 15f;

        private enum Direction { LEFT, RIGHT, NONE };
        private Direction jumpAngle;

        private static readonly Vector2 RIGHT = new Vector2(250, 0);
        private static readonly Vector2 LEFT = new Vector2(-250, 0);

        public FallState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(208, 0, 16, 32), TimeSpan.FromSeconds(1), "jump");
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            handleInput();

            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                owner.velocity = Vector2.Add(owner.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (FALLMULT - 1));
            else
                owner.velocity = Vector2.Add(owner.velocity, Game1.GRAV_CONSTANT * (float)gameTime.ElapsedGameTime.TotalSeconds * (JUMPMULT - 1));

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

            if (owner.Grounded() && jumpAngle == Direction.NONE)
                owner.changeState("Idle");
            else if (owner.Grounded())
                owner.changeState("Walk");
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

