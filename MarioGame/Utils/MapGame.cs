using System.Collections.Generic;
using System.IO;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json.Linq;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.DataStructures;
using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils
{
    /*
     * Represents a game map.
     */
    public class MapGame
    {
        private Dictionary<Vector2, int> _tilemap;
        private List<(string type, Position position)> _backgroundEntities;
        private int _levelHeight;
        private const int TileSize = 64;
        private World physicsWorld;

        public MapGame(string pathMap,  string backgroundJsonPath, string backgroundEntitiesPath, SpriteData spriteData, World physicsWorld)
        {
            if(spriteData == null) throw new System.ArgumentNullException(nameof(spriteData));
            _backgroundEntities = new List<(string type, Position position)>();
            LoadMap(pathMap);
            this.physicsWorld = physicsWorld;
            LoadBackground(backgroundJsonPath);
            LoadBackground(backgroundEntitiesPath);
            CreateCollisionBodies();
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
         * Creates collision bodies for each tile in the tilemap that has a value greater than 0.
         */
        private void CreateCollisionBodies()
        {
            foreach (var tilePosition in _tilemap.Keys)
            {
                int tileValue = _tilemap[tilePosition];

                if (tileValue > 0)
                {
                    AetherVector2 worldPosition = new AetherVector2(
                        (tilePosition.X + 0.5f) * TileSize / Constants.pixelPerMeter,
                        (tilePosition.Y + 0.5f) * TileSize / Constants.pixelPerMeter
                    );

                    Body tileBody = physicsWorld.CreateBody(worldPosition, 0f, BodyType.Static);

                    tileBody.CreateRectangle(TileSize / Constants.pixelPerMeter, TileSize / Constants.pixelPerMeter, 1f, AetherVector2.Zero);
                    tileBody.Tag = tileValue;
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

            foreach (var (type, position) in _backgroundEntities)
            {
                if (Animations.mapTexturesBackground.ContainsKey(type))
                {
                    Texture2D texture = Animations.mapTexturesBackground[type];
                    Rectangle dest = new Rectangle(position.x, position.y, texture.Width, texture.Height);
                    spriteData?.spriteBatch.Draw(texture, dest, Color.White);
                }
            }
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

        private void LoadBackground(string backgroundJsonPath)
        {
            using (StreamReader reader = new StreamReader(backgroundJsonPath))
            {
                string jsonContent = reader.ReadToEnd();
                JObject jsonObject = JObject.Parse(jsonContent);

                JArray entities = (JArray)jsonObject["entities"];
                foreach (var entity in entities)
                {
                    string type = entity["type"].ToString();
                    Position position = new Position
                    {
                        x = (int)entity["position"]["x"],
                        y = (int)entity["position"]["y"]
                    };
                    _backgroundEntities.Add((type, position));
                }
            }
        }
    }
}
