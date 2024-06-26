using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems
{
    public class NonPlayerMovementSystem : BaseSystem
    {
        private HashSet<Entity> registeredEntities = new HashSet<Entity>();
        private Dictionary<Entity, float> mushroomVerticalMovement = new Dictionary<Entity, float>();


        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> movementEntities = entities.WithComponents(typeof(ColliderComponent), typeof(MovementComponent), typeof(AnimationComponent));
            foreach (var entity in movementEntities)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var animation = entity.GetComponent<AnimationComponent>();
                if (entity.HasComponent<PlayerComponent>()) continue;
                if (collider != null && movement != null && animation != null)
                {
                    if (!registeredEntities.Contains(entity))
                    {
                        RegisterChangePositionEvent(collider, movement, animation);
                        registeredEntities.Add(entity);
                    }
                    float verticalVelocity = collider.collider.LinearVelocity.Y;
                    float horizontalVelocity = collider.velocity;
                    BaseComponent entityComponent;

                    if (entity.HasComponent<StarComponent>())
                    {
                        entityComponent = entity.GetComponent<StarComponent>();
                        if (gameTime != null)
                            HandleStarComponentMovement(entity, collider, ref horizontalVelocity, gameTime,
                                entityComponent);
                    }

                    else if (entity.HasComponent<MushroomComponent>())
                    {
                        if (gameTime != null)
                            HandleMushroomComponentMovement(entity, collider, ref horizontalVelocity, gameTime);
                    }
                    else if (entity.HasComponent<FlowerComponent>())
                    {
                        if (gameTime != null)
                            HandleFlowerComponentMovement(entity, collider, ref horizontalVelocity, gameTime);
                    }
                    else if (entity.HasComponent<CoinComponent>())
                    {
                        if (gameTime != null)
                            HandleCoinComponentMovement(entity, collider, ref horizontalVelocity, gameTime);
                    }

                    switch (movement.Direction)
                    {
                        case MovementType.LEFT:
                            collider.collider.LinearVelocity = new AetherVector2(-horizontalVelocity, collider.collider.LinearVelocity.Y);
                            break;
                        case MovementType.RIGHT:
                            collider.collider.LinearVelocity = new AetherVector2(horizontalVelocity, collider.collider.LinearVelocity.Y);
                            break;
                    }
                }
            }
        }

        private void HandleMushroomComponentMovement(Entity entity, ColliderComponent collider, ref float horizontalVelocity, GameTime gameTime)
        {
            var mushroomComponent = entity.GetComponent<MushroomComponent>();
            if (mushroomComponent.statusBlock)
            {
                if (!mushroomVerticalMovement.ContainsKey(entity))
                {
                    mushroomVerticalMovement[entity] = 0;
                }
                if (mushroomVerticalMovement[entity] < 0.7f)
                {
                    PowerUpRise(gameTime, entity, collider, ref horizontalVelocity, 0.5f, false);
                }
                else
                {
                    collider.Enabled(true);
                    horizontalVelocity = mushroomComponent.HorizontalVelocity;
                }
            }
        }

        private void HandleFlowerComponentMovement(Entity entity, ColliderComponent collider, ref float horizontalVelocity, GameTime gameTime)
        {
            var mushroomComponent = entity.GetComponent<FlowerComponent>();
            if (mushroomComponent.statusBlock)
            {
                if (!mushroomVerticalMovement.ContainsKey(entity))
                {
                    mushroomVerticalMovement[entity] = 0;
                }
                if (mushroomVerticalMovement[entity] < 0.7f)
                {
                    PowerUpRise(gameTime, entity, collider, ref horizontalVelocity, 0.5f, false);
                }
                else
                {
                    collider.Enabled(true);
                }
            }
        }
        private void HandleCoinComponentMovement(Entity entity, ColliderComponent collider, ref float horizontalVelocity, GameTime gameTime)
        {
            var coinComponent = entity.GetComponent<CoinComponent>();
            if (coinComponent.statusBlock)
            {
                if (!mushroomVerticalMovement.ContainsKey(entity))
                {
                    mushroomVerticalMovement[entity] = 0;
                }
                if (mushroomVerticalMovement[entity] < 1.5f)
                {
                    PowerUpRise(gameTime, entity, collider, ref horizontalVelocity, 4.5f, false);
                }
                else if (mushroomVerticalMovement[entity] <= 3.0f)
                {
                    PowerUpRise(gameTime, entity, collider, ref horizontalVelocity, 4.5f, true);
                }
                else
                {
                    mushroomVerticalMovement[entity] = 0;
                    coinComponent.statusBlock = false;

                }
            }
        }
        private void HandleStarComponentMovement(Entity entity, ColliderComponent collider, ref float horizontalVelocity, GameTime gameTime, BaseComponent entityComponent)
        {
            var mushroomComponent = entity.GetComponent<StarComponent>();
            if (mushroomComponent.statusBlock)
            {
                if (!mushroomVerticalMovement.ContainsKey(entity))
                {
                    mushroomVerticalMovement[entity] = 0;
                }
                if (mushroomVerticalMovement[entity] < 0.7f)
                {
                    PowerUpRise(gameTime, entity, collider, ref horizontalVelocity, 0.5f, false);
                }
                else
                {
                    collider.Enabled(true);
                    ((StarComponent)entityComponent).VerticalVelocity = Math.Min(collider.collider.LinearVelocity.Y + 0.1f, 5f);
                    horizontalVelocity = ((StarComponent)entityComponent).HorizontalVelocity;
                    if (!collider.isJumping())
                    {
                        collider.collider.ApplyLinearImpulse(new AetherVector2(0, -3.8f));
                    }
                }
            }
        }

        public void PowerUpRise(GameTime gameTime, Entity entity, ColliderComponent collider, ref float horizontalVelocity, float upPosition, bool IsDescent)
        {
            if (gameTime != null)
            {
                float increment = upPosition * (float)gameTime.ElapsedGameTime.TotalSeconds;
                mushroomVerticalMovement[entity] += increment;
                if (collider != null)
                {
                    var currentPosition = collider.collider.Position;
                    if (IsDescent)
                    {
                        currentPosition.Y += increment;
                    }
                    else
                    {
                        currentPosition.Y -= increment;
                    }
                    collider.collider.Position = currentPosition;
                }
            }
            if (collider != null) collider.Enabled(false);
            horizontalVelocity = 0;
        }

        private static void RegisterChangePositionEvent(ColliderComponent collider, MovementComponent movement, AnimationComponent animation)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                AetherVector2 normal = contact.Manifold.LocalNormal;
                if (CollisionAnalyzer.GetCollisionType(contact) == CollisionType.HORIZONTAL)
                {
                    if (movement.Direction == MovementType.LEFT)
                    {
                        movement.Direction = MovementType.RIGHT;
                        if (animation.containsState(AnimationState.WALKRIGHT) && animation.currentState != AnimationState.KNOCKED && animation.currentState != AnimationState.REVIVE)
                        {
                            animation.Play(AnimationState.WALKRIGHT);
                        }
                    }
                    else if (movement.Direction == MovementType.RIGHT)
                    {
                        movement.Direction = MovementType.LEFT;
                        if (animation.containsState(AnimationState.WALKLEFT) && animation.currentState != AnimationState.KNOCKED && animation.currentState != AnimationState.REVIVE)
                        {
                            animation.Play(AnimationState.WALKLEFT);
                        }
                    }
                }
                return true;
            };
        }
    }
}

