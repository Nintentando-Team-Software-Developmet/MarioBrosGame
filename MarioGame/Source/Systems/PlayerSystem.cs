using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem{

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
            foreach(var player in players){
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var movementComponent = player.GetComponent<MovementComponent>();
                var playerPosition = colliderComponent.Position;
                if(playerPosition.Y > GameConstants.CameraViewportHeight + 300)
                {
                    playerComponent.IsAlive = false;
                }
                if (!registeredEntities.Contains(player))
                {
                    RegisterEnemyEvents(colliderComponent, playerComponent, movementComponent);
                    registeredEntities.Add(player);
                }

            }

        }
        private void RegisterEnemyEvents(ColliderComponent collider, PlayerComponent playerComponent, MovementComponent movement)
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
                    if (CollisionAnalyzer.GetDirectionCollision(contact) != CollisionType.UP)
                    {
                        playerComponent.IsAlive = false;
                    }
                }
                return true;
            };
        }
    }
}
