using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Source.Systems
{
    public class EnemySystem : BaseSystem
    {
        private HashSet<Entity> registeredEntities = new HashSet<Entity>();

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> enemies = entities.WithComponents(typeof(ColliderComponent), typeof(EnemyComponent), typeof(MovementComponent));
            foreach (var entity in enemies)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var enemy = entity.GetComponent<EnemyComponent>();
                var animation = entity.GetComponent<AnimationComponent>();

                if (entity.HasComponent<PlayerComponent>()) continue;
                if (enemy == null) Console.WriteLine("EnemyComponent is null: " + entity);
                if (collider == null || movement == null && enemy == null) continue;
                if (!registeredEntities.Contains(entity))
                {
                    RegisterEnemyEvents(collider, animation, movement, entity, enemy);
                    registeredEntities.Add(entity);
                }
                if (entity.HasComponent<KoopaComponent>())
                {
                    var koopa = entity.GetComponent<KoopaComponent>();

                    if (koopa.IsKnocked)
                    {
                        koopa.KnockedTime -= (float)gameTime?.ElapsedGameTime.TotalSeconds;
                        if (koopa.KnockedTime < 0)
                        {
                            koopa.IsKnocked = false;
                            koopa.KnockedTime = GameConstants.KoopaKnockedTime;
                            koopa.IsReviving = true;
                            animation.Play(AnimationState.REVIVE);
                        }
                    }
                    else if (koopa.IsReviving)
                    {
                        koopa.RevivingTime -= (float)gameTime?.ElapsedGameTime.TotalSeconds;
                        if (koopa.RevivingTime < 0)
                        {
                            koopa.IsReviving = false;
                            koopa.RevivingTime = GameConstants.KoopaReviveTime;
                            if (movement.Direction == MovementType.RIGHT)
                            {
                                animation.Play(AnimationState.WALKRIGHT);
                            }
                            else if (movement.Direction == MovementType.LEFT)
                            {
                                animation.Play(AnimationState.WALKLEFT);
                            }
                            collider.velocity = 1.1f;
                            enemy.IsAlive = true;
                        }
                    }

                }
            }
        }

        private static void RegisterEnemyEvents(ColliderComponent collider, AnimationComponent animation = null, MovementComponent movement = null, Entity entity = null, EnemyComponent enemy = null)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {

                AetherVector2 normal = contact.Manifold.LocalNormal;
                if (CollisionAnalyzer.GetDirectionCollision(contact) == CollisionType.UP)
                {
                    //collider.velocity = 0;
                    movement.Direction = MovementType.STOP;
                    if (entity.HasComponent<KoopaComponent>())
                    {
                        var koopa = entity.GetComponent<KoopaComponent>();
                        if (koopa.IsKnocked)
                        {
                            movement.Direction = MovementType.RIGHT;
                            collider.velocity = 3.1f;
                        }
                        else
                        {
                            koopa.IsKnocked = true;
                            animation.Play(AnimationState.KNOCKED);
                        }
                    }
                    else
                    {
                        enemy.IsAlive = false;
                        animation.Play(AnimationState.DIE);
                    }
                }
                return true;
            };
        }

    }
}