using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public class AnimationSystem : BaseSystem, IRenderableSystem
    {
        private readonly SpriteBatch _spriteBatch;

        public AnimationSystem(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null)
                foreach (var entity in entities)
                {
                    var animation = entity.GetComponent<AnimationComponent>();

                    if (animation != null && animation.IsAnimating)
                    {
                        if (gameTime != null) animation.TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

                        if (animation.TimeElapsed >= animation.FrameTime)
                        {
                            animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.Textures.Count;
                            animation.TimeElapsed = 0f;
                        }
                    }
                }
        }

        public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
        {

            if (entities != null)
                foreach (var entity in entities)
                {
                    var animation = entity.GetComponent<AnimationComponent>();
                    var position = entity.GetComponent<PositionComponent>();

                    if (animation != null && position != null)
                    {
                        _spriteBatch.Draw(animation.Textures[animation.CurrentFrame], position.Position, Color.White);
                    }
                }

        }
    }
}
