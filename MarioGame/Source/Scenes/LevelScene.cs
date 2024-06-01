using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

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
        private bool _disposed;

        /*
         * Loads resources and initializes entities for the level scene.
         *
         * Parameters:
         *   spriteData: SpriteData object containing content manager for loading resources.
         *               If null, no resources will be loaded.
         */
        public void Load(SpriteData spriteData)
        {
            // Load player entity
            var playerTextures = new Texture2D[]
            {
                Utils.Sprites.BigStop,
                Utils.Sprites.BigWalk1,
                Utils.Sprites.BigWalk2,
                Utils.Sprites.BigWalk1Left,
                Utils.Sprites.BigWalk2Left,
                Utils.Sprites.BigBend,
                Utils.Sprites.BigBendLeft,
                Utils.Sprites.BigStopLeft,
                Utils.Sprites.BigJumpBack,
                Utils.Sprites.BigJumpBackLeft,
                Utils.Sprites.BigWalk3,
                Utils.Sprites.BigWalk3Left,
                Utils.Sprites.BigRun,
                Utils.Sprites.BigRunLeft
            };
            var player = new PlayerEntity(playerTextures, new Vector2(100, 100));
            Entities.Add(player);

            Systems.Add(new InputSystem());
            Systems.Add(new MovementSystem());
            if (spriteData != null) Systems.Add(new AnimationSystem(spriteData.spriteBatch));
        }

        /*
         * Unloads resources and performs cleanup operations for the level scene.
         * This method is called when the scene is being unloaded or switched.
         * It prints the current entities information to the console.
         */
        public void Unload()
        {
            Console.WriteLine("Unloading LevelScene. Entities: " + Entities.Count);
            Entities.Clear();
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
            foreach (var system in Systems)
            {
                system.Update(gameTime, Entities);
            }
        }

        /*
         * Draws the level scene on the screen.
         * This method clears the graphics device, then draws all entities in the scene.
         */
        public void Draw(SpriteData spriteData, GameTime gameTime)
        {
            if (spriteData == null) return;

            spriteData.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteData.spriteBatch.Begin();

            // Draw entities using the AnimationSystem
            foreach (var system in Systems)
            {
                if (system is IRenderableSystem renderableSystem)
                {
                    renderableSystem.Draw(gameTime, Entities);
                }
            }
            spriteData.spriteBatch.End();

        }

        public string GetSceneType()
        {
            return "Level";
        }

        public void Draw(SpriteData spriteData)
        {
            throw new NotImplementedException();
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
                // Dispose of any disposable objects here
                Entities.Clear();
            }

            _disposed = true;
        }
    }
}
