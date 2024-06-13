using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems;

public class WinGameSystem
{
    private Texture2D[] spritesheets;
    public Vector2 InitialPosition { get; set; }
    private Vector2 spritePosition;
    private Vector2 targetPosition;

    private const float movementSpeed = 90f;
    private const float distanceToTravel = 200f;
    private float currentXPosition;
    private float currentYPosition;
    private float currentYPosition2;
    private float currentMoreYPosition;
    private float distanceTraveled;
    private bool isDrawing;
    private bool hasJumped;
    private float timeSinceLanding;
    private float elapsedTime;

    private bool hasFinishedDescending;

    public void DrawWinGame(IEnumerable<Entity> entities, SpriteBatch spriteBatch, bool collision)
    {
        if (entities == null || spriteBatch == null)
        {
            return;
        }

        var playerEntitiesWin = entities.Where(e =>
            e.HasComponent<AnimationComponent>() &&
            e.HasComponent<PositionComponent>() &&
            e.HasComponent<WinGameComponent>()
        );

        foreach (var entity in playerEntitiesWin)
        {
            var playerAnimation = entity.GetComponent<AnimationComponent>();
            var position = entity.GetComponent<PositionComponent>();

            InitialPosition = position.Position;
            spritesheets = new Texture2D[] { playerAnimation.Textures[0], playerAnimation.Textures[1] };

            if (spritePosition == Vector2.Zero)
            {
                spritePosition = new Vector2(2800, 10);
                targetPosition = spritePosition;
                currentYPosition2 = position.Position.Y +20 ;
            }


            if (collision)
            {
                if (!hasFinishedDescending && currentYPosition2 <= 500)
                {
                    if (elapsedTime <= 2f)
                    {
                        currentYPosition2 += 2;
                        elapsedTime = 2f;
                    }
                }
                else
                {
                    hasFinishedDescending = true;
                }

            }
            targetPosition = new Vector2(2800, currentYPosition2 );

            spriteBatch.Draw(spritesheets[1], targetPosition, Color.White);
            spriteBatch.Draw(spritesheets[0], position.Position, Color.White);
        }
    }

    public void DrawWinMario(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] spritesheetsWin,
        Vector2 jumpEndY, Texture2D[] spritesheetsWinLeft, Texture2D[] spritesheetsWinRun)
    {
        const float frameDuration = 0.1f;
        const float waitBeforeJump = 0.2f;

        if (spritesheetsWin == null || spritesheetsWinLeft == null || spritesheetsWinRun == null || spriteBatch == null)
        {
            return;
        }

        int numSpritesWinRun = spritesheetsWinRun.Length;
        int numSpritesWin = spritesheetsWin.Length;

        if (gameTime != null)
        {
            float timeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;
            int currentFrameIndex = (int)(timeElapsed / frameDuration) % numSpritesWin;

            if (currentXPosition == 0f || currentYPosition == 0f)
            {
                currentXPosition = InitialPosition.X;
                currentYPosition = jumpEndY.Y;
                currentMoreYPosition = jumpEndY.Y;
                isDrawing = true;
            }

            if (isDrawing)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float yMovement = movementSpeed * deltaTime;
                float xMovement = movementSpeed * deltaTime;

                if (!hasFinishedDescending)
                {
                    if (currentYPosition < position.Y - 120)
                    {
                        currentYPosition += yMovement;
                        currentMoreYPosition = currentYPosition;
                    }

                    spriteBatch.Draw(spritesheetsWin[currentFrameIndex], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                }
                else
                {
                    if (distanceTraveled == 0)
                    {
                        spriteBatch.Draw(spritesheetsWinLeft[0], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                        currentXPosition = InitialPosition.X + 60;
                        timeSinceLanding += deltaTime;
                    }

                    if (timeSinceLanding >= waitBeforeJump && !hasJumped)
                    {
                        const float jumpHeight = 50f;
                        const float jumpDistance = 30f;

                        float jumpProgress = Math.Min(distanceTraveled / jumpDistance, 1.0f);

                        currentXPosition = InitialPosition.X + 100 + jumpDistance * jumpProgress;

                        float startY = currentYPosition;
                        float endY = position.Y;
                        currentMoreYPosition = startY + (jumpHeight * 7 * (jumpProgress - 0.4f) * (jumpProgress - 0.4f) - jumpHeight);

                        if (jumpProgress >= 1.0f)
                        {
                            currentMoreYPosition = endY;
                            hasJumped = true;
                            distanceTraveled = 0;
                        }

                        distanceTraveled += xMovement;
                        spriteBatch.Draw(spritesheetsWinLeft[1], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                    }
                    else if (hasJumped)
                    {
                        if (distanceTraveled < distanceToTravel)
                        {
                            currentXPosition += xMovement;
                            distanceTraveled += xMovement;
                        }
                        else
                        {
                            isDrawing = false;
                        }

                        if (isDrawing)
                        {
                            spriteBatch.Draw(spritesheetsWinRun[currentFrameIndex], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                        }
                    }
                }
            }
        }
    }
}

