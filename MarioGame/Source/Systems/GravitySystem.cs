using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public class GravitySystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities == null) return;
            float deltaTime = (float)gameTime?.ElapsedGameTime.TotalSeconds;

            foreach (var entity in entities)
            {
                if (entity.HasComponent<VelocityComponent>() && entity.HasComponent<PositionComponent>())
                {
                    var velocityComponent = entity.GetComponent<VelocityComponent>();
                    var positionComponent = entity.GetComponent<PositionComponent>();

                    // Apply gravity if the entity has a GravityComponent
                    if (entity.HasComponent<GravityComponent>())
                    {
                        var gravityComponent = entity.GetComponent<GravityComponent>();
                        
                        // Update the velocity due to gravity
                        var newVelocity = velocityComponent.Velocity;
                        newVelocity.Y += gravityComponent.gravity * deltaTime;
                        velocityComponent.Velocity = newVelocity;
                    }

                    // Update position based on velocity
                    var newPosition = positionComponent.Position;
                    newPosition += velocityComponent.Velocity * deltaTime;
                    positionComponent.Position = newPosition;

                    // Update the components
                    entity.AddComponent(positionComponent);
                    entity.AddComponent(velocityComponent);
                }
            }
        }
    }
}
