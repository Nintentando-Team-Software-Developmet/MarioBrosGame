using System.Collections.Generic;
using System.IO;

using MarioGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json.Linq;

using SuperMarioBros.Source.Components;
using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Utils
{
    /*
     * Represents a game map.
     */   
    public class MapGame
    {
        private Dictionary<Vector2, int> _tilemap;
        private Camera _camera;
        private int _levelHeight;
        private const int TileSize = 64;

        public MapGame(string pathMap, SpriteData spriteData)
        {   
            if(spriteData == null) throw new System.ArgumentNullException(nameof(spriteData));  
             _camera = new Camera(spriteData.graphics.GraphicsDevice.Viewport, 13824, 720);
            LoadMap(pathMap);
        }
        
        /*
        * Loads the map from the specified path.
        *
        * Parameters:
        *   pathMap: A string representing the path to the map data.
        */
        private void LoadMap(string pathMap)
        {
           _tilemap = new Dictionary<Vector2, int>();

            using (StreamReader reader = new StreamReader(pathMap))
            {
                string jsonContent = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(jsonContent);

                JArray layers = (JArray)jsonObject["layers"];
                JObject layer = (JObject)layers[0];
                JArray data = (JArray)layer["data"];

                int width = (int)jsonObject["width"];
                int height = (int)jsonObject["height"];
                _levelHeight = height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int value = (int)data[y * width + x];
                        if (value > 0)
                        {
                            _tilemap[new Vector2(x, y)] = value;
                        }
                    }
                }
            }
        }

        /*
        * Makes the camera follow the player entity.
        *
        * Parameters:
        *   player: The player entity to follow.
        */
        public void Follow (Entity player)
        {
            if (player != null)
            {
                var positionComponent = player.GetComponent<PositionComponent>();
                if (positionComponent != null)
                {
                    _camera.Follow(positionComponent.Position);
                }
            }
        }

        /*
        * Draws the map.
        *
        * Parameters:
        *   spriteData: The sprite data to draw the map.
        */
        public void Draw(SpriteData spriteData)
        {
            foreach (var item in _tilemap)
            {
                Rectangle dest = new Rectangle(
                    (int)item.Key.X * TileSize,
                    (int)item.Key.Y * TileSize,
                    TileSize,
                    TileSize
                );
                int index = item.Value;
                if (Animations.mapTextures.ContainsKey(index))
                {
                    Texture2D texture = Animations.mapTextures[index];
                    spriteData?.spriteBatch.Draw(texture, dest, Color.White);
                }
            }
        }

        /*
        * Gets the camera of the map.
        */
        public Camera Camera 
        { 
            get { return _camera; } 
        }

        /*
        * Gets the height of the map.
        */
        public int LevelHeight 
        { 
            get { return _levelHeight; } 
        }

        /*
        * Gets the tilemap of the map.
        */
        public Dictionary<Vector2, int> Tilemap 
        { 
            get { return _tilemap; } 
        }
    }
}