using Microsoft.Xna.Framework;
using Components;
using Managers;

namespace Systems
{
    public class CameraSystem : BaseSystem
    {
        private ComponentManager _componentManager;

        public CameraSystem(ComponentManager componentManager)
        {
            _componentManager = componentManager;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in _componentManager.GetAllEntitiesWithComponent<CameraComponent>())
            {
                var camera = _componentManager.GetComponent<CameraComponent>(entityId);
                var targetTransform = camera?.Target.GetComponent<TransformComponent>();

                if (camera != null && targetTransform != null)
                {
                    camera.Position = targetTransform.Position - new Vector2(400, 300); // Centering camera
                }
            }
        }
    }
}
