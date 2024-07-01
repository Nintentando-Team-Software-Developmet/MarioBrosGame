using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using nkast.Aether.Physics2D.Dynamics;

using SuperMarioBros.Utils.DataStructures;

using AetherVector2 = nkast.Aether.Physics2D.Common.Vector2;

namespace SuperMarioBros.Utils.Maps;

public class SecretLevelMapGame : BaseMapGame
{
    public SecretLevelMapGame(string pathMap, string backgroundEntitiesPath, SpriteData spriteData, World physicsWorld)
        : base(pathMap, spriteData, physicsWorld)
    {
        LoadStaticEntities(backgroundEntitiesPath);
        CreateCollisionBodies(new List<AetherVector2>
        {
            new AetherVector2(44.16f, 0.6f),
            new AetherVector2(0.64f, 10.24f),
            new AetherVector2(5.12f, 2.56f),
        }.AsReadOnly(), new List<AetherVector2>
        {
            new AetherVector2(22.08f, 8f),
            new AetherVector2(0.32f, 5.12f),
            new AetherVector2(5.76f, 6.4f),
        }.AsReadOnly());
    }

    /*
     * Draws the map.
     *
     * Parameters:
     *   spriteData: The sprite data to draw the map.
     */
    public new void Draw(SpriteData spriteData)
    {
        base.Draw(spriteData);
        foreach (var item in Tilemap)
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
}
