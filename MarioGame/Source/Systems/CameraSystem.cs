using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

using Constants = SuperMarioBros.Utils.Constants;

namespace SuperMarioBros.Source.Systems
{
    public class CameraSystem : BaseSystem
    {
        private float lastXPosition;
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var playerEntities = entities
                .Where(e => e.HasComponent<PlayerComponent>()
                            && e.HasComponent<PositionComponent>()
                            && e.HasComponent<CameraComponent>()
                            && e.HasComponent<ColliderComponent>());

            foreach (var playerEntity in playerEntities)
            {
                var camera = playerEntity.GetComponent<CameraComponent>();
                var playerPosition = playerEntity.GetComponent<PositionComponent>();
                var colliderComponent = playerEntity.GetComponent<ColliderComponent>();

                if (playerPosition != null && camera != null && colliderComponent != null)
                {
                    if (playerPosition.Position.X > camera.LastXPosition &&
                        (colliderComponent.collider.Position.X - 0.006f) > lastXPosition)
                    {
                        camera.Position = new Vector2(
                            MathHelper.Clamp((colliderComponent.collider.Position.X * Constants.pixelPerMeter) - camera.Viewport.Width / 2, 0, camera.WorldWidth - camera.Viewport.Width),
                            MathHelper.Clamp((colliderComponent.collider.Position.Y * Constants.pixelPerMeter) - camera.Viewport.Height / 2, 0, camera.WorldHeight - camera.Viewport.Height)
                        );

                        lastXPosition = colliderComponent.collider.Position.X;
                    }

                    camera.Transform = Matrix.CreateTranslation(new Vector3(-camera.Position, 0));
                }
            }
        }
    }
}
