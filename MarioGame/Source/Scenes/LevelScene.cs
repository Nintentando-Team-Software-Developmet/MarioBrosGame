using System;
using System.Collections.Generic;
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
        private ProgressDataManager _progressDataManager;
        private bool _isFlagEventPlayed { get; set; }
        private bool _isLevelCompleted { get; set; }
        private double _levelCompleteDisplayTime;
        private const double LevelCompleteMaxDisplayTime = 10.0;
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
            MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/level1_naruto"));
            MediaPlayer.IsRepeating = true;
            Console.WriteLine("Loaded Entities: " + Entities.Count);
        }

        private void InitializeSystems(SpriteData spriteData)
        {
            Systems.Add(new AnimationSystem(spriteData.spriteBatch));
            Systems.Add(new CameraSystem());
            Systems.Add(new NonPlayerMovementSystem());
            Systems.Add(new PlayerMovementSystem());
            Systems.Add(new PlayerSystem());
            Systems.Add(new EnemySystem());
            //TODO: Review system
            //Systems.Add(new KoopaAnimationSystem(spriteData.spriteBatch));
            //Systems.Add(new KoopaMovementSystem());
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
                Entities.Add(EntityFactory.CreateEntity(playerEntityData, physicsWorld));
                _loadedEntities.Add(GetEntityKey(playerEntityData));
            }

            var initialStaticEntities = map.staticEntities.entities.Where(entityData =>
            {
                var entityPosition = new Vector2(entityData.position.x, entityData.position.y);
                return Vector2.Distance(new Vector2(playerEntityData.position.x, playerEntityData.position.y), entityPosition) <= LoadRadius;
            }).ToList();

            foreach (var entity in initialStaticEntities)
            {
                Entities.Add(EntityFactory.CreateEntity(entity, physicsWorld));
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

            foreach (var body in physicsWorld.BodyList.ToList())
            {
                physicsWorld.Remove(body);
            }
            MediaPlayer.Stop();
            _progressDataManager.ResetTime();
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
            }

            if (!_isFlagEventPlayed)
            {
                CheckFlagEvent();
                UpdateProgressData(gameTime);
                CheckGameOverConditions(sceneManager);
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

            UpdateSystems(gameTime);
            CheckPlayerState(sceneManager);
            Console.WriteLine("Active Entities: " + Entities.Count);
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
                Entities.Add(EntityFactory.CreateEntity(entityData, physicsWorld));
                _loadedEntities.Add(GetEntityKey(entityData));
            }

            var staticEntitiesToLoad = map.staticEntities.entities.Where(entityData =>
            {
                var entityPosition = new Vector2(entityData.position.x, entityData.position.y);
                return Vector2.Distance(playerPosition, entityPosition) <= radius && !_loadedEntities.Contains(GetEntityKey(entityData));
            }).ToList();

            foreach (var entityData in staticEntitiesToLoad)
            {
                Entities.Add(EntityFactory.CreateEntity(entityData, physicsWorld));
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
                player.HasReachedEnd = false;
                _isFlagEventPlayed = true;
                PlayFlagSound();
            }
        }

        private void UpdateProgressData(GameTime gameTime)
        {
            _progressDataManager.Update(gameTime);
        }

        private void CheckGameOverConditions(SceneManager sceneManager)
        {
            if (_progressDataManager.Time <= 0)
            {
                HandleTimeOver(sceneManager);
            }
            else if (_progressDataManager.Lives <= 0)
            {
                sceneManager.ChangeScene(SceneName.GameOver);
            }
        }

        private void HandleTimeOver(SceneManager sceneManager)
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

        private void UpdateSystems(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }

        private void CheckPlayerState(SceneManager sceneManager)
        {
            var playerEntity = Entities.FirstOrDefault(e => e.HasComponent<PlayerComponent>());
            if (playerEntity == null) return;

            var player = playerEntity.GetComponent<PlayerComponent>();
            if (player != null && !player.IsAlive)
            {
                HandlePlayerDeath(sceneManager);
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
            DrawEntities(gameTime);
            CommonRenders.DrawProgressData(Entities,
                                            spriteData, _progressDataManager.Score,
                                            _progressDataManager.Coins,
                                            "1-1",
                                            _progressDataManager.Time);
            spriteData.spriteBatch.End();
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
