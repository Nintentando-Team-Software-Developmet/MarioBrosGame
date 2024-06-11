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
    public void DrawWinGame(GameTime gameTime, IEnumerable<Entity> entities,SpriteBatch spriteBatch)
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
                Console.WriteLine(playerAnimation.Textures.Count);
                spritesheets = new Texture2D[] { playerAnimation.Textures[0]};
                if (spriteBatch != null) spriteBatch.Draw(spritesheets[0], position.Position, Color.White);
            }
        }
    }
}
