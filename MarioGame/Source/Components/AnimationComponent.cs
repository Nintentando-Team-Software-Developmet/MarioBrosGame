using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Components
{
    public class AnimationComponent : BaseComponent
    {
        private List<Texture2D> _textures;
        public IReadOnlyList<Texture2D> Textures => _textures;
        public int CurrentFrame { get; set; }
        public float FrameTime { get; set; }
        public float TimeElapsed { get; set; }
        public bool IsAnimating { get; set; }
        public Rectangle textureRectangle { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public AnimationComponent(Texture2D[] textures, int width = 0, int height = 0, float frameTime = 0.0f)
        {
            _textures = new List<Texture2D>(textures);
            CurrentFrame = 0;
            FrameTime = frameTime;
            TimeElapsed = 0f;
            IsAnimating = true;
            if ((width == 0 || height == 0) && textures?.Length > 0)
            {
                textureRectangle = new Rectangle(0, 0, textures[0].Width, textures[0].Height);
            }
            else
            {
                textureRectangle = new Rectangle(0, 0, width, height);
            }
            this.width = textureRectangle.Width;
            this.height = textureRectangle.Height;
        }
    }
}
