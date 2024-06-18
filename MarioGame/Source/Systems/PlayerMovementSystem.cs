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
            float maxSpeed = 3f; 
            float acceleration = 2f; 
            float friction = 0.9f;
            IEnumerable<Entity> playerEntities = entities.WithComponents(typeof(PlayerComponent), typeof(AnimationComponen), typeof(ColliderComponent), typeof(MovementComponent));
            foreach (var player in playerEntities)
            {
                var collider = player.GetComponent<ColliderComponent>();
                var animation = player.GetComponent<AnimationComponen>();
                var movement = player.GetComponent<MovementComponent>();
                
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    animation.Play(AnimationState.WALKLEFT);
                    movement.direcction = MovementType.LEFT;
                    if (collider.collider.LinearVelocity.X > -maxSpeed)
                    {
                        collider.collider.ApplyForce(new AetherVector2(-acceleration, 0));
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    animation.Play(AnimationState.WALKRIGHT);
                    movement.direcction = MovementType.RIGHT;
                    if (collider.collider.LinearVelocity.X < maxSpeed)
                    {
                        collider.collider.ApplyForce(new AetherVector2(acceleration, 0));
                    };
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                }
                else
                {   if(collider.collider.LinearVelocity.Y == 0){
                    collider.collider.LinearVelocity *= friction;}
                    if (movement.direcction == MovementType.LEFT || movement.direcction == MovementType.UPLEFT)
                    {
                        animation.Play(AnimationState.STOPLEFT);
                    }
                    else if (movement.direcction == MovementType.RIGHT || movement.direcction == MovementType.UPRIGHT)
                    {
                        animation.Play(AnimationState.STOP);
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && collider.collider.LinearVelocity.Y == 0)
                {

                    if(movement.direcction == MovementType.LEFT || movement.direcction == MovementType.UPLEFT)
                    {
                        animation.Play(AnimationState.JUMPLEFT);
                        movement.direcction = MovementType.UPLEFT;
                    }
                    else if(movement.direcction == MovementType.RIGHT || movement.direcction == MovementType.UPRIGHT)
                    {
                        animation.Play(AnimationState.JUMPRIGHT);
                        movement.direcction = MovementType.UPRIGHT;
                    }
                    collider.collider.ApplyForce(new AetherVector2(0, -180f));
    
                }
        
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
}