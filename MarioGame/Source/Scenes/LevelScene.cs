using System;
using System.Collections.Generic;

using MarioGame.Source.Scenes;

using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Scenes
{
    public class LevelScene : IScene
    {
        private List<Entity> Entities { get; set; } = new();

        public void Load(SpriteData spriteData)
        {
            throw new NotImplementedException();
        }

        public void Unload()
        {
            Console.WriteLine(Entities);
        }

        public void Draw(SpriteData spriteData)
        {

            Console.WriteLine(Entities);
        }
    }
}
