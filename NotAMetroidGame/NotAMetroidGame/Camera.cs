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

        public void Update(Vector2 newPosition, GraphicsDeviceManager graphics)
        {
            int bufferHeight = graphics.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int bufferWidth = graphics.GraphicsDevice.PresentationParameters.BackBufferWidth;
            if (height != bufferHeight || width != bufferWidth)
            {
                height = bufferHeight;
                width = bufferWidth;
            }
            position = Vector2.Subtract(newPosition, new Vector2(width / 2, height / 2));
        }
    }
}
