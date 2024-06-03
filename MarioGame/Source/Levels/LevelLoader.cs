using System;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Managers;

namespace SuperMarioBros.Source.Levels
{
    public class LevelLoader
    {
        private readonly EntityManager _entityManager;
        private readonly ComponentManager _componentManager;

        public LevelLoader(EntityManager entityManager, ComponentManager componentManager)
        {
            _entityManager = entityManager ?? throw new ArgumentNullException(nameof(entityManager));
            _componentManager = componentManager ?? throw new ArgumentNullException(nameof(componentManager));
        }

        public void LoadLevel(LevelData levelData)
        {
            if (levelData == null) throw new ArgumentNullException(nameof(levelData));

            foreach (var entityData in levelData.Entities)
            {
                var entity = _entityManager.CreateEntity(entityData.Type);

                foreach (var componentData in entityData.Components)
                {
                    var component = CreateComponent(componentData);
                    _componentManager.AddComponent(entity.Id, component);
                }

                var transform = _componentManager.GetComponent<TransformBaseComponent>(entity.Id);
                if (transform != null)
                {
                    transform.Position = entityData.Position;
                }
            }

            // Handle tile data if present
            if (levelData.Tiles != null)
            {
                foreach (var tileData in levelData.Tiles)
                {
                    var tileEntity = _entityManager.CreateEntity("Tile");
                    var transform = new TransformBaseComponent { Position = tileData.Position };
                    var sprite = new SpriteBaseComponent { Texture = LoadTexture(tileData.TexturePath) };

                    _componentManager.AddComponent(tileEntity.Id, transform);
                    _componentManager.AddComponent(tileEntity.Id, sprite);
                }
            }
        }

        private static BaseComponent CreateComponent(ComponentData componentData)
        {
            if (componentData == null) throw new ArgumentNullException(nameof(componentData));

            // Example implementation, you should expand this to cover all component types
            switch (componentData.Type)
            {
                case "TransformComponent":
                    return new TransformBaseComponent
                    {
                        Position = componentData.Position,
                        Rotation = componentData.Rotation,
                        Scale = componentData.Scale
                    };
                case "SpriteComponent":
                    return new SpriteBaseComponent
                    {
                        Texture = LoadTexture(componentData.TexturePath)
                    };
                // Add cases for other component types
                default:
                    throw new ArgumentException($"Unknown component type: {componentData.Type}");
            }
        }

        private static Texture2D LoadTexture(string texturePath)
        {
            if (string.IsNullOrEmpty(texturePath)) throw new ArgumentNullException(nameof(texturePath));

            // Load the texture from the content pipeline or other source
            // This is a placeholder implementation
            return null;
        }
    }
}
