using System.Collections.Generic;

using MarioGame;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Systems
{
    public class CoinSystem : BaseSystem
    {
        private HashSet<Entity> _entities = new();
        private List<Body> _bodiesToRemove = new();
        private ProgressDataManager _progressDataManager;

        public CoinSystem(ProgressDataManager progressDataManager)
        {
            _progressDataManager = progressDataManager;
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> coins = entities.WithComponents(typeof(ColliderComponent), typeof(CoinComponent), typeof(AnimationComponent));
            IEnumerable<Entity> players = entities.WithComponents(typeof(ColliderComponent), typeof(PlayerComponent));

            foreach (var entity in coins)
            {
                var collider = entity.GetComponent<ColliderComponent>();
                var coin = entity.GetComponent<CoinComponent>();
                var animation = entity.GetComponent<AnimationComponent>();

                if (collider == null || coin == null || animation == null) continue;

                animation.animations = Animations.coinBlink;
                animation.velocity = 0.15f;
                animation.Play(AnimationState.BlINK);

                if (!_entities.Contains(entity))
                {
                    _entities.Add(entity);
                    collider.collider.BodyType = BodyType.Static;
                }

                foreach (var player in players)
                {
                    var playerCollider = player.GetComponent<ColliderComponent>();

                    if (IsEntityNearby(collider.Position, playerCollider.Position))
                    {
                        coin.IsCollected = true;
                        _progressDataManager.Coins++;
                        _progressDataManager.AddCollectItem(200);
                        EventDispatcher.Instance.Dispatch(new SoundEffectEvent(SoundEffectType.CoinCollected));
                    }
                }

                if (coin.IsCollected)
                {
                    _bodiesToRemove.Add(collider.collider);
                    entity.RemoveComponent<ColliderComponent>();
                    foreach (var body in _bodiesToRemove)
                    {
                        body.World.Remove(body);
                    }
                    _bodiesToRemove.Clear();
                }
            }
        }

        private static bool IsEntityNearby(Vector2 coinPosition, Vector2 playerPosition)
        {
            float distanceThreshold = 50f;
            return Vector2.Distance(coinPosition, playerPosition) <= distanceThreshold;
        }
    }
}
