using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    /// <summary>
    /// Represents a system that handles the movement of entities.
    /// </summary>
    public class MovementSystem : BaseSystem
    {
        /// <summary>
        /// Updates the position of each entity based on its velocity.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="entities">The list of entities to update.</param>
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    var position = entity.GetComponent<PositionComponent>();
                    var velocity = entity.GetComponent<VelocityComponent>();

                    // If the entity has both a position and velocity component, update its position
                    if (position != null && velocity != null && gameTime != null)
                    {
                        position.Position +=
                            velocity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds *
                            100;
                    }
                }
            }
        }
    }
}
