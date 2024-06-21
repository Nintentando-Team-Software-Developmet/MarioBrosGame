using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class AnimationComponent : BaseComponent
    {
        public Dictionary<AnimationState, Texture2D[]> animations { get; private set; }
        public AnimationState currentState { get; private set; }
        public Rectangle textureRectangle { get; set; }
        public float velocity { get; set; }
        public float timeElapsed { get; set; }
        public int currentFrame { get; set; }
        public int width { get; set; }
        public int height { get; set; }


        public AnimationComponent(Dictionary<AnimationState, Texture2D[]> animations, int width = 0, int height = 0, float velocity = 0.8f)
        {
            if(animations == null) throw new ArgumentException("Animations cannot be null");
            this.animations = animations;
            this.velocity = velocity;
            currentFrame = 0;
            if ((width == 0 || height == 0) && animations.Count > 0)
            {
                textureRectangle = new Rectangle(0, 0, animations[AnimationState.STOP][0].Width, animations[AnimationState.STOP][0].Height);
            }
            else
            {
                textureRectangle = new Rectangle(0, 0, width, height);
            }
            this.width = textureRectangle.Width;
            this.height = textureRectangle.Height;
            if (containsState(AnimationState.STOP))
            {
                currentState = AnimationState.STOP;
            }
            else if (containsState(AnimationState.BlINK))
            {
                currentState = AnimationState.BlINK;
            }
        }

        public void Play(AnimationState state)
        {
            if (!animations.ContainsKey(state)) throw new ArgumentException("Animation state not found");
            currentState = state;
            if (currentState != state)
            {
                currentFrame = 0;
                timeElapsed = 0;
            }
        }

        public bool containsState(AnimationState state)
        {
            return animations.ContainsKey(state);
        }

        public bool notCurrentState(params AnimationState[] state)
        {
            return !state.Contains(currentState);
        }

    }
}
