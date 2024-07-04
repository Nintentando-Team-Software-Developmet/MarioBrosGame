using System;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.Scene;

namespace SuperMarioBros.Source.Entities
{
    public static class EntityFactory
    {
        /*
         * <summary>
         * Creates an entity based on the given entity data.
         * </summary>
         * <param name="entityData">The data of the entity to create.</param>
         * <returns>The created entity.</returns>
         */
        public static Entity CreateEntity(EntityData entityData, World physicsWorld)
        {
            if (entityData == null)
                throw new System.ArgumentNullException(nameof(entityData));
            Entity entity = new Entity();
            switch (entityData.type)
            {
                case EntityType.ENEMY:
                    AnimationComponent animationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64, 0.8f);
                    entity.AddComponent(animationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, animationComponent.textureRectangle, BodyType.Dynamic));
                    entity.AddComponent(new MovementComponent(MovementType.LEFT));
                    if (entityData.name == EntitiesName.KOOPA)
                    {
                        entity.AddComponent(new KoopaComponent());
                        entity.GetComponent<AnimationComponent>().velocity = 0.5f;
                    }
                    entity.AddComponent(new EnemyComponent());
                    entity.GetComponent<ColliderComponent>().velocity = 1.1f;
                    break;
                case EntityType.POWERUP:
                    if (entityData.name == EntitiesName.MUSHROOM)
                    {
                        AnimationComponent mushroomAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                        entity.AddComponent(mushroomAnimationComponent);
                        entity.AddComponent(new MushroomComponent());
                        ColliderComponent colliderComponentMushroom = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, mushroomAnimationComponent.textureRectangle, BodyType.Dynamic);
                        entity.AddComponent(colliderComponentMushroom);
                        entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                        colliderComponentMushroom.Enabled(false);
                    }
                    if (entityData.name == EntitiesName.FLOWER)
                    {
                        AnimationComponent fireFlowerAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                        entity.AddComponent(fireFlowerAnimationComponent);
                        entity.AddComponent(new FlowerComponent());
                        ColliderComponent colliderComponentFireFlower = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, fireFlowerAnimationComponent.textureRectangle, BodyType.Dynamic);
                        entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                        entity.AddComponent(colliderComponentFireFlower);
                        colliderComponentFireFlower.Enabled(false);
                    }
                    if (entityData.name == EntitiesName.STAR)
                    {
                        AnimationComponent superStarAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                        entity.AddComponent(superStarAnimationComponent);
                        entity.AddComponent(new StarComponent());
                        ColliderComponent colliderComponent3 = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, superStarAnimationComponent.textureRectangle, BodyType.Dynamic);
                        entity.AddComponent(colliderComponent3);
                        entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                        colliderComponent3.Enabled(false);
                    }
                    break;

                case EntityType.COINANIMATION:
                    AnimationComponent coinAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 45, 45);
                    entity.AddComponent(coinAnimationComponent);
                    entity.AddComponent(new CoinComponent());
                    ColliderComponent colliderComponentCoin = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, coinAnimationComponent.textureRectangle, BodyType.Static);
                    entity.AddComponent(colliderComponentCoin);
                    entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                    colliderComponentCoin.Enabled(false);
                    break;

                case EntityType.PLAYER:
                    PlayerComponent playerComponent = new PlayerComponent();
                    entity.AddComponent(playerComponent);
                    AnimationComponent playerAnimationComponent = new AnimationComponent(Animations.GetAnimation(playerComponent.State), 64,128, 0.09f);
                    entity.AddComponent(playerAnimationComponent);
                    ColliderComponent colliderComponent = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, playerAnimationComponent.textureRectangle, BodyType.Dynamic);
                    colliderComponent.maxSpeed = 3f;
                    colliderComponent.velocity = 3f;
                    colliderComponent.friction = 0.97f;
                    colliderComponent.fixture.CollisionCategories = Categories.Player;
                    colliderComponent.fixture.CollidesWith = Categories.World | Categories.LeftWall | Categories.Player;
                    entity.AddComponent(colliderComponent);
                    entity.AddComponent(new CameraComponent(
                        new Viewport(0, 0, GameConstants.CameraViewportWidth, GameConstants.CameraViewportHeight),
                        GameConstants.CameraWorldWidth,
                        GameConstants.CameraViewportHeight,
                        physicsWorld));
                    entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                    entity.AddComponent(new InputComponent());
                    break;

                case EntityType.WINGAME:
                    entity.AddComponent(new WinPoleSensorComponent());
                    var winAnimation = new AnimationComponent(Animations.entityTextures[entityData.name]);
                    entity.AddComponent(winAnimation);
                    entity.AddComponent(new PositionComponent(new Vector2(entityData.position.x, entityData.position.y)));
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, winAnimation.textureRectangle, BodyType.Static));
                    break;
                case EntityType.WINFLAG:
                    var flagAnimation = new AnimationComponent(Animations.entityTextures[entityData.name]);
                    entity.AddComponent(flagAnimation);
                    entity.AddComponent(new WinFlagComponent());
                    entity.AddComponent(new PositionComponent(new Vector2(entityData.position.x, entityData.position.y)));
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, flagAnimation.textureRectangle, BodyType.Dynamic));
                    entity.GetComponent<ColliderComponent>().collider.IgnoreGravity = true;
                    break;

                case EntityType.QUESTIONBLOCK:
                    AnimationComponent questionBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(questionBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, questionBlockAnimationComponent.textureRectangle, BodyType.Static));
                    QuestionBlockComponent questionBlockComponent = new QuestionBlockComponent(entityData.TypeContent, entityData.Quantity);
                    entity.AddComponent(questionBlockComponent);
                    entity.AddComponent(new BlockComponent(false, BlockType.QuestionMark));
                    break;

                case EntityType.COINBLOCK:
                    AnimationComponent coinBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(coinBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, coinBlockAnimationComponent.textureRectangle, BodyType.Static));
                    CoinBlockComponent coinBlockComponent = new CoinBlockComponent(entityData.TypeContent, entityData.Quantity);
                    entity.AddComponent(coinBlockComponent);
                    if (entityData.TypeContent == EntitiesName.NORMAL)
                        entity.AddComponent(new BlockComponent(true, BlockType.Normal));
                    else
                    {
                        entity.AddComponent(new BlockComponent(false, BlockType.Coin));
                    }
                    break;

                case EntityType.FIREBALL:
                    AnimationComponent fireBlockAnimationComponent = new AnimationComponent(Animations.fire, 34, 34);
                    fireBlockAnimationComponent.velocity = fireBlockAnimationComponent.velocity / 3;
                    entity.AddComponent(fireBlockAnimationComponent);
                    ColliderComponent colliderFireComponent = new ColliderComponent(physicsWorld, -100, 750, fireBlockAnimationComponent.textureRectangle, BodyType.Static);
                    entity.AddComponent(colliderFireComponent);
                    FireBoolComponent fireBlockComponent = new FireBoolComponent();
                    entity.AddComponent(fireBlockComponent);
                    break;

                case EntityType.BLOCK:
                    AnimationComponent blockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(blockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, blockAnimationComponent.textureRectangle, BodyType.Static));
                    break;

                case EntityType.DUCT:
                    AnimationComponent ductAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 128, 128);
                    entity.AddComponent(ductAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, ductAnimationComponent.textureRectangle, BodyType.Static));
                    break;

                case EntityType.DUCTEXTENSION:
                    AnimationComponent ductExtensionAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 128, 64);
                    entity.AddComponent(ductExtensionAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, ductExtensionAnimationComponent.textureRectangle, BodyType.Static));
                    entity.AddComponent(new DuctExtensionComponent());

                    break;
                case EntityType.DUCTCROSS:
                    AnimationComponent ductCrossAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 131, 128);
                    entity.AddComponent(ductCrossAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, ductCrossAnimationComponent.textureRectangle, BodyType.Static));
                    break;
                case EntityType.SECRETLEVELDUCTENTRANCE:
                    AnimationComponent ductCrossSquareAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 128, 128);
                    entity.AddComponent(ductCrossSquareAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, ductCrossSquareAnimationComponent.textureRectangle, BodyType.Static));
                    break;

                default:
                    break;
            }
            return entity;
        }


    }
}
