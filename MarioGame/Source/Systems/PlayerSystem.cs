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

            var players = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            foreach (var entity in players)
            {
                var player = entity.GetComponent<PlayerComponent>();
                var collider = entity.GetComponent<ColliderComponent>();

                if (collider.collider.Position.Y * 100 > FallThreshold && player.IsAlive)
                {
                    Console.WriteLine(new StringBuilder().Append("Player fell off the map!").ToString());
                    player.IsAlive = false;
                }
            }
        }
    }
}
