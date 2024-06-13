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
            entity.AddComponent(new PositionComponent(new Vector2(entityData.position.x, entityData.position.y)));
            switch (entityData.type)
            {
                case EntityType.ENEMY:
                    AnimationComponent animationComponent = new AnimationComponent(Animations.entityTextures[entityData.name], 64, 64);
                    entity.AddComponent(animationComponent);
                    entity.AddComponent(new EnemyComponent());
                    entity.AddComponent(new VelocityComponent(Vector2.Zero));
                    entity.AddComponent(new ColliderComponent(physicsWorld, entityData.position.x, entityData.position.y, animationComponent.textureRectangle, BodyType.Dynamic));
                    break;
                case EntityType.PLAYER:
                    entity.AddComponent(new AnimationComponent(Animations.entityTextures[entityData.name]));
                    entity.AddComponent(new PlayerComponent());
                    entity.AddComponent(new VelocityComponent(Vector2.Zero));
                    entity.AddComponent(new InputComponent());
                    entity.AddComponent(new CameraComponent(
                        new Viewport(0, 0, Constants.CameraViewportWidth,
                            Constants.CameraViewportHeight),
                        Constants.CameraWorldWidth,
                        Constants.CameraViewportHeight));
                    break;
                case EntityType.WINGAME:
                    entity.AddComponent(new WinGameComponent());
                    entity.AddComponent(new AnimationComponent(Animations.entityTextures[entityData.name]));
                    break;
                //TODO: Implement other entity types
                default:
                    break;
            }

            return entity;
        }
    }
}
