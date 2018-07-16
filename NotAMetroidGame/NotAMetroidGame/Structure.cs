using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NotAMetroidGame
{
    public class Structure
    {
        public Texture2D sprite;

        public Vector2 position;

        public BoundingBox bound;

        public bool solid;

        public Structure()
        {

        }
    }

    public class TestFloor : Structure
    {
        public TestFloor(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 position)
        {
            this.sprite = sprite = content.Load<Texture2D>("test_floor");
            this.position = position;
            this.solid = true;
            this.bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 64, this.position.Y + 64, 0));
        }
    }

    public class Air : Structure
    {
        public Air(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 position)
        {
            this.position = position;
            this.solid = false;
        }
    }
}
