using Microsoft.Xna.Framework;
using System;

namespace NotAMetroidGame
{
    public class AttackState : State
    {
        private int recoveryTimer;

        public AttackState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(256, 0, 16, 32), TimeSpan.FromSeconds(0.15), "windup");
            animation.AddFrame(new Rectangle(304, 0, 16, 32), TimeSpan.FromSeconds(0.15), "swing");
            animation.AddFrame(new Rectangle(352, 0, 16, 32), TimeSpan.FromSeconds(0.15), "recovery");
            recoveryTimer = 0;
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

            if (owner.Grounded())
                owner.velocity.X = 0;

            if (String.Equals(animation.getFrameName(), "swing"))
            {
                if (owner.getFacing() == 0)
                {
                    owner.hit = new BoundingBox(new Vector3(owner.position.X + 38, owner.position.Y, 0),
                                 new Vector3(owner.position.X + 76, owner.position.Y + 30, 0));
                }
                else
                {
                    owner.hit = new BoundingBox(new Vector3(owner.position.X - 36, owner.position.Y, 0),
                                new Vector3(owner.position.X, owner.position.Y + 30, 0));
                }
            }
            else if (String.Equals(animation.getFrameName(), "recovery"))
            {
                owner.hit = new BoundingBox(new Vector3(0, 0, 0),
                                new Vector3(0, 0, 0));
                recoveryTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (recoveryTimer >= 120)
                {
                    recoveryTimer = 0;
                    owner.changeState("Idle");
                }
            }
        }

        public override string handleInput()
        {
            throw new NotImplementedException();
        }
    }
}

