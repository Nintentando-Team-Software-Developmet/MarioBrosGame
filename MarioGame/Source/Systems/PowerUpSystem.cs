using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class PowerUpSystem : BaseSystem
    {
        private Dictionary<Entity, float> powerUpTimers = new Dictionary<Entity, float>();
        private Dictionary<Entity, PowerUpType> activePowerUps = new Dictionary<Entity, PowerUpType>();

        public PowerUpSystem()
        {
            EventDispatcher.Instance.Subscribe<PowerUpEvent>(OnPowerUpCollision);
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (gameTime != null)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var players = entities.WithComponents(typeof(PlayerComponent), typeof(PlayerStateComponent));

                foreach (var player in players)
                {
                    if (activePowerUps.ContainsKey(player))
                    {
                        powerUpTimers[player] -= deltaTime;
                        if (powerUpTimers[player] <= 0)
                        {
                            RemovePowerUp(player);
                        }
                    }
                }
            }
        }

        private void OnPowerUpCollision(object eventArgs)
        {
            var powerUpEvent = (PowerUpEvent)eventArgs;
            var player = powerUpEvent.Player;
            var powerUp = powerUpEvent.PowerUp;
            var powerUpType = powerUpEvent.PowerUpType;

            if (powerUpType == PowerUpType.Star)
            {
                ActivateStarPowerUp(player);
            }
            else if (powerUpType == PowerUpType.Mushroom)
            {
                ActivateMushroomPowerUp(player);
            }
            else if (powerUpType == PowerUpType.FireFlower)
            {
                ActivateFireFlowerPowerUp(player);
            }

            powerUp.GetComponent<ColliderComponent>().Enabled(false);
            powerUp.ClearComponents();
        }

        private void ActivateStarPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerStateComponent>();
            playerState.IsInvincible = true;
            activePowerUps[player] = PowerUpType.Star;
            powerUpTimers[player] = 10.0f;
        }

        private void ActivateMushroomPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerStateComponent>();
            playerState.IsBig = true;
            activePowerUps[player] = PowerUpType.Mushroom;
            powerUpTimers[player] = 0;
        }

        private void ActivateFireFlowerPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerStateComponent>();
            playerState.HasFirePower = true;
            activePowerUps[player] = PowerUpType.FireFlower;
            powerUpTimers[player] = 0;
        }

        private void RemovePowerUp(Entity player)
        {
            if (activePowerUps.ContainsKey(player))
            {
                var powerUpType = activePowerUps[player];
                var playerState = player.GetComponent<PlayerStateComponent>();

                if (powerUpType == PowerUpType.Star)
                {
                    playerState.IsInvincible = false;
                }
                else if (powerUpType == PowerUpType.Mushroom)
                {
                    playerState.IsBig = false;
                }
                else if (powerUpType == PowerUpType.FireFlower)
                {
                    playerState.HasFirePower = false;
                }

                activePowerUps.Remove(player);
                powerUpTimers.Remove(player);
            }
        }
    }
}
