using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils.SceneCommonData;

namespace SuperMarioBros.Source.Systems
{
    public class AnimationSystem : BaseSystem, IRenderableSystem
    {
        private SpriteBatch spriteBatch;
        public AnimationSystem(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> animationEntities = entities.WithComponents(typeof(AnimationComponent));
            foreach (var entity in animationEntities)
            {
                var animation = entity.GetComponent<AnimationComponent>();
                if (animation != null)
                {
                    animation.timeElapsed += (float)gameTime?.ElapsedGameTime.TotalSeconds;
                    if (animation.timeElapsed > animation.velocity)
                    {
                        animation.currentFrame++;
                        if (animation.currentFrame >= animation.animations[animation.currentState].Length)
                        {
                            animation.currentFrame = 0;
                        }
                        animation.timeElapsed = 0;
                    }

                }
            }
        }
        public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> animationEntities = entities.WithComponents(typeof(AnimationComponent));
            foreach (var entity in animationEntities)
            {
                var animation = entity.GetComponent<AnimationComponent>();
                var collider = entity.GetComponent<ColliderComponent>();
                if (animation != null && collider != null)
                {
                    if (animation.currentFrame >= animation.animations[animation.currentState].Length)
                    {
                        animation.currentFrame = 0;
                    }
                    CommonRenders.DrawEntity(spriteBatch, animation, collider);
                }
            }
        }
    }
}