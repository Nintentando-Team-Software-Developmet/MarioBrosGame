using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class CameraSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var playerEntities = entities
                .Where(e => e.HasComponent<PlayerComponent>()
                            && e.HasComponent<ColliderComponent>()
                            && e.HasComponent<CameraComponent>());

            foreach (var playerEntity in playerEntities)
            {
                var camera = playerEntity.GetComponent<CameraComponent>();
                var playerPosition = playerEntity.GetComponent<ColliderComponent>();

                if (playerPosition != null && camera != null)
                {
                    if (playerPosition.collider.Position.X * Constants.pixelPerMeter> camera.LastXPosition)
                    {
                        camera.Position = new Vector2(
                            MathHelper.Clamp(playerPosition.collider.Position.X * Constants.pixelPerMeter - camera.Viewport.Width / 2, 0, camera.WorldWidth - camera.Viewport.Width),
                            MathHelper.Clamp(playerPosition.collider.Position.Y * Constants.pixelPerMeter - camera.Viewport.Height / 2, 0, camera.WorldHeight - camera.Viewport.Height)
                        );

                        camera.LastXPosition = playerPosition.collider.Position.X * Constants.pixelPerMeter;
                    }

                    camera.Transform = Matrix.CreateTranslation(new Vector3(-camera.Position, 0));
                }
            }
        }
    }
}
