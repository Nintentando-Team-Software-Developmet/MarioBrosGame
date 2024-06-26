using System.Collections.Generic;
using System.IO;

using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils
{
    public class SecretLevelMapGame
    {
        private Dictionary<Vector2, int> _tilemap;
        public StaticEntitiesData staticEntities { get; set; }

        private const int TileSize = 64;
        private World physicsWorld;

        public SecretLevelMapGame(string pathMap, SpriteData spriteData, World physicsWorld)
        {
            if (spriteData == null) throw new System.ArgumentNullException(nameof(spriteData));
            LoadMap(pathMap);
            this.physicsWorld = physicsWorld;
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

                Texture2D textureToDraw = null;

                if (index == 1)
                {
                    textureToDraw = Sprites.StoneBlockBlue;
                }
                else if (index == 2)
                {
                    textureToDraw = Sprites.NoIluminatedBrickBlockBlue;
                }
                if (textureToDraw != null)
                {
                    spriteData?.spriteBatch.Draw(textureToDraw, dest, Color.White);
                }
            }
        }

        /*
         * Creates large collision bodies with specified sizes at the current positions.
         */
        private void CreateCollisionBodies()
        {
            var sizes = new List<AetherVector2>
            {
                new AetherVector2(12.16f, 0.65f),
            };

            var positions = new List<AetherVector2>
            {
                new AetherVector2(22.08f, 10.88f),
            };

            for (int i = 0; i < sizes.Count; i++)
            {
                AetherVector2 size = sizes[i];
                AetherVector2 position = positions[i];

                Body largeBody = physicsWorld.CreateBody(position, 0f, BodyType.Static);

                largeBody.CreateRectangle(size.X, size.Y, 1f, AetherVector2.Zero);
                largeBody.Tag = i + 1;
            }
        }

        private void LoadStaticEntities(string staticEntitiesPath)
        {
            string json = File.ReadAllText(staticEntitiesPath);
            staticEntities = JsonConvert.DeserializeObject<StaticEntitiesData>(json);

        }
    }
}
