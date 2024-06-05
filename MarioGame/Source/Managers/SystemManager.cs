using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Systems;

namespace SuperMarioBros.Source.Managers
{
    public class SystemManager
    {
        private List<BaseSystem> _systems = new();

        public void AddSystem(BaseSystem system)
        {
            _systems.Add(system);
        }

        public void UpdateSystems(GameTime gameTime, IEnumerable<Entity> entities)
        {
            foreach (var system in _systems)
            {
                system.Update(gameTime, entities);
            }
        }

        public void DrawSystems(GameTime gameTime, IEnumerable<Entity> entities)
        {
            foreach (var system in _systems)
            {
                if (system is IRenderableSystem renderableSystem)
                {
                    renderableSystem.Draw(gameTime, entities);
                }
            }
        }
    }
}
