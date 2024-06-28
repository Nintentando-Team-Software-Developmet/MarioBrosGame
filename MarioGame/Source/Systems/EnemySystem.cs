using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
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
        private HashSet<Entity> registeredEntities = new();
        private List<Body> _bodiesToRemove = new();
        private List<Body> _playerBodies = new();

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> players = entities.WithComponents(typeof(ColliderComponent), typeof(PlayerComponent));

            foreach (var player in players)
            {
                _playerBodies.Add(player.GetComponent<ColliderComponent>().collider);
            }

            IEnumerable<Entity> enemies = entities.WithComponents(typeof(ColliderComponent), typeof(EnemyComponent), typeof(MovementComponent));
            foreach (var entity in enemies)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var enemy = entity.GetComponent<EnemyComponent>();
                var animation = entity.GetComponent<AnimationComponent>();

                if (enemy == null) Console.WriteLine("EnemyComponent is null: " + entity);
                if (collider == null || movement == null && enemy == null) continue;
                if (!registeredEntities.Contains(entity))
                {
                    var player = EntityExtensions.GetPlayer(entities.ToList());
                    if (player == null) continue;
                    RegisterEnemyEvents(collider, player.GetComponent<PlayerComponent>(), animation, movement, entity, enemy);
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
                            koopa.IsKillable = false;
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

                if (!enemy.IsAlive)
                {
                    enemy.KillTime -= (float)gameTime?.ElapsedGameTime.TotalSeconds;
                    collider.collider.LinearVelocity = new AetherVector2(0, 1);
                    if (enemy.KillTime <= 0)
                    {
                        foreach (var body in _bodiesToRemove)
                        {
                            body.World.Remove(body);
                        }
                        _bodiesToRemove.Clear();
                        entity.ClearComponents();
                    }

                }
            }
        }

        private void RegisterEnemyEvents(ColliderComponent collider, PlayerComponent player, AnimationComponent animation = null, MovementComponent movement = null, Entity entity = null, EnemyComponent enemy = null)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                var enemyBody = fixtureA.Body;
                var otherBody = fixtureB.Body;
                if (!enemy.IsAlive)
                {
                    return false;
                }

                if (_playerBodies.Contains(otherBody))
                {
                    if (player != null)
                        if (player.IsStarInvincible)
                        {
                            enemy.IsAlive = false;
                            animation?.Play(AnimationState.DIE);
                            enemyBody.ResetDynamics();
                            fixtureA.CollidesWith = Category.None;
                            _bodiesToRemove.Add(enemyBody);
                            Console.WriteLine("Enemy destroyed by star-powered player.");
                            return true;
                        }

                    if (CollisionAnalyzer.GetDirectionCollision(contact) == CollisionType.UP)
                    {
                        movement.Direction = MovementType.STOP;
                        if (entity.HasComponent<KoopaComponent>())
                        {
                            var koopa = entity.GetComponent<KoopaComponent>();
                            if (koopa.IsKnocked && !koopa.IsKillable)
                            {
                                movement.Direction = MovementType.RIGHT;
                                koopa.IsKillable = true;
                                collider.velocity = 5.1f;
                            }
                            else if (koopa.IsKillable)
                            {
                                enemy.IsAlive = false;
                                animation.Play(AnimationState.DIE);
                                enemyBody.ResetDynamics();
                                fixtureA.CollidesWith = Category.None;
                                enemyBody.ApplyForce(new AetherVector2(0, 64));
                                _bodiesToRemove.Add(enemyBody);
                            }
                            else
                            {
                                koopa.IsKnocked = true;
                                otherBody.ApplyForce(new AetherVector2(0, -300));
                                animation.Play(AnimationState.KNOCKED);
                            }
                        }
                        else
                        {
                            enemy.IsAlive = false;
                            animation.Play(AnimationState.DIE);
                            otherBody.ApplyForce(new AetherVector2(0, -300));
                            enemyBody.ResetDynamics();
                            _bodiesToRemove.Add(enemyBody);
                        }
                    }
                }
                return true;
            };
        }

        private Entity GetEntityFromBody(Body body)
        {
            return registeredEntities.FirstOrDefault(e =>
            {
                var colliderComponent = e.GetComponent<ColliderComponent>();
                return colliderComponent != null && colliderComponent.collider == body;
            });
        }
    }
}
