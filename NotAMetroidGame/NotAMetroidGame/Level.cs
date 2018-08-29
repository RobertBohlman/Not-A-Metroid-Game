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

        //Set to true when level is finished. 
        protected bool isOver;

        //Set to true to inform Game1 to not allow player controls and camera updates.
        protected bool isPlayingCutscene;

        //Spawn location for the player
        // This should be set in InitMap()
        protected Vector2 spawn_location = new Vector2(10, 60);

        //Background image for the level.
        protected Texture2D background;

        protected List<LevelLayer> bgLayers;

        protected List<LevelLayer> fgLayers;

        //The scenery around the map, including interactable and non-interactable terrain
        protected List<Structure> structures;
        //The creatures located in this map
        protected List<Creature> creatures;

        protected Level nextLevel;

        public Level()
        {
            structures = new List<Structure>();
            creatures = new List<Creature>();
            width = DEFAULT_WIDTH;
            height = DEFAULT_HEIGHT;
            isOver = false;
            isPlayingCutscene = false;
            bgLayers = new List<LevelLayer>();
            fgLayers = new List<LevelLayer>();
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

        public List<Creature> GetCreatures()
        {
            return this.creatures;
        }

        public Level GetNextLevel()
        {
            return nextLevel;
        }

        public BoundingBox GetBoundaries()
        {
            return new BoundingBox(new Vector3(0, 0, 0), new Vector3(width, height, 0));
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
        /// Returns true if the player is currently out of the current level's boundaries, false otherwise.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        protected bool OutOfBounds(Player player)
        {
            BoundingBox levelbounds = new BoundingBox(new Vector3(-15, -15, 0), new Vector3(width + 15, height + 15, 0));
            if (player.bound.Intersects(levelbounds))
            {
                return false;
            }
            else
            {
                Transition(player);
                return true;
            }
        }

        /// <summary>
        /// Returns whether or not the level is ready to be replaced by another level.
        /// </summary>
        /// <returns></returns>
        public bool IsOver()
        {
            if (isOver && nextLevel == null)
            {
                //If level is transitioning but has not set the next level, nextLevel
                //is set to avoid crashing.
                nextLevel = new Test_00();
            }
            return isOver;
        }

        public bool IsPlayingCutscene()
        {
            return isPlayingCutscene;
        }

        protected abstract void Transition(Player player);

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

            OutOfBounds(player);
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
            if (bgLayers.Count > 0)
            {
                bgLayers.ForEach((l) => l.Draw(sb, camera));
            }
            structures.ForEach((s) => s.Draw(sb, camera));
            creatures.ForEach((e) => e.Draw(sb, camera));
        }
        public virtual void DrawForeground(SpriteBatch sb, Camera camera)
        {
            if (fgLayers.Count > 0)
            {
                fgLayers.ForEach((l) => l.Draw(sb, camera));
            }
        }
    }

    public class LevelLayer
    {
        protected List<Structure> entities;

        protected float scale;

        public LevelLayer() : this(1.0f)
        {

        }

        public LevelLayer(float scale)
        {
            entities = new List<Structure>();
            this.scale = scale;
        }

        public List<Structure> GetEntities()
        {
            return entities;
        }

        public void AddEntity(Structure entity)
        {
            entity.SetScale(scale);
            entities.Add(entity);
        }

        public void RemoveEntity(int index)
        {
            if (index < entities.Count && index >= 0)
            {
                entities.RemoveAt(index);
            }
        }

        public void Draw(SpriteBatch spritebatch, Camera camera)
        {
            Camera tempcam = new Camera(camera);
            tempcam.position.X *= scale;
            tempcam.position.Y *= scale;
            entities.ForEach((e) => e.Draw(spritebatch, tempcam));
        }
    }

    /// <summary>
    /// A Level that contains platforms for collision testing.
    /// </summary>
    public class Test_00 : Level
    {
        private static string BG_NAME = "test_background";

        public Test_00() : base()
        {
            width = 1200;
        }

        public override void InitMap(ContentManager content)
        {
            width = 1200;
            AddObject(new TestFloor(content, 0, 500));
            AddObject(new TestFloor(content, 800, 500));
            AddObject(new TestFloor(content, -800, 500));
            AddObject(new TestPlatformSmall(content, 0, 300));
            AddObject(new TestPlatformMoving(content, 350, 300));
            AddObject(new TestFloorSmall(content, 200, 415));
            AddObject(new TestFloorSmall(content, 700, 415));
            //AddObject(new Skeleton(content, new Vector2(500, 385)));
            background = content.Load<Texture2D>(BG_NAME);
            LevelLayer backlayer = new LevelLayer(0.5f);
            backlayer.AddEntity(new TestPlatformMoving(content, 350, 200));
            bgLayers.Add(backlayer);
            LevelLayer frontlayer = new LevelLayer(2.0f);
            frontlayer.AddEntity(new TestPlatformMoving(content, 300, 400));
            frontlayer.AddEntity(new TestPlatformMoving(content, 800, 600));
            fgLayers.Add(frontlayer);
        }

        protected override void Transition(Player player)
        {
            nextLevel = new Test_00();

            int levelWidth = nextLevel.GetWidth();
            int levelHeight = nextLevel.GetHeight();

            float playerSize = player.GetSize().X;

            player.position = new Vector2(Math.Abs((player.position.X + levelWidth) % nextLevel.GetWidth()) + playerSize,
                Math.Abs((player.position.Y + levelHeight + 1) % nextLevel.GetHeight()));
        }
    }

    public class Test_01 : Level
    {
        private static string BG_NAME = "test_background";

        public override void InitMap(ContentManager content)
        {
            width = 1600;
            AddObject(new TestFloor(content, 0, 500));
            AddObject(new TestFloor(content, 800, 500));
            AddObject(new TestFloor(content, -800, 500));
            AddObject(new TestFloorSmall(content, 200, 415));
            AddObject(new TestFloorSmall(content, 700, 415));
            background = content.Load<Texture2D>(BG_NAME);
        }

        protected override void Transition(Player player)
        {
            nextLevel = new Test_00();

            int levelWidth = nextLevel.GetWidth();
            int levelHeight = nextLevel.GetHeight();

            float playerSize = player.GetSize().X;

            player.position = new Vector2(Math.Abs((player.position.X + levelWidth) % nextLevel.GetWidth()) + playerSize,
                Math.Abs((player.position.Y + levelHeight + 1) % nextLevel.GetHeight()));
        }
    }
}
