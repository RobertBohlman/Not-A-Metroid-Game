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

        public Vector2 offset;

        public string solidSurfaces;

        protected Vector2 scaleVector;

        public Structure()
        {
            offset = new Vector2(0,0);
            scaleVector = new Vector2(1, 1);
        }

        public BoundingBox GetBounds()
        {
            return bound;
        }

        public void SetScale(float scale)
        {
            scaleVector = new Vector2(scale, scale);
        }

        public abstract void Update(GameTime gametime);

        public virtual void Draw(SpriteBatch spritebatch, Camera camera)
        {
            //spritebatch.Draw(sprite, Vector2.Subtract(position, camera.position), Color.White);
            spritebatch.Draw(sprite, Vector2.Subtract(position, camera.position), null, Color.White, 0, Vector2.Zero, scaleVector, SpriteEffects.None, 0f);
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
            solidSurfaces = "udlr";
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
            solidSurfaces = "udlr";
            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(x + width, y + height, 0));
        }

        public override void Update(GameTime gametime)
        {
            //Do nothing
        }
    }

    public class TestPlatformSmall : Structure
    {
        private static string SPRITE_NAME = "test_platform_small";
        private static int DEFAULT_WIDTH = 64;
        private static int DEFAULT_HEIGHT = 16;

        public TestPlatformSmall(ContentManager content, int x, int y)
        {
            sprite = content.Load<Texture2D>(SPRITE_NAME);
            position = new Vector2(x, y);
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
            solidSurfaces = "u";
            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(x + width, y + height, 0));
        }

        public override void Update(GameTime gametime)
        {
            //Do nothing
        }
    }

    public class TestPlatformMoving : Structure
    {
        private static string SPRITE_NAME = "test_platform_small";
        private static int DEFAULT_WIDTH = 64;
        private static int DEFAULT_HEIGHT = 16;

        private float timeElapsed;
        private Vector2 initialPosition;

        public TestPlatformMoving(ContentManager content, int x, int y)
        {
            sprite = content.Load<Texture2D>(SPRITE_NAME);
            position = new Vector2(x, y);
            initialPosition = new Vector2(x, y);
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
            solidSurfaces = "u";
            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(position.X + width, position.Y + height, 0));
            timeElapsed = 0;
        }

        public override void Update(GameTime gametime)
        {
            timeElapsed += gametime.ElapsedGameTime.Milliseconds;

            float prevPosition = position.X;

            position.X = initialPosition.X + (float)(Math.Cos(timeElapsed/1000)*100);
            offset = new Vector2(position.X - prevPosition, 0);

            bound = new BoundingBox(new Vector3(position, 0),
                new Vector3(position.X + width, position.Y + height, 0));
        }
    }
}
