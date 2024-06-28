using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Systems
{
    /// <summary>
    /// The PowerUpSystem class is responsible for managing power-ups in the game.
    /// It keeps track of active power-ups and their timers, and handles power-up collision events.
    /// </summary>
    public class PowerUpSystem : BaseSystem
    {
        private Dictionary<Entity, float> powerUpTimers = new Dictionary<Entity, float>();
        private Dictionary<Entity, PowerUpType> activePowerUps = new Dictionary<Entity, PowerUpType>();

        /// <summary>
        /// The constructor subscribes to the PowerUpEvent.
        /// </summary>
        public PowerUpSystem()
        {
            EventDispatcher.Instance.Subscribe<PowerUpEvent>(OnPowerUpCollision);
        }

        /// <summary>
        /// The Update method is called every frame and updates the power-up timers.
        /// </summary>
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (gameTime != null)
            {
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var players = entities.WithComponents(typeof(PlayerComponent));

                foreach (var player in players)
                {
                    if (activePowerUps.ContainsKey(player))
                    {
                        powerUpTimers[player] -= deltaTime;
                        if (powerUpTimers[player] <= 0 && activePowerUps[player] == PowerUpType.Star)
                        {
                            RemovePowerUp(player);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The OnPowerUpCollision method is called when a power-up collision event is received.
        /// It activates the corresponding power-up for the player.
        /// </summary>
        private void OnPowerUpCollision(object eventArgs)
        {
            var powerUpEvent = (PowerUpEvent)eventArgs;
            var player = powerUpEvent.Player;
            var powerUp = powerUpEvent.PowerUp;
            var powerUpType = powerUpEvent.PowerUpType;

            Console.WriteLine($"PowerUp collision event received: {powerUpType}");

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
        }

        /// <summary>
        /// The ActivateStarPowerUp method activates the Star power-up for a player.
        /// </summary>
        private void ActivateStarPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerComponent>();
            playerState.IsStarInvincible = true;
            activePowerUps[player] = PowerUpType.Star;
            powerUpTimers[player] = 10.0f;

            Console.WriteLine("Star power-up activated: Player is invincible." + playerState.IsStarInvincible);
        }

        /// <summary>
        /// The ActivateMushroomPowerUp method activates the Mushroom power-up for a player.
        /// </summary>
        private void ActivateMushroomPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerComponent>();
            playerState.IsBig = true;
            activePowerUps[player] = PowerUpType.Mushroom;
            powerUpTimers[player] = 0;

            Console.WriteLine("Mushroom power-up activated: Player is big." + playerState.IsBig);
        }

        /// <summary>
        /// The ActivateFireFlowerPowerUp method activates the Fire Flower power-up for a player.
        /// </summary>
        private void ActivateFireFlowerPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerComponent>();
            if (playerState.IsBig)
            {
                playerState.IsFire = true;
            }
            else
            {
                ActivateMushroomPowerUp(player);
                return;
            }
            activePowerUps[player] = PowerUpType.FireFlower;
            powerUpTimers[player] = 0;

            Console.WriteLine("Fire Flower power-up activated: Player has fire power." + playerState.IsFire);
        }

        /// <summary>
        /// The RemovePowerUp method removes an active power-up from a player.
        /// </summary>
        private void RemovePowerUp(Entity player)
        {
            if (activePowerUps.ContainsKey(player))
            {
                var powerUpType = activePowerUps[player];
                var playerState = player.GetComponent<PlayerComponent>();

                if (powerUpType == PowerUpType.Star)
                {
                    playerState.IsStarInvincible = false;
                    Console.WriteLine("Star power-up deactivated: Player is no longer invincible." + playerState.IsStarInvincible);
                }
                else if (powerUpType == PowerUpType.Mushroom)
                {
                    playerState.IsBig = false;
                    Console.WriteLine("Mushroom power-up deactivated: Player is no longer big." + playerState.IsBig);
                }
                else if (powerUpType == PowerUpType.FireFlower)
                {
                    playerState.IsFire = false;
                    playerState.IsBig = false;
                    Console.WriteLine("Fire Flower power-up deactivated: Player no longer has fire power." + playerState.IsFire);
                }

                activePowerUps.Remove(player);
                powerUpTimers.Remove(player);
            }
        }
    }
}
