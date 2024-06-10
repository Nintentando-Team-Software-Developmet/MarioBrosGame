using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

namespace SuperMarioBros.Source.Systems
{
    public class GravitySystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (gameTime == null || entities == null)
                return;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var gravityEntities = entities.WithComponents<VelocityComponent, PositionComponent, GravityComponent>();

            foreach (var entity in gravityEntities)
            {
                var velocity = entity.GetComponent<VelocityComponent>();
                var position = entity.GetComponent<PositionComponent>();
                var gravity = entity.GetComponent<GravityComponent>();

                velocity.Velocity += new Vector2(0, gravity.gravity * deltaTime);
                position.Position += velocity.Velocity * deltaTime;
            }
        }
    }
}
