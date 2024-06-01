using System;
using System.Collections.Generic;

using MarioGame.Source.Scenes;

using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    public class LevelScene : IScene
    {
        private List<Entity> Entities { get; set; } = new();


        public void Load()
        {
            Console.WriteLine(Entities);
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
