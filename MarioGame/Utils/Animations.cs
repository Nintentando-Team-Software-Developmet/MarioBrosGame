using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace MarioGame
{
    public static class Animations
    {
        public static readonly Dictionary<int, Texture2D> mapTextures = new Dictionary<int, Texture2D>
        {
            { 1, Sprites.StoneBlockBrown },
        };

        public static readonly Texture2D[] questionBlockTextures = new Texture2D[]
        {
            Sprites.QuestionBlockBrown,
            Sprites.QuestionBlockAny
        };

        public static readonly Texture2D[] blockedBlockBrown = new Texture2D[]
        {
            Sprites.BlockedBlockBrown,
            Sprites.BlockedBlockBrown
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



        public static readonly Dictionary<AnimationState,Texture2D[]>  smallPlayerAnimations = new Dictionary<AnimationState, Texture2D[]>
            {
                { AnimationState.WALKRIGHT, new Texture2D[] { Sprites.SmallWalk1, Sprites.SmallWalk3, Sprites.SmallWalk2 } },
                { AnimationState.WALKLEFT, new Texture2D[] { Sprites.SmallWalk1Left, Sprites.SmallWalk3Left, Sprites.SmallWalk2Left } },
                { AnimationState.JUMPLEFT, new Texture2D[] { Sprites.SmallJumpLeft } },
                { AnimationState.JUMPRIGHT, new Texture2D[] { Sprites.SmallJump } },
                { AnimationState.RUNLEFT, new Texture2D[] { Sprites.SmallRunLeft } },
                { AnimationState.RUNRIGHT, new Texture2D[] { Sprites.SmallRun } },
                { AnimationState.STOP, new Texture2D[] { Sprites.SmallStop } },
                { AnimationState.STOPLEFT, new Texture2D[] { Sprites.SmallStopLeft } },
                { AnimationState.DIE, new Texture2D[] { Sprites.SmallDie } },
                { AnimationState.WIN, new Texture2D[] { Sprites.LowerThePostLeft } }
            };

        public static readonly Dictionary<AnimationState, Texture2D[]> bigPlayerAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.WALKRIGHT, new Texture2D[] { Sprites.BigWalk1, Sprites.BigWalk2, Sprites.BigWalk3 } },
            { AnimationState.WALKLEFT, new Texture2D[] { Sprites.BigWalk1Left, Sprites.BigWalk2Left, Sprites.BigWalk3Left } },
            { AnimationState.JUMPLEFT, new Texture2D[] { Sprites.BigJumpBackLeft } },
            { AnimationState.JUMPRIGHT, new Texture2D[] { Sprites.BigJumpBack } },
            { AnimationState.RUNLEFT, new Texture2D[] { Sprites.BigRunLeft } },
            { AnimationState.RUNRIGHT, new Texture2D[] { Sprites.BigRun } },
            { AnimationState.STOP, new Texture2D[] { Sprites.BigStop } },
            { AnimationState.STOPLEFT, new Texture2D[] { Sprites.BigStopLeft } },
            { AnimationState.BENDLEFT , new Texture2D[] { Sprites.BigBendLeft } },
            { AnimationState.BENDRIGHT, new Texture2D[] { Sprites.BigBend} },
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
        public static readonly Dictionary<AnimationState, Texture2D[]> blockedLockAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.BlockedBlockBrown, Sprites.BlockedBlockBrown } }
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
            { AnimationState.STOP, new Texture2D[] { Sprites.WinFlag } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> flagWinBallAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.WinFlagGreenBall } }
        };

        public static readonly Dictionary<AnimationState, Texture2D[]> poleWinAnimations = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.STOP, new Texture2D[] { Sprites.WinFlagGreen } }
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
        public static readonly Dictionary<AnimationState, Texture2D[]> coin = new Dictionary<AnimationState, Texture2D[]>
        {
            { AnimationState.BlINK, new Texture2D[] { Sprites.CoinIcon } }
        };

        public static readonly  Dictionary<PlayerState, Dictionary<AnimationState, Texture2D[]>> playerTextures = new Dictionary<PlayerState, Dictionary<AnimationState, Texture2D[]>>
        {
            { PlayerState.SMALL, smallPlayerAnimations},
            { PlayerState.BIG, bigPlayerAnimations }
        };

        public static readonly Dictionary<EntitiesName, Dictionary<AnimationState, Texture2D[]>> entityTextures = new Dictionary<EntitiesName, Dictionary<AnimationState, Texture2D[]>>
        {
            { EntitiesName.GOOMBA, goombaAnimations },
            { EntitiesName.FLAG, flagWinAnimations },
            { EntitiesName.FLAGBALL, flagWinBallAnimations },
            { EntitiesName.POLE, poleWinAnimations },
            { EntitiesName.QUESTIONBLOCK,  questionBLockAnimations },
            { EntitiesName.COINBLOCK,  coinBlockAnimations},
            { EntitiesName.BLOCK,  blockAnimations},
            { EntitiesName.DUCTEXTENSION,  ductExtensionAnimations},
            { EntitiesName.DUCT,  ductAnimations},
            { EntitiesName.MUSHROOM,  mushroomAnimations},
            { EntitiesName.FLOWER,  fireFlowerAnimations},
            { EntitiesName.STAR,  superStarAnimations},
            { EntitiesName.KOOPA, koopaAnimations},
            { EntitiesName.BLOCKERBLOCKBROWN, blockedLockAnimations},
            { EntitiesName.COIN, coin}
        };

        public static  Dictionary<AnimationState, Texture2D[]> GetAnimation(EntitiesName entityName)
        {
            return entityTextures[entityName];
        }

        public static  Dictionary<AnimationState, Texture2D[]> GetAnimation(PlayerState playerState)
        {
            return playerTextures[playerState];
        }
    

       



    }
}
