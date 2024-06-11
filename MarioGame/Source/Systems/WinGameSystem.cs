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

    private const float movementSpeed = 30f;
    private const float distanceToTravel = 200f;
    private float currentXPosition { get; set; }
    private float currentYPosition { get; set; }
    private float currentMoreYPosition { get; set; }
    private float distanceTraveled { get; set; }
    private bool isDrawing { get; set; }
    private bool hasJumped;
    public void DrawWinGame(GameTime gameTime, IEnumerable<Entity> entities, SpriteBatch spriteBatch, bool colition)
    {
        if (entities != null)
        {
            var playerEntities = entities.Where(e =>
                e.HasComponent<AnimationComponent>() &&
                e.HasComponent<PositionComponent>() &&
                e.HasComponent<WinGameComponent>()
            );

            foreach (var entity in playerEntities)
            {
                var playerAnimation = entity.GetComponent<AnimationComponent>();
                var position = entity.GetComponent<PositionComponent>();
                initialPosition = position.Position;
                spritesheets = new Texture2D[] { playerAnimation.Textures[0], playerAnimation.Textures[1] };
                if (spritePosition == Vector2.Zero)
                {
                    spritePosition = new Vector2(2800, 80);
                    targetPosition = spritePosition;
                }
                if (colition)
                {
                    targetPosition = new Vector2(2800, currentYPosition + 130);
                }
                if (gameTime != null)
                {
                    float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    lerpAmount += deltaTime * 1;
                }
                spritePosition = Vector2.Lerp(spritePosition, targetPosition, lerpAmount);
                if (spriteBatch != null)
                {
                    spriteBatch.Draw(spritesheets[1], spritePosition, Color.White);
                    spriteBatch.Draw(spritesheets[0], position.Position, Color.White);
                }
                if (lerpAmount >= 1.0f)
                {
                    lerpAmount = 0.0f;
                }
            }
        }
    }

public void DrawWinMario(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, Texture2D[] spritesheetsWin,
    Vector2 jumpEndY, Texture2D[] spritesheetsWinLeft, Texture2D[] spritesheetsWinRun)
{
    const float frameDuration = 1f;

    if (spritesheetsWin == null || spritesheetsWinLeft == null || spritesheetsWinRun == null) return;

    int numSpritesWinRun = spritesheetsWinRun.Length;
    int numSpritesWin = spritesheetsWin.Length;
    int numSpritesWinLeft = spritesheetsWinLeft.Length;

    if (currentXPosition == 0f || currentYPosition == 0f)
    {
        currentXPosition = initialPosition.X;
        currentYPosition = jumpEndY.Y;
        currentMoreYPosition = jumpEndY.Y;
        isDrawing = true; // Inicializamos el control de dibujo
    }

    if (gameTime != null && isDrawing)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        float yMovement = movementSpeed * deltaTime;
        float xMovement = movementSpeed * deltaTime;

        if (currentYPosition < position.Y - 120)
        {
            currentYPosition += yMovement;
            currentMoreYPosition = currentYPosition;
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
                currentXPosition = initialPosition.X + 60;
            }
            if (!hasJumped)
            {
                float timeElapsed2 = (float)gameTime.TotalGameTime.TotalSeconds;
                int currentFrameIndex2 = (int)(timeElapsed2 / frameDuration) % numSpritesWinLeft;

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
                    spriteBatch.Draw(spritesheetsWinLeft[currentFrameIndex2], new Vector2(currentXPosition, currentMoreYPosition), Color.White);
                }
            }
            else
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
                    isDrawing = false; // Detenemos el dibujo una vez que se completa el movimiento en X
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
