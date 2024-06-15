using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Utils.SceneCommonData;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Systems
{
    public class MarioAnimationSystem : BaseSystem, IRenderableSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private Texture2D[] spritesheets { get; set; }
        private Texture2D[] spritesheetsRunLeft { get; set; }
        private Texture2D[] spritesheetsJump { get; set; }
        private Texture2D[] spritesheetsJump2 { get; set; }
        private Texture2D[] spritesheetsBend { get; set; }
        private Texture2D[] spritesheetsBend2 { get; set; }
        private Texture2D[] spritesheetsWin { get; set; }
        private Texture2D[] spritesheetsWinLeft { get; set; }
        private Texture2D[] spritesheetsWinRun { get; set; }

        private int currentTextureIndex { get; set; }
        private int frames { get; set; }
        private float timeSinceLastFrame { get; set; }
        private bool isActive { get; set; }
        private bool isMovingLeft { get; set; }
        private readonly float movementSpeedScale = 0.5f;
        private bool hasLoopedOnce { get; set; }
        private bool isJumping { get; set; }
        private float currentJumpHeight { get; set; }
        private float jumpAnimationFrameTime = 50.2f;
        private Vector2 positionBed { get; set; }
        private bool isDescending { get; set; } = true;
        const float maxJumpHeight = 400f;
        const float jumpSpeed = 400f;
        const float thenJumpSpeed = 400f;
        private Vector2 jumpEndY { get; set; }
        private Vector2 lastJumpingPosition { get; set; }
        private bool wasJumping { get; set; }
        private WinGameSystem WinGame { get; set; } = new WinGameSystem();
        public MarioAnimationSystem(SpriteBatch spriteBatch)
        {
            isActive = false;
            _spriteBatch = spriteBatch;
            isJumping = false;
            wasJumping = false;
        }

        public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null)
            {
                entities = entities.Where(e =>
                    e.HasComponent<AnimationComponent>() &&
                    e.HasComponent<PositionComponent>() &&
                    e.HasComponent<VelocityComponent>() &&
                    e.HasComponent<PlayerComponent>());

                foreach (var entity in entities)
                {
                    var animation = entity.GetComponent<AnimationComponent>();
                    var position = entity.GetComponent<PositionComponent>();
                    var velocity = entity.GetComponent<VelocityComponent>();
                    var player = entity.GetComponent<PlayerComponent>();

                    if (animation != null && animation.IsAnimating)
                    {
                        if (gameTime != null)
                        {
                            animation.TimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }

                        if (animation.TimeElapsed >= animation.FrameTime)
                        {
                            animation.CurrentFrame = (animation.CurrentFrame + 1) % animation.Textures.Count;
                            animation.TimeElapsed = 0f;
                        }

                        position.Position += velocity.Velocity * movementSpeedScale * 10;

                        if (velocity.Velocity.X != 0)
                        {
                            isMovingLeft = velocity.Velocity.X < 0;
                        }
                        SetActive(velocity.Velocity != Vector2.Zero);
                    }
                    if (player.colition && wasJumping)
                    {
                        player.HasReachedEnd = true;
                    }

                }

            }

        }

        public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
        {
            if (entities != null)
            {
                var playerEntities = entities.Where(e =>
                    e.HasComponent<AnimationComponent>() &&
                    e.HasComponent<PositionComponent>() &&
                    e.HasComponent<PlayerComponent>()
                    );


                foreach (var entity in playerEntities)
                {
                    var playerAnimation = entity.GetComponent<AnimationComponent>();
                    var position = entity.GetComponent<PositionComponent>();
                    var playerComponent = entity.GetComponent<PlayerComponent>();
                    var collider= entity.GetComponent<ColliderComponent>();


                    if (playerAnimation != null && position != null)
                    {
                        spritesheets = new Texture2D[]
                        {
                            playerAnimation.Textures[0], playerAnimation.Textures[1], playerAnimation.Textures[2],
                            playerAnimation.Textures[3], playerAnimation.Textures[4]
                        };
                        spritesheetsRunLeft = new Texture2D[]
                        {
                            playerAnimation.Textures[5], playerAnimation.Textures[6], playerAnimation.Textures[7],
                            playerAnimation.Textures[8], playerAnimation.Textures[9]
                        };
                        spritesheetsJump = new Texture2D[] { playerAnimation.Textures[12] };
                        spritesheetsJump2 = new Texture2D[] { playerAnimation.Textures[13] };
                        spritesheetsBend = new Texture2D[] { playerAnimation.Textures[10] };
                        spritesheetsBend2 = new Texture2D[] { playerAnimation.Textures[11] };
                        spritesheetsWin = new Texture2D[] { playerAnimation.Textures[15], playerAnimation.Textures[14] };
                        spritesheetsWinLeft = new Texture2D[] { playerAnimation.Textures[16], playerAnimation.Textures[8] };
                        spritesheetsWinRun = new Texture2D[] { playerAnimation.Textures[2], playerAnimation.Textures[3], playerAnimation.Textures[4] };
                        if (playerComponent.colition && wasJumping)
                        {

                            if (gameTime != null) WinGame.DrawWinMario(_spriteBatch, position.Position, gameTime, spritesheetsWin, jumpEndY, spritesheetsWinLeft, spritesheetsWinRun);
                        }
                        else
                        {
                            playerComponent.colition = false;
                            bool hasDrawn = false;
                            if (isJumping || isDescending)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumping(_spriteBatch, position.LastPosition, gameTime,collider,playerAnimation);
                                    hasDrawn = true;

                                }
                            }
                            else if (position.pass == false)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumping(_spriteBatch, position.LastPosition, gameTime,collider,playerAnimation);
                                    isJumping = true;
                                    hasDrawn = true;
                                }
                            }
                            else if (position.passR == false)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumpingWhitRun(_spriteBatch, position.LastPosition, gameTime,collider,playerAnimation);
                                    isJumping = true;
                                    hasDrawn = true;
                                }
                            }
                            else if (position.passBed == false)
                            {
                                positionBed = positionBed with { X = position.Position.X };
                                positionBed = positionBed with { Y = position.Position.Y + 33 };

                                DrawBed(_spriteBatch, positionBed,collider,playerAnimation);
                                hasDrawn = true;
                            }
                            else if (position.Position.X != position.LastPosition.X)
                            {
                                DrawRunning(_spriteBatch, gameTime, position.Position, position.LastPosition,collider,playerAnimation);
                                hasDrawn = true;

                            }

                            if (!hasDrawn)
                            {
                                DrawStopped(_spriteBatch, position.LastPosition,collider,playerAnimation);
                                wasJumping = false;
                            }
                        }
                    }
                    WinGame.DrawWinGame(entities, _spriteBatch, playerComponent.colition);
                }
            }
        }

        private void DrawJumpingWhitRun(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, ColliderComponent collider, AnimationComponent animationComponent)
        {
            SetForceJump(gameTime, collider, 20f, 3);

            int currentFrameIndex = (int)(gameTime.TotalGameTime.TotalSeconds / jumpAnimationFrameTime) % spritesheetsJump.Length;
            if (isMovingLeft)
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsJump2[currentFrameIndex], collider, animationComponent.width, animationComponent.height,0);
            }
            else
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsJump[currentFrameIndex], collider,  animationComponent.width, animationComponent.height,0);
            }
            wasJumping = true;
        }

        private void DrawJumping(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, ColliderComponent collider, AnimationComponent animationComponent)
        {

            SetForceJump(gameTime, collider, 12f, (float)0.5);

            int currentFrameIndex = (int)(gameTime.TotalGameTime.TotalSeconds / jumpAnimationFrameTime) % spritesheetsJump.Length;
            if (isMovingLeft)
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsJump2[currentFrameIndex], collider,  animationComponent.width, animationComponent.height,0);
            }
            else
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsJump[currentFrameIndex], collider,  animationComponent.width, animationComponent.height,0);
            }

            wasJumping = true;
        }

        private void SetForceJump(GameTime gameTime, ColliderComponent collider, float pushUp ,float pushUpR  )
        {
            if (collider == null || collider.collider == null)
                return;

            if (!isDescending)
            {

                float jumpForceMagnitude = -pushUp;
                AetherVector2 jumpForce = new AetherVector2(pushUpR, jumpForceMagnitude);
                collider.collider.ApplyForce(jumpForce);

                currentJumpHeight += jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentJumpHeight >= maxJumpHeight)
                {
                    currentJumpHeight = maxJumpHeight;
                    isDescending = true;
                }
            }
            else
            {


                currentJumpHeight -= thenJumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (currentJumpHeight <= 0)
                {
                    currentJumpHeight = 0;
                    isDescending = false;
                    isJumping = false;
                }
            }

        }


        private void DrawRunning(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, Vector2 previousPosition,ColliderComponent collider, AnimationComponent animationComponent)
        {

            if (position.X > previousPosition.X)
            {
                RunRight(spriteBatch, position, gameTime,collider,animationComponent);
                isMovingLeft = false;
            }
            else
            {
                RunLeft(spriteBatch, position, gameTime,collider,animationComponent);
                isMovingLeft = true;
            }
        }
        public void RunRight(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime,ColliderComponent collider, AnimationComponent animationComponent)
        {
            if (!hasLoopedOnce)
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheets,collider,animationComponent);
            }
            else
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheets.Skip(2).Concat(spritesheets.Skip(2)).ToArray(),collider,animationComponent);
            }
            frames = spritesheets.Length;
            if (collider != null && collider.collider != null)
            {
                float forceMagnitude = 8f;
                AetherVector2 force = new AetherVector2(forceMagnitude, 0);
                collider.collider.ApplyForce(force);
            }

            if (gameTime != null)
            {
                UpdateFrameTiming(gameTime);
            }
        }
        public void RunLeft(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime,ColliderComponent collider, AnimationComponent animationComponent)
        {
            if (!hasLoopedOnce)
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheetsRunLeft,collider,animationComponent);
            }
            else
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheetsRunLeft.Skip(2).Concat(spritesheetsRunLeft.Skip(2)).ToArray(),collider,animationComponent);
            }
            frames = spritesheetsRunLeft.Length;
            if (collider != null && collider.collider != null)
            {
                float forceMagnitude = -10f;
                AetherVector2 force = new AetherVector2(forceMagnitude, 5);
                collider.collider.ApplyForce(force);
            }

            if (gameTime != null)
            {
                UpdateFrameTiming(gameTime);
            }
        }

        public void DrawMovement(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] sprites,ColliderComponent collider, AnimationComponent animationComponent)
        {
            if (sprites != null && sprites.Length > 0)
            {
                var currentSprite = sprites[currentTextureIndex];

                if (animationComponent != null && collider != null && collider.collider != null)
                {
                    CommonRenders.DrawEntity(spriteBatch, currentSprite, collider,
                        animationComponent.width, animationComponent.height,0);
                }
            }
        }

        private void DrawStopped(SpriteBatch spriteBatch, Vector2 position,ColliderComponent collider,AnimationComponent animationComponent)
        {

            if (isMovingLeft)
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsRunLeft[0], collider,animationComponent.width,animationComponent.height,0);
            }
            else
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheets[0], collider,animationComponent.width,animationComponent.height,0);
            }
        }
        private void DrawBed(SpriteBatch spriteBatch, Vector2 position,ColliderComponent collider,AnimationComponent animationComponent)
        {

            if (isMovingLeft)
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsBend2[0], collider,animationComponent.width,animationComponent.height-27,20);

            }
            else
            {
                CommonRenders.DrawEntity(spriteBatch, spritesheetsBend[0], collider,animationComponent.width,animationComponent.height-27,20);

            }
        }

        private void UpdateFrameTiming(GameTime gameTime)
        {
            timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            float frameTime = (currentTextureIndex == 1) ? 200f : 100f;

            if (timeSinceLastFrame >= frameTime)
            {
                timeSinceLastFrame -= frameTime;
                currentTextureIndex++;
                if (currentTextureIndex >= frames)
                {
                    if (!hasLoopedOnce)
                    {
                        hasLoopedOnce = true;
                    }
                    currentTextureIndex = 2;
                }
            }
        }

        public void SetActive(bool active)
        {
            isActive = active;
            if (!isActive)
            {
                currentTextureIndex = 0;
                hasLoopedOnce = false;

            }
        }
    }
}
