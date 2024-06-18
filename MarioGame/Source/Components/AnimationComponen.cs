using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class AnimationComponen : BaseComponent
    {
        public Dictionary<AnimationState, Texture2D[]> animations { get; private set; }
        public AnimationState currentState { get; private set; }
        public Rectangle textureRectangle { get; set; }
        public float velocity { get; set; }
        public float timeElapsed { get; set; }
        public int currentFrame { get; set; }
        public int width { get; set; }
        public int height { get; set; }


        public AnimationComponen(Dictionary<AnimationState, Texture2D[]> animations, int width = 0, int height = 0, float velocity = 0.8f)
        {
            this.animations = animations;
            this.velocity = velocity;
            currentState = AnimationState.STOP;
            currentFrame = 0;
            if ((width == 0 || height == 0) && animations?.Count > 0)
            {
                textureRectangle = new Rectangle(0, 0, animations[AnimationState.STOP][0].Width, animations[AnimationState.STOP][0].Height);
            }
            else
            {
                textureRectangle = new Rectangle(0, 0, width, height);
            }
            this.width = textureRectangle.Width;
            this.height = textureRectangle.Height;

            Console.WriteLine("AnimationComponen created: animations size: " + animations?.Count + " width: " + width + " height: " + height + " velocity: " + velocity);
        }

        public void Play(AnimationState state)
        {
            if(!animations.ContainsKey(state)) throw new ArgumentException("Animation state not found");
            currentState = state;
            if(currentState != state)
            {
                currentFrame = 0;
                timeElapsed = 0;
            }
        }
    }
}
