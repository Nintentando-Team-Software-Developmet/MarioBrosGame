using System;
using System.IO;
using System.Linq;

using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;

using Newtonsoft.Json;

using nkast.Aether.Physics2D.Dynamics;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Scenes
{
    public class SecretLevelScene : IScene, IDisposable
    {

        private SecretLevelMapGame map;
        private World physicsWorld;
        private LevelData _levelData;
        private bool _disposed;
        private ProgressDataManager _progressDataManager;

        public SecretLevelScene(string pathScene, ProgressDataManager progressDataManager)
        {
            string json = File.ReadAllText(pathScene);
            _levelData = JsonConvert.DeserializeObject<LevelData>(json);
            _progressDataManager = progressDataManager;
            physicsWorld = new World(new AetherVector2(0, 20f));
        }

        public void Load(SpriteData spriteData)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));
            map = new SecretLevelMapGame(_levelData.pathMap, spriteData, physicsWorld);
        }


        public void Unload()
        {
            foreach (var body in physicsWorld.BodyList.ToList())
            {
                physicsWorld.Remove(body);
            }
            _progressDataManager.ResetTime();
        }

        public void Update(GameTime gameTime, SceneManager sceneManager)
        {
            if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
            physicsWorld.Step((float)gameTime?.ElapsedGameTime.TotalSeconds);

        }

        public void Draw(SpriteData spriteData, GameTime gameTime)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));

            spriteData.spriteBatch.Begin();

            spriteData.graphics.GraphicsDevice.Clear(Color.Black);
            map.Draw(spriteData);

            using (var deb = new DebuggerColliders(physicsWorld, spriteData))
            {
                deb.DrawColliders();
            }

            spriteData.spriteBatch.End();
        }

        public SceneType GetSceneType()
        {
            return SceneType.SecretLevel;
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
            _disposed = true;
        }
    }
}
