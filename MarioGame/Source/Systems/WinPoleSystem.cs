using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;

namespace SuperMarioBros.Source.Systems;

public class WinPoleSystem : BaseSystem
{
    private HashSet<Entity> registeredEntities = new();
    private List<Body> _playerBodies = new();

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        IEnumerable<Entity> players = entities.WithComponents(typeof(ColliderComponent), typeof(PlayerComponent));

        foreach (var player in players)
        {
            _playerBodies.Add(player.GetComponent<ColliderComponent>().collider);
        }

        IEnumerable<Entity> winPole = entities.WithComponents(typeof(ColliderComponent), typeof(WinGameComponent));
        foreach (Entity entity in winPole)
        {
            var collider = entity.GetComponent<ColliderComponent>();
            var pole = entity.GetComponent<WinGameComponent>();
            if (collider == null || pole == null) continue;
            if (!registeredEntities.Contains(entity))
            {
                RegisterPoleEvents(collider, pole);
                registeredEntities.Add(entity);
            }
        }
    }

    private void RegisterPoleEvents(ColliderComponent collider, WinGameComponent winGameComponent)
    {
        collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
        {
            var otherBody = fixtureB.Body;

            if (winGameComponent.MarioContact)
            {
                return false;
            }
            if (_playerBodies.Contains(otherBody))
            {
                winGameComponent.MarioContact = true;
            }
            return true;
        };
    }
}
