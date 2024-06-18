using System;
using System.Collections.Generic;
using System.Threading;

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
            if(entity.HasComponent<PlayerComponent>()) continue;
            if (collider != null && movement != null)
            {
                if (!registeredEntities.Contains(entity))
                {
                    RegisterChangeKnockedEvent(collider, koopa);
                    registeredEntities.Add(entity);
                }

                if (koopa.IsKnocked)
                {
                    if (movement.direcction == MovementType.LEFT)
                    {
                        collider.collider.LinearVelocity = new AetherVector2(-4.1f, collider.collider.LinearVelocity.Y);
                    }
                    else if (movement.direcction == MovementType.RIGHT)
                    {
                        collider.collider.LinearVelocity = new AetherVector2(4.1f, collider.collider.LinearVelocity.Y);
                    }
                }
                else if (koopa.IsReviving)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                }
                else if (movement.direcction == MovementType.LEFT)
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

    private static void RegisterChangeKnockedEvent(ColliderComponent collider, KoopaComponent koopaComponent)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            AetherVector2 normal = contact.Manifold.LocalNormal;
            if (Math.Abs(normal.X) < Math.Abs(normal.Y))
            {
                if (normal.Y > 0.4)
                {
                    koopaComponent.IsKnocked = true;
                    koopaComponent.IsReviving = false;
                    koopaComponent.KnockedTime = GameConstants.KoopaKnockedTime;
                    koopaComponent.RevivingTime = GameConstants.KoopaReviveTime;
                }
            }
            return true;
        };
    }
}
