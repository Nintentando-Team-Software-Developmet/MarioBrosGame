using System.Collections.Generic;

using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Systems;

public class BlinkAnimationSystem : BaseSystem, IRenderableSystem
{

    private readonly SpriteBatch _spriteBatch;
    public BlinkAnimationSystem(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }
    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {

    }

    public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
    {
        if (entities != null)
        {
            var enemies = entities.WithComponents(typeof(AnimationComponent), typeof(ColliderComponent));
            foreach (var entity in enemies)
            {
                if(entity.HasComponent<PlayerComponent>()) continue;
                var colliderComponent = entity.GetComponent<ColliderComponent>();
                var animation = entity.GetComponent<AnimationComponent>();
                animation.FrameTime = 0.8f;
                if (colliderComponent != null && animation != null)
                {
                    CommonRenders.DrawEntity(_spriteBatch, animation, colliderComponent);
                    if (animation.IsAnimating)
                    {
                        animation.TimeElapsed += (float)gameTime?.ElapsedGameTime.TotalSeconds;
                        if (animation.TimeElapsed > animation.FrameTime)
                        {
                            animation.CurrentFrame++;
                            if (animation.CurrentFrame >= animation.Textures.Count)
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
