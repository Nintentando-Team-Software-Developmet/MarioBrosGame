using System;

using Microsoft.Xna.Framework;

using SuperMarioBros;
using SuperMarioBros.Source.Scenes;
using SuperMarioBros.Source.Systems;


namespace MarioGame
{
    /**
    * This class is responsible for managing the game world.
    * It contains a SystemManager and a SceneManager.
    */
    public class WorldGame : IDisposable
    {
        private SystemManager systemManager;
        private GameDataSystem _gameDataSystem;
        private SceneManager sceneManager;
        private bool disposed;

        public WorldGame(SpriteData spriteData)
        {
            systemManager = new SystemManager();
            sceneManager = new SceneManager(spriteData);
            _gameDataSystem = new GameDataSystem(0123, 0, "1-1", 300);
        }

        public void Initialize()
        {
            using (var menuScene = new GameOverScene(_gameDataSystem))
            {
                sceneManager.AddScene("Menu", menuScene);
                sceneManager.setScene("Menu");
                sceneManager.LoadScene("Menu");
            }
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime != null) _gameDataSystem.Time -= gameTime.ElapsedGameTime.TotalSeconds;
            systemManager.Update();
        }

        public void Draw()
        {
            sceneManager.DrawScene();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                systemManager?.Dispose();
                sceneManager?.Dispose();
            }

            disposed = true;
        }
    }
}
