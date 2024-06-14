using System.Collections.Generic;


using Microsoft.Xna.Framework.Graphics;


using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace MarioGame
{
    public static class Animations
    {
        public static readonly Texture2D[] playerTextures = new Texture2D[]
        {
            Sprites.BigStop,
            Sprites.BigRunLeft,
            Sprites.BigWalk1,
            Sprites.BigWalk3,
            Sprites.BigWalk2,

            Sprites.BigStopLeft,
            Sprites.BigRun,
            Sprites.BigWalk1Left,
            Sprites.BigWalk2Left,
            Sprites.BigWalk3Left,

            Sprites.BigBend,
            Sprites.BigBendLeft,

            Sprites.BigJumpBack,
            Sprites.BigJumpBackLeft,

            Sprites.LowerThePost1,
            Sprites.LowerThePost2,
            Sprites.LowerThePostLeft
        };

        public static readonly Texture2D[] goombaTextures = new Texture2D[]
        {
            Sprites.Goomba1,
            Sprites.Goomba2,
        };
        public static readonly Texture2D[] FlagWinTextures = new Texture2D[]
        {
            Sprites.WinFlagGreen,
            Sprites.WinFlag
        };

        public static readonly Dictionary<int, Texture2D> mapTextures = new Dictionary<int, Texture2D>
        {
            { 1, Sprites.StoneBlockBrown },
        };

        public static readonly Texture2D[] questionBlockTextures = new Texture2D[]
        {
            Sprites.QuestionBlockBrown,
            Sprites.QuestionBlockAny
        };

        public static readonly Texture2D[] coinBlockTextures = new Texture2D[]
        {
            Sprites.BrickBlockBrown
        };
        public static readonly Texture2D[] blockTextures = new Texture2D[]
        {
            Sprites.PolishedStoneBlockBrown
        };

        public static readonly Texture2D[] ductTextures = new Texture2D[]
        {
            Sprites.DuctSquareGreen
        };

        public static readonly Texture2D[] ductExtensionTextures = new Texture2D[]
        {
            Sprites.DuctVerticalGreen
        };

        public static readonly Dictionary<string,Texture2D> mapTexturesBackground = new Dictionary<string, Texture2D>
        {
            { "BUSH", Sprites.BushMenu },
            { "DUCT", Sprites.DuctSquareGreen},
            { "DUCT_1", Sprites.DuctVerticalGreen},
            { "CASTLE" , Sprites.CastleBrown},
            { "SIMPLE_BUSH", Sprites.SimpleBush},
            { "DOUBLE_BUSH", Sprites.DoubleBush},
            { "BIG_BUSH_1", Sprites.BigBush1},
            { "BIG_BUSH_2", Sprites.BigBush2},
            { "SIMPLE_CLOUD", Sprites.SimpleCloud},
            { "DOUBLE_CLOUD", Sprites.DoubleCloud},
            { "TRIPLE_CLOUD", Sprites.TripleCloud},
            { "BLOCK" , Sprites.PolishedStoneBlockBrown}
        };

        public static readonly Dictionary<EntitiesName, Texture2D[]> entityTextures = new Dictionary<EntitiesName, Texture2D[]>
        {
            { EntitiesName.MARIO, playerTextures },
            { EntitiesName.GOOMBA, goombaTextures },
            { EntitiesName.FLAG, FlagWinTextures },
            { EntitiesName.QUESTIONBLOCK,  questionBlockTextures },
            { EntitiesName.COINBLOCK,  coinBlockTextures},
            { EntitiesName.BLOCK,  blockTextures},
            { EntitiesName.DUCTEXTENSION,  ductExtensionTextures},
            { EntitiesName.DUCT,  ductTextures},


        };
    }
}
