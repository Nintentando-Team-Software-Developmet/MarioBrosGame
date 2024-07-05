using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

using nkast.Aether.Physics2D.Dynamics;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Systems;

public class MarioPowersSystem : BaseSystem
{
    private bool colitionMushroom;
    private bool colitionFlower;
    private HashSet<ColliderComponent> registeredColliders = new HashSet<ColliderComponent>();
    private readonly Collection<Action> pendingActions = new Collection<Action>();
    private static double invulnerabilityEndTime { get; set; }
    private bool isInvulnerable { get; set; }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        var playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent), typeof(AnimationComponent));
        var mushroomEntities = entities.WithComponents(typeof(MushroomComponent), typeof(ColliderComponent));
        var flowerEntities = entities.WithComponents(typeof(FlowerComponent), typeof(ColliderComponent));
        var enemyEntities = entities.WithComponents(typeof(EnemyComponent), typeof(ColliderComponent), typeof(AnimationComponent));

        foreach (var player in playerEntities)
        {
            var playerCollider = player.GetComponent<ColliderComponent>();
            var playerAnimation = player.GetComponent<AnimationComponent>();
            var playerComponent = player.GetComponent<PlayerComponent>();

            if (!registeredColliders.Contains(playerCollider))
            {
                RegisterCollisionEvent(playerCollider, player, mushroomEntities, flowerEntities);
                registeredColliders.Add(playerCollider);
            }

            if (colitionMushroom)
            {
                if (playerComponent.statusMario == StatusMario.SmallMario)
                {
                    ChangeAnimationColliderPlayer.TransformToBigMario(playerAnimation, playerCollider);
                    playerComponent.statusMario = StatusMario.BigMario;

                    isInvulnerable = true;
                    if (gameTime != null)
                    {
                        invulnerabilityEndTime = gameTime.TotalGameTime.TotalSeconds + 5.0;
                    }
                }
            }
            else if (colitionFlower)
            {
                if (playerComponent.statusMario == StatusMario.BigMario)
                {
                    ChangeAnimationColliderPlayer.TransformToFireMario(playerAnimation, playerCollider);
                    playerComponent.statusMario = StatusMario.FireMario;
                }
            }

            if (gameTime != null && isInvulnerable && gameTime.TotalGameTime.TotalSeconds < invulnerabilityEndTime)
            {
                CheckEnemyProximity(playerCollider, enemyEntities,gameTime);
            }

        }

        ExecutePendingActions();
    }

    private void RegisterCollisionEvent(ColliderComponent collider, Entity player, IEnumerable<Entity> mushroomEntities, IEnumerable<Entity> flowerEntities)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var otherEntity = GetEntityFromBody(fixtureB.Body, mushroomEntities);
            var flowerEntity = GetEntityFromBody(fixtureB.Body, flowerEntities);
            if (otherEntity != null && otherEntity.HasComponent<MushroomComponent>())
            {
                colitionMushroom = true;
                pendingActions.Add(() => RemoveMushroomCollider(otherEntity));
                colitionFlower = false;
            }
            else if (flowerEntity != null && flowerEntity.HasComponent<FlowerComponent>())
            {
                colitionFlower = true;
                pendingActions.Add(() => RemoveMushroomCollider(flowerEntity));
            }
            else
            {
                colitionMushroom = false;
            }

            return true;
        };
    }

    private static void CheckEnemyProximity(ColliderComponent playerCollider, IEnumerable<Entity> enemyEntities, GameTime gameTime)
    {
        var playerPosition = playerCollider.collider.Position;

        foreach (var enemyEntity in enemyEntities)
        {
            var enemyCollider = enemyEntity.GetComponent<ColliderComponent>();
            if (enemyCollider != null)
            {
                var enemyPosition = enemyCollider.collider.Position;
                float distance;
                AetherVector2.Distance(ref playerPosition, ref enemyPosition, out distance);

                if (distance < 0.7f)
                {
                    enemyCollider.Enabled(false);
                }

                if (gameTime.TotalGameTime.TotalSeconds > invulnerabilityEndTime)
                {
                    enemyCollider.Enabled(true);
                }
                else
                {
                    enemyCollider.Enabled(true);
                }
            }
        }
    }

    private static void RemoveMushroomCollider(Entity mushroom)
    {
        var mushroomCollider = mushroom.GetComponent<ColliderComponent>();
        if (mushroomCollider != null)
        {
            mushroomCollider.RemoveCollider();
            mushroomCollider.Enabled(false);
            mushroomCollider.collider.Position = new nkast.Aether.Physics2D.Common.Vector2(100, 100);
        }
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

    private void ExecutePendingActions()
    {
        foreach (var action in pendingActions)
        {
            action();
        }
        pendingActions.Clear();
    }
}
