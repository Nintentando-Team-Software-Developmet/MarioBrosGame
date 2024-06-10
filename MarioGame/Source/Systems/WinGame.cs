using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems;

public class WinGame
{

    private bool isActive { get; set; }
    private Texture2D[] spritesheets { get; set; }

    private int currentTextureIndex { get; set; }
    private bool hasLoopedOnce { get; set; }
    private MarioAnimationSystem marioAnimationSystem{ get; set; }
    public Vector2 initialPosition { get; set; }
    private Vector2 spritePosition { get; set; }
    private Vector2 targetPosition { get; set; }
    private float lerpAmount { get; set; }

    public void DrawWinGame(GameTime gameTime, IEnumerable<Entity> entities, SpriteBatch spriteBatch, bool colition, Vector2 jumpEndY, float currentYPosition)
    {
        if (entities != null)
        {
            var playerEntities = entities.Where(e => e is WinFlagEntity);

            foreach (var entity in playerEntities)
            {
                var playerAnimation = entity.GetComponent<AnimationComponent>();
                var position = entity.GetComponent<PositionComponent>();
                initialPosition = position.Position;

                spritesheets = new Texture2D[] { playerAnimation.Textures[0], playerAnimation.Textures[1] };


                if (spritePosition == Vector2.Zero)
                {
                    spritePosition = new Vector2(300, 70);
                    targetPosition = spritePosition;
                }


                if (colition)
                {
                    targetPosition = new Vector2(300, currentYPosition + 130);
                }

                // Interpola gradualmente hacia la nueva posición
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
}
