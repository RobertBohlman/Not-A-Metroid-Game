using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace NotAMetroidGame
{
    public abstract class Structure
    {
        //Sprite of the structure
        protected Texture2D sprite;

        //Location
        protected Vector2 position;

        //The bounding box
        protected BoundingBox bound;

        protected int width;

        protected int height;

        public Structure()
        {

        }

        public BoundingBox GetBounds()
        {
            return bound;
        }

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spritebatch, Camera camera)
        {
            spritebatch.Draw(sprite, Vector2.Subtract(position, camera.position), Color.White);
        }
    }

    // A checkered floor used to test collisions
    public class TestFloor : Structure
    {
        private static string SPRITE_NAME = "TestFloor";
        private static int DEFAULT_WIDTH = 800;
        private static int DEFAULT_HEIGHT = 100;


        public TestFloor(ContentManager content, int x, int y)
        {
            sprite = content.Load<Texture2D>(SPRITE_NAME);
            position = new Vector2(x, y);
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
            bound = new BoundingBox(new Vector3(position, 0), 
                new Vector3(x + width, y + height, 0));
        }

        public override void Update(GameTime gametime)
        {
            //Do nothing
        }
    }

    public class TestFloorSmall : Structure
    {
        private static string SPRITE_NAME = "test_floor";
        private static int DEFAULT_WIDTH = 64;
        private static int DEFAULT_HEIGHT = 64;


        public TestFloorSmall(ContentManager content, int x, int y)
        {
            sprite = content.Load<Texture2D>(SPRITE_NAME);
            position = new Vector2(x, y);
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(x + width, y + height, 0));
        }

        public override void Update(GameTime gametime)
        {
            //Do nothing
        }
    }
}
