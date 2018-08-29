using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NotAMetroidGame
{
    public class Camera
    {
        public Vector2 position;

        public int width;

        public int height;

        public Camera() : this(0, 0) {
        }

        public Camera(int x, int y)
        {
            position = new Vector2(x, y);
            width = 800;
            height = 600;
        }

        public Camera(Camera c)
        {
            position = new Vector2(c.position.X, c.position.Y);
            width = c.width;
            height = c.height;
        }

        public void Update(Vector2 newPosition, Level level, GraphicsDeviceManager graphics)
        {
            int bufferHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int bufferWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            if (height != bufferHeight || width != bufferWidth)
            {
                height = bufferHeight;
                width = bufferWidth;
            }
            position = Vector2.Subtract(newPosition, new Vector2(width / 2, height / 2));

            BoundingBox levelBoundary = level.GetBoundaries();

            //Restricts camera movement past level boundaries.  This should be moved at some point.
            if (position.X < levelBoundary.Min.X)
            {
                position.X = levelBoundary.Min.X;
            }
            if (position.X > levelBoundary.Max.X - width)
            {
                position.X = levelBoundary.Max.X - width;
            }
            if (position.Y < levelBoundary.Min.Y)
            {
                position.Y = levelBoundary.Min.Y;
            }
            if (position.Y > levelBoundary.Max.Y - height)
            {
                position.Y = levelBoundary.Max.Y - height;
            }
        }
    }
}
