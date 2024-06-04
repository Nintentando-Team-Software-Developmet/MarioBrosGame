using MarioGame;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Core;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Levels;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Source.Scenes;
using SuperMarioBros.Source.Systems;

namespace Services
{
    public class WorldInitializer
    {
        private readonly GraphicsDevice _graphicsDevice;

        public WorldInitializer(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            var entityManager = new EntityManager();
            var componentManager = new ComponentManager();
            var systemManager = new SystemManager();
            var eventDispatcher = new EventDispatcher();
            //var sceneManager = new SceneManager();
            var levelLoader = new LevelLoader(entityManager, componentManager);
            var renderingSystem = new RenderingSystem(componentManager, new SpriteBatch(_graphicsDevice));

            //if (world != null)
            //    world.SetManagers(entityManager, componentManager, systemManager, eventDispatcher, sceneManager,
            //        levelLoader);

            systemManager.AddSystem(new InputSystem(eventDispatcher));
            systemManager.AddSystem(new PhysicsSystem(componentManager));
            systemManager.AddSystem(renderingSystem);
            systemManager.AddSystem(new CameraSystem(componentManager));
        }
    }
}
