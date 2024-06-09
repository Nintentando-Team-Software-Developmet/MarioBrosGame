using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Systems;

public class WinGame: BaseSystem, IRenderableSystem
{
    private readonly SpriteBatch _spriteBatch;
    private bool isActive { get; set; }
    private Texture2D[] spritesheets { get; set; }

    private int currentTextureIndex { get; set; }
    private bool hasLoopedOnce { get; set; }
    public WinGame(SpriteBatch spriteBatch)
    {
        isActive = false;
        _spriteBatch = spriteBatch;

    }
    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        if (entities != null)
        {
            var playerEntities = entities.Where(e => e is WinFlagEntity);

            foreach (var entity in playerEntities)
            {
                var animation = entity.GetComponent<AnimationComponent>();

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
                }
            }
        }
    }

    public void Draw(GameTime gameTime, IEnumerable<Entity> entities)
    {
        if (entities != null)
        {
            var playerEntities = entities.Where(e => e is WinFlagEntity);


            foreach (var entity in playerEntities)
            {

                if (entity is PlayerEntity)
                {
                    // La entidad es un PlayerEntity, por lo tanto, obtenemos su AnimationComponent
                    var playerAnimation2 = entity.GetComponent<AnimationComponent>();
                    Console.WriteLine(playerAnimation2.Textures.Count);
                    // Resto del código para la animación del jugador...
                }
                else if (entity is WinFlagEntity)
                {
                    var winAnimation = entity.GetComponent<AnimationComponent>();
                    Console.WriteLine(winAnimation.Textures.Count);
                    // Resto del código para la animación de la bandera de victoria...
                }

                var playerAnimation = entity.GetComponent<AnimationComponent>();
                var position = entity.GetComponent<PositionComponent>();

                spritesheets = new Texture2D[] { playerAnimation.Textures[0]};
                _spriteBatch.Draw(spritesheets[0], position.Position, Color.White);

            }
        }
    }

}
