using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NotAMetroidGame
{
    public class Structure
    {
        //Sprite of the structure
        public Texture2D sprite;

        //Location
        public Vector2 position;

        //The bounding box
        public BoundingBox bound;

        //Determines if this structure can be collided with or not.
        public bool solid;

        //A string that can contain the letters 'l', 'r', 'u', or 'd'.  Determines which directions are valid for collision
        public string validDirections;

        public Structure()
        {

        }
    }

    // A checkered floor used to test collisions
    public class TestFloor : Structure
    {
        public TestFloor(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 position)
        {
            this.sprite = sprite = content.Load<Texture2D>("test_floor");
            this.position = position;
            this.solid = true;
            this.validDirections = "udlr";
            this.bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 64, this.position.Y + 64, 0));
        }
    }

    public class TestPlatform : Structure
    {
        public TestPlatform(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 position)
        {
            this.sprite = sprite = content.Load<Texture2D>("test_floor");
            this.position = position;
            this.solid = true;
            this.validDirections = "u";
            this.bound = new BoundingBox(new Vector3(this.position.X, this.position.Y, 0),
                new Vector3(this.position.X + 64, this.position.Y + 64, 0));
        }
    }

    // Air is the open space in the game
    public class Air : Structure
    {
        public Air(Microsoft.Xna.Framework.Content.ContentManager content, Vector2 position)
        {
            this.position = position;
            this.solid = false;
        }
    }
}
