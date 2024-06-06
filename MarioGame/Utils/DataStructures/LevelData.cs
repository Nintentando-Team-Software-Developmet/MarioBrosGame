using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SuperMarioBros.Utils.DataStructures;
using System.Collections.Generic;
#pragma warning disable CA1002, CA2227

namespace MarioGame.Utils.DataStructures
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class EntityData
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EntityType type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EntitiesName name { get; set; }

        public Position position { get; set; }
    }

    /*
     * Represents the data of a level.
     * Object to be serialized and deserialized from JSON.
     */
    public class LevelData
    {
        public string nameLevel { get; set; }
        public string pathMap { get; set; }
        public  List<EntityData> entities { get; set; }
    }
}
