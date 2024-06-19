using System;
using System.Collections.Generic;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems;

public class KoopaAnimationSystem : BaseSystem, IRenderableSystem
{

    private readonly SpriteBatch _spriteBatch;
    public KoopaAnimationSystem(SpriteBatch spriteBatch)
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
            var enemies = entities.WithComponents(typeof(AnimationComponent), typeof(ColliderComponent), typeof(FacingComponent), typeof(MovementComponent), typeof(KoopaComponent));
            foreach (var entity in enemies)
            {
                if(entity.HasComponent<PlayerComponent>()) continue;
                var colliderComponent = entity.GetComponent<ColliderComponent>();
                var animation = entity.GetComponent<AnimationComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var facing = entity.GetComponent<FacingComponent>();
                var koopa = entity.GetComponent<KoopaComponent>();
                var enemy = entity.GetComponent<EnemyComponent>();
                animation.FrameTime = 0.8f;
                if (colliderComponent == null || animation == null || movement == null) continue;
                CommonRenders.DrawEntity(_spriteBatch, animation, colliderComponent);
                if (!animation.IsAnimating) continue;
                animation.TimeElapsed += (float)gameTime?.ElapsedGameTime.TotalSeconds;

                if (koopa.IsKnocked)
                {
                    koopa.KnockedTime -= (float)gameTime?.ElapsedGameTime.TotalSeconds;
                    if (koopa.KnockedTime < 0)
                    {
                        koopa.IsKnocked = false;
                        koopa.KnockedTime = GameConstants.KoopaKnockedTime;
                        koopa.IsReviving = true;
                    }
                }
                else if (koopa.IsReviving)
                {
                    koopa.RevivingTime -= (float)gameTime?.ElapsedGameTime.TotalSeconds;
                    if (koopa.RevivingTime < 0)
                    {
                        koopa.IsReviving = false;
                        koopa.RevivingTime = GameConstants.KoopaReviveTime;
                    }
                }

                if (!(animation.TimeElapsed > animation.FrameTime)) continue;
                animation.CurrentFrame++;
                if (animation.CurrentFrame >= animation.Textures.Count)
                {
                    if (!enemy.IsAlive)
                    {
                        entity.AddComponent(new AnimationComponent(Animations.entityTextures[enemy.DiedName], 64, 64));
                    }
                    else if (koopa.IsKnocked)
                    {
                        entity.AddComponent(new AnimationComponent(Animations.entityTextures[facing.KnockedName], 64, 64));
                    }
                    else if (koopa.IsReviving)
                    {
                        entity.AddComponent(new AnimationComponent(Animations.entityTextures[facing.RevivingName], 64, 64));
                    }
                    else switch (movement.direcction)
                    {
                        case MovementType.LEFT:
                            entity.AddComponent(new AnimationComponent(Animations.entityTextures[facing.LeftName], 64, 64));
                            break;
                        case MovementType.RIGHT:
                            entity.AddComponent(new AnimationComponent(Animations.entityTextures[facing.RigthName], 64, 64));
                            break;
                    }
                }
                animation.TimeElapsed = 0;
            }
        }
    }
}
