using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Source.Scenes;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros
{
    public class WorldGame : IDisposable
    {
        private SceneManager _sceneManager;
        private bool _disposed;
        private MenuScene _menuScene;
        private LevelScene _levelScene;

        public WorldGame(SpriteData spriteData)
        {
            _sceneManager = new SceneManager(spriteData);
        }

        public void Initialize()
        {
            _menuScene = new MenuScene();
            _levelScene = new LevelScene();

            _sceneManager.AddScene("Menu", _menuScene);
            _sceneManager.AddScene("Level1", _levelScene);

            _sceneManager.LoadScene("Menu");
        }

        public void Update(GameTime gameTime)
        {
            // Here you can add logic to transition from menu to Level1
            // For example, by checking a key press or a button click
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _sceneManager.ChangeScene("Level1");
            }
        }

        public void Draw()
        {
            _sceneManager.DrawScene();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _sceneManager?.Dispose();
                _menuScene?.Dispose();
                _levelScene?.Dispose();
            }

            _disposed = true;
        }
    }
}
