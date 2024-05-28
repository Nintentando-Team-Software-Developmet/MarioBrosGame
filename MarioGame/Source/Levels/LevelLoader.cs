using Components;
using Managers;

using Microsoft.Xna.Framework.Graphics;

using System;

namespace Levels
{
    public class LevelLoader
    {
        private EntityManager _entityManager;
        private ComponentManager _componentManager;

        public LevelLoader(EntityManager entityManager, ComponentManager componentManager)
        {
            _entityManager = entityManager;
            _componentManager = componentManager;
        }

        public void LoadLevel(LevelData levelData)
        {
            foreach (var entityData in levelData.Entities)
            {
                var entity = _entityManager.CreateEntity(entityData.Type);

                foreach (var componentData in entityData.Components)
                {
                    var component = CreateComponent(componentData);
                    _componentManager.AddComponent(entity.Id, component);
                }

                var transform = _componentManager.GetComponent<TransformComponent>(entity.Id);
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
                    var transform = new TransformComponent { Position = tileData.Position };
                    var sprite = new SpriteComponent { Texture = LoadTexture(tileData.TexturePath) };

                    _componentManager.AddComponent(tileEntity.Id, transform);
                    _componentManager.AddComponent(tileEntity.Id, sprite);
                }
            }
        }

        private Component CreateComponent(ComponentData componentData)
        {
            // Example implementation, you should expand this to cover all component types
            switch (componentData.Type)
            {
                case "TransformComponent":
                    return new TransformComponent
                    {
                        Position = componentData.Position,
                        Rotation = componentData.Rotation,
                        Scale = componentData.Scale
                    };
                case "SpriteComponent":
                    return new SpriteComponent
                    {
                        Texture = LoadTexture(componentData.TexturePath)
                    };
                // Add cases for other component types
                default:
                    throw new ArgumentException($"Unknown component type: {componentData.Type}");
            }
        }

        private Texture2D LoadTexture(string texturePath)
        {
            // Load the texture from the content pipeline or other source
            // This is a placeholder implementation
            return null;
        }
    }
}
