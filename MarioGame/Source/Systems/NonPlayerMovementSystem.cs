using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
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
                    if (movement.direcction == MovementType.LEFT)
                    {
                        collider.collider.LinearVelocity = new AetherVector2(-1.1f, collider.collider.LinearVelocity.Y);
                    }
                    else if (movement.direcction == MovementType.RIGHT)
                    {
                        collider.collider.LinearVelocity = new AetherVector2(1.1f, collider.collider.LinearVelocity.Y);
                    }
                }
            }
        }

        private static void RegisterChangePositionEvent(ColliderComponent collider, MovementComponent movement, AnimationComponent animation)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                AetherVector2 normal = contact.Manifold.LocalNormal;
                if (Math.Abs(normal.X) > Math.Abs(normal.Y))
                {
                    if (movement.direcction == MovementType.LEFT)
                    {
                        movement.direcction = MovementType.RIGHT;
                        if(animation.containsState(AnimationState.WALKRIGHT))
                        {
                            animation.Play(AnimationState.WALKRIGHT);
                        }
                    }
                    else if (movement.direcction == MovementType.RIGHT)
                    {
                        movement.direcction = MovementType.LEFT;
                        if(animation.containsState(AnimationState.WALKLEFT))
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