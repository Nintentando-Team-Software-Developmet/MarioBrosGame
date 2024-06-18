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
                    AnimationComponent animationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(animationComponent);
                    entity.AddComponent(new EnemyComponent());
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, animationComponent.textureRectangle, BodyType.Dynamic));
                    entity.AddComponent(new MovementComponent(MovementType.LEFT));
                    if (entityData.name == EntitiesName.KOOPALEFT)
                    {
                        entity.AddComponent(new KoopaComponent());
                        entity.AddComponent(new FacingComponent(EntitiesName.KOOPALEFT, EntitiesName.KOOPARIGTH, EntitiesName.KOOPAKNOCKED, EntitiesName.KOOPAREVIVE));
                    }
                    else if (entityData.name == EntitiesName.GOOMBA)
                    {
                        entity.AddComponent(new GoombaComponent());
                    }
                    break;
                case EntityType.PLAYER:
                    entity.AddComponent(new PositionComponent(new Vector2(entityData.position.x, entityData.position.y)));
                    AnimationComponent playerAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name]);
                    entity.AddComponent(playerAnimationComponent);
                    entity.AddComponent(new PlayerComponent());
                    entity.AddComponent(new VelocityComponent(Vector2.Zero));
                    entity.AddComponent(new InputComponent());
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, playerAnimationComponent.textureRectangle, BodyType.Dynamic));
                    entity.AddComponent(new CameraComponent(
                        new Viewport(0, 0, GameConstants.CameraViewportWidth, GameConstants.CameraViewportHeight),
                        GameConstants.CameraWorldWidth,
                        GameConstants.CameraViewportHeight));
                    break;
                case EntityType.WINGAME:
                    entity.AddComponent(new WinGameComponent());
                    entity.AddComponent(new AnimationComponent(Animations.entityTextures[entityData.name]));
                    break;
                case EntityType.QUESTIONBLOCK:
                    AnimationComponent questionBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(questionBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, questionBlockAnimationComponent.textureRectangle, BodyType.Static));
                    entity.AddComponent(new QuestionBlockComponent());
                    break;
                case EntityType.COINBLOCK:
                    AnimationComponent coinBlockAnimationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(coinBlockAnimationComponent);
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, coinBlockAnimationComponent.textureRectangle, BodyType.Static));
                    entity.AddComponent(new CoinBlockComponent());
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
