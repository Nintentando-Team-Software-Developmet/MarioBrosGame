using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
   public class EnemyAnimationSystem : BaseSystem, IRenderableSystem
   {
        private readonly SpriteBatch _spriteBatch;
        public EnemyAnimationSystem(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
          
        }
        public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null) {
                foreach (var entity in entities)
                {
                    if(!(entity is EnemyEntity)) continue;
                    var position = entity.GetComponent<PositionComponent>();
                    var animation = entity.GetComponent<AnimationComponent>();
                    animation.FrameTime = 0.8f;
                    if (position != null && animation != null)
                    {
                        _spriteBatch.Draw(animation.Textures[animation.CurrentFrame], position.Position, Color.White);
                        if(animation.IsAnimating)
                        {
                            animation.TimeElapsed += (float)gameTime?.ElapsedGameTime.TotalSeconds;
                            if(animation.TimeElapsed > animation.FrameTime)
                            {
                                animation.CurrentFrame++;
                                if(animation.CurrentFrame >= animation.Textures.Count)
                                {
                                    animation.CurrentFrame = 0;
                                }
                                animation.TimeElapsed = 0;
                            }
                        }
                    }
                }
            }
        }
   }
}