using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Managers;

namespace SuperMarioBros.Source.Systems
{
    public class PhysicsSystem : BaseSystem
    {
        private ComponentManager _componentManager;

        public PhysicsSystem(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in _componentManager.GetAllEntitiesWithComponent<PhysicsBaseComponent>())
            {
                var transform = _componentManager.GetComponent<TransformBaseComponent>(entityId);
                var physics = _componentManager.GetComponent<PhysicsBaseComponent>(entityId);

                if (transform != null && physics != null)
                {
                    if (gameTime != null)
                        transform.Position += physics.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
