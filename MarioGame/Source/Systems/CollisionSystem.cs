using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems
{
    /*
    * The CollisionSystem class is responsible for handling collision detection
    * and response for entities in the game.
    */
    public class CollisionSystem : BaseSystem
    {
        private static double TOLERANCE { get; set; } = 1.01f;

        /*
        * Updates the collision system, checking for and responding to collisions for each entity.
        *
        * @param gameTime Provides a snapshot of timing values.
        * @param entities The collection of entities to check for collisions.
        */
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities == null)
                return;

            foreach (var entity in entities)
            {
                ColitionWinGame(entities);
            }
        }

        private static void ColitionWinGame(IEnumerable<Entity> entities)
        {
            if (entities == null)
                return;

            var playerEntities = entities.Where(e =>
                e.HasComponent<AnimationComponent>() &&
                e.HasComponent<PositionComponent>() &&
                e.HasComponent<PlayerComponent>()
            );

            var winFlagEntities = entities.Where(e =>
                e.HasComponent<AnimationComponent>() &&
                e.HasComponent<PositionComponent>() &&
                e.HasComponent<WinGameComponent>()
            );

            foreach (var playerEntity in playerEntities)
            {
                var playerPosition = playerEntity.GetComponent<PositionComponent>().Position.X;
                var playerComponent = playerEntity.GetComponent<PlayerComponent>();
                foreach (var winFlagEntity in winFlagEntities)
                {
                    var winFlagPosition = winFlagEntity.GetComponent<PositionComponent>().Position.X;
                    if (Math.Abs(playerPosition - winFlagPosition) < TOLERANCE)
                    {
                        playerComponent.colition = true;
                    }
                }
            }
        }
    }
}
