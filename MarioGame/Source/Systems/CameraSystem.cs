using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;


namespace SuperMarioBros.Source.Systems
{
    public class CameraSystem : BaseSystem
    {
        private ColliderComponent colliderCamera { get; set; }
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            var playerEntities = entities
                .Where(e => e.HasComponent<PlayerComponent>()
                            && e.HasComponent<PositionComponent>()
                            && e.HasComponent<CameraComponent>());

            foreach (var playerEntity in playerEntities)
            {
                var camera = playerEntity.GetComponent<CameraComponent>();
                var playerPosition = playerEntity.GetComponent<PositionComponent>();
                var colliderComponent = playerEntity.GetComponent<ColliderComponent>();

                if (playerPosition != null && camera != null)
                {
                    if (playerPosition.Position.X > camera.LastXPosition)
                    {
                        camera.Position = new Vector2(
                            MathHelper.Clamp(playerPosition.Position.X - camera.Viewport.Width / 2, 0, camera.WorldWidth - camera.Viewport.Width),
                            MathHelper.Clamp(playerPosition.Position.Y - camera.Viewport.Height / 2, 0, camera.WorldHeight - camera.Viewport.Height)
                        );
                        camera.LastXPosition = playerPosition.Position.X;
                        if (colliderCamera != null)
                        {
                            colliderCamera.collider.Position = new AetherVector2(camera.Position.X / Constants.pixelPerMeter, colliderCamera.collider.Position.Y);
                        }
                        else
                        {
                            colliderCamera = new ColliderComponent(colliderComponent.collider.World, camera.Position.X , 100, new Rectangle(100, 100, 5, 1500), BodyType.Static);
                        }
                    }

                    camera.Transform = Matrix.CreateTranslation(new Vector3(-camera.Position, 0));
                }
            }
        }
    }
}
