using System;
using System.Collections.Generic;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using nkast.Aether.Physics2D.Dynamics.Contacts;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

using Vector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems;

public class BlockSystem : BaseSystem
{
    private HashSet<Entity> registeredEntities = new HashSet<Entity>();
    private static HashSet<Entity> entitiesInContact = new HashSet<Entity>();
    private static Dictionary<Entity, bool> entitiesProcessed = new Dictionary<Entity, bool>();
    private static Dictionary<Entity, float> entityTimers = new Dictionary<Entity, float>();
    private static Dictionary<Entity, string> entityStates = new Dictionary<Entity, string>();
    private static bool statusMario { get; set; }

    static int number { get; set; }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        var questionBlockEntities = entities.WithComponents(typeof(QuestionBlockComponent), typeof(ColliderComponent));
        var coinBlockEntities = entities.WithComponents(typeof(CoinBlockComponent), typeof(ColliderComponent));
        var mushroomEntities = entities.WithComponents(typeof(MushroomComponent), typeof(ColliderComponent));
        var startEntities = entities.WithComponents(typeof(StarComponent), typeof(ColliderComponent));
        var playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
        var flowerEntities = entities.WithComponents(typeof(FlowerComponent), typeof(ColliderComponent));
        var coinEntities = entities.WithComponents(typeof(CoinComponent), typeof(ColliderComponent));


        UpdateBlocks(questionBlockEntities, gameTime, mushroomEntities,startEntities,playerEntities,flowerEntities,coinEntities);
        UpdateBlocks(coinBlockEntities, gameTime, mushroomEntities,startEntities,playerEntities,flowerEntities,coinEntities);

    }


    private void UpdateBlocks(IEnumerable<Entity> entities, GameTime gameTime, IEnumerable<Entity> entities2, IEnumerable<Entity> entities3,IEnumerable<Entity> entitiesPlayer,
        IEnumerable<Entity> entitiesflower, IEnumerable<Entity> entitiesCoin)
    {
        foreach (var entity in entities)
        {
            var collider = entity.GetComponent<ColliderComponent>();
            var animation = entity.GetComponent<AnimationComponent>();

            if (collider.collider.ContactList != null)
            {
                if (!registeredEntities.Contains(entity))
                {
                    RegisterBlock(entity, collider,entitiesPlayer);
                }

                HandleBlockMovement(gameTime, collider, entity, animation, entities2,entities3,entitiesflower,entitiesCoin);

            }
        }
    }

    private void RegisterBlock(Entity entity, ColliderComponent collider,IEnumerable<Entity> entities)
    {
        registeredEntities.Add(entity);
        RegisterCollisionEvent(collider, entity,entities);
        entityTimers[entity] = 0;
        entityStates[entity] = "idle";
    }

    private static void HandleBlockMovement(GameTime gameTime, ColliderComponent collider, Entity entity,
        AnimationComponent animationComponent, IEnumerable<Entity> entities, IEnumerable<Entity> entities3,
        IEnumerable<Entity> flowerEntities,IEnumerable<Entity> coinEntities)
    {
        float movementSpeed = 10f / GameConstants.pixelPerMeter;
        float timeToMove = 0.02f;
        float waitTime = 0.02f;
        Entity entity2 = new Entity();
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
            case "movingUp2":
                if (entityTimers[entity] >= timeToMove)
                {
                    MoveBlock(collider, entity, -movementSpeed);
                    entityStates[entity] = "waiting";
                    entityTimers[entity] = 0;
                    if (entity.HasComponent<CoinBlockComponent>())
                    {
                        if (coinBlock.TypeContent == EntitiesName.STAR)
                        {
                            ActivateMushroomsStart(entities3, collider.collider.Position);

                        }
                        Console.WriteLine(coinBlock.TypeContent);
                    }

                    if (entity.HasComponent<QuestionBlockComponent>())
                    {
                        AnimationComponent animationComponent2 =
                            new AnimationComponent(Animations.entityTextures[EntitiesName.BLOCKERBLOCKBROWN], 64, 64);
                        animationComponent.animations = animationComponent2.animations;

                        number += 1;
                        if (questionBlock.TypeContent == EntitiesName.POWERUP)
                        {
                            if (statusMario)
                            {
                                ActivateMushrooms(entities, collider.collider.Position);
                            }
                            else
                            {
                                ActivateFlower(flowerEntities, collider.collider.Position);
                            }


                        }
                        else if (questionBlock.TypeContent == EntitiesName.COIN)
                        {

                            Console.WriteLine(questionBlock.TypeContent);
                            ActivateCoin(coinEntities,collider.collider.Position);
                        }
                    }
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
                    MoveBlock(collider, entity, movementSpeed);
                    entityStates[entity] = "idle";
                    entityTimers[entity] = 0;
                    entitiesInContact.Remove(entity);
                }

                break;
        }
    }

    private static void ActivateMushrooms(IEnumerable<Entity> mushroomEntities, AetherVector2 questionBlockPosition)
    {
        foreach (var entity in mushroomEntities)
        {
            var mushroom = entity.GetComponent<MushroomComponent>();
            var collider = entity.GetComponent<ColliderComponent>();

            if (collider.collider.Position.X == questionBlockPosition.X)
            {
                mushroom.statusBlock = true;
            }
        }
    }
    private static void ActivateFlower(IEnumerable<Entity> mushroomEntities, AetherVector2 questionBlockPosition)
    {
        foreach (var entity in mushroomEntities)
        {
            var mushroom = entity.GetComponent<FlowerComponent>();
            var collider = entity.GetComponent<ColliderComponent>();

            if (collider.collider.Position.X == questionBlockPosition.X)
            {
                mushroom.statusBlock = true;
            }
        }
    }
    private static void ActivateCoin(IEnumerable<Entity> mushroomEntities, AetherVector2 questionBlockPosition)
    {
        foreach (var entity in mushroomEntities)
        {
            var mushroom = entity.GetComponent<CoinComponent>();
            var collider = entity.GetComponent<ColliderComponent>();

            if (collider.collider.Position.X == questionBlockPosition.X)
            {
                mushroom.statusBlock = true;
                Console.WriteLine(mushroom.statusBlock);
            }


        }
    }
    private static void ActivateMushroomsStart(IEnumerable<Entity> mushroomEntities, AetherVector2 questionBlockPosition)
    {
        foreach (var entity in mushroomEntities)
        {
            var mushroom = entity.GetComponent<StarComponent>();
            var collider = entity.GetComponent<ColliderComponent>();

            if (collider.collider.Position.X == questionBlockPosition.X)
            {
                mushroom.statusBlock = true;
            }
        }
    }


    private static void MoveBlock(ColliderComponent collider, Entity entity, float speed)
    {
        var currentPosition = collider.collider.Position;
        currentPosition.Y += speed;
        collider.collider.Position = currentPosition;
    }

    private static void RegisterCollisionEvent(ColliderComponent collider, Entity entity,IEnumerable<Entity> entities)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            Body colliderBody = collider.collider;
            Fixture colliderFixture = colliderBody.FixtureList[0];

            float blockBottomY = colliderBody.Position.Y * GameConstants.pixelPerMeter + colliderFixture.Shape.Radius;

            if (IsCollisionAtBase(contact, blockBottomY,entities))
            {
                if (entityStates[entity] == "idle")
                {
                    entityStates[entity] = "movingUp2";
                }
            }
            else
            {

                ApplyRepulsionForce(contact);
            }
            return true;
        };
    }

    private static bool IsCollisionAtBase(Contact contact, float blockBottomY, IEnumerable<Entity> entities)
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

        bool isPlayerInvolved = false;

        foreach (var playerEntity in entities)
        {
            var playerCollider = playerEntity.GetComponent<ColliderComponent>().collider;
            if (playerCollider == bodyA || playerCollider == bodyB)
            {
                isPlayerInvolved = true;
                break;
            }
        }

        return isContactAtBase && isPlayerInvolved;
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
