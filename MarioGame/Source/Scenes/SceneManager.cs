using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    public class SceneManager : IDisposable
    {
        private Dictionary<string, IScene> _scenes = new();
        private IScene _currentScene;
        private SpriteData _spriteData;
        private bool _disposed;

        public SceneManager(SpriteData spriteData)
        {
            _spriteData = spriteData;
        }

        public void AddScene(string name, IScene scene)
        {
            _scenes[name] = scene;
        }

        public void ChangeScene(string name)
        {
            _currentScene?.Unload();
            _currentScene = _scenes[name];
            _currentScene.Load(_spriteData);
        }

        public void LoadScene(string name)
        {
            _currentScene = _scenes[name];
            _currentScene.Load(_spriteData);
        }

        public void DrawScene(GameTime gameTime)
        {
            if (_currentScene.GetSceneType() == "Menu")
            {
                _currentScene.Draw(_spriteData);
            }
            else
            {
                _currentScene.Draw(_spriteData, gameTime);
            }
        }

        public void UpdateScene(GameTime gameTime)
        {
            _currentScene?.Update(gameTime, this);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var scene in _scenes.Values)
                    {
                        if (scene is IDisposable disposableScene)
                        {
                            disposableScene.Dispose();
                        }
                    }
                }

                _disposed = true;
            }
        }

        ~SceneManager()
        {
            Dispose(false);
        }
    }
}
