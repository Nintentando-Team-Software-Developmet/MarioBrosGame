using Microsoft.Xna.Framework;
using System.Collections.Generic;
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

        public void UpdateSystems(GameTime gameTime)
        {
            foreach (var system in _systems)
            {
                system.Update(gameTime);
            }
        }

        public void RenderSystems(GameTime gameTime)
        {
            foreach (var system in _systems)
            {
                if (system is IRenderableSystem renderableSystem)
                {
                    renderableSystem.Render(gameTime);
                }
            }
        }
    }
}
