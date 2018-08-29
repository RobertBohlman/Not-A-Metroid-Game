using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace NotAMetroidGame
{
    public class AttackState : State
    {
        //How long until we return control to the player.
        private int recoveryTimer;

        //Moevement constants.
        private static readonly Vector2 RIGHT = new Vector2(250, 0);
        private static readonly Vector2 LEFT = new Vector2(-250, 0);

        public AttackState(Creature owner) : base(owner)
        {
            animation = new Animation();
            animation.AddFrame(new Rectangle(256, 0, 16, 32), TimeSpan.FromSeconds(0.15), "windup");
            animation.AddFrame(new Rectangle(304, 0, 16, 32), TimeSpan.FromSeconds(0.15), "swing");
            animation.AddFrame(new Rectangle(352, 0, 16, 32), TimeSpan.FromSeconds(0.15), "recovery");
            recoveryTimer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!owner.Grounded())
            {
                KeyboardState kstate = Keyboard.GetState();

                if (kstate.IsKeyDown(Keys.Right))
                    owner.Move(RIGHT);
                else if (kstate.IsKeyDown(Keys.Left))
                    owner.Move(LEFT);
                else
                    owner.velocity.X = 0;
            } 

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
                    if (owner.Grounded())
                        owner.changeState("Idle");
                    else if (owner.velocity.Y < 0)
                        owner.changeState("Jump");
                    else if (owner.velocity.Y > 0)
                        owner.changeState("Fall");
                }
            }    
            
        }

        public override string handleInput()
        {
            return null;
        }
    }
}

