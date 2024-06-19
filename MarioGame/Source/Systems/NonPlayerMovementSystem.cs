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
            IEnumerable<Entity> movementEntities = entities.WithComponents(typeof(ColliderComponent), typeof(MovementComponent));
            foreach (var entity in movementEntities)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var movement = entity.GetComponent<MovementComponent>();
                if(entity.HasComponent<PlayerComponent>()) continue;
                if (collider != null && movement != null)
                {
                    if (!registeredEntities.Contains(entity))
                    {
                        RegisterChangePositionEvent(collider, movement, entity);
                        registeredEntities.Add(entity);
                    }

                    float verticalVelocity = collider.collider.LinearVelocity.Y;
                    float horizontalVelocity = 1.1f;
                    BaseComponent entityComponent;
                    
                    if (entity.HasComponent<StarComponent>())
                    {
                        entityComponent = entity.GetComponent<StarComponent>();
                        ((StarComponent)entityComponent).VerticalVelocity = Math.Min(collider.collider.LinearVelocity.Y + 0.1f, 5f);
                        horizontalVelocity = ((StarComponent)entityComponent).HorizontalVelocity;
                        verticalVelocity = ((StarComponent)entityComponent).VerticalVelocity;
                    }

                    else if (entity.HasComponent<MushroomComponent>())
                    {
                        entityComponent = entity.GetComponent<MushroomComponent>();
                        horizontalVelocity = ((MushroomComponent)entityComponent).HorizontalVelocity;
                    }

                    switch (movement.Direction)
                    {
                        case MovementType.LEFT:
                            collider.collider.LinearVelocity = new AetherVector2(-horizontalVelocity, verticalVelocity);
                            break;
                        case MovementType.RIGHT:
                            collider.collider.LinearVelocity = new AetherVector2(horizontalVelocity, verticalVelocity);
                            break;
                    }
                }
            }
        }

        private static void RegisterChangePositionEvent(ColliderComponent collider, MovementComponent movement, Entity entity)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                AetherVector2 normal = contact.Manifold.LocalNormal;
                if (Math.Abs(normal.X) > Math.Abs(normal.Y))
                {
                    if (movement.Direction == MovementType.LEFT)
                    {
                        movement.Direction = MovementType.RIGHT;
                    }
                    else if (movement.Direction == MovementType.RIGHT)
                    {
                        movement.Direction = MovementType.LEFT;
                    }
                }
                else
                {
                    if (entity.HasComponent<StarComponent>())
                    {
                        collider.collider.LinearVelocity = new AetherVector2(collider.collider.LinearVelocity.X, -6f);
                    }
                }
                return true;
            };
        }
    }
}

