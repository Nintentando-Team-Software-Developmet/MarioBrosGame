using System;
using System.Collections.Generic;

using MarioGame;

using Microsoft.Xna.Framework;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    /// <summary>
    /// Manages the game scenes.
    /// </summary>
    public class SceneManager : IDisposable
    {
        private Dictionary<SceneName, IScene> _scenes = new();
        private IScene _currentScene;
        private SpriteData _spriteData;
        private bool _disposed;

        public SceneName CurrentSceneName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SceneManager class.
        /// </summary>
        /// <param name="spriteData">The sprite data.</param>
        public SceneManager(SpriteData spriteData)
        {
            _spriteData = spriteData;
        }

        /// <summary>
        /// Adds a new scene to the manager.
        /// </summary>
        /// <param name="name">The name of the scene.</param>
        /// <param name="scene">The scene to add.</param>
        public void AddScene(SceneName name, IScene scene)
        {
            _scenes[name] = scene;
        }

        /// <summary>
        /// Changes the current scene to the specified scene.
        /// </summary>
        /// <param name="name">The name of the scene to change to.</param>
        public void ChangeScene(SceneName name)
        {
            _currentScene?.Unload();
            _currentScene = _scenes[name];
            CurrentSceneName = name;
            _currentScene.Load(_spriteData);
        }

        /// <summary>
        /// Loads the specified scene.
        /// </summary>
        /// <param name="name">The name of the scene to load.</param>
        public void LoadScene(SceneName name)
        {
            _currentScene = _scenes[name];
            CurrentSceneName = name;
            _currentScene.Load(_spriteData);
        }

        /// <summary>
        /// Draws the current scene.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void DrawScene(GameTime gameTime)
        {
            _currentScene?.Draw(_spriteData, gameTime);
        }

        /// <summary>
        /// Updates the current scene.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        public void UpdateScene(GameTime gameTime)
        {
            _currentScene?.Update(gameTime, this);
        }

        /// <summary>
        /// Releases all resource used by the SceneManager object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the SceneManager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                foreach (var scene in _scenes.Values)
                {
                    scene.Unload();
                    (scene as IDisposable)?.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Finalizes an instance of the SceneManager class.
        /// </summary>
        ~SceneManager()
        {
            Dispose(false);
        }
    }
}
