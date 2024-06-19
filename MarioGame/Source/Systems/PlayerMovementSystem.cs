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

        private bool jumpReleased = true;
        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            float maxSpeed = 3f;
            float acceleration = 3f;
            //float friction = 0.97f;

            //float lowFriction = 1.10f;
            IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponen), typeof(ColliderComponent), typeof(MovementComponent));
            foreach (var player in playerEntities)
            {
                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponen>();
                var movement = player.GetComponent<MovementComponent>();
                
                var keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    if (!collider.isJumping()) animation.Play(AnimationState.WALKLEFT);
                    movement.direcction = MovementType.LEFT;
                    if (collider.collider.LinearVelocity.X > -maxSpeed)
                    {
                        collider.collider.ApplyForce(new AetherVector2(-acceleration, 0));
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Right))
                {
                    if (!collider.isJumping()) animation.Play(AnimationState.WALKRIGHT);
                    movement.direcction = MovementType.RIGHT;
                    if (collider.collider.LinearVelocity.X < maxSpeed)
                    {
                        collider.collider.ApplyForce(new AetherVector2(acceleration, 0));
                    };
                }
                else
                {
                    //aplicar fircction
                    //if(!collider.isJumping())
                    //{
                    //    collider.collider.LinearVelocity = new AetherVector2(collider.collider.LinearVelocity.X * lowFriction, collider.collider.LinearVelocity.Y);
                    //}
                    if(movement.direcction == MovementType.LEFT && !collider.isJumping())
                    {
                        animation.Play(AnimationState.STOPLEFT);
                    }
                    else if (movement.direcction == MovementType.RIGHT && !collider.isJumping())
                    {
                        animation.Play(AnimationState.STOP);
                    }
                }

                
                if (keyboardState.IsKeyDown(Keys.Z) && !collider.isJumping() && jumpReleased)
                {
                    jumpReleased = false;
                    if (movement.direcction == MovementType.LEFT)
                    {
                        animation.Play(AnimationState.JUMPLEFT);
                        movement.direcction = MovementType.LEFT;
                    }
                    else if (movement.direcction == MovementType.RIGHT)
                    {
                        animation.Play(AnimationState.JUMPRIGHT);
                        movement.direcction = MovementType.RIGHT;
                    }
                    collider.collider.ApplyLinearImpulse(new AetherVector2(0, -4.29f));
                }
                if (keyboardState.IsKeyUp(Keys.Z))
                {
                    jumpReleased = true;
                }

            
                LimitSpeed(collider, maxSpeed);
                Console.WriteLine("Animation State " + animation.currentState);
               // Console.WriteLine("HOrizontal velocity: " + collider.collider.LinearVelocity.X + " Vertical velocity: " + collider.collider.LinearVelocity.Y);
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