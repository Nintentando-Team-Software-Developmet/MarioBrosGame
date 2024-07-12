using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private World physicsWorld;
        private LevelData _levelData;
        private bool _disposed;
        private Song _flagSoundEffect { get; set; }
        private Song _runningOutOfTimeSong { get; set; }
        private ProgressDataManager _progressDataManager;
        private bool _isFlagEventPlayed { get; set; }
        private bool _isLevelCompleted { get; set; }
        private bool _isRunningOutOfTime { get; set; }
        private double _levelCompleteDisplayTime;
        private const double LevelCompleteMaxDisplayTime = 6.5;
        private HashSet<string> _loadedEntities { get; }
        private const int LoadRadius = 1000;


        public Matrix Camera => (Matrix)Entities.FirstOrDefault(
            e => e.HasComponent<CameraComponent>())?.GetComponent<CameraComponent>().Transform;


        /*
         * Constructs a new LevelScene object.
         * This constructor initializes the level scene with the specified path to the scene data.
         *
         * Parameters:
         *   pathScene: A string representing the path to the scene data.
         */
        public LevelScene(string pathScene, ProgressDataManager progressDataManager)
        {
            string json = File.ReadAllText(pathScene);
            _levelData = JsonConvert.DeserializeObject<LevelData>(json);
            _progressDataManager = progressDataManager;
            physicsWorld = new World(new AetherVector2(0, 20f));
            _flagSoundEffect = null;
            _isFlagEventPlayed = false;
            _isLevelCompleted = false;
            _levelCompleteDisplayTime = 0;
            _loadedEntities = new HashSet<string>();
            _runningOutOfTimeSong = null;
        }

        /*
         * Loads resources and initializes the level scene.
         * This method is called when the scene is being loaded or switched.
         * It creates the map and systems for the level scene.
         */
        public void Load(SpriteData spriteData)
        {
            if (spriteData == null) throw new ArgumentNullException(nameof(spriteData));
            map = new MapGame(_levelData.pathMap, _levelData.backgroundJsonPath, _levelData.backgroundEntitiesPath, spriteData, physicsWorld);
            LoadEssentialEntities();
            InitializeSystems(spriteData);
            _flagSoundEffect = spriteData.content.Load<Song>("Sounds/win_music");
            _runningOutOfTimeSong = spriteData.content.Load<Song>("Sounds/fast_level");
            MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/level1_naruto"));
            MediaPlayer.IsRepeating = true;
            LoadSoundEffects(spriteData);
        }


        private static void LoadSoundEffects(SpriteData spriteData)
        {
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.BlockDestroyed, "SoundEffects/block_destroy");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.NonBreakableBlockCollided, "SoundEffects/block_not_destroy");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.CoinCollected, "SoundEffects/coin_collected");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.EnemyDestroyed, "SoundEffects/smash");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PlayerFireball, "SoundEffects/fireball");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PlayerFireballCollided, "SoundEffects/fireball_hit");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PlayerJump, "SoundEffects/jump");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PlayerLostLife, "SoundEffects/loss_life");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PlayerLostPowerUpBecauseHit, "SoundEffects/lost_power_up");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.PowerUpCollected, "SoundEffects/power_up_collected");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.BlockPowerUpCollided, "SoundEffects/block_power_up");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.Ducting, "SoundEffects/duct_entry");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.Mushroom, "SoundEffects/mushroom");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.Star, "SoundEffects/fireball_powerup");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.Flower, "SoundEffects/star_powerup");
            SoundEffectManager.Instance.LoadSoundEffect(spriteData.content, SoundEffectType.EnemyDestroyedByStar, "SoundEffects/enemy_destroyed_star");
        }


        /*
         * Initializes the systems for the level scene.
         * This method creates and adds systems to the scene.
         *
         * Parameters:
         *   spriteData: SpriteData object containing sprite batch and content manager.
         */

        private void InitializeSystems(SpriteData spriteData)
        {
            Systems.Add(new AnimationSystem(spriteData.spriteBatch));
            Systems.Add(new CameraSystem());
            Systems.Add(new NonPlayerMovementSystem());
            Systems.Add(new PlayerMovementSystem());
            Systems.Add(new PlayerSystem(_progressDataManager));
            Systems.Add(new EnemySystem(_progressDataManager));
            Systems.Add(new BlockSystem(_progressDataManager));
            Systems.Add(new WinPoleSystem(_progressDataManager));
            Systems.Add(new FireBoolSystem(_progressDataManager));
            Systems.Add(new MarioPowersSystem(_progressDataManager, spriteData));
            Systems.Add(new SoundEffectSystem());
        }

        /*
         * Loads entities from the level data.
         * This method creates entities based on the level data and adds them to the scene.
         */
        private void LoadEssentialEntities()
        {
            var playerEntityData = _levelData.entities.FirstOrDefault(e => e.type == EntityType.PLAYER);
            if (playerEntityData != null)
            {
                Entities.Add(EntityFactory.CreateEntity(playerEntityData, physicsWorld, _progressDataManager.Data));
                _loadedEntities.Add(GetEntityKey(playerEntityData));
            }

            var initialStaticEntities = map.staticEntities.entities.Where(entityData =>
            {
                var entityPosition = new Vector2(entityData.position.x, entityData.position.y);
                return Vector2.Distance(new Vector2(playerEntityData.position.x, playerEntityData.position.y), entityPosition) <= LoadRadius;
            }).ToList();

            foreach (var entity in initialStaticEntities)
            {
                Entities.Add(EntityFactory.CreateEntity(entity, physicsWorld, _progressDataManager.Data));
                _loadedEntities.Add(GetEntityKey(entity));
            }
        }

        private static string GetEntityKey(EntityData entityData)
        {
            return $"{entityData.type}_{entityData.position.x}_{entityData.position.y}";
        }


        public void PlayFlagSound()
        {
            if (_isFlagEventPlayed)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(_flagSoundEffect);
            }
        }

        private void CheckRunningOutTime()
        {
            if (_progressDataManager.Time <= 101)
            {
                MediaPlayer.Stop();
                MediaPlayer.Volume = 0.3f;
                MediaPlayer.Play(_runningOutOfTimeSong);
                _isRunningOutOfTime = true;
            }
        }

        /*
         * Unloads resources and performs cleanup operations for the level scene.
         * This method is called when the scene is being unloaded or switched.
         * It prints the current entities information to the console.
         */
        public void Unload()
        {
            Entities.ClearAll();
            Systems.Clear();
            _loadedEntities.Clear();
            SoundEffect.MasterVolume = SoundEffect.MasterVolume - SoundEffect.MasterVolume * 0.33f;
            _isRunningOutOfTime = false;

            foreach (var body in physicsWorld.BodyList.ToList())
            {
                physicsWorld.Remove(body);
            }
            MediaPlayer.Stop();
            _isLevelCompleted = false;
            _isFlagEventPlayed = false;
            _levelCompleteDisplayTime = 0;
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
            if (sceneManager == null) throw new ArgumentNullException(nameof(sceneManager));
            if (gameTime?.ElapsedGameTime.TotalSeconds != null)
                physicsWorld.Step((float)gameTime?.ElapsedGameTime.TotalSeconds);

            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity != null)
            {
                var playerPosition = playerEntity.GetComponent<ColliderComponent>().Position;
                LoadEntitiesNearPlayer(playerPosition, LoadRadius);

                if (playerEntity.GetComponent<PlayerComponent>().IsInSecretLevel)
                {
                    _progressDataManager.Data.PlayerComponent.PlayerPositionX = 128;
                    _progressDataManager.Data.PlayerComponent.PlayerPositionY = 32;
                    sceneManager.ChangeScene(SceneName.SecretLevelTransition);
                }
            }


            if (!_isFlagEventPlayed)
            {
                CheckFlagEvent();
                UpdateProgressData(gameTime);
                CheckGameOverConditions();
            }

            if (!_isRunningOutOfTime)
            {
                CheckRunningOutTime();
            }

            else if (!_isLevelCompleted)
            {
                _levelCompleteDisplayTime += gameTime.ElapsedGameTime.TotalSeconds;
                if (_levelCompleteDisplayTime >= LevelCompleteMaxDisplayTime)
                {
                    _isLevelCompleted = true;
                    sceneManager.ChangeScene(SceneName.Win);
                }
            }

            UpdateProgressManager(gameTime);
            UpdateSystems(gameTime);
            CheckPlayerState(gameTime, sceneManager);
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

        private void LoadEntitiesNearPlayer(Vector2 playerPosition, float radius)
        {
            var entitiesToLoad = _levelData.entities.Where(entityData =>
            {
                var entityPosition = new Vector2(entityData.position.x, entityData.position.y);
                return Vector2.Distance(playerPosition, entityPosition) <= radius && !_loadedEntities.Contains(GetEntityKey(entityData));
            }).ToList();

            foreach (var entityData in entitiesToLoad)
            {
                Entities.Add(EntityFactory.CreateEntity(entityData, physicsWorld, _progressDataManager.Data));
                _loadedEntities.Add(GetEntityKey(entityData));
            }

            var staticEntitiesToLoad = map.staticEntities.entities.Where(entityData =>
            {
                var entityPosition = new Vector2(entityData.position.x, entityData.position.y);
                return Vector2.Distance(playerPosition, entityPosition) <= radius && !_loadedEntities.Contains(GetEntityKey(entityData));
            }).ToList();

            foreach (var entityData in staticEntitiesToLoad)
            {
                Entities.Add(EntityFactory.CreateEntity(entityData, physicsWorld, _progressDataManager.Data));
                _loadedEntities.Add(GetEntityKey(entityData));
            }
        }



        private void CheckFlagEvent()
        {
            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity == null) return;

            var player = playerEntity.GetComponent<PlayerComponent>();
            if (player != null && player.HasReachedEnd)
            {
                _isRunningOutOfTime = true;
                _isFlagEventPlayed = true;
                PlayFlagSound();
            }
        }

        private void UpdateProgressData(GameTime gameTime)
        {
            _progressDataManager.Update(gameTime);
        }

        private void CheckGameOverConditions()
        {
            if (_progressDataManager.Time <= 0 && !_progressDataManager.Data.PlayerComponent.HasReachedEnd)
            {
                HandleTimeOver();
            }
        }

        private void HandleTimeOver()
        {
            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity == null) return;

            var playerComponent = playerEntity.GetComponent<PlayerComponent>();
            var animationComponent = playerEntity.GetComponent<AnimationComponent>();
            var colliderComponent = playerEntity.GetComponent<ColliderComponent>();

            if (playerComponent != null && animationComponent != null && colliderComponent != null && playerComponent.IsAlive)
            {
                playerComponent.IsAlive = false;
                PlayerSystem.StartDeathAnimation(playerComponent, colliderComponent, 50,animationComponent);
            }
        }

        private void UpdateSystems(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }

        private void CheckPlayerState(GameTime gameTime, SceneManager sceneManager)
        {
            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity == null) return;

            var player = playerEntity.GetComponent<PlayerComponent>();
            if (player != null && !player.IsAlive)
            {
                var animationComponent = playerEntity.GetComponent<AnimationComponent>();
                var colliderComponent = playerEntity.GetComponent<ColliderComponent>();

                if (animationComponent != null && colliderComponent != null)
                {
                    PlayerSystem.UpdateDeathAnimation(gameTime, player, colliderComponent, animationComponent);
                }

                if (player.DeathAnimationComplete)
                {
                    HandlePlayerDeath(sceneManager);
                }
            }
        }

        private void HandlePlayerDeath(SceneManager sceneManager)
        {
            _progressDataManager.Lives--;
            if (_progressDataManager.Lives > 0)
            {
                sceneManager.ChangeScene(SceneName.Lives);
            }
            else
            {
                sceneManager.ChangeScene(SceneName.GameOver);
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
            DrawProgressManager(gameTime, spriteData);

            spriteData.spriteBatch.End();
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

        public Entity GetPlayerEntity()
        {
            return Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
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
    }
}
