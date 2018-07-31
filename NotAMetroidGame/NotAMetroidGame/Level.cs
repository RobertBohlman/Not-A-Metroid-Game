using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
    public abstract class Level
    {

        private static int DEFAULT_WIDTH = 800;

        private static int DEFAULT_HEIGHT = 600;

        //Level dimensions
        protected int width;
        protected int height;

        //Spawn location for the player
        // This should be set in InitMap()
        protected Vector2 spawn_location = new Vector2(10, 60);

        //Background image for the level.
        protected Texture2D background;

        //The scenery around the map, including interactable and non-interactable terrain
        protected List<Structure> structures;
        //The creatures located in this map
        protected List<Creature> creatures;

        public Level()
        {
            structures = new List<Structure>();
            creatures = new List<Creature>();
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
        }

        // Getters

        public Vector2 GetSpawnLocation()
        {
            return spawn_location;
        }

        public List<Structure> GetStructures()
        {
            return structures;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }


        /// <summary>
        /// This function is intended to populate the Structure and Entity lists and make any
        /// other necessary changes before the level is interactable.
        /// </summary>
        /// <param name="content">ContentManager used for obtaining any necessary assets.</param>
        public abstract void InitMap(ContentManager content);

        /// <summary>
        /// Adds the given Structure or Creature to the level and returns true.  
        /// If the object is not either of those, the function returns false.
        /// </summary>
        /// <param name="obj">The Object to be added.</param>
        /// <returns>Whether or not the Object has been added.</returns>
        protected bool AddObject(Object obj)
        {
            if (obj is Structure newStructure)
            {
                structures.Add(newStructure);
                return true;
            }
            else if (obj is Creature newCreature)
            {
                creatures.Add(newCreature);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Calls the Update function for every Structure and Creature in the level.
        /// The camera position is also locked within the boundaries of the level.
        /// </summary>
        /// <param name="gametime"></param>
        /// <param name="player"></param>
        /// <param name="camera"></param>
        public virtual void Update(GameTime gametime, Player player, Camera camera)
        {
            //Every Structure and Creature is updated
            structures.ForEach((s) => s.Update(gametime));
            creatures.ForEach((c) => c.Update(gametime, this, player));

            //Restricts camera movement past level boundaries.  This should be moved at some point.
            if (camera.position.X < 0)
            {
                camera.position.X = 0;
            }
            if (camera.position.X > width - camera.width)
            {
                camera.position.X = width - camera.width;
            }
            if (camera.position.Y < 0)
            {
                camera.position.Y = 0;
            }
            if (camera.position.Y > height - camera.height)
            {
                camera.position.Y = height - camera.height;
            }
        }

        /// <summary>
        /// Draws the background image if one exists and calls the Draw
        /// function on every Structure and Creature in the level.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="camera"></param>
        public virtual void Draw(SpriteBatch sb, Camera camera)
        {
            if (background != null)
            {
                sb.Draw(background, new Vector2(0, 0), Color.White);
            }
            structures.ForEach((s) => s.Draw(sb, camera));
            creatures.ForEach((e) => e.Draw(sb, camera));
        }
    }

    /// <summary>
    /// A Level that contains platforms for collision testing.
    /// </summary>
    public class Test_00 : Level
    {
        private static string BG_NAME = "test_background";

        public override void InitMap(ContentManager content)
        {
            AddObject(new TestFloor(content, 0, 500));
            AddObject(new TestFloorSmall(content, 0, 300));
            AddObject(new TestFloorSmall(content, 400, 400));
            background = content.Load<Texture2D>(BG_NAME);
        }
    }
}
