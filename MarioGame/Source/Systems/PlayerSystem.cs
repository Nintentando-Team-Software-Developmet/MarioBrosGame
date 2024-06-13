using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem
    {
        private const float FallThreshold = 720;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities == null) return;

            var players = entities.WithComponents(typeof(PlayerComponent), typeof(PositionComponent));
            foreach (var entity in players)
            {
                var position = entity.GetComponent<PositionComponent>();
                var player = entity.GetComponent<PlayerComponent>();

                if (position.Position.Y > FallThreshold && player.IsAlive)
                {
                    Console.WriteLine(new StringBuilder().Append("Player fell off the map!").ToString());
                    player.IsAlive = false;
                }
            }
        }
    }
}
