using System;
using System.Collections.Generic;

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
    public class PlayerSystem : BaseSystem
    {
        private List<Body> _enemyBodies = new();
        private HashSet<Entity> registeredEntities = new();

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> enemies = entities.WithComponents(typeof(EnemyComponent), typeof(ColliderComponent));
            foreach (Entity enemy in enemies)
            {
                _enemyBodies.Add(enemy.GetComponent<ColliderComponent>().collider);
            }
            IEnumerable<Entity> players = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            foreach (var player in players)
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var animationComponent = player.GetComponent<AnimationComponent>();
                var movementComponent = player.GetComponent<MovementComponent>();
                var playerPosition = colliderComponent.Position;

                if (playerComponent.ShouldProcessDeath)
                {
                    StartDeathAnimation(playerComponent, colliderComponent, 50);
                    playerComponent.ShouldProcessDeath = false;
                }

                if (playerPosition.Y > GameConstants.CameraViewportHeight + 100 && playerComponent.IsAlive)
                {
                    playerComponent.IsAlive = false;
                    StartDeathAnimation(playerComponent, colliderComponent, 500);
                }
                if (!registeredEntities.Contains(player))
                {
                    RegisterEnemyEvents(colliderComponent, playerComponent, movementComponent, animationComponent);
                    registeredEntities.Add(player);
                }


                if (!playerComponent.IsAlive)
                {
                    if (gameTime != null)
                    {
                        UpdateDeathAnimation(gameTime, playerComponent, colliderComponent, animationComponent);
                    }
                }
            }

        }
        private void RegisterEnemyEvents(ColliderComponent collider, PlayerComponent playerComponent, MovementComponent movement, AnimationComponent animationComponent)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                if (!playerComponent.IsAlive)
                {
                    return false;
                }
                var otherBody = fixtureB.Body;

                if (_enemyBodies.Contains(otherBody))
                {
                    CollisionType collisionDirection = CollisionAnalyzer.GetDirectionCollision(contact);

                    if (collisionDirection == CollisionType.DOWN ||
                        collisionDirection == CollisionType.LEFT ||
                        collisionDirection == CollisionType.RIGHT)
                    {
                        playerComponent.IsAlive = false;
                        playerComponent.ShouldProcessDeath = true;
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

        public static void UpdateDeathAnimation(GameTime gameTime, PlayerComponent playerComponent, ColliderComponent colliderComponent, AnimationComponent animationComponent)
        {
            if (playerComponent == null) throw new ArgumentNullException(nameof(playerComponent));
            if (gameTime == null) throw new ArgumentNullException(nameof(gameTime));
            if (colliderComponent == null) throw new ArgumentNullException(nameof(colliderComponent));
            playerComponent.DeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (animationComponent == null) throw new ArgumentNullException(nameof(animationComponent));
            animationComponent.Play(AnimationState.DIE);

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
