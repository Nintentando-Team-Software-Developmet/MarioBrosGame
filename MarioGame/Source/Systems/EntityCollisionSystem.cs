using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class EntityCollisionSystem : BaseSystem
    {
        private Entity playerEntity;
        private HashSet<Entity> registeredEntities = new HashSet<Entity>();
        private List<Body> powerUpBodies = new List<Body>();

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var entityArray = entities as Entity[] ?? entities.ToArray();

            playerEntity = FindPlayerEntity(entityArray);
            if (!registeredEntities.Contains(playerEntity))
            {
                RegisterPlayerCollisionEvents(playerEntity);
                registeredEntities.Add(playerEntity);
            }

            UpdatePowerUpBodies(entityArray);
        }

        private static Entity FindPlayerEntity(IEnumerable<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.HasComponent<PlayerComponent>())
                {
                    return entity;
                }
            }
            return null;
        }

        private void RegisterPlayerCollisionEvents(Entity player)
        {
            var colliderComponent = player.GetComponent<ColliderComponent>();
            colliderComponent.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                var otherEntity = GetEntityFromBody(fixtureB.Body);
                if (otherEntity != null && otherEntity.HasComponent<PowerUpComponent>())
                {
                    HandleCollision(player, otherEntity, contact);
                    Console.WriteLine("Collision detected between player and power-up.");
                }
                return true;
            };
        }

        private void UpdatePowerUpBodies(IEnumerable<Entity> entities)
        {
            powerUpBodies.Clear();
            var powerUpEntities = entities.WithComponents(typeof(ColliderComponent), typeof(PowerUpComponent));
            foreach (var powerUp in powerUpEntities)
            {
                powerUpBodies.Add(powerUp.GetComponent<ColliderComponent>().collider);
            }
        }

        private Entity GetEntityFromBody(Body body)
        {
            foreach (var entity in registeredEntities)
            {
                var colliderComponent = entity.GetComponent<ColliderComponent>();
                if (colliderComponent.collider == body)
                {
                    return entity;
                }
            }
            return null;
        }

        private static void HandleCollision(Entity player, Entity powerUp, Contact contact)
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
