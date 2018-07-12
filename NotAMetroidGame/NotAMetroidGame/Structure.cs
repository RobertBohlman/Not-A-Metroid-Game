using Microsoft.Xna.Framework.Graphics;
using System;

namespace NotAMetroidGame
{
    public class Structure
    {
        public Texture2D sprite;
        public Structure()
        {

        }
    }

    public class TestFloor : Structure
    {
        public TestFloor(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            this.sprite = sprite = content.Load<Texture2D>("test_floor");
        }
    }

    public class Air : Structure
    {

    }
}
