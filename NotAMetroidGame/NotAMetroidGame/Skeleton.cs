using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace NotAMetroidGame
{
    public class Skeleton : Creature
    {

        public Skeleton(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.sprite = sprite = content.Load<Texture2D>("orc_skeleton_single");
            this.position = new Vector2(10, 380);
            this.velocity = new Vector2(0, 0);
        }

        public override void Action(GameTime gameTime)
        {
            //if (this.position.Y == 380)
            //{
               // Debug.WriteLine("Grounded");
                //this.Move(new Vector2(0, -200), gameTime);
                //this.Move(new Vector2(5, 0), gameTime);

            //}
          
        }
    }
}
