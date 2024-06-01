using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Source.Systems;


namespace SuperMarioBros.Source.Core
{
    public class World
    {
        private EntityManager _entityManager;
        private SystemManager _systemManager;

        public World(SpriteBatch spriteBatch)
        {
            _entityManager = new EntityManager();
            _systemManager = new SystemManager();

            _systemManager.AddSystem(new AnimationSystem(spriteBatch));
            _systemManager.AddSystem(new InputSystem());
            _systemManager.AddSystem(new MovementSystem());
        }

        public void AddEntity(Entity entity)
        {
            _entityManager.AddEntity(entity);
        }

        public void Update(GameTime gameTime)
        {
            var entities = _entityManager.GetEntities();
            _systemManager.UpdateSystems(gameTime, entities);
        }

        public void Draw(GameTime gameTime)
        {
            var entities = _entityManager.GetEntities();
            _systemManager.DrawSystems(gameTime, entities);
        }
    }
}
