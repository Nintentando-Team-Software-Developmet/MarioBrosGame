using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class EntityCollisionSystem : BaseSystem
    {
        private static bool colition { get; set; }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            var playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            var powerUpEntities = entities.WithComponents(typeof(PowerUpComponent), typeof(ColliderComponent));

            foreach (var player in playerEntities)
            {
                var playerCollider = player.GetComponent<ColliderComponent>();
                RegisterCollisionEvent(playerCollider, player, powerUpEntities);
            }
        }

        private static void RegisterCollisionEvent(ColliderComponent collider, Entity entity, IEnumerable<Entity> powerUpEntities)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                var otherEntity = GetEntityFromBody(fixtureB.Body, powerUpEntities);
                if (otherEntity != null && otherEntity.HasComponent<PowerUpComponent>())
                {
                    Console.WriteLine("Hay colision");
                    HandleCollision(entity, otherEntity);
                    colition = true;
                }
                else
                {
                    Console.WriteLine("NO Hay colision ");

                }

                return true;
            };
        }

        private static Entity GetEntityFromBody(Body body, IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                var colliderComponent = entity.GetComponent<ColliderComponent>();
                if (colliderComponent.collider == body)
                {
                    return entity;
                }
            }
            return null;
        }

        private static void HandleCollision(Entity player, Entity powerUp)
        {
            DispatchCollisionEvent(player, powerUp);
            DebugCollision(player, powerUp);
        }

        private static void DispatchCollisionEvent(Entity player, Entity powerUp)
        {
            if (powerUp.HasComponent<MushroomComponent>())
            {
                EventDispatcher.Instance.Dispatch(new PowerUpEvent(player, powerUp, PowerUpType.Mushroom));
            }
            else if (powerUp.HasComponent<StarComponent>())
            {
                EventDispatcher.Instance.Dispatch(new PowerUpEvent(player, powerUp, PowerUpType.Star));
            }
            else if (powerUp.HasComponent<FlowerComponent>())
            {
                EventDispatcher.Instance.Dispatch(new PowerUpEvent(player, powerUp, PowerUpType.FireFlower));
            }
        }

        private static void DebugCollision(Entity player, Entity other)
        {
            Console.WriteLine($"Collision detected between Player and {other.GetType().Name}");
        }
    }
}
