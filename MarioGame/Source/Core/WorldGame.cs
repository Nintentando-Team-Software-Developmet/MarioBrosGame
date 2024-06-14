using System;
using System.Diagnostics.CodeAnalysis;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using nkast.Aether.Physics2D.Dynamics;


using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Source.Scenes;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros
{
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
    public class WorldGame : IDisposable
    {
        private SceneManager _sceneManager;
        private EventDispatcher _eventDispatcher;
        private ProgressDataManager _progressDataManager;
        private bool _disposed;
        private bool _enterPressed;

        public WorldGame(SpriteData spriteData)
        {
            _eventDispatcher = EventDispatcher.Instance;
            _sceneManager = new SceneManager(spriteData);
            _progressDataManager = new ProgressDataManager();

            _sceneManager.AddScene(SceneName.MainMenu, new MenuScene(_progressDataManager));
            _sceneManager.AddScene(SceneName.Level1, new LevelScene(LevelPath.Level1, _progressDataManager));
            _sceneManager.AddScene(SceneName.GameOver, new GameOverScene(_progressDataManager));
            _sceneManager.AddScene(SceneName.Lives, new LivesScene(_progressDataManager));

            _sceneManager.LoadScene(SceneName.MainMenu);
        }


        public void Update(GameTime gameTime)
        {
            HandleInput();
            _sceneManager.UpdateScene(gameTime);
        }

        private void HandleInput()
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                if (!_enterPressed)
                {
                    _enterPressed = true;
                    if (_sceneManager.CurrentSceneName == SceneName.MainMenu)
                    {
                        _sceneManager.ChangeScene(SceneName.Level1);
                    }
                }
            }
            else
            {
                _enterPressed = false;
            }
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
            if (_disposed) return;
            if (disposing)
            {
                _sceneManager?.Dispose();
            }
            _disposed = true;
        }
    }
}
