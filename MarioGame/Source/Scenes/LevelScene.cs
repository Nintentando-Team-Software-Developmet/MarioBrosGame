using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    /*
     * Represents a scene of a game level.
     * Implements the IScene interface for managing game scenes.
     * Also implements IDisposable for safe resource disposal.
     */
    public class LevelScene : IScene, IDisposable
    {
        private List<Entity> Entities { get; set; } = new();
        private List<BaseSystem> Systems { get; set; } = new();
        private bool _disposed;
        private Dictionary<Vector2, int> _tilemap;
        private Camera _camera;
        private Dictionary<int, Texture2D> _spriteMap;
        private int _levelHeight;
        private const int TileSize = 64;

        /*
         * Loads resources and initializes entities for the level scene.
         *
         * Parameters:
         *   spriteData: SpriteData object containing content manager for loading resources.
         *               If null, no resources will be loaded.
         */
        public void Load(SpriteData spriteData)
        {
            if (spriteData != null)
            {
                _spriteMap = new Dictionary<int, Texture2D>
                {
                    { 1, Sprites.StoneBlockBrown },
                };
                _camera = new Camera(spriteData.graphics.GraphicsDevice.Viewport, 13824, 720);
            }
            // Load player entity
            var playerTextures = new Texture2D[]
            {
                Sprites.BigStop,
                Sprites.BigRunLeft,
                Sprites.BigWalk1,
                Sprites.BigWalk2,
                Sprites.BigWalk3,

                Sprites.BigStopLeft,
                Sprites.BigRun,
                Sprites.BigWalk1Left,
                Sprites.BigWalk2Left,
                Sprites.BigWalk3Left,

                Sprites.BigBend,
                Sprites.BigBendLeft,

                Sprites.BigJumpBack,
                Sprites.BigJumpBackLeft

            };
            var player = new PlayerEntity(playerTextures, new Vector2(100, 517));
            Entities.Add(player);

            Systems.Add(new InputSystem());
            Systems.Add(new MovementSystem());
            if (spriteData != null) Systems.Add(new MarioAnimationSystem(spriteData.spriteBatch));

            _tilemap = LoadMap("../../../Data/level-surface.json");
            Systems.Add(new CollisionSystem(_tilemap,_levelHeight));

        }

        /*
         * Unloads resources and performs cleanup operations for the level scene.
         * This method is called when the scene is being unloaded or switched.
         * It prints the current entities information to the console.
         */
        public void Unload()
        {
            //Console.WriteLine("Unloading LevelScene. Entities: " + Entities.Count);
            Entities.Clear();
        }

/*
         * Updates the level scene.
         * This method updates all entities in the scene and processes systems.
         *
         * Parameters:
         *   gameTime: GameTime object containing timing information.
         */
        public void Update(GameTime gameTime, SceneManager sceneManager)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }

            var player = Entities.Find(e => e is PlayerEntity);
            if (player != null)
            {
                var positionComponent = player.GetComponent<PositionComponent>();
                if (positionComponent != null)
                {
                    _camera.Follow(positionComponent.Position);
                }
            }
        }

        /*
         * Draws the level scene on the screen.
         * This method clears the graphics device, then draws all entities in the scene.
         */
        public void Draw(SpriteData spriteData, GameTime gameTime)
        {
            if (spriteData == null) return;

            spriteData.graphics.GraphicsDevice.Clear(new Color(121, 177, 249));

            spriteData.spriteBatch.Begin(transformMatrix: _camera.Transform);

            foreach (var item in _tilemap)
            {
                Rectangle dest = new Rectangle(
                    (int)item.Key.X * TileSize,
                    (int)item.Key.Y * TileSize,
                    TileSize,
                    TileSize
                );

                int index = item.Value;
                if (_spriteMap.ContainsKey(index))
                {
                    Texture2D texture = _spriteMap[index];
                    spriteData.spriteBatch.Draw(texture, dest, Color.White);
                }
            }

            // Draw entities using the AnimationSystem
            foreach (var system in Systems)
            {
                if (system is IRenderableSystem renderableSystem)
                {
                    renderableSystem.Draw(gameTime, Entities);
                }
            }
            spriteData.spriteBatch.End();
        }

        public SceneType GetSceneType()
        {
            return SceneType.Level;
        }

        public void Draw(SpriteData spriteData)
        {
            throw new NotImplementedException();
        }

        /*
         * Performs cleanup operations and releases resources.
         * This method is called to dispose of the LevelScene object.
         */
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /*
         * Releases managed resources if disposing is true.
         */
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Dispose of any disposable objects here
                Entities.Clear();
            }

            _disposed = true;
        }

        private Dictionary<Vector2, int> LoadMap(string filepath)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                string jsonContent = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(jsonContent);

                JArray layers = (JArray)jsonObject["layers"];
                JObject layer = (JObject)layers[0];
                JArray data = (JArray)layer["data"];

                int width = (int)jsonObject["width"];
                int height = (int)jsonObject["height"];
                _levelHeight = height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int value = (int)data[y * width + x];
                        if (value > 0)
                        {
                            result[new Vector2(x, y)] = value;
                        }
                    }
                }
            }

            return result;
        }
    }
}
