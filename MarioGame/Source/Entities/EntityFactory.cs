using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Dynamics;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;


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
                    entity.AddComponent(new EnemyComponent());
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, animationComponent.textureRectangle, BodyType.Dynamic));
                    entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                    break;

                case EntityType.PLAYER:
                    AnimationComponent playerAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64, 0.09f);
                    entity.AddComponent(playerAnimationComponent);
                    entity.AddComponent(new PlayerComponent());
                    ColliderComponent colliderComponent = new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, playerAnimationComponent.textureRectangle, BodyType.Dynamic);
                    colliderComponent.maxSpeed = 3f;
                    colliderComponent.acceleration = 3f;
                    colliderComponent.friction = 0.97f;
                    entity.AddComponent(colliderComponent);
                    entity.AddComponent(new CameraComponent(
                        new Viewport(0, 0, Constants.CameraViewportWidth, Constants.CameraViewportHeight),
                        Constants.CameraWorldWidth,
                        Constants.CameraViewportHeight));
                    entity.AddComponent(new MovementComponent(MovementType.RIGHT));
                    break;

                case EntityType.WINGAME:
                    entity.AddComponent(new WinGameComponent());
                    entity.AddComponent(new AnimationComponent(Animations.entityTextures[entityData.name]));
                    entity.AddComponent(new PositionComponent(new Vector2(entityData.position.x, entityData.position.y)));

                    break;

                case EntityType.QUESTIONBLOCK:
                    AnimationComponent questionBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(questionBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, questionBlockAnimationComponent.textureRectangle, BodyType.Static));
                    QuestionBlockComponent questionBlockComponent = new QuestionBlockComponent(entityData.TypeContent,entityData.Quantity);
                    entity.AddComponent(questionBlockComponent);
                    break;

                case EntityType.COINBLOCK:
                    AnimationComponent coinBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(coinBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, coinBlockAnimationComponent.textureRectangle, BodyType.Static));
                    CoinBlockComponent coinBlockComponent = new CoinBlockComponent(entityData.TypeContent,entityData.Quantity);
                    entity.AddComponent(coinBlockComponent);
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
                    break;

                default:
                    break;
            }
            return entity;
        }


    }
}