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
    public class EnemyMovementSystem : BaseSystem
    {
        private HashSet<Entity> registeredEntities = new HashSet<Entity>();
    
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> enemies = entities.WithComponents(typeof(ColliderComponent), typeof(EnemyComponent));
            foreach (var enemy in enemies)
            {
                var collider = enemy.GetComponent<ColliderComponent>();
                var movement = enemy.GetComponent<MovementComponent>();
                if (collider != null && movement != null)
                {
                    if (!registeredEntities.Contains(enemy))
                    {
                        RegisterEvents(collider, movement);
                        registeredEntities.Add(enemy);
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

        private static void RegisterEvents(ColliderComponent collider, MovementComponent movement)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                AetherVector2 normal = contact.Manifold.LocalNormal;
                if (Math.Abs(normal.X) > Math.Abs(normal.Y))
                {
                    if (movement.direcction == MovementType.LEFT)
                    {
                        movement.direcction = MovementType.RIGHT;
                    }
                    else if (movement.direcction == MovementType.RIGHT)
                    {
                        movement.direcction = MovementType.LEFT;
                    }
                     collider.collider.ApplyLinearImpulse(new AetherVector2(0, 10f));
                }
                return true;
            };
        }
    }
}