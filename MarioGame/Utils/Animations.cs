using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace MarioGame
{
    public static class Animations {
        public static readonly Texture2D[] playerTextures = new Texture2D[]
        {
            Sprites.BigStop,
            Sprites.BigRunLeft,
            Sprites.BigWalk1,
            Sprites.BigWalk2,
            Sprites.BigWalk3,

            Sprites.BigStopLeft,
            Sprites.BigRun,
            Sprites.BigWalk1Left,
            Sprites.BigWalk2Left,
            Sprites.BigWalk3Left,

            Sprites.BigBend,
            Sprites.BigBendLeft,

            Sprites.BigJumpBack,
            Sprites.BigJumpBackLeft
        };

        public static readonly Texture2D[] goombaTextures = new Texture2D[]
        {
            Sprites.Goomba1,
            Sprites.Goomba2,
        };

        public static readonly Dictionary<int,Texture2D> mapTextures = new Dictionary<int, Texture2D>
        {
            { 1, Sprites.StoneBlockBrown },
        };

        public static readonly Dictionary<string,Texture2D> mapTexturesBackground = new Dictionary<string, Texture2D>
        {
            { "BUSH", Sprites.BushMenu },
            { "DUCT", Sprites.DuctSquareGreen},
            { "DUCT_1", Sprites.DuctVerticalGreen}
        };

        public static readonly Dictionary<EntitiesName, Texture2D[]> entityTextures = new Dictionary<EntitiesName, Texture2D[]>
        {
            { EntitiesName.MARIO, playerTextures },
            { EntitiesName.GOOMBA, goombaTextures }
        };
    }
}
