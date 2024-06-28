using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Services;
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
        private Dictionary<Entity, Guid> activeTimers = new Dictionary<Entity, Guid>();


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
            if (gameTime != null) TimerService.Instance.Update(gameTime);
        }

        /// <summary>
        /// The OnPowerUpCollision method is called when a power-up collision event is received.
        /// It activates the corresponding power-up for the player.
        /// </summary>
        private void OnPowerUpCollision(object eventArgs)
        {
            var powerUpEvent = (PowerUpEvent)eventArgs;
            var player = powerUpEvent.Player;
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

            if (activeTimers.ContainsKey(player))
            {
                TimerService.Instance.StopTimer(activeTimers[player]);
            }

            activeTimers[player] = TimerService.Instance.StartTimer(10.0f, () =>
            {
                playerState.IsStarInvincible = false;
                activeTimers.Remove(player);
                Console.WriteLine("Star power-up deactivated: Player is no longer invincible.");
            });

            Console.WriteLine("Star power-up activated: Player is invincible." + playerState.IsStarInvincible);
        }

        /// <summary>
        /// The ActivateMushroomPowerUp method activates the Mushroom power-up for a player.
        /// </summary>
        private static void ActivateMushroomPowerUp(Entity player)
        {
            var playerState = player.GetComponent<PlayerComponent>();
            playerState.IsBig = true;

            Console.WriteLine("Mushroom power-up activated: Player is big." + playerState.IsBig);
        }

        /// <summary>
        /// The ActivateFireFlowerPowerUp method activates the Fire Flower power-up for a player.
        /// </summary>
        private static void ActivateFireFlowerPowerUp(Entity player)
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
            Console.WriteLine("Fire Flower power-up activated: Player has fire power." + playerState.IsFire);
        }

    }
}
