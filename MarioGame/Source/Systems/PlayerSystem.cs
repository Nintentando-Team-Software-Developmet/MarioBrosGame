using System;
using System.Collections.Generic;

using MarioGame;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Collision.Shapes;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
using Vector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem
    {
        private List<Body> _enemyBodies = new();
        private HashSet<Entity> registeredEntities = new();
        private List<Body> _poleBodies = new();
        private static double invulnerabilityEndTime { get; set; }
        private bool isInvulnerable { get; set; }


        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {

            IEnumerable<Entity> enemies = entities.WithComponents(typeof(EnemyComponent), typeof(ColliderComponent),typeof(AnimationComponent));
            foreach (Entity enemy in enemies)
            {
                _enemyBodies.Add(enemy.GetComponent<ColliderComponent>().collider);
            }
            IEnumerable<Entity> winPoles = entities.WithComponents(typeof(WinPoleSensorComponent), typeof(ColliderComponent));
            foreach (Entity winPole in winPoles)
            {
                _poleBodies.Add(winPole.GetComponent<ColliderComponent>().collider);
            }
            IEnumerable<Entity> players = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            foreach (var player in players)
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var animationComponent = player.GetComponent<AnimationComponent>();
                var movementComponent = player.GetComponent<MovementComponent>();
                var playerPosition = colliderComponent.Position;
                ChangeAnimationColliderPlayer.TransformTogetherWithPlayerStatus(playerComponent,animationComponent,colliderComponent);

                playerComponent.PlayerPositionX = (int) playerPosition.X;
                playerComponent.PlayerPositionY = (int) playerPosition.Y;
                if (playerComponent.ShouldProcessDeath)
                {
                    StartDeathAnimation(playerComponent, colliderComponent, 50,animationComponent);
                    playerComponent.ShouldProcessDeath = false;
                }

                if (playerPosition.Y > GameConstants.CameraViewportHeight + 100 && playerComponent.IsAlive)
                {
                    playerComponent.IsAlive = false;
                    StartDeathAnimation(playerComponent, colliderComponent, 500,animationComponent);
                }
                if (!registeredEntities.Contains(player))
                {
                    RegisterPlayerEvents(colliderComponent, playerComponent, animationComponent,gameTime,enemies);
                    registeredEntities.Add(player);
                }
                if (playerComponent.MayTeleport)
                {
                    colliderComponent.collider.Position = new Vector2(colliderComponent.collider.Position.X + 0.64f, colliderComponent.collider.Position.Y + -0.5f);
                    playerComponent.MayTeleport = false;
                }
                if (colliderComponent.collider.Position.X < 131.25)
                {
                    if (playerComponent.HasReachedEnd && animationComponent.currentState == AnimationState.WALKRIGHT)
                    {
                        colliderComponent.collider.LinearVelocity = new Vector2(2, 4);
                    }
                    if (playerComponent.HasReachedEnd && animationComponent.currentState == AnimationState.WIN)
                    {
                        colliderComponent.collider.LinearVelocity = new Vector2(0, 1);
                    }
                }
                else
                {
                    player.RemoveComponent<AnimationComponent>();
                }
                if (!playerComponent.IsAlive)
                {
                    if (gameTime != null)
                    {
                        UpdateDeathAnimation(gameTime, playerComponent, colliderComponent, animationComponent);
                    }
                }

                if (gameTime != null && isInvulnerable &&
                    gameTime.TotalGameTime.TotalSeconds < invulnerabilityEndTime &&
                    playerComponent.statusMario == StatusMario.SmallMario)
                {
                    ChangeAnimationColliderPlayer.CheckEnemyProximity(colliderComponent,animationComponent, enemies, gameTime,
                        invulnerabilityEndTime);
                }

                if (playerComponent.IsInTransition)
                {
                    if (!playerComponent.IsInSecretLevel)
                    {
                        animationComponent.Play(AnimationState.STOPLEFT);
                        if (playerComponent.PlayerPositionY > 600)
                        {
                            playerComponent.IsInTransition = false;
                            playerComponent.IsInSecretLevel = true;
                        }
                    }
                    else
                    {
                        animationComponent.Play(AnimationState.STOP);
                        colliderComponent.collider.ApplyForce(new Vector2(4, 0));
                        if (playerComponent.PlayerPositionX > 950)
                        {
                            playerComponent.IsInTransition = false;
                            playerComponent.IsInSecretLevel = false;
                        }
                    }
                }
            }


        }
        private void RegisterPlayerEvents(ColliderComponent collider, PlayerComponent playerComponent, AnimationComponent animationComponent,GameTime gameTime,IEnumerable<Entity> enemyEntities)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                if (!playerComponent.IsAlive )
                {
                    return false;
                }
                var otherBody = fixtureB.Body;
                if (_poleBodies.Contains(otherBody))
                {
                    playerComponent.HasReachedEnd = true;
                    animationComponent.Play(AnimationState.WIN);
                    playerComponent.MayTeleport = true;
                }
                if (_enemyBodies.Contains(otherBody))
                {
                    CollisionType collisionDirection = CollisionAnalyzer.GetDirectionCollision(contact);

                    if (playerComponent.statusMario == StatusMario.BigMario || playerComponent.statusMario == StatusMario.FireMario )
                    {
                        if (collisionDirection == CollisionType.DOWN ||
                            collisionDirection == CollisionType.LEFT ||
                            collisionDirection == CollisionType.RIGHT)
                        {
                            playerComponent.statusMario = StatusMario.SmallMario;
                            isInvulnerable = true;
                            if (gameTime != null)
                            {
                                invulnerabilityEndTime = gameTime.TotalGameTime.TotalSeconds + 2.0;
                            }
                        }
                    }

                    else if (collisionDirection == CollisionType.DOWN ||
                        collisionDirection == CollisionType.LEFT ||
                        collisionDirection == CollisionType.RIGHT)
                    {
                        if( playerComponent.statusMario != StatusMario.StarMarioBig && playerComponent.statusMario != StatusMario.StarMarioSmall )
                        {
                            playerComponent.IsAlive = false;
                            playerComponent.ShouldProcessDeath = true;
                        }

                    }
                }
                if (animationComponent.currentState == AnimationState.WIN && CollisionAnalyzer.GetDirectionCollision(contact) == CollisionType.UP)
                {
                    animationComponent.Play(AnimationState.WALKRIGHT);
                    collider.collider.IgnoreGravity = false;
                }

                if (playerComponent.IsInTransition)
                {
                    EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.Ducting));
                    fixtureA.Body.ResetDynamics();
                    fixtureB.CollidesWith = Category.None;
                }
                return true;
            };
        }

        public static void StartDeathAnimation(PlayerComponent playerComponent, ColliderComponent colliderComponent, float jumpForce,AnimationComponent animationComponent)
        {

            if (playerComponent == null) throw new ArgumentNullException(nameof(playerComponent));
            if (colliderComponent == null) throw new ArgumentNullException(nameof(colliderComponent));
            playerComponent.IsDying = true;
            playerComponent.DeathTimer = 0;
            playerComponent.statusMario = StatusMario.SmallMario;
            colliderComponent.RemoveCollider();
            colliderComponent.collider.ApplyForce(new AetherVector2(0, -jumpForce));
            EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.PlayerLostLife));
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
