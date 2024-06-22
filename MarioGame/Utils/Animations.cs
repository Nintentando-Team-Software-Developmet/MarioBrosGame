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
            Sprites.BigWalk3Left,
            Sprites.BigWalk2Left,

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
        public static readonly Texture2D[] koopaLeftTextures = new Texture2D[]
        {
            Sprites.Koopa1,
            Sprites.Koopa2
        };
        public static readonly Texture2D[] koopaRigthTextures = new Texture2D[]
        {
            Sprites.Koopa3,
            Sprites.Koopa4
        };

        public static readonly Texture2D[] koopaKnockedTextures = new Texture2D[]
        {
            Sprites.Koopa6
        };

        public static readonly Texture2D[] koopaReviveTextures = new Texture2D[]
        {
            Sprites.Koopa5
        };

        public static readonly Texture2D[] koopaDiesTextures = new Texture2D[]
        {
            Sprites.Koopa7
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

        public static readonly Texture2D[] mushroomTextures = new Texture2D[]
        {
            Sprites.GrowMushroom1
        };

        public static readonly Texture2D[] fireFlowerTextures = new Texture2D[]
        {
            Sprites.FireFlower1,
            Sprites.FireFlower2,
            Sprites.FireFlower3
        };

        public static readonly Texture2D[] superStarTextures = new Texture2D[]
        {
            Sprites.SuperStar1,
            Sprites.SuperStar2,
            Sprites.SuperStar3
        };

        public static readonly Dictionary<string, Texture2D> mapTexturesBackground = new Dictionary<string, Texture2D>
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
            { "BLOCK" , Sprites.PolishedStoneBlockBrown},
            {"COIN_BLOCK", Sprites.BrickBlockBrown},
            { "QUESTION_BLOCK", Sprites.QuestionBlockBrown}
        };



        public static readonly Dictionary<AnimationState,Texture2D[]>  playerAnimations = new Dictionary<AnimationState, Texture2D[]>
            {
                { AnimationState.WALKRIGHT, new Texture2D[] { Sprites.SmallWalk1, Sprites.SmallWalk3, Sprites.SmallWalk2 } },
                { AnimationState.WALKLEFT, new Texture2D[] { Sprites.SmallWalk1Left, Sprites.SmallWalk3Left, Sprites.SmallWalk2Left } },
                { AnimationState.JUMPLEFT, new Texture2D[] { Sprites.SmallJumpLeft } },
                { AnimationState.JUMPRIGHT, new Texture2D[] { Sprites.SmallJump } },
                { AnimationState.RUNLEFT, new Texture2D[] { Sprites.SmallRunLeft } },
                { AnimationState.RUNRIGHT, new Texture2D[] { Sprites.SmallRun } },
                { AnimationState.STOP, new Texture2D[] { Sprites.SmallStop } },
                { AnimationState.STOPLEFT, new Texture2D[] { Sprites.SmallStopLeft } },
                { AnimationState.DIE, new Texture2D[] { Sprites.SmallDie } }
            };

        public static readonly Dictionary<AnimationState, Texture2D[]> koopaAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.WALKRIGHT, new Texture2D[] { Sprites.Koopa3, Sprites.Koopa4 } },
            { AnimationState.WALKLEFT, new Texture2D[] { Sprites.Koopa1, Sprites.Koopa2 } },
            { AnimationState.KNOCKED, new Texture2D[] { Sprites.Koopa6 } },
            { AnimationState.REVIVE, new Texture2D[] { Sprites.Koopa5 } },
            { AnimationState.DIE, new Texture2D[] { Sprites.Koopa7 } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> goombaAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.Goomba1, Sprites.Goomba2 } },
            { AnimationState.DIE, new Texture2D[] { Sprites.Goomba3 } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> questionBLockAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.QuestionBlockAny, Sprites.QuestionBlockBrown } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> coinBlockAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.BrickBlockBrown } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> blockAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.PolishedStoneBlockBrown } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> ductExtensionAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.DuctVerticalGreen } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> ductAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.DuctSquareGreen } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> flagWinAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.WinFlagGreen, Sprites.WinFlag } }
            //TODO: Modify for the flag win animation
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> mushroomAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.GrowMushroom1 } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> fireFlowerAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.FireFlower1, Sprites.FireFlower2, Sprites.FireFlower3 } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> superStarAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.SuperStar1, Sprites.SuperStar2, Sprites.SuperStar3 } }
        };

        public static readonly Dictionary<EntitiesName, Dictionary<AnimationState, Texture2D[]>> entityTextures = new Dictionary<EntitiesName,  Dictionary<AnimationState, Texture2D[]>>
        {
            { EntitiesName.MARIO, playerAnimations },
            { EntitiesName.GOOMBA, goombaAnimations },
            { EntitiesName.FLAG, flagWinAnimations },
            { EntitiesName.QUESTIONBLOCK,  questionBLockAnimations },
            { EntitiesName.COINBLOCK,  coinBlockAnimations},
            { EntitiesName.BLOCK,  blockAnimations},
            { EntitiesName.DUCTEXTENSION,  ductExtensionAnimations},
            { EntitiesName.DUCT,  ductAnimations},
            { EntitiesName.MUSHROOM,  mushroomAnimations},
            { EntitiesName.FLOWER,  fireFlowerAnimations},
            { EntitiesName.STAR,  superStarAnimations},
            { EntitiesName.KOOPA, koopaAnimations}
        };


    }
}
