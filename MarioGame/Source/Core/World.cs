using Managers;
using Events;
using Scenes;
using Levels;

using Microsoft.Xna.Framework;

namespace Core
{
    public class World
    {
        private EntityManager _entityManager;
        private ComponentManager _componentManager;
        private SystemManager _systemManager;
        private EventDispatcher _eventDispatcher;
        private SceneManager _sceneManager;
        private LevelLoader _levelLoader;

        public void SetManagers(EntityManager entityManager, ComponentManager componentManager, SystemManager systemManager, EventDispatcher eventDispatcher, SceneManager sceneManager, LevelLoader levelLoader)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _systemManager = systemManager;
            _eventDispatcher = eventDispatcher;
            _sceneManager = sceneManager;
            _levelLoader = levelLoader;
        }

        public void Update(GameTime gameTime)
        {
            _systemManager.UpdateSystems(gameTime);
        }

        public void Render(GameTime gameTime)
        {
            _systemManager.RenderSystems(gameTime);
        }

        public void LoadLevel(LevelData levelData)
        {
            _levelLoader.LoadLevel(levelData);
        }
    }
}
