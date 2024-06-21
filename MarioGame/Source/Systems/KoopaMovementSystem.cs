using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

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
    private List<Body> _bodiesToRemove = new List<Body>();

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
                collider.collider.LinearVelocity = new AetherVector2(0, 1);
                if (enemy.KillTime <= 0)
                {
                    foreach (var body in _bodiesToRemove)
                    {
                        body.World.Remove(body);
                    }
                    _bodiesToRemove.Clear();
                    entity.ClearComponents();
                }

            }
            else if (koopa.IsKnocked)
            {
                if (movement.Direction == MovementType.LEFT && koopa.Killable)
                {
                    collider.collider.LinearVelocity = new AetherVector2(-4.1f, collider.collider.LinearVelocity.Y);
                    koopa.KnockedTime += 0.01f;
                }
                else if (movement.Direction == MovementType.RIGHT && koopa.Killable)
                {
                    collider.collider.LinearVelocity = new AetherVector2(4.1f, collider.collider.LinearVelocity.Y);
                    koopa.KnockedTime += 0.01f;
                }
                else if (movement.Direction == MovementType.RIGHT && !koopa.Killable)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                }
                else if (movement.Direction == MovementType.LEFT && !koopa.Killable)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                }
            }

            else if (koopa.IsReviving)
            {
                collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                koopa.Killable = false;
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

    private static void RegisterChangeKnockedEvent(ColliderComponent collider, KoopaComponent koopaComponent, EnemyComponent enemyComponent, Entity entity)

    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var manifold = contact.Manifold;

            var bodyA = fixtureA.Body;
            var bodyB = fixtureB.Body;
            var normal = manifold.LocalNormal;

            if (Math.Abs(normal.X) < Math.Abs(normal.Y) && bodyA.Position.Y > bodyB.Position.Y)
            {
                if (!koopaComponent.IsKnocked && !koopaComponent.IsReviving)
                {
                    koopaComponent.IsKnocked = true;
                    koopaComponent.IsReviving = false;
                    koopaComponent.KnockedTime = GameConstants.KoopaKnockedTime;
                    koopaComponent.RevivingTime = GameConstants.KoopaReviveTime;
                    bodyB.ResetDynamics();
                    bodyB.ApplyForce(new AetherVector2(0, -64));
                }
                else if ((koopaComponent.IsKnocked || koopaComponent.IsReviving) && koopaComponent.Killable)
                {
                    enemyComponent.IsAlive = false;
                    fixtureA.CollidesWith = Category.None;
                }
                else if ((koopaComponent.IsKnocked || koopaComponent.IsReviving))
                {
                    koopaComponent.Killable = true;
                }
            }

            return true;
        };
    }
}
