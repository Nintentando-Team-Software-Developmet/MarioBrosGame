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
    private Texture2D[] spritesheets { get; set; }
   public Vector2 initialPosition { get; set; }
    private Vector2 spritePosition { get; set; }
    private Vector2 targetPosition { get; set; }
    private float lerpAmount { get; set; }

    private const float movementSpeed = 60f;
    private const float distanceToTravel = 200f;
    private float currentXPosition { get; set; }
    private float currentYPosition { get; set; }
    private float currentYPosition2 { get; set; }
    private float currentMoreYPosition { get; set; }
    private float distanceTraveled { get; set; }
    private bool isDrawing { get; set; }
    private bool hasJumped;
    private float timeSinceLanding { get; set; }
    private bool checkFlaw { get; set; }
    float elapsedTime { get; set; }

    bool hasFinishedDescending { get; set; }

    public void DrawWinGame(GameTime gameTime, IEnumerable<Entity> entities, SpriteBatch spriteBatch, bool colition)
    {
        if (entities != null)
        {
            var playerEntitiesWin = entities.Where(e =>
                e.HasComponent<AnimationComponent>() &&
                e.HasComponent<PositionComponent>() &&
                e.HasComponent<WinGameComponent>()
            );

            foreach (var entity in playerEntitiesWin)
            {
                var playerAnimation = entity.GetComponent<AnimationComponent>();
                var position = entity.GetComponent<PositionComponent>();

                initialPosition = position.Position;
                spritesheets = new Texture2D[] { playerAnimation.Textures[0], playerAnimation.Textures[1] };

                if (spritePosition == Vector2.Zero)
                {
                    spritePosition = new Vector2(2800, 80);
                    targetPosition = spritePosition;
                    currentYPosition2 = position.Position.Y - 313;
                }

                if (!hasFinishedDescending && currentYPosition2 <= 370)
                {
                    if (elapsedTime < 3f)
                    {
                        currentYPosition2 += 1;
                        elapsedTime = 1f;
                    }
                }
                else
                {
                    hasFinishedDescending = true;
                }
                if (colition)
                {
                    targetPosition = new Vector2(2800, currentYPosition2 + 130);
                }

                if (spriteBatch != null)
                {
                    spriteBatch.Draw(spritesheets[1], targetPosition, Color.White);
                    spriteBatch.Draw(spritesheets[0], position.Position, Color.White);
                }
            }
        }
    }
public void DrawWinMario(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] spritesheetsWin,
    Vector2 jumpEndY, Texture2D[] spritesheetsWinLeft, Texture2D[] spritesheetsWinRun)
{
    const float frameDuration = 1f;

    const float waitBeforeJump = 0.2f;

    if (spritesheetsWin == null || spritesheetsWinLeft == null || spritesheetsWinRun == null) return;

    int numSpritesWinRun = spritesheetsWinRun.Length;
    int numSpritesWin = spritesheetsWin.Length;


    if (currentXPosition == 0f || currentYPosition == 0f)
    {
        currentXPosition = initialPosition.X;
        currentYPosition = jumpEndY.Y;
        currentMoreYPosition = jumpEndY.Y;
        isDrawing = true;
    }

    if (gameTime != null && isDrawing)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float yMovement = movementSpeed * deltaTime;
        float xMovement = movementSpeed * deltaTime;

        if (!hasFinishedDescending)
        {
            if (currentYPosition < position.Y - 120 )
            {
                currentYPosition += yMovement;
                currentMoreYPosition = currentYPosition;

            }
            float timeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;
            int currentFrameIndex = (int)(timeElapsed / frameDuration) % numSpritesWin;

            if (spriteBatch != null)
            {
                spriteBatch.Draw(spritesheetsWin[currentFrameIndex], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
            }
        }
        else
        {
            if (distanceTraveled == 0)
            {
                if (spriteBatch != null)
                {
                    spriteBatch.Draw(spritesheetsWinLeft[0], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                }
                currentXPosition = initialPosition.X + 60;
                timeSinceLanding += deltaTime;
            }


            if (timeSinceLanding >= waitBeforeJump && !hasJumped)
            {

                const float jumpHeight = 50f;
                const float jumpDistance = 30f;

                float jumpProgress = Math.Min(distanceTraveled / jumpDistance, 1.0f);

                currentXPosition = initialPosition.X + 100 + jumpDistance * jumpProgress;

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
                if (spriteBatch != null)
                {
                    spriteBatch.Draw(spritesheetsWinLeft[1], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                }
            }
            else if (hasJumped)
            {
                float timeElapsed = (float)gameTime.TotalGameTime.TotalSeconds;
                int currentFrameIndex = (int)(timeElapsed / frameDuration) % numSpritesWinRun;

                if (distanceTraveled < distanceToTravel)
                {
                    currentXPosition += xMovement;
                    distanceTraveled += xMovement;
                }
                else
                {
                    isDrawing = false;
                }

                if (spriteBatch != null && isDrawing)
                {
                    spriteBatch.Draw(spritesheetsWinRun[currentFrameIndex], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                }
            }
        }
    }
}

}
