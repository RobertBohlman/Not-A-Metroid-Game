using Microsoft.Xna.Framework;
using System;

namespace NotAMetroidGame
{
    public abstract class State
    {
        //Reference to the creature this state belongs to.
        protected Creature owner;

        //The individual state's animation, set in contructor.
        protected Animation animation;

        //String representing the new state to change to.
        protected String stateChange;

        protected State(Creature owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// This method will be called immediatly 
        /// after changing to a new state
        /// Sets new animations, initalizes important values, etc
        /// </summary>
        public virtual void Enter()
        {
            owner.SetAnimation(animation);
            owner.GetAnimation().Reset();
        }

        /// <summary>
        /// This method will be called immediatly 
        /// before changing to a new state
        /// </summary>
        public virtual void Exit()
        {
            owner.GetAnimation().Reset();
        }

        /// <summary>
        /// Every state will have an update where it
        /// will handle control/AI inputs as well as
        /// timer based updates.
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            owner.GetAnimation().Update(gameTime);
        }

        /// <summary>
        /// Method to decompose the input handling from
        /// the update method
        /// </summary>
        public abstract String handleInput();
    }
}

