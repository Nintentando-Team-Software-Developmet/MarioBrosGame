using System;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


using SuperMarioBros.Utils.DataStructures;

#pragma warning disable CA1002, CA2227

namespace MarioGame.Utils.DataStructures
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class StaticEntitiesData
    {
        public List<EntityData> entities { get; set; }
    }
    public class EntityData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EntitiesName name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EntitiesName TypeContent { get; set; }
        public int Quantity { get; set; }

        public Position position { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
    }

    /*
     * Represents the data of a level.
     * Object to be serialized and deserialized from JSON.
     */
    public class LevelData
    {
        public string nameLevel { get; set; }
        public string pathMap { get; set; }
        public string backgroundJsonPath { get; set; }
        public string backgroundEntitiesPath { get; set; }
        public List<EntityData> entities { get; set; }
    }
}
