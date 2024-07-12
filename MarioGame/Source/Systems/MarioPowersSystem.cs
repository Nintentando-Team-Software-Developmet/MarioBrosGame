using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Systems;

public class MarioPowersSystem : BaseSystem
{
    private bool colitionMushroom;
    private bool colitionFlower;
    private bool colitionStar;
    private bool isStarPowerActive { get; set; }
    private bool isInvulnerable { get; set; }

    private HashSet<ColliderComponent> registeredColliders = new HashSet<ColliderComponent>();
    private readonly Collection<Action> pendingActions = new Collection<Action>();
    private static double starEndTime { get; set; }
    private static double invulnerabilityEndTime { get; set; }
    private SpriteData spriteData { get; set; }
    private ProgressDataManager _progressDataManager;

    public MarioPowersSystem(ProgressDataManager progressDataManager, SpriteData spriteData)
    {
        _progressDataManager = progressDataManager;
        this.spriteData = spriteData;
    }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        var playerEntities = entities.WithComponents(typeof(PlayerComponent));
        var mushroomEntities = entities.WithComponents(typeof(MushroomComponent));
        var flowerEntities = entities.WithComponents(typeof(FlowerComponent));
        var starEntities = entities.WithComponents(typeof(StarComponent));
        var enemyEntities = entities.WithComponents(typeof(EnemyComponent));

        foreach (var player in playerEntities)
        {
            var playerCollider = player.GetComponent<ColliderComponent>();
            var playerAnimation = player.GetComponent<AnimationComponent>();
            var playerComponent = player.GetComponent<PlayerComponent>();

            if (!registeredColliders.Contains(playerCollider))
            {
                RegisterCollisionEvent(playerCollider, mushroomEntities, flowerEntities, starEntities, enemyEntities);
                registeredColliders.Add(playerCollider);
            }

            if (colitionMushroom)
            {
                if (playerComponent.statusMario == StatusMario.SmallMario)
                {
                    _progressDataManager.AddCollectItem(1000);
                    ChangeAnimationColliderPlayer.TransformToBigMario(playerAnimation, playerCollider);
                    playerComponent.statusMario = StatusMario.BigMario;
                    isInvulnerable = true;
                    if (gameTime != null)
                    {
                        invulnerabilityEndTime = gameTime.TotalGameTime.TotalSeconds + 1.0;
                    }
                }
            }
            else if (colitionFlower)
            {
                if (playerComponent.statusMario == StatusMario.BigMario || playerComponent.statusMario == StatusMario.SmallMario)
                {
                    _progressDataManager.AddCollectItem(1000);
                    ChangeAnimationColliderPlayer.TransformToFireMario(playerAnimation, playerCollider);
                    playerComponent.statusMario = StatusMario.FireMario;
                    colitionFlower = false;
                }
            }
            if (colitionStar)
            {
                if (!isStarPowerActive)
                {
                    if (spriteData != null)
                        MediaPlayer.Play(spriteData.content.Load<Song>("SoundEffects/StarMarioPower"));

                    playerComponent.previousStatusMario = playerComponent.statusMario;
                    if (playerComponent.previousStatusMario == StatusMario.BigMario || playerComponent.previousStatusMario == StatusMario.FireMario)
                    {
                        playerComponent.statusMario = StatusMario.StarMarioBig;
                    }
                    else if (playerComponent.previousStatusMario == StatusMario.SmallMario)
                    {
                        playerComponent.statusMario = StatusMario.StarMarioSmall;
                    }
                    if (gameTime != null)
                    {
                        starEndTime = gameTime.TotalGameTime.TotalSeconds + 10.0;
                    }
                    isStarPowerActive = true;
                }
            }
            if (isStarPowerActive && colitionMushroom)
            {
                playerComponent.statusMario = StatusMario.StarMarioBig;
                playerComponent.previousStatusMario = StatusMario.BigMario;
                colitionMushroom = false;
            }
            if (isStarPowerActive && colitionFlower)
            {
                playerComponent.statusMario = StatusMario.StarMarioBig;
                playerComponent.previousStatusMario = StatusMario.FireMario;
                colitionFlower = false;
            }
            if (gameTime != null && isInvulnerable && gameTime.TotalGameTime.TotalSeconds < invulnerabilityEndTime && playerComponent.statusMario == StatusMario.BigMario)
            {
                ChangeAnimationColliderPlayer.CheckEnemyProximity(playerCollider, playerAnimation, enemyEntities, gameTime, invulnerabilityEndTime);
            }
            if (gameTime != null && isStarPowerActive && gameTime.TotalGameTime.TotalSeconds >= starEndTime)
            {
                playerComponent.statusMario = playerComponent.previousStatusMario;
                colitionStar = false;
                isStarPowerActive = false;
                if (_progressDataManager.Time < 101)
                {
                    MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/fast_level"));
                }
                else
                {
                    MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/level1_naruto"));
                }
            }
        }

        ExecutePendingActions();
    }

    private void RegisterCollisionEvent(ColliderComponent collider, IEnumerable<Entity> mushroomEntities, IEnumerable<Entity> flowerEntities, IEnumerable<Entity> starEntities, IEnumerable<Entity> enemyEntities)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var otherEntity = GetEntityFromBody(fixtureB.Body, mushroomEntities);
            var flowerEntity = GetEntityFromBody(fixtureB.Body, flowerEntities);
            var starEntity = GetEntityFromBody(fixtureB.Body, starEntities);
            var enemyEntity = GetEntityFromBody(fixtureB.Body, enemyEntities);

            if (otherEntity != null && otherEntity.HasComponent<MushroomComponent>())
            {
                colitionMushroom = true;
                EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.Mushroom));
                pendingActions.Add(() => RemoveMushroomCollider(otherEntity));
                colitionFlower = false;
            }
            else if (flowerEntity != null && flowerEntity.HasComponent<FlowerComponent>())
            {
                colitionFlower = true;
                EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.Flower));
                pendingActions.Add(() => RemoveMushroomCollider(flowerEntity));
            }
            else if (starEntity != null && starEntity.HasComponent<StarComponent>())
            {
                colitionStar = true;
                EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.Star));
                pendingActions.Add(() => RemoveMushroomCollider(starEntity));
            }
            else if (colitionStar && enemyEntity != null && enemyEntity.HasComponent<EnemyComponent>())
            {
                EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.EnemyDestroyedByStar));
                pendingActions.Add(() => enemyEntity.GetComponent<ColliderComponent>().RemoveCollider());

            }
            else
            {
                colitionMushroom = false;
            }

            return true;
        };
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
