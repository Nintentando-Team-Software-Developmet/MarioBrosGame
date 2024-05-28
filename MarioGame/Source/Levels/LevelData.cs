using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Levels
{
    public class LevelData
    {
        public List<EntityData> Entities { get; set; }
        public List<TileData> Tiles { get; set; }
    }

    public class EntityData
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public List<ComponentData> Components { get; set; }
    }

    public class ComponentData
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public string TexturePath { get; set; }
        // Add other component-specific fields as needed
    }

    public class TileData
    {
        public Vector2 Position { get; set; }
        public string TexturePath { get; set; }
    }
}
