using Microsoft.Xna.Framework;
using Components;
using Managers;

namespace Systems
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
            foreach (var entityId in _componentManager.GetAllEntitiesWithComponent<PhysicsComponent>())
            {
                var transform = _componentManager.GetComponent<TransformComponent>(entityId);
                var physics = _componentManager.GetComponent<PhysicsComponent>(entityId);

                if (transform != null && physics != null)
                {
                    transform.Position += physics.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }
    }
}
