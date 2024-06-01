using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

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

        public AnimationComponent(Texture2D[] textures, float frameTime = 0.1f)
        {
            _textures = new List<Texture2D>(textures);
            CurrentFrame = 0;
            FrameTime = frameTime;
            TimeElapsed = 0f;
            IsAnimating = true;
        }
    }
}
