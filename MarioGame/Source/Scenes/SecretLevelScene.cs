using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

using Newtonsoft.Json;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Extensions;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.Maps;
using SuperMarioBros.Utils.SceneCommonData;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Source.Scenes
{
    public class SecretLevelScene : IScene, IDisposable
    {
        private List<Entity> Entities { get; set; } = new();
        private List<BaseSystem> Systems { get; set; } = new();
        private SecretLevelMapGame _map;
        private readonly World _physicsWorld;
        private readonly LevelData _levelData;
        private bool _disposed;
        private readonly ProgressDataManager _progressDataManager;
        private HashSet<string> LoadedEntities { get; }

        public SecretLevelScene(string pathScene, ProgressDataManager progressDataManager)
        {
            string json = File.ReadAllText(pathScene);
            _levelData = JsonConvert.DeserializeObject<LevelData>(json);
            _progressDataManager = progressDataManager;
            _physicsWorld = new World(new AetherVector2(0, 20f));
            LoadedEntities = new HashSet<string>();
        }

        public void Load(SpriteData spriteData)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));
            _map = new SecretLevelMapGame(_levelData.pathMap, _levelData.backgroundEntitiesPath, spriteData, _physicsWorld);
            LoadEntities();
            InitializeSystems(spriteData);
            MediaPlayer.Volume = 0.6f;
            MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/secret-level-song"));
            MediaPlayer.IsRepeating = true;
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.CoinCollected, "SoundEffects/coin_collected");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.Ducting, "SoundEffects/duct_entry");
        }

        private void InitializeSystems(SpriteData spriteData)
        {
            Systems.Add(new AnimationSystem(spriteData.spriteBatch));
            Systems.Add(new PlayerMovementSystem());
            Systems.Add(new PlayerSystem());
            Systems.Add(new CoinSystem(_progressDataManager));
            Systems.Add(new SoundEffectSystem());
        }

        private void LoadEntities()
        {
            var playerEntityData = _levelData.entities.FirstOrDefault(e => e.type == EntityType.PLAYER);
            if (playerEntityData != null)
            {
                Entities.Add(EntityFactory.CreateEntity(playerEntityData, _physicsWorld, _progressDataManager.Data));
                LoadedEntities.Add(GetEntityKey(playerEntityData));
            }
            foreach (var entityData in _levelData.entities)
            {
                if (entityData.type != EntityType.PLAYER)
                {
                    Entities.Add(EntityFactory.CreateEntity(entityData, _physicsWorld, _progressDataManager.Data));
                    LoadedEntities.Add(GetEntityKey(entityData));
                }
            }
            foreach (var entityData in _map.staticEntities.entities)
            {
                Entities.Add(EntityFactory.CreateEntity(entityData, _physicsWorld, _progressDataManager.Data));
                LoadedEntities.Add(GetEntityKey(entityData));
            }
        }
        private static string GetEntityKey(EntityData entityData)
        {
            return $"{entityData.type}_{entityData.position.x}_{entityData.position.y}";
        }

        public void Unload()
        {
            Entities.ClearAll();
            Systems.Clear();
            MediaPlayer.Stop();
            foreach (var body in _physicsWorld.BodyList.ToList())
            {
                _physicsWorld.Remove(body);
            }
        }

        public void Update(GameTime gameTime, SceneManager sceneManager)
        {
            if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
            if (gameTime?.ElapsedGameTime.TotalSeconds != null)
                _physicsWorld.Step((float)gameTime?.ElapsedGameTime.TotalSeconds);

            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity != null)
            {
                if (!playerEntity.GetComponent<PlayerComponent>().IsInSecretLevel)
                {
                    _progressDataManager.Data.PlayerComponent.PlayerPositionX = 10520;
                    _progressDataManager.Data.PlayerComponent.PlayerPositionY = 500;
                    sceneManager.ChangeScene(SceneName.Level1Transition);
                }
            }
            UpdateProgressManager(gameTime);
            UpdateProgressData(gameTime);
            UpdateSystems(gameTime);
        }

        private void UpdateProgressManager(GameTime gameTime)
        {
            var expiredScores = new List<TemporaryScore>();

            foreach (var tempScore in _progressDataManager.TemporaryScores)
            {
                tempScore.Update(gameTime);
                if (tempScore.IsExpired())
                {
                    expiredScores.Add(tempScore);
                }
            }

            foreach (var expiredScore in expiredScores)
            {
                _progressDataManager.TemporaryScores.Remove(expiredScore);
            }
        }

        private void UpdateSystems(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }
        private void UpdateProgressData(GameTime gameTime)
        {
            _progressDataManager.Update(gameTime);
        }

        public void Draw(SpriteData spriteData, GameTime gameTime)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));

            spriteData.spriteBatch.Begin();

            spriteData.graphics.GraphicsDevice.Clear(Color.Black);
            _map.Draw(spriteData);
            DrawProgressManager(gameTime, spriteData);

            spriteData.spriteBatch.End();
        }

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

        private void DrawProgressManager(GameTime gameTime, SpriteData spriteData)
        {
            DrawEntities(gameTime);
            CommonRenders.DrawProgressData(Entities,
                spriteData, _progressDataManager.Score,
                _progressDataManager.Coins,
                "1-1",
                _progressDataManager.Time);

            foreach (var tempScore in _progressDataManager.TemporaryScores)
            {
                Debug.Assert(spriteData != null, nameof(spriteData) + " != null");
                spriteData.spriteBatch.DrawString(spriteData.spriteFont, $"{tempScore.Value}", tempScore.Position, Color.White);
            }
        }
    }
}
