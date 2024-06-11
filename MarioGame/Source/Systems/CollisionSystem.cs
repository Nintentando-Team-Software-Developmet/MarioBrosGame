using System.Collections.Generic;


using Microsoft.Xna.Framework;


using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using System;
using System.Linq;

namespace SuperMarioBros.Source.Systems
{
    /*
    * The CollisionSystem class is responsible for handling collision detection
    * and response for entities in the game.
    */
    public class CollisionSystem : BaseSystem
    {
        private readonly Dictionary<Vector2, int> _tilemap;
        private readonly int _levelHeight;

        /*
        * Initializes a new instance of the CollisionSystem class.
        *
        * @param tilemap A dictionary representing the tilemap where keys are tile positions and values are tile types.
        * @param levelHeight The height of the level in tiles.
        */
        public CollisionSystem(Dictionary<Vector2, int> tilemap, int levelHeight)
        {
            _tilemap = tilemap;
            _levelHeight = levelHeight;
        }

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
                var positionComponent = entity.GetComponent<PositionComponent>();
                var velocityComponent = entity.GetComponent<VelocityComponent>();

                if (positionComponent != null && velocityComponent != null)
                {
                    Vector2 futurePosition = positionComponent.Position + velocityComponent.Velocity;
                    if (IsColliding(futurePosition))
                    {
                        positionComponent.Position = AdjustPosition(positionComponent.Position, velocityComponent.Velocity);
                        velocityComponent.Velocity = Vector2.Zero;
                    }
                    else
                    {
                        positionComponent.Position = futurePosition;
                    }
                }

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
                var playerPosition = playerEntity.GetComponent<PositionComponent>().Position;
                var playerComponent = playerEntity.GetComponent<PlayerComponent>();
                foreach (var winFlagEntity in winFlagEntities)
                {
                    var winFlagPosition = winFlagEntity.GetComponent<PositionComponent>().Position;
                    var winMario = winFlagEntity.GetComponent<WinGameComponent>();

                    if (playerPosition.X >=  winFlagPosition.X)
                    {
                        Console.WriteLine('A');
                        playerComponent.colition = true;
                    }
                }
            }
        }

        /*
        * Determines if a given position is colliding with a tile in the tilemap.
        *
        * @param position The position to check for collisions.
        * @returns True if the position is colliding with a tile; otherwise, false.
        */
        private bool IsColliding(Vector2 position)
        {
            if (_tilemap == null)
                return false;

            int tileX = (int)position.X / 64;
            int tileY = (int)position.Y / 55;

            if (tileY == _levelHeight - 2)
            {
                return _tilemap.ContainsKey(new Vector2(tileX, tileY));
            }

            return false;
        }

        /*
        * Adjusts the position of an entity to resolve a collision based on its velocity.
        *
        * @param position The current position of the entity.
        * @param velocity The current velocity of the entity.
        * @returns The adjusted position to resolve the collision.
        */
        private static Vector2 AdjustPosition(Vector2 position, Vector2 velocity)
        {
            int tileX = (int)position.X / 64;
            int tileY = (int)position.Y / 64;

            if (velocity.X > 0)
                position.X = tileX * 64 - 1;
            else if (velocity.X < 0)
                position.X = (tileX + 1) * 64;

            if (velocity.Y > 0)
                position.Y = tileY * 64 - 1;
            else if (velocity.Y < 0)
                position.Y = (tileY + 1) * 64;

            return position;
        }
    }
}
