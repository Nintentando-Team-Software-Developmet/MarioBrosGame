using System;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


using SuperMarioBros.Source.Scenes;
using SuperMarioBros.Utils.DataStructures;

using SuperMarioBros.Source.Managers;

namespace SuperMarioBros
{
    public class WorldGame : IDisposable
    {
        private SceneManager _sceneManager;
        private MenuScene _menuScene;
        private LevelScene _levelScene;
        private GameOverScene _gameOverScene;
        private LivesScene _livesScene;
        private ProgressDataManager _progressDataManager { get; }
        private bool _disposed;


        public WorldGame(SpriteData spriteData)
        {
            _sceneManager = new SceneManager(spriteData);
            _progressDataManager = new ProgressDataManager();
        }

        public void Initialize()
        {
            _menuScene = new MenuScene(_progressDataManager);
            _levelScene = new LevelScene(LevelPath.Level1, _progressDataManager);
            _gameOverScene = new GameOverScene(_progressDataManager);
            _livesScene = new LivesScene(_progressDataManager);

            _sceneManager.AddScene(SceneName.MainMenu, _menuScene);
            _sceneManager.AddScene(SceneName.Level1, _levelScene);
            _sceneManager.AddScene(SceneName.GameOver, _gameOverScene);
            _sceneManager.AddScene(SceneName.Lives, _livesScene);

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
                _gameOverScene?.Dispose();
                _livesScene?.Dispose();
            }

            _disposed = true;
        }
    }
}
