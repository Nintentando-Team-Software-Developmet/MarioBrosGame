using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using MarioGame;
using MarioGame.Utils.DataStructures;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.DataStructures;
using SuperMarioBros.Utils.Scene;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils.Maps
{
    public abstract class BaseMapGame
    {
        private Dictionary<Vector2, int> _tilemap;
        private List<(string type, Position position)> _backgroundEntities;
        public StaticEntitiesData staticEntities { get; protected set; }
        protected const int TileSize = 64;
        private World physicsWorld;
        private int _levelHeight;

        protected BaseMapGame(string pathMap, SpriteData spriteData, World physicsWorld)
        {
            if (spriteData == null) throw new System.ArgumentNullException(nameof(spriteData));
            _backgroundEntities = new List<(string type, Position position)>();
            LoadMap(pathMap);
            this.physicsWorld = physicsWorld;
        }

        protected void LoadMap(string pathMap)
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

        public void CreateCollisionBodies(ReadOnlyCollection<AetherVector2> sizes, ReadOnlyCollection<AetherVector2> positions)
        {
            if (sizes == null) throw new ArgumentNullException(nameof(sizes));
            if (positions == null) throw new ArgumentNullException(nameof(positions));
            for (int i = 0; i < sizes.Count; i++)
            {
                AetherVector2 size = sizes[i];
                AetherVector2 position = positions[i];

                Body largeBody = physicsWorld.CreateBody(position, 0f, BodyType.Static);
<<<<<<< HEAD:MarioGame/Utils/MapGame.cs

                Fixture fixture = largeBody.CreateRectangle(size.X, size.Y, 1f, AetherVector2.Zero);
                fixture.CollisionCategories = Categories.World;
                fixture.CollidesWith = Categories.Player | Categories.World;
                largeBody.FixedRotation = true;
=======
                largeBody.CreateRectangle(size.X, size.Y, 1f, AetherVector2.Zero);
>>>>>>> develop:MarioGame/Utils/Maps/BaseMapGame.cs
                largeBody.Tag = i + 1;
            }
        }

        public virtual void Draw(SpriteData spriteData)
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

        protected void LoadBackground(string backgroundJsonPath)
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

        protected void LoadStaticEntities(string staticEntitiesPath)
        {
            string json = File.ReadAllText(staticEntitiesPath);
            staticEntities = JsonConvert.DeserializeObject<StaticEntitiesData>(json);
        }

        public Dictionary<Vector2, int> Tilemap
        {
            get => _tilemap;
        }
    }
}
