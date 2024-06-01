using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using SuperMarioBros.Source.Entities;
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
            //if (spriteData == null) return;

            // Example loading logic, replace with actual entity loading
            /*var mario = new Mario
            {
                Position = new Vector2(100, 100),
                Texture = spriteData.content.Load<Texture2D>("Sprites/Mario")
            };

            Entities.Add(mario);
*/
            // Load other entities like enemies, obstacles, etc.
            // Example: var goomba = new Goomba { Position = new Vector2(200, 100), Texture = spriteData.content.Load<Texture2D>("Sprites/Goomba") };
            // Entities.Add(goomba);

            //MediaPlayer.Play(spriteData.content.Load<Song>("Sounds/mario-bros-remix.mp3"));
            //MediaPlayer.IsRepeating = true;
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
         * Draws the level scene on the screen.
         * This method clears the graphics device, then draws all entities in the scene.
         */
        public void Draw(SpriteData spriteData)
        {
            if (spriteData == null) return;

            spriteData.graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //spriteData.spriteBatch.Begin();

            /*foreach (var entity in Entities)
            {
                spriteData.spriteBatch.Draw(entity.Texture, entity.Position, Color.White);
            }
            */

            //spriteData.spriteBatch.End();
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
