using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems;

public class KoopaMovementSystem : BaseSystem
{
    private HashSet<Entity> registeredEntities = new HashSet<Entity>();

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        IEnumerable<Entity> movementEntities = entities.WithComponents(typeof(ColliderComponent), typeof(MovementComponent), typeof(KoopaComponent));
        foreach (var entity in movementEntities)
        {
            var collider = entity.GetComponent<ColliderComponent>();
            var movement = entity.GetComponent<MovementComponent>();
            var koopa = entity.GetComponent<KoopaComponent>();
            var enemy = entity.GetComponent<EnemyComponent>();
            if (entity.HasComponent<PlayerComponent>()) continue;
            if (collider == null || movement == null) continue;
            if (!registeredEntities.Contains(entity))
            {
                RegisterChangeKnockedEvent(collider, koopa, enemy, entity);
                registeredEntities.Add(entity);
            }
            if (!enemy.IsAlive)
            {
                collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
            }
            else if (koopa.IsKnocked)
            {
                if (movement.Direction == MovementType.LEFT && koopa.Hits == 3)
                {
                    collider.collider.LinearVelocity = new AetherVector2(-4.1f, collider.collider.LinearVelocity.Y);
                    koopa.KnockedTime += 0.01f;
                }
                else if (movement.Direction == MovementType.RIGHT && koopa.Hits == 3)
                {
                    collider.collider.LinearVelocity = new AetherVector2(4.1f, collider.collider.LinearVelocity.Y);
                    koopa.KnockedTime += 0.01f;
                }
                else if (movement.Direction == MovementType.RIGHT && koopa.Hits == 1)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                }
                else if (movement.Direction == MovementType.LEFT && koopa.Hits == 1)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                }
            }

            else if (koopa.IsReviving)
            {
                collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                koopa.Hits = default;
            }
            else
            {
                collider.collider.LinearVelocity = movement.Direction switch
                {
                    MovementType.LEFT => new AetherVector2(-1.1f, collider.collider.LinearVelocity.Y),
                    MovementType.RIGHT => new AetherVector2(1.1f, collider.collider.LinearVelocity.Y),
                    _ => collider.collider.LinearVelocity
                };
            }
        }
    }

    private void RegisterChangeKnockedEvent(ColliderComponent collider, KoopaComponent koopaComponent, EnemyComponent enemyComponent, Entity entity)

    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var normal = contact.Manifold.LocalNormal;

            if (Math.Abs(normal.X) < Math.Abs(normal.Y) && normal.Y < 0)
            {
                if (normal.Y < 0 && !koopaComponent.IsKnocked && !koopaComponent.IsReviving)
                {
                    koopaComponent.IsKnocked = true;
                    koopaComponent.IsReviving = false;
                    koopaComponent.KnockedTime = GameConstants.KoopaKnockedTime;
                    koopaComponent.RevivingTime = GameConstants.KoopaReviveTime;
                    koopaComponent.Hits = 1;
                    registeredEntities.Remove(entity);
                }
                else if (normal.Y < 0 && (koopaComponent.IsKnocked || koopaComponent.IsReviving) && koopaComponent.Hits >= 4)
                {
                    enemyComponent.IsAlive = false;
                    registeredEntities.Remove(entity);
                }
                else if (normal.Y < 0 && (koopaComponent.IsKnocked || koopaComponent.IsReviving) &&
                         koopaComponent.Hits < 4)
                {
                    koopaComponent.Hits += 1;
                    registeredEntities.Remove(entity);
                }
            }

            return true;
        };
    }
}
