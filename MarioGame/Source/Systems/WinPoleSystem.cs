using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

using Vector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems;

public class WinPoleSystem : BaseSystem
{
    private HashSet<Entity> registeredEntities = new();
    private List<Body> _playerBodies = new();

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        IEnumerable<Entity> players = entities.WithComponents(typeof(ColliderComponent), typeof(PlayerComponent));
        IEnumerable<Entity> flags = entities.WithComponents(typeof(ColliderComponent), typeof(WinFlagComponent));
        foreach (var player in players)
        {
            _playerBodies.Add(player.GetComponent<ColliderComponent>().collider);
        }

        IEnumerable<Entity> winPole = entities.WithComponents(typeof(ColliderComponent), typeof(WinPoleSensorComponent));
        foreach (Entity entity in winPole)
        {
            var collider = entity.GetComponent<ColliderComponent>();
            var pole = entity.GetComponent<WinPoleSensorComponent>();
            if (collider == null || pole == null) continue;
            if (!registeredEntities.Contains(entity))
            {
                RegisterPoleEvents(collider, pole);
                registeredEntities.Add(entity);
            }

            if (pole.MarioContact)
            {
                foreach (Entity flag in flags)
                {
                    flag.GetComponent<ColliderComponent>().collider.IgnoreGravity = false;
                    flag.GetComponent<ColliderComponent>().collider.LinearVelocity = new Vector2(0, 3);

                }
            }
        }
    }

    private void RegisterPoleEvents(ColliderComponent collider, WinPoleSensorComponent winPoleSensorComponent)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var otherBody = fixtureB.Body;

            if (winPoleSensorComponent.MarioContact)
            {
                return false;
            }
            if (_playerBodies.Contains(otherBody))
            {
                winPoleSensorComponent.MarioContact = true;
            }
            return true;
        };
    }
}
