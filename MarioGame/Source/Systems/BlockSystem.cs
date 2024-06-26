using System;
using System.Collections.Generic;

using MarioGame;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Source.Systems;

public class BlockSystem : BaseSystem
{
    private ProgressDataManager _progressDataManager;
    private HashSet<Entity> registeredEntities = new HashSet<Entity>();
    private static HashSet<Entity> entitiesInContact = new HashSet<Entity>();
    private static Dictionary<Entity, bool> entitiesProcessed = new Dictionary<Entity, bool>();
    private static Dictionary<Entity, float> entityTimers = new Dictionary<Entity, float>();
    private static Dictionary<Entity, string> entityStates = new Dictionary<Entity, string>();
    private List<Body> _bodiesToRemove = new List<Body>();
    private static bool statusMario { get; set; }

    public BlockSystem(ProgressDataManager progressDataManager)
    {
        _progressDataManager = progressDataManager;
    }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        var questionBlockEntities = entities.WithComponents(typeof(QuestionBlockComponent), typeof(ColliderComponent));
        var coinBlockEntities = entities.WithComponents(typeof(CoinBlockComponent), typeof(ColliderComponent));
        var mushroomEntities = entities.WithComponents(typeof(MushroomComponent), typeof(ColliderComponent));
        var startEntities = entities.WithComponents(typeof(StarComponent), typeof(ColliderComponent));
        var playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
        var flowerEntities = entities.WithComponents(typeof(FlowerComponent), typeof(ColliderComponent));
        var coinEntities = entities.WithComponents(typeof(CoinComponent), typeof(ColliderComponent));

        UpdateBlocks(questionBlockEntities, gameTime, mushroomEntities, startEntities, playerEntities, flowerEntities, coinEntities);
        UpdateBlocks(coinBlockEntities, gameTime, mushroomEntities, startEntities, playerEntities, flowerEntities, coinEntities);

        if (_bodiesToRemove.Count == 0)
        {
            return;
        }
        Console.WriteLine("Destroying bodies " + _bodiesToRemove.Count);
        _bodiesToRemove.ForEach(body => body.World.Remove(body));
        _bodiesToRemove.Clear();

    }
    private void UpdateBlocks(IEnumerable<Entity> entities, GameTime gameTime, IEnumerable<Entity> entitiesMushroom, IEnumerable<Entity> entitiesStar, IEnumerable<Entity> entitiesPlayer,
        IEnumerable<Entity> entitiesflower, IEnumerable<Entity> entitiesCoin)
    {
        foreach (var entity in entities)
        {
            var collider = entity.GetComponent<ColliderComponent>();
            if (collider.collider.ContactList != null)
            {
                if (!registeredEntities.Contains(entity))
                {
                    RegisterBlock(entity, collider, entitiesPlayer);
                }

                HandleBlockMovement(gameTime, collider, entity, entitiesMushroom, entitiesStar, entitiesflower, entitiesCoin);

            }
        }
    }

    private void RegisterBlock(Entity entity, ColliderComponent collider, IEnumerable<Entity> entities)
    {
        registeredEntities.Add(entity);
        RegisterCollisionEvent(collider, entity, entities);
        entityTimers[entity] = 0;
        entityStates[entity] = "idle";
    }

    private void HandleBlockMovement(GameTime gameTime, ColliderComponent collider, Entity entity, IEnumerable<Entity> mushroomEntities,
        IEnumerable<Entity> starEntities, IEnumerable<Entity> flowerEntities, IEnumerable<Entity> coinEntities)
    {
        float movementSpeed = 10f / GameConstants.pixelPerMeter;
        float timeToMove = 0.02f;
        float waitTime = 0.02f;
        if (!entitiesInContact.Contains(entity))
        {
            entitiesInContact.Add(entity);
            entitiesProcessed[entity] = false;
        }

        entityTimers[entity] += (float)gameTime.ElapsedGameTime.TotalSeconds;
        var questionBlock = entity.GetComponent<QuestionBlockComponent>();
        var coinBlock = entity.GetComponent<CoinBlockComponent>();

        switch (entityStates[entity])
        {
            case "movingUp":
                if (entityTimers[entity] >= timeToMove)
                {
                    MoveBlock(collider, -movementSpeed);
                    entityStates[entity] = "waiting";
                    entityTimers[entity] = 0;
                    HandleBlockContent(entity, collider, questionBlock, coinBlock, mushroomEntities, starEntities, flowerEntities, coinEntities);

                }

                break;
            case "waiting":
                if (entityTimers[entity] >= waitTime)
                {
                    entityStates[entity] = "movingDown";
                    entityTimers[entity] = 0;
                }

                break;
            case "movingDown":
                if (entityTimers[entity] >= timeToMove)
                {
                    MoveBlock(collider, movementSpeed);
                    entityStates[entity] = "idle";
                    entityTimers[entity] = 0;
                    entitiesInContact.Remove(entity);
                }

                break;
        }
    }

    private void HandleBlockContent(Entity entity, ColliderComponent collider,
        QuestionBlockComponent questionBlock, CoinBlockComponent coinBlock,
        IEnumerable<Entity> mushroomEntities, IEnumerable<Entity> starEntities, IEnumerable<Entity> flowerEntities,
        IEnumerable<Entity> coinEntities)
    {
        if (coinBlock != null)
        {
            HandleCoinBlockContent(coinBlock, collider, entity, starEntities, coinEntities);
        }

        if (questionBlock != null)
        {
            HandleQuestionBlockContent(questionBlock, collider, entity, mushroomEntities, flowerEntities, coinEntities);
        }
    }

    private void HandleCoinBlockContent(CoinBlockComponent coinBlock, ColliderComponent collider, Entity entity,
        IEnumerable<Entity> starEntities, IEnumerable<Entity> coinEntities)
    {
        if (coinBlock.TypeContent == EntitiesName.STAR)
        {
            ActivateEntities<StarComponent>(starEntities, collider.collider.Position);
            var animationComponent = entity.GetComponent<AnimationComponent>();
            animationComponent.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.BLOCKERBLOCKBROWN], 64, 64).animations;
            coinBlock.HasMoved = true;
        }
        else if (coinBlock.TypeContent == EntitiesName.COIN && coinBlock.Quantity > 0)
        {
            ActivateEntities<CoinComponent>(coinEntities, collider.collider.Position);
            coinBlock.Quantity--;
            _progressDataManager.Coins++;
            if (coinBlock.Quantity == 0)
            {
                coinBlock.statusBlock = false;
                var animationComponent = entity.GetComponent<AnimationComponent>();
                animationComponent.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.BLOCKERBLOCKBROWN], 64, 64).animations;
                coinBlock.HasMoved = true;
            }
        }

    }

    private void HandleQuestionBlockContent(QuestionBlockComponent questionBlock, ColliderComponent collider, Entity entity,
        IEnumerable<Entity> mushroomEntities, IEnumerable<Entity> flowerEntities, IEnumerable<Entity> coinEntities)
    {
        var animationComponent = entity.GetComponent<AnimationComponent>();
        animationComponent.animations = new AnimationComponent(Animations.entityTextures[EntitiesName.BLOCKERBLOCKBROWN], 64, 64).animations;

        if (questionBlock.TypeContent == EntitiesName.POWERUP)
        {
            if (!statusMario)
            {
                ActivateEntities<MushroomComponent>(mushroomEntities, collider.collider.Position);
            }
            else
            {
                ActivateEntities<FlowerComponent>(flowerEntities, collider.collider.Position);
            }
        }
        else if (questionBlock.TypeContent == EntitiesName.COIN)
        {
            ActivateEntities<CoinComponent>(coinEntities, collider.collider.Position);
            _progressDataManager.Coins++;
        }
        questionBlock.HasMoved = true;
    }


    private static void ActivateEntities<T>(IEnumerable<Entity> entities, AetherVector2 position) where T : BaseComponent
    {
        foreach (var entity in entities)
        {
            var component = entity.GetComponent<T>();
            var collider = entity.GetComponent<ColliderComponent>();

            if (collider.collider.Position.X == position.X)
            {
                dynamic dynComponent = component;
                dynComponent.statusBlock = true;
            }
        }
    }

    private static void MoveBlock(ColliderComponent collider, float speed)
    {
        var currentPosition = collider.collider.Position;
        currentPosition.Y += speed;
        collider.collider.Position = currentPosition;
    }

    private static Entity GetPlayerEntity(IEnumerable<Entity> entities)
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


    private void RegisterCollisionEvent(ColliderComponent collider, Entity blockEntity, IEnumerable<Entity> entities)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            Body colliderBody = collider.collider;
            Fixture colliderFixture = colliderBody.FixtureList[0];

            float blockBottomY = colliderBody.Position.Y * GameConstants.pixelPerMeter + colliderFixture.Shape.Radius;

            var playerEntity = GetPlayerEntity(entities);
            if (playerEntity != null)
            {
                var playerState = playerEntity.GetComponent<PlayerStateComponent>();

                if (IsCollisionAtBase(contact, blockBottomY, playerEntity))
                {
                    var questionBlock = blockEntity.GetComponent<QuestionBlockComponent>();
                    var coinBlock = blockEntity.GetComponent<CoinBlockComponent>();
                    var destructible = blockEntity.GetComponent<BlockComponent>().IsDestrucible;

                    Console.WriteLine("Player state: " + playerState.IsBig);
                    Console.WriteLine("Block is destructible: " + destructible);
                    Console.WriteLine("Block type: " + blockEntity.GetComponent<BlockComponent>().BlockType);
                    // Destroy only destructible blocks when player is big
                    if (playerState.IsBig && destructible)
                    {
                        Console.WriteLine("The player is big " + playerState.IsBig + " and the block is destructible " + destructible);
                        blockEntity.ClearComponents();
                        _bodiesToRemove.Add(colliderBody);
                        registeredEntities.Remove(blockEntity);
                    }

                    // Move only question and coin blocks (if not already moved)
                    if (entityStates[blockEntity] == "idle" &&
                        ((questionBlock != null && !questionBlock.HasMoved) || (coinBlock != null && !coinBlock.HasMoved)))
                    {
                        entityStates[blockEntity] = "movingUp";
                    }
                }
                else
                {
                    ApplyRepulsionForce(contact);
                }
            }
            return true;
        };
    }


    private static bool IsCollisionAtBase(Contact contact, float blockBottomY, Entity playerEntity)
    {
        Fixture fixtureA = contact.FixtureA;
        Body bodyA = fixtureA.Body;

        Fixture fixtureB = contact.FixtureB;
        Body bodyB = fixtureB.Body;

        float bodyAPositionY = bodyA.Position.Y * GameConstants.pixelPerMeter + fixtureA.Shape.Radius;
        float bodyBPositionY = bodyB.Position.Y * GameConstants.pixelPerMeter + fixtureB.Shape.Radius;

        bool isBodyAAtBase = bodyAPositionY >= blockBottomY;
        bool isBodyBAtBase = bodyBPositionY >= blockBottomY;
        bool isContactAtBase = isBodyAAtBase && isBodyBAtBase;

        var playerCollider = playerEntity.GetComponent<ColliderComponent>().collider;
        return isContactAtBase && (playerCollider == bodyA || playerCollider == bodyB);
    }

    private static void ApplyRepulsionForce(Contact contact)
    {
        Body bodyA = contact.FixtureA.Body;
        Body bodyB = contact.FixtureB.Body;
        AetherVector2 normal = contact.Manifold.LocalNormal;

        float repulsionForce = 0.1f;

        bodyA.ApplyLinearImpulse(-repulsionForce * normal);
        bodyB.ApplyLinearImpulse(repulsionForce * normal);
    }
}
