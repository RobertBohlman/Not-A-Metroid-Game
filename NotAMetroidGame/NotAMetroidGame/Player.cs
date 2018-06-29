using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotAMetroidGame
{
    public class Player : Creature
    {
        //using skeleton sprite for now
        private static string SPRITE = "orc_skeleton_single";

        //keeps track of elapsed time since start of an attack
        private float attackTimer;

        //in an attacking state or not
        public bool attacking;

        //bounding box of attack
        public BoundingBox hit;

        public Player(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            attacking = false;
            // Copied from Skeleton for now
            this.sprite = sprite = content.Load<Texture2D>(SPRITE);
            this.position = new Vector2(400, 380);
            this.velocity = new Vector2(0, 0);
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

        public override void Action(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
