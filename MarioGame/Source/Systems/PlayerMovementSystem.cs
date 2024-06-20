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

        private bool keyboardJumpReleased = true;
        private bool gamepadJumpReleased = true;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponent), typeof(ColliderComponent), typeof(MovementComponent));
            foreach (var player in playerEntities)
            {
                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponent>();
                var movement = player.GetComponent<MovementComponent>();
                var keyboardState = Keyboard.GetState();
                var gamePadState = GamePad.GetState(PlayerIndex.One);
                if (keyboardState.IsKeyDown(Keys.Left) || gamePadState.DPad.Left == ButtonState.Pressed)
                {
                    HandleLeftKey(collider, animation, movement);
                }
                else if (keyboardState.IsKeyDown(Keys.Right) || gamePadState.DPad.Right == ButtonState.Pressed)
                {
                    HandleKeyRight(collider, animation, movement);
                }
                else
                {
                    HandleStop(collider, animation, movement);
                }
                HandleUpKey(gamePadState, keyboardState, collider, animation, movement);
                LimitSpeed(collider, collider.maxSpeed);
            }
        }

        private static void HandleLeftKey(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if (!collider.isJumping())
            {
                if (collider.collider.LinearVelocity.X > 0)
                {
                    animation.Play(AnimationState.RUNLEFT);
                }
                else
                {
                    animation.Play(AnimationState.WALKLEFT);
                }
            }
            movement.Direction = MovementType.LEFT;
            if (collider.collider.LinearVelocity.X > -collider.maxSpeed)
            {
                collider.collider.ApplyForce(new AetherVector2(-collider.acceleration, 0));
            }
        }

        private static void HandleKeyRight(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if (!collider.isJumping())
            {
                if (collider.collider.LinearVelocity.X < 0)
                {
                    animation.Play(AnimationState.RUNRIGHT);
                }
                else
                {
                    animation.Play(AnimationState.WALKRIGHT);
                }
            }
            movement.Direction = MovementType.RIGHT;
            if (collider.collider.LinearVelocity.X < collider.maxSpeed)
            {
                collider.collider.ApplyForce(new AetherVector2(collider.acceleration, 0));
            };
        }

        private void HandleUpKey(GamePadState gamePadState, KeyboardState keyboardState, ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if ((keyboardState.IsKeyDown(Keys.Z) && keyboardJumpReleased || gamePadState.Buttons.A == ButtonState.Pressed && gamepadJumpReleased) && !collider.isJumping())
            {
                if (keyboardState.IsKeyDown(Keys.Z)) keyboardJumpReleased = false;
                if (gamePadState.Buttons.A == ButtonState.Pressed) gamepadJumpReleased = false;
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
            }
            if (keyboardState.IsKeyUp(Keys.Z))
            {
                keyboardJumpReleased = true;
            }
            if (gamePadState.Buttons.A == ButtonState.Released)
            {
                gamepadJumpReleased = true;
            }
        }

        private static void HandleStop(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if (!collider.isJumping())
            {
                collider.collider.LinearVelocity = new AetherVector2(collider.collider.LinearVelocity.X * collider.friction, collider.collider.LinearVelocity.Y);
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
            if (collider.collider.LinearVelocity.X > maxSpeed)
            {
                collider.collider.LinearVelocity = new AetherVector2(maxSpeed, collider.collider.LinearVelocity.Y);
            }
            else if (collider.collider.LinearVelocity.X < -maxSpeed)
            {
                collider.collider.LinearVelocity = new AetherVector2(-maxSpeed, collider.collider.LinearVelocity.Y);
            }
        }
    }
}