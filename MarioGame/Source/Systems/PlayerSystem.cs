using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Services;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem
    {
        private List<Body> _enemyBodies = new();
        private HashSet<Entity> registeredEntities = new();
        private Guid _invincibilityTimerId;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var enemyBodiesCache = new List<Body>();

            foreach (var enemy in entities.WithComponents(typeof(EnemyComponent), typeof(ColliderComponent)))
            {
                enemyBodiesCache.Add(enemy.GetComponent<ColliderComponent>().collider);
            }
            _enemyBodies = enemyBodiesCache;

            foreach (var player in entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent)))
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var playerPosition = colliderComponent.Position;

                if (!playerComponent.IsAlive && !playerComponent.IsInvincibleAfterHit)
                {
                    if (playerComponent.ShouldProcessDeath)
                    {
                        StartDeathAnimation(playerComponent, colliderComponent, 50);
                        playerComponent.ShouldProcessDeath = false;
                    }
                    if (gameTime != null)
                    {
                        UpdateDeathAnimation(gameTime, playerComponent, colliderComponent);
                    }
                    continue;
                }

                if (playerPosition.Y > GameConstants.CameraViewportHeight + 100 && playerComponent.IsAlive)
                {
                    playerComponent.IsAlive = false;
                    StartDeathAnimation(playerComponent, colliderComponent, 500);
                }
                if (!registeredEntities.Contains(player))
                {
                    RegisterEnemyEvents(colliderComponent, playerComponent);
                    registeredEntities.Add(player);
                }
            }
        }

        private void RegisterEnemyEvents(ColliderComponent collider, PlayerComponent playerComponent)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                if (!playerComponent.IsAlive || playerComponent.IsInvincibleAfterHit)
                {
                    return false;
                }

                var otherBody = fixtureB.Body;
                if (_enemyBodies.Contains(otherBody))
                {
                    CollisionType collisionDirection = CollisionAnalyzer.GetDirectionCollision(contact);
                    Console.WriteLine($"Player collided with enemy. Collision direction: {collisionDirection}");

                    if (collisionDirection == CollisionType.DOWN ||
                        collisionDirection == CollisionType.LEFT ||
                        collisionDirection == CollisionType.RIGHT)
                    {
                        if (playerComponent.IsBig)
                        {
                            Console.WriteLine("Player is big and collided with enemy. Becoming small and invincible.");
                            playerComponent.IsBig = false;
                            playerComponent.IsInvincibleAfterHit = true;
                            _invincibilityTimerId = TimerService.Instance.StartTimer(3.0f, () =>
                            {
                                playerComponent.IsInvincibleAfterHit = false;
                                Console.WriteLine("Player is no longer invincible.");
                            });
                        }
                        else
                        {
                            Console.WriteLine("Player is not big and collided with enemy. Dying.");
                            playerComponent.IsAlive = false;
                            playerComponent.ShouldProcessDeath = true;
                        }
                    }
                }
                return true;
            };
        }

        public static void StartDeathAnimation(PlayerComponent playerComponent, ColliderComponent colliderComponent, float jumpForce)
        {
            if (playerComponent == null) throw new ArgumentNullException(nameof(playerComponent));
            if (colliderComponent == null) throw new ArgumentNullException(nameof(colliderComponent));
            playerComponent.IsDying = true;
            playerComponent.DeathTimer = 0;
            colliderComponent.RemoveCollider();
            colliderComponent.collider.ApplyForce(new AetherVector2(0, -jumpForce));
        }

        public static void UpdateDeathAnimation(GameTime gameTime, PlayerComponent playerComponent, ColliderComponent colliderComponent)
        {
            if (playerComponent == null) throw new ArgumentNullException(nameof(playerComponent));
            if (gameTime == null) throw new ArgumentNullException(nameof(gameTime));
            if (colliderComponent == null) throw new ArgumentNullException(nameof(colliderComponent));
            playerComponent.DeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            Console.WriteLine($"Death timer: {playerComponent.DeathTimer}");

            if (playerComponent.DeathTimer <= 1f)
            {
                colliderComponent.collider.ApplyForce(new AetherVector2(0, -22f));
            }
            else if (playerComponent.DeathTimer > 1f && playerComponent.DeathTimer <= 2f)
            {
                colliderComponent.collider.ApplyForce(new AetherVector2(0, 22f));
            }
            else
            {
                colliderComponent.collider.LinearVelocity = AetherVector2.Zero;
                playerComponent.DeathAnimationComplete = true;
            }
        }
    }
}
