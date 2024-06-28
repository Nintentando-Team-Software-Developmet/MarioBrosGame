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

    /// <summary>
    /// The EntityCollisionSystem class is responsible for managing collisions between entities in the game.
    /// It keeps track of entities that need to be destroyed due to collisions and handles collision events.
    /// </summary>
    public class EntityCollisionSystem : BaseSystem
    {

        private List<Body> _bodiesToDestroy = new List<Body>();

        /// <summary>
        /// The Update method is called every frame and checks for collisions between player entities and power-up entities.
        /// </summary>
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            var playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            var powerUpEntities = entities.WithComponents(typeof(PowerUpComponent), typeof(ColliderComponent));

            foreach (var player in playerEntities)
            {
                var playerCollider = player.GetComponent<ColliderComponent>();
                RegisterCollisionEvent(playerCollider, player, powerUpEntities);

                if (_bodiesToDestroy.Count == 0)
                {
                    return;
                }
                Console.WriteLine("Destroying bodies " + _bodiesToDestroy.Count);
                _bodiesToDestroy.ForEach(body => body.World.Remove(body));
                _bodiesToDestroy.Clear();
            }
        }

        /// <summary>
        /// The RegisterCollisionEvent method registers a collision event for a given entity.
        /// </summary>
        private void RegisterCollisionEvent(ColliderComponent collider, Entity entity, IEnumerable<Entity> powerUpEntities)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                var otherEntity = GetEntityFromBody(fixtureB.Body, powerUpEntities);
                if (otherEntity != null && otherEntity.HasComponent<PowerUpComponent>())
                {
                    Console.WriteLine("Collision Detected");
                    _bodiesToDestroy.Add(fixtureB.Body);
                    DebugCollision(otherEntity);
                    DispatchCollisionEvent(entity, otherEntity);
                }
                return true;
            };
        }

        /// <summary>
        /// The GetEntityFromBody method returns the entity associated with a given body.
        /// </summary>
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

        /// <summary>
        /// The DispatchCollisionEvent method dispatches a collision event for a player and a power-up.
        /// </summary>
        private static void DispatchCollisionEvent(Entity player, Entity powerUp)
        {
            if (powerUp.HasComponent<MushroomComponent>())
            {
                var evt = new PowerUpEvent(player, powerUp, PowerUpType.Mushroom);
                EventDispatcher.Instance.Dispatch(evt);
                Console.WriteLine($"Dispatched {evt}");
            }
            else if (powerUp.HasComponent<StarComponent>())
            {
                var evt = new PowerUpEvent(player, powerUp, PowerUpType.Star);
                EventDispatcher.Instance.Dispatch(evt);
                Console.WriteLine($"Dispatched {evt}");
            }
            else if (powerUp.HasComponent<FlowerComponent>())
            {
                var evt = new PowerUpEvent(player, powerUp, PowerUpType.FireFlower);
                EventDispatcher.Instance.Dispatch(evt);
                Console.WriteLine($"Dispatched {evt}");
            }
            powerUp.ClearComponents();
        }

        /// <summary>
        /// The DebugCollision method logs a collision between a player and another entity.
        /// </summary>
        private static void DebugCollision(Entity other)
        {
            Console.WriteLine($"Collision detected between Player and {other.GetComponent<PowerUpComponent>().PowerUpType}");
        }
    }
}
