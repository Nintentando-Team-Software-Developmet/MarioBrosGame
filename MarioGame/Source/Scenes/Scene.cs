using System;
using System.Collections.Generic;
using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Scenes
{
    public class Scene
    {
        private List<Entity> Entities { get;  set; } = new();

        public void Load()
        {
            Console.WriteLine(Entities);
        }

        public void Unload()
        {
            Console.WriteLine(Entities);
        }
    }
}
