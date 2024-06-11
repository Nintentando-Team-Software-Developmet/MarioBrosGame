using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;


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
        private Texture2D[] spritesheetsWinRun{ get; set; }

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
        private  Vector2 positionBed { get; set; }
        private bool isDescending { get; set; } = true;
        const float maxJumpHeight = 200f;
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

                        position.Position += velocity.Velocity * movementSpeedScale;

                        if (velocity.Velocity.X != 0)
                        {
                            isMovingLeft = velocity.Velocity.X < 0;
                        }
                        SetActive(velocity.Velocity != Vector2.Zero);
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
                        spritesheetsWinLeft = new Texture2D[] {playerAnimation.Textures[16],playerAnimation.Textures[8] };
                        spritesheetsWinRun = new Texture2D[] { playerAnimation.Textures[2], playerAnimation.Textures[3], playerAnimation.Textures[4]};

                        if (playerComponent.colition && wasJumping)
                        {

                            if (gameTime != null) WinGame.DrawWinMario(_spriteBatch, position.Position, gameTime,spritesheetsWin, jumpEndY,spritesheetsWinLeft,spritesheetsWinRun);
                        }
                        else
                        {
                            playerComponent.colition = false;
                            bool hasDrawn = false;
                            if (isJumping || isDescending)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumping(_spriteBatch, position.LastPosition, gameTime);
                                    hasDrawn = true;

                                }
                            }
                            else if (position.pass == false)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumping(_spriteBatch, position.LastPosition, gameTime);
                                    isJumping = true;
                                    hasDrawn = true;
                                }
                            }
                            else if (position.passR == false)
                            {
                                if (gameTime != null)
                                {
                                    DrawJumping(_spriteBatch, position.LastPosition, gameTime);
                                    isJumping = true;
                                    hasDrawn = true;
                                }
                            }
                            else if (position.passBed == false)
                            {
                                positionBed = positionBed with { X = position.Position.X };
                                positionBed = positionBed with { Y = position.Position.Y + 33 };

                                DrawBed(_spriteBatch, positionBed);
                                hasDrawn = true;
                            }
                            else if (position.Position.X != position.LastPosition.X)
                            {
                                DrawRunning(_spriteBatch, gameTime, position.Position, position.LastPosition);
                                hasDrawn = true;
                            }

                            if (!hasDrawn)
                            {
                                DrawStopped(_spriteBatch, position.LastPosition);
                                wasJumping = false;
                            }
                        }

                    }

                    WinGame.DrawWinGame(gameTime, entities, _spriteBatch, playerComponent.colition);
                }
            }
        }


        private void DrawJumping(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            lastJumpingPosition = position;
            if (!isDescending)
            {
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
            position = position with { Y = position.Y - currentJumpHeight };
            jumpEndY = jumpEndY with { Y = position.Y };
            int currentFrameIndex = (int)(gameTime.TotalGameTime.TotalSeconds / jumpAnimationFrameTime) % spritesheetsJump.Length;
            if (isMovingLeft)
            {
                spriteBatch.Draw(spritesheetsJump2[currentFrameIndex], position, Color.White);
            }
            else
            {
                spriteBatch.Draw(spritesheetsJump[currentFrameIndex], position, Color.White);
            }
            wasJumping = true;
        }

        private void DrawRunning(SpriteBatch spriteBatch, GameTime gameTime, Vector2 position, Vector2 previousPosition)
        {
            wasJumping = false;
            if (position.X > previousPosition.X)
            {
                RunRight(spriteBatch, position, gameTime);
                isMovingLeft = false;
            }
            else
            {
                RunLeft(spriteBatch, position, gameTime);
                isMovingLeft = true;
            }
        }
        public void RunRight(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            if (!hasLoopedOnce)
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheets);
            }
            else
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheets.Skip(2).Concat(spritesheets.Skip(2)).ToArray());
            }
            frames = spritesheets.Length;
        }
        public void RunLeft(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            if (!hasLoopedOnce)
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheetsRunLeft);
            }
            else
            {
                DrawMovement(spriteBatch, position, gameTime, spritesheetsRunLeft.Skip(2).Concat(spritesheetsRunLeft.Skip(2)).ToArray());
            }
            frames = spritesheetsRunLeft.Length;
        }

        public void DrawMovement(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] sprites)
        {
            if (sprites != null)
            {
                var currentSprite = sprites[currentTextureIndex];
                if (spriteBatch != null) spriteBatch.Draw(currentSprite, position, Color.White);
            }
            if (gameTime != null) UpdateFrameTiming(gameTime);
        }

        private void DrawStopped(SpriteBatch spriteBatch, Vector2 position)
        {
            wasJumping = false;
            if (isMovingLeft)
            {
                spriteBatch.Draw(spritesheetsRunLeft[0], position, Color.White);
            }
            else
            {
                spriteBatch.Draw(spritesheets[0], position, Color.White);
            }
        }
        private void DrawBed(SpriteBatch spriteBatch, Vector2 position)
        {
            wasJumping = false;
            if (isMovingLeft)
            {
                spriteBatch.Draw(spritesheetsBend2[0], position, Color.White);
            }
            else
            {
                spriteBatch.Draw(spritesheetsBend[0], position, Color.White);
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
