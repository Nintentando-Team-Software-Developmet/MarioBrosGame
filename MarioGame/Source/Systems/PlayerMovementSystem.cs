using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Source.Systems
{
    public class PlayerMovementSystem : BaseSystem
    {
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> playerEntities = entities.WithComponents(
                typeof(PlayerComponent),
                typeof(AnimationComponent),
                typeof(ColliderComponent),
                typeof(MovementComponent),
                typeof(InputComponent)
            );

            foreach (var player in playerEntities)
            {
                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponent>();
                var movement = player.GetComponent<MovementComponent>();
                var input = player.GetComponent<InputComponent>();
                if (input.LEFT.IsPressed)
                {
                    HandleLeftKey(collider, animation, movement);
                }
                else if (input.RIGHT.IsPressed)
                {
                    HandleKeyRight(collider, animation, movement);
                }
                else
                {
                    HandleStop(collider, animation, movement);
                }
                HandleUpKey(input, collider, animation, movement);
                LimitSpeed(collider, collider.maxSpeed);
            }
        }

        private static void HandleLeftKey(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            float velocityX = collider.collider.LinearVelocity.X;
            if (!collider.isJumping())
            {
                if (velocityX > 0)
                {
                    animation.Play(AnimationState.RUNLEFT);
                }
                else
                {
                    animation.Play(AnimationState.WALKLEFT);
                }
            }
            movement.Direction = MovementType.LEFT;
            if (velocityX > -collider.maxSpeed)
            {
                collider.collider.ApplyForce(new AetherVector2(-collider.velocity, 0));
            }
        }

        private static void HandleKeyRight(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            float velocityX = collider.collider.LinearVelocity.X;
            if (!collider.isJumping())
            {
                if (velocityX < 0)
                {
                    animation.Play(AnimationState.RUNRIGHT);
                }
                else
                {
                    animation.Play(AnimationState.WALKRIGHT);
                }
            }
            movement.Direction = MovementType.RIGHT;
            if (velocityX < collider.maxSpeed)
            {
                collider.collider.ApplyForce(new AetherVector2(collider.velocity, 0));
            };
        }

        private static void HandleUpKey(InputComponent input, ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if (input.A.IsPressed && input.A.IsHeld && !collider.isJumping())
            {
               
                if (movement.Direction == MovementType.LEFT)
                {
                    animation.Play(AnimationState.JUMPLEFT);
                    movement.Direction = MovementType.LEFT;
                }
                else if (movement.Direction == MovementType.RIGHT)
                {
                    animation.Play(AnimationState.JUMPRIGHT);
                    movement.Direction = MovementType.RIGHT;
                }
                collider.collider.ApplyLinearImpulse(new AetherVector2(0, -4.29f));
                input.A.setHeld(false);
            }
             
        }

        private static void HandleStop(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            float velocityX = collider.collider.LinearVelocity.X;
            float velocityY = collider.collider.LinearVelocity.Y;
            if (!collider.isJumping())
            {
                collider.collider.LinearVelocity = new AetherVector2(velocityX * collider.friction, velocityY);
            }
            if (movement.Direction == MovementType.LEFT && !collider.isJumping())
            {
                animation.Play(AnimationState.STOPLEFT);
            }
            else if (movement.Direction == MovementType.RIGHT && !collider.isJumping())
            {
                animation.Play(AnimationState.STOP);
            }
        }

        private static void LimitSpeed(ColliderComponent collider, float maxSpeed)
        {
            float velocityX = collider.collider.LinearVelocity.X;
            float velocityY = collider.collider.LinearVelocity.Y;

            if (velocityX > maxSpeed)
            {
                collider.collider.LinearVelocity = new AetherVector2(maxSpeed, velocityY);
            }
            else if (velocityX < -maxSpeed)
            {
                collider.collider.LinearVelocity = new AetherVector2(-maxSpeed, velocityY);
            }
        }
    }
}