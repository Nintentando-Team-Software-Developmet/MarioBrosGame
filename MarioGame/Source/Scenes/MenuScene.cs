using System;
using System.Collections.Generic;

using MarioGame.Source.Scenes;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Scenes
{
    public class MenuScene : IScene
    {
        private string Screen { get; set; } = "Screen";

        public void Load()
        {
            Console.WriteLine(Screen);
        }

        public void Unload()
        {
            Console.WriteLine(Screen);
        }

        public void Draw(SpriteData spriteData)
        {
            spriteData?.spriteBatch.Begin();

            // Dibuja el texto "Bienvenidos" en la posición (100, 100) con el color blanco
            spriteData?.spriteBatch.DrawString(spriteData.spriteFont, "Bienvenidos", new Vector2(100, 100), Color.White);

            spriteData?.spriteBatch.End();
        }
    }
}
