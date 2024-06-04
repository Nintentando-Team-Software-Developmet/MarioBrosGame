using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SuperMarioBros.Source.Levels
{
    public class LevelData
    {
        private readonly List<EntityData> _entities = new();
        private readonly List<TileData> _tiles = new();

        public IReadOnlyList<EntityData> Entities => _entities.AsReadOnly();
        public IReadOnlyList<TileData> Tiles => _tiles.AsReadOnly();

        public void AddEntity(EntityData entity) => _entities.Add(entity);
        public void AddTile(TileData tile) => _tiles.Add(tile);
    }

    public class EntityData
    {
        private readonly List<ComponentData> _components = new();

        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public IReadOnlyList<ComponentData> Components => _components.AsReadOnly();
    }

    public class ComponentData
    {
        public string Type { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public string TexturePath { get; set; }
    }

    public class TileData
    {
        public Vector2 Position { get; set; }
        public string TexturePath { get; set; }
    }
}
