using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAMetroidGame
{
    public class Level
    {
        public int levelWidth = 20;
        public int levelHeight = 10;
        public static int TILE_SIZE = 64;
        public static Vector2 SPAWN_LOCATION = new Vector2();
        public Texture2D background;
        public float left;
        public float right;
        public float top;
        public float bottom;
        public int drawPosX;
        public int drawPosY;
        public int drawPosWidth;
        public int drawPosHeight;
        public Vector2 cameraPos;
        public Vector2 offset;
        public Structure[,] grid;

        public Level()
        {
            left = 0;
            top = 0;
            right = (levelWidth+1) * TILE_SIZE;
            bottom = (levelHeight+1) * TILE_SIZE;
            grid = new Structure[levelWidth, levelHeight];
        }

        public void InitMap(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            background = content.Load<Texture2D>("test_background");
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    if (y >= levelHeight - 2)
                        grid[x, y] = new TestFloor(content);
                    else
                        grid[x, y] = new Air();
                }
            }
        }

        public void Update(GameTime gametime, Camera camera)
        {
            drawPosX = (int)(camera.position.X / TILE_SIZE);
            drawPosWidth = drawPosX + (int)(camera.width / TILE_SIZE);
            drawPosY = (int)(camera.position.Y / TILE_SIZE);
            drawPosHeight = drawPosY + (int)(camera.height / TILE_SIZE);
            offset = new Vector2(camera.position.X % TILE_SIZE, camera.position.Y % TILE_SIZE);
            cameraPos = camera.position;
        }

        public void Draw(SpriteBatch sb)
        {
            int currX = 0;
            int currY = 0;
            if (drawPosX < 0)
                drawPosX = 0;
            if (drawPosY < 0)
                drawPosY = 0;
            if (drawPosWidth >= levelWidth)
                drawPosWidth = levelWidth - 1;
            if (drawPosHeight >= levelHeight)
                drawPosHeight = levelHeight - 1;
            sb.Draw(background, new Vector2((0 - cameraPos.X)/ 4, (0 - cameraPos.Y)/4), Color.White);
            for (int x = drawPosX; x <= drawPosWidth; x++)
            {
                for (int y = drawPosY; y <= drawPosHeight; y++)
                {
                    if (grid[x, y].sprite != null)
                    {
                        sb.Draw(grid[x, y].sprite, new Vector2(currX * TILE_SIZE - offset.X, currY * TILE_SIZE - offset.Y), Color.White);
                    }
                    currY++;
                }
                currY = 0;
                currX++;
            }
        }
    }

    public class TestLevel : Level
    {

        public TestLevel()
        {
            this.levelWidth = 20;
            this.levelHeight = 10;
        }


        public new void InitMap(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    if (y >= levelHeight - 2)
                        grid[x, y] = new TestFloor(content);
                    else
                        grid[x, y] = new Air();
                }
            }
        }
    }
}
