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

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> movementEntities = entities.WithComponents(typeof(ColliderComponent), typeof(MovementComponent), typeof(AnimationComponent));
            foreach (var entity in movementEntities)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                var animation = entity.GetComponent<AnimationComponent>();
                if(entity.HasComponent<PlayerComponent>()) continue;
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
                        ((StarComponent)entityComponent).VerticalVelocity = Math.Min(collider.collider.LinearVelocity.Y + 0.1f, 5f);
                        horizontalVelocity = ((StarComponent)entityComponent).HorizontalVelocity;
                        verticalVelocity = ((StarComponent)entityComponent).VerticalVelocity;
                    
                        if (!collider.isJumping())
                        {
                            collider.collider.ApplyLinearImpulse(new AetherVector2(0, -3.8f));
                        }
                    }

                    else if (entity.HasComponent<MushroomComponent>())
                    {
                        entityComponent = entity.GetComponent<MushroomComponent>();
                        horizontalVelocity = ((MushroomComponent)entityComponent).HorizontalVelocity;
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
                        if(animation.containsState(AnimationState.WALKRIGHT) && animation.currentState != AnimationState.KNOCKED && animation.currentState != AnimationState.REVIVE)
                        {
                            animation.Play(AnimationState.WALKRIGHT);
                        }
                    }
                    else if (movement.Direction == MovementType.RIGHT)
                    {
                        movement.Direction = MovementType.LEFT;
                        if(animation.containsState(AnimationState.WALKLEFT) && animation.currentState != AnimationState.KNOCKED && animation.currentState != AnimationState.REVIVE)
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

