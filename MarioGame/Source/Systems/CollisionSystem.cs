using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems;

public class CollisionSystem : BaseSystem
{
    private Dictionary<Vector2, int> _tilemap;
    private int _levelHeight;

    public CollisionSystem(Dictionary<Vector2, int> tilemap, int levelHeight)
    {
        _tilemap = tilemap;
        _levelHeight = levelHeight;
    }

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
        }
    }

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
