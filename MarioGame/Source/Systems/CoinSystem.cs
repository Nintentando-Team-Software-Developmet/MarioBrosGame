using System.Collections.Generic;
using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class CoinSystem : BaseSystem
    {
        private HashSet<Entity> _entities = new();
        private List<Body> _bodiesToRemove = new();

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> coins = entities.WithComponents(typeof(ColliderComponent), typeof(CoinComponent));
            foreach (var entity in coins)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var coin = entity.GetComponent<CoinComponent>();
                if (collider == null || coin == null) continue;

                if (!_entities.Contains(entity))
                {
                    RegisterCoinEvents(collider, coin);
                    _entities.Add(entity);
                    collider.collider.BodyType = BodyType.Static;
                }

                if (coin.IsCollected)
                {
                    entity.RemoveComponent<ColliderComponent>();
                }
            }
            foreach (var body in _bodiesToRemove)
            {
                body.World.Remove(body);
            }
            _bodiesToRemove.Clear();
        }

        private void RegisterCoinEvents(ColliderComponent collider, CoinComponent coin)
        {
            collider.collider.OnCollision += (fixtureA, fixtureB, contact) =>
            {
                var collisionDirection = CollisionAnalyzer.GetDirectionCollision(contact);
                var coinBody = fixtureA.Body;

                if (collisionDirection == CollisionType.RIGHT || collisionDirection == CollisionType.LEFT ||
                    collisionDirection == CollisionType.DOWN || collisionDirection == CollisionType.UP)
                {
                    coin.IsCollected = true;
                    _bodiesToRemove.Add(coinBody);
                }
                return true;
            };
            collider.Enabled(true);
        }
    }
}
