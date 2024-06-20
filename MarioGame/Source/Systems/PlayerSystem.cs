using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem{
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities){
            IEnumerable<Entity> players = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent));
            foreach(var player in players){
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var playerPosition = colliderComponent.Position;
                if(playerPosition.Y > Constants.CameraViewportHeight + 300){
                    playerComponent.IsAlive = false;
                }
            }
            
        }
    }
}