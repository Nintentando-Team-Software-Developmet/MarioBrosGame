using System.Collections.Generic;
using System.Collections.ObjectModel;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils.Maps;

public class MapGame : BaseMapGame
{
    public MapGame(string pathMap, string backgroundJsonPath, string backgroundEntitiesPath, SpriteData spriteData, World physicsWorld)
        : base(pathMap, spriteData, physicsWorld)
    {
        LoadBackground(backgroundJsonPath);
        LoadStaticEntities(backgroundEntitiesPath);
        CreateCollisionBodies(new List<AetherVector2>
        {
            new AetherVector2(44.16f, 0.6f),
            new AetherVector2(9.60f, 1.28f),
            new AetherVector2(40.96f, 1.28f),
            new AetherVector2(40.32f, 1.28f),
            new AetherVector2(1.28f, 0.64f),
            new AetherVector2(1.32f, 2.56f),
            new AetherVector2(1.32f, 2.56f),
            new AetherVector2(0.64f, 3.2f),
        }.AsReadOnly(), new List<AetherVector2>
        {
            new AetherVector2(22.08f, 8f),
            new AetherVector2(50.25f, 8.32f),
            new AetherVector2(77.43f, 8.32f),
            new AetherVector2(119.36f,8.32f),
            new AetherVector2(24.32f,7.04f),
            new AetherVector2(29.44f,6.4f),
            new AetherVector2(36.48f,6.4f),
            new AetherVector2(99.47f,6.73f)
        }.AsReadOnly());
    }
}
