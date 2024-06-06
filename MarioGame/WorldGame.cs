using System;

using SuperMarioBros.Source.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Utils.DataStructures;
using MarioGame;

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
            _levelScene = new LevelScene(LevelPath.Level1);
            _sceneManager.AddScene(SceneName.MainMenu, _menuScene);
            _sceneManager.AddScene(SceneName.Level1, _levelScene);
            _sceneManager.LoadScene(SceneName.MainMenu);
        }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _sceneManager.ChangeScene(SceneName.Level1);
            }
            _sceneManager.UpdateScene(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _sceneManager.DrawScene(gameTime);
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
