using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NotAMetroidGame
{
    public class Animation
    {
        List<AnimationFrame> frames = new List<AnimationFrame>();
        TimeSpan timeIntoAnimation;

        TimeSpan Duration
        {
            get
            {
                double totalSeconds = 0;
                foreach (var frame in frames)
                {
                    totalSeconds += frame.duration.TotalSeconds;
                }

                return TimeSpan.FromSeconds(totalSeconds);
            }
        }

        public String getFrameName()
        {
            AnimationFrame currentFrame = null;

            TimeSpan accumulatedTime = TimeSpan.Zero;
            foreach (var frame in frames)
            {
                if (accumulatedTime + frame.duration >= timeIntoAnimation)
                {
                    currentFrame = frame;
                    break;
                }
                else
                {
                    accumulatedTime += frame.duration;
                }
            }

            if (currentFrame == null)
            {
                Debug.WriteLine("Null rectangle");
                currentFrame = frames.LastOrDefault();
            }

            if (currentFrame != null)
            {
                return currentFrame.name;
            }
            else
            {
                return null;
            }
        }

        public void AddFrame(Rectangle rectangle, TimeSpan duration, String name)
        {
            AnimationFrame newFrame = new AnimationFrame()
            {
                sourceRectangle = rectangle,
                duration = duration,
                name = name
            };

            frames.Add(newFrame);
        }

        public void Update(GameTime gameTime)
        {
            double secondsIntoAnimation =
                timeIntoAnimation.TotalSeconds + gameTime.ElapsedGameTime.TotalSeconds;


            double remainder = secondsIntoAnimation % Duration.TotalSeconds;

            timeIntoAnimation = TimeSpan.FromSeconds(remainder);
        }

        //Resets this animation to its first frame
        public void Reset()
        {
            timeIntoAnimation = TimeSpan.Zero;
        }

        public Rectangle CurrentRectangle
        {
            get
            {
                AnimationFrame currentFrame = null;

                // See if we can find the frame
                TimeSpan accumulatedTime = TimeSpan.Zero;
                foreach (var frame in frames)
                {
                    if (accumulatedTime + frame.duration >= timeIntoAnimation)
                    {
                        currentFrame = frame;
                        break;
                    }
                    else
                    {
                        accumulatedTime += frame.duration;
                    }
                }

                // If no frame was found, then try the last frame, 
                // just in case timeIntoAnimation somehow exceeds Duration
                if (currentFrame == null)
                {
                    Debug.WriteLine("Null rectangle");
                    currentFrame = frames.LastOrDefault();
                }

                // If we found a frame, return its rectangle, otherwise
                // return an empty rectangle (one with no width or height)
                if (currentFrame != null)
                {
                    return currentFrame.sourceRectangle;
                }
                else
                {
                    return Rectangle.Empty;
                }
            }
        }
    }
}