using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAMetroidGame
{
    public class Level
    {
        //Level dimensions
        public int levelWidth = 20;
        public int levelHeight = 10;
        //Size of each square on the grid
        public static int TILE_SIZE = 64;

        //Spawn location for the player (unused)
        public Vector2 spawn_location = new Vector2(10, 60);

        //Background image for the level.
        public Texture2D background;

        //The boundary coordinates for the map
        public float left;
        public float right;
        public float top;
        public float bottom;

        //Bounds used to determine what structures should be drawn.
        public int drawPosX;
        public int drawPosY;
        public int drawPosWidth;
        public int drawPosHeight;

        //Camera position
        public Vector2 cameraPos;

        //Used for shifting the drawn structures when the camera position is inbetween structures.
        public Vector2 offset;

        //The entire grid of the level.  Contains all the structures within the level.
        public Structure[,] grid;

        public Level()
        {
            left = 0;
            top = 0;
            right = (levelWidth) * TILE_SIZE;
            bottom = (levelHeight) * TILE_SIZE;
            grid = new Structure[levelWidth, levelHeight];
        }

        //populates the grid array with all the structures on the map.
        public void InitMap(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            background = content.Load<Texture2D>("test_background");
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    if (y >= levelHeight - 2)
                        grid[x, y] = new TestFloor(content, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                    else if (x > 10 && y >= levelHeight - 4)
                        grid[x, y] = new TestFloor(content, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                    else
                        grid[x, y] = new Air(content, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                }
            }
        }

        //Gets all the structures within the provided rectangle
        public Object[] GetTiles(Rectangle area)
        {
            int areaX = area.X / TILE_SIZE;
            int areaY = area.Y / TILE_SIZE;
            int areaWidth = areaX + (area.Width / TILE_SIZE);
            int areaHeight = areaY + (area.Height / TILE_SIZE);

            if (areaWidth > grid.GetLength(0) - 1)
                areaWidth = grid.GetLength(0) - 1;
            if (areaHeight > grid.GetLength(1) - 1)
                areaHeight = grid.GetLength(1) - 1;
            if (areaX < 0) areaX = 0;
            if (areaY < 0) areaY = 0;
            int numTiles = (areaWidth - areaX + 1) * (areaHeight - areaY + 1);
            if (numTiles < 0) numTiles = 0;
            //Debug.WriteLine(numTiles);
            ArrayList tile = new ArrayList();
            Structure[] tiles = new Structure[numTiles];
            //int index = 0;
            for (int i = areaX; i <= areaWidth; i++)
            {
                for (int j = areaY; j <= areaHeight; j++)
                {
                    if (grid[i,j].solid == true)
                    {
                        //tiles[index] = grid[i, j];
                        tile.Add(grid[i, j]);
                        //index++;
                    }
                }
            }
            return tile.ToArray();
        }

        // Adjusts the camera if needed.  Sets the drawing positions to determine what structures are currently visible.
        public void Update(GameTime gametime, Camera camera)
        {
            if (camera.position.X < left)
            {
                camera.position.X = left;
            }
            if (camera.position.X > right - camera.width)
            {
                camera.position.X = right - camera.width;
            }
            if (camera.position.Y < top)
            {
                camera.position.Y = top;
            }
            if (camera.position.Y > bottom - camera.height)
            {
                camera.position.Y = bottom - camera.height;
            }
            cameraPos = camera.position;
            drawPosX = (int)(cameraPos.X / TILE_SIZE);
            drawPosWidth = drawPosX + (int)(camera.width / TILE_SIZE) + 1;
            drawPosY = (int)(cameraPos.Y / TILE_SIZE);
            drawPosHeight = drawPosY + (int)(camera.height / TILE_SIZE) + 1;
            offset = new Vector2(cameraPos.X % TILE_SIZE, cameraPos.Y % TILE_SIZE);

        }

        // Draws only the visible structures on the screen.
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
    /*
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
                        grid[x, y] = new TestFloor(content, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                    else
                        grid[x, y] = new Air(content, new Vector2(x * TILE_SIZE, y * TILE_SIZE));
                }
            }
        }
    }
    */
}
