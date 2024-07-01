using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

using Microsoft.Xna.Framework;
using AetherVector = nkast.Aether.Physics2D.Common.Vector2;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils;
using System;

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
                    if (playerPosition.collider.Position.X * GameConstants.pixelPerMeter > camera.LastXPosition)
                    {
                        camera.Position = new Vector2(
                            MathHelper.Clamp(playerPosition.collider.Position.X * GameConstants.pixelPerMeter - camera.Viewport.Width / 2, 0, camera.WorldWidth - camera.Viewport.Width),
                            MathHelper.Clamp(playerPosition.collider.Position.Y * GameConstants.pixelPerMeter - camera.Viewport.Height / 2, 0, camera.WorldHeight - camera.Viewport.Height)
                        );

                        camera.LastXPosition = playerPosition.collider.Position.X * GameConstants.pixelPerMeter;
                    }
                    camera.LeftWall.Position = new AetherVector(camera.Position.X / GameConstants.pixelPerMeter, camera.Position.Y / GameConstants.pixelPerMeter);
                    Console.WriteLine(camera.Position.X + " " + camera.Position.Y);
                    camera.Transform = Matrix.CreateTranslation(new Vector3(-camera.Position, 0));
                }
            }
        }
    }
}
