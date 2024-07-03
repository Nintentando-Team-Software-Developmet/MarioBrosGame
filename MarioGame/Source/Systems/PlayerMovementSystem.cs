using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
namespace SuperMarioBros.Source.Systems
{
    public class PlayerMovementSystem : BaseSystem
    {
        private static ColliderComponent colliderCamera { get; set; }
        private bool keyboardJumpReleased = true;
        private bool gamepadJumpReleased = true;

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponent), typeof(ColliderComponent), typeof(MovementComponent));
            foreach (var player in playerEntities)
            {
                var playerComponent = player.GetComponent<PlayerComponent>();
                if (playerComponent != null && !playerComponent.IsAlive)
                {
                    continue;
                }

                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponent>();
                var movement = player.GetComponent<MovementComponent>();
                var keyboardState = Keyboard.GetState();
                var gamePadState = GamePad.GetState(PlayerIndex.One);
                var camera = player.GetComponent<CameraComponent>();
                if (!player.GetComponent<PlayerComponent>().HasReachedEnd)
                {
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
                    if (gameTime != null) HandleUpKey(gamePadState, keyboardState, collider, animation, movement, gameTime);
                    LimitSpeed(collider, collider.maxSpeed);
                    CreateInvisibleWall(camera, collider);
                }
            }
        }

        private static void CreateInvisibleWall(CameraComponent camera, ColliderComponent collider)
        {

            if (colliderCamera != null)
            {
                colliderCamera.collider.Position = new AetherVector2(camera.Position.X / GameConstants.pixelPerMeter, colliderCamera.collider.Position.Y);
            }
            else
            {
                colliderCamera = new ColliderComponent(collider.collider.World, camera.Position.X + 1f, 100, new Rectangle(100, 100, 10, 10), BodyType.Static);
            }
        }

        private static void HandleLeftKey(ColliderComponent collider, AnimationComponent animation, MovementComponent movement)
        {
            if (collider == null || animation == null || movement == null || colliderCamera == null)
            {
                return;
            }

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

            float velocityThreshold = collider.maxSpeed * 0.5f;
            bool isWithImpulse = Math.Abs(collider.collider.LinearVelocity.X) > velocityThreshold;

            float limit = isWithImpulse ? 1.4f : 0.7f;

            if (collider.collider.Position.X >= colliderCamera.collider.Position.X + limit)
            {
                movement.Direction = MovementType.LEFT;
                if (collider.collider.LinearVelocity.X > -collider.maxSpeed)
                {
                    collider.collider.ApplyForce(new AetherVector2(-collider.velocity, 0));
                }
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
                collider.collider.ApplyForce(new AetherVector2(collider.velocity, 0));
            };
        }

        private void HandleUpKey(GamePadState gamePadState, KeyboardState keyboardState, ColliderComponent collider, AnimationComponent animation, MovementComponent movement, GameTime gameTime)
        {
            if (collider == null || animation == null || movement == null || colliderCamera == null)
            {
                return;
            }

            bool jumpCondition = (keyboardState.IsKeyDown(Keys.Z) && keyboardJumpReleased) || (gamePadState.Buttons.A == ButtonState.Pressed && gamepadJumpReleased);

            if (jumpCondition && !collider.isJumping())
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


            if (collider.isJumping())
            {
                if (collider.collider.Position.X + collider.collider.LinearVelocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds <= colliderCamera.collider.Position.X + 0.3f)
                {
                    collider.collider.LinearVelocity = new AetherVector2(0, collider.collider.LinearVelocity.Y);
                    animation.Play(AnimationState.JUMPLEFT);
                }
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
