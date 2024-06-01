using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    public class MovementSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null)
                foreach (var entity in entities)
                {
                    var position = entity.GetComponent<PositionComponent>();
                    var velocity = entity.GetComponent<VelocityComponent>();

                    if (position != null && velocity != null)
                    {
                        if (gameTime != null)
                            position.Position +=
                                velocity.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds *
                                100;
                    }
                }
        }
    }
}
