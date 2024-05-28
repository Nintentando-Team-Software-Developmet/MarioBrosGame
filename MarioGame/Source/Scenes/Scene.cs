using System.Collections.Generic;
using Entities;

namespace Scenes
{
    public class Scene
    {
        public List<Entity> Entities { get; private set; } = new();

        public void Load()
        {
            // Load entities and initialize scene
        }

        public void Unload()
        {
            // Clean up entities and scene
        }
    }
}
