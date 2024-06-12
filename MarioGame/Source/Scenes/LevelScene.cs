using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using MarioGame;
using MarioGame.Utils.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.SceneCommonData;

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
        private MapGame map;
        private LevelData _levelData;
        private bool _disposed;

        private int _score;
        private HighScoreManager _highScoreManager = new HighScoreManager();
        public Matrix Camera => (Matrix)Entities.FirstOrDefault(
            e => e.HasComponent<CameraComponent>())?.GetComponent<CameraComponent>().Transform;

        /*
         * Constructs a new LevelScene object.
         * This constructor initializes the level scene with the specified path to the scene data.
         *
         * Parameters:
         *   pathScene: A string representing the path to the scene data.
         */
        public LevelScene(string pathScene)
        {
            _score = 0;
            string json = File.ReadAllText(pathScene);
            _levelData = JsonConvert.DeserializeObject<LevelData>(json);

        }

        /*
         * Loads resources and initializes the level scene.
         * This method is called when the scene is being loaded or switched.
         * It creates the map and systems for the level scene.
         */
        public void Load(SpriteData spriteData)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));
            map = new MapGame(_levelData.pathMap, spriteData);
            LoadEntities();
            //TODO: Refactor
            Systems.Add(new InputSystem());
            Systems.Add(new MovementSystem());
            Systems.Add(new MarioAnimationSystem(spriteData.spriteBatch));
            Systems.Add(new EnemyAnimationSystem(spriteData.spriteBatch));
            Systems.Add(new GravitySystem());
            Systems.Add(new CollisionSystem(map.Tilemap, map.LevelHeight));
            Systems.Add(new CameraSystem());
        }

        /*
         * Loads entities from the level data.
         * This method creates entities based on the level data and adds them to the scene.
         */
        private void LoadEntities()
        {
            foreach (var entity in _levelData.entities)
            {
                Entities.Add(EntityFactory.CreateEntity(entity));
            }
        }

        /*
         * Unloads resources and performs cleanup operations for the level scene.
         * This method is called when the scene is being unloaded or switched.
         * It prints the current entities information to the console.
         */
        public void Unload()
        {
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
            WorldGame.ProgressDataManager.Update(gameTime);
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }

        /*
         * Draws the level scene on the screen.
         * This method clears the graphics device, then draws all entities in the scene.
         */
        public void Draw(SpriteData spriteData, GameTime gameTime)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));
            spriteData.graphics.GraphicsDevice.Clear(new Color(121, 177, 249));
            spriteData.spriteBatch.Begin(transformMatrix: Camera);
            map.Draw(spriteData);
            DrawEntities(gameTime);
            CommonRenders.DrawProgressData(spriteData, WorldGame.ProgressDataManager.Data.Score,
                                            WorldGame.ProgressDataManager.Data.Coins,
                                            "1-1",
                                            WorldGame.ProgressDataManager.Data.Time);
            spriteData.spriteBatch.End();
        }

        /*
         * Draws all entities in the level scene.
         * This method calls the Draw method of all renderable systems in the scene.
         *
         * Parameters:
         *   gameTime: GameTime object containing timing information.
         */
        private void DrawEntities(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                if (system is IRenderableSystem renderableSystem)
                {
                    renderableSystem.Draw(gameTime, Entities);
                }
            }
        }

        /*
         * Gets the type of the scene.
         * This method returns the SceneType.Level enum value.
         */
        public SceneType GetSceneType()
        {
            return SceneType.Level;
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
                Entities.Clear();
            }
            _disposed = true;
        }
        public void UpdateHighScore()
        {
            _highScoreManager.UpdateHighScore(_score);
        }

        public int GetHighScore()
        {
            return _highScoreManager.GetHighScore();
        }
    }
}
