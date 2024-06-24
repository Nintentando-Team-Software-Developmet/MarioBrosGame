using System.Collections.Generic;

using Microsoft.Xna.Framework;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Systems
{
    public class PlayerSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> players = entities.WithComponents(typeof(PlayerComponent), typeof(ColliderComponent), typeof(AnimationComponent));
            foreach (var player in players)
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                var colliderComponent = player.GetComponent<ColliderComponent>();
                var animationComponent = player.GetComponent<AnimationComponent>();
                var playerPosition = colliderComponent.Position;

                if (playerPosition.Y > GameConstants.CameraViewportHeight + 300 && playerComponent.IsAlive)
                {
                    playerComponent.IsAlive = false;
                    StartDeathAnimation(playerComponent, animationComponent, colliderComponent);
                }

                if (!playerComponent.IsAlive)
                {
                    if (gameTime != null)
                    {
                        UpdateDeathAnimation(gameTime, playerComponent, colliderComponent);
                    }
                }
            }
        }

        private static void StartDeathAnimation(PlayerComponent playerComponent, AnimationComponent animationComponent, ColliderComponent colliderComponent)
        {
            playerComponent.IsDying = true;
            playerComponent.DeathTimer = 0;
            animationComponent.Play(AnimationState.DIE);

            colliderComponent.collider.BodyType = BodyType.Dynamic;

            float jumpForce = 30f;
            colliderComponent.collider.ApplyLinearImpulse(new AetherVector2(0, -jumpForce));

            colliderComponent.collider.LinearVelocity = new AetherVector2(0, -5f);
        }

        private static void UpdateDeathAnimation(GameTime gameTime, PlayerComponent playerComponent, ColliderComponent colliderComponent)
        {
            playerComponent.DeathTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (playerComponent.DeathTimer <= 1f)
            {
                colliderComponent.collider.LinearVelocity = new AetherVector2(0, -5f);
            }
            else if (playerComponent.DeathTimer > 1f && playerComponent.DeathTimer <= 2f)
            {
                colliderComponent.collider.LinearVelocity = new AetherVector2(0, 5f);
            }
            else
            {
                colliderComponent.collider.LinearVelocity = AetherVector2.Zero;
                playerComponent.DeathAnimationComplete = true;
            }
        }
    }
}
