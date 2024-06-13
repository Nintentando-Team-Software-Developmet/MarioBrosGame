using System;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioBros.Utils
{
    public static class Sprites
    {
        //blocks
        public static Texture2D BlockedBlockBrown { get; set; }
        public static Texture2D BlockedBlockBlue { get; set; }
        public static Texture2D BlockedBlockGrey { get; set; }
        public static Texture2D BrickBlockBrown { get; set; }
        public static Texture2D BrickBlockBlue { get; set; }
        public static Texture2D BrickBlockGrey { get; set; }
        public static Texture2D DuctCrossGreen { get; set; }
        public static Texture2D DuctCrossOrange { get; set; }
        public static Texture2D DuctCrossColors { get; set; }
        public static Texture2D DuctSquareGreen { get; set; }
        public static Texture2D DuctSquareOrange { get; set; }
        public static Texture2D DuctSquareColors { get; set; }
        public static Texture2D DuctVerticalGreen { get; set; }
        public static Texture2D DuctVerticalOrange { get; set; }
        public static Texture2D DuctVerticalColors { get; set; }
        public static Texture2D NoIluminatedBrickBlockBrown { get; set; }
        public static Texture2D NoIluminatedBrickBlockBlue { get; set; }
        public static Texture2D NoIluminatedBrickBlockGrey { get; set; }
        public static Texture2D PolishedStoneBlockBrown { get; set; }
        public static Texture2D PolishedStoneBlockBlue { get; set; }
        public static Texture2D PolishedStoneBlockGrey { get; set; }
        public static Texture2D QuestionBlockBrown { get; set; }
        public static Texture2D QuestionBlockBlue { get; set; }
        public static Texture2D QuestionBlockGrey { get; set; }
        public static Texture2D QuestionBlockAny { get; set; }
        public static Texture2D StoneBlockBrown { get; set; }
        public static Texture2D StoneBlockBlue { get; set; }
        public static Texture2D StoneBlockGrey { get; set; }
        //Enemies
        public static Texture2D Goomba1 { get; set; }
        public static Texture2D Goomba2 { get; set; }
        public static Texture2D Goomba3 { get; set; }
        public static Texture2D Koopa1 { get; set; }
        public static Texture2D Koopa2 { get; set; }
        public static Texture2D Koopa3 { get; set; }
        public static Texture2D Koopa4 { get; set; }
        public static Texture2D Koopa5 { get; set; }
        public static Texture2D Koopa6 { get; set; }
        public static Texture2D PiranhaPlant1 { get; set; }
        public static Texture2D PiranhaPlant2 { get; set; }
        // Props
        public static Texture2D CastleBrown { get; set; }
        public static Texture2D CastleBlue { get; set; }
        public static Texture2D CoinIcon { get; set; }
        public static Texture2D WinFlag{ get; set; }
        public static Texture2D WinFlagGreen { get; set; }
        public static Texture2D WinFlagBrown { get; set; }
        public static Texture2D WinFlagWhite { get; set; }

        public static Texture2D BigBush1 { get; set; }
        public static Texture2D BigBush2 { get; set; }

        public static Texture2D SimpleBush { get; set; }

        public static Texture2D DoubleBush { get; set; }

        public static Texture2D SimpleCloud { get; set; }

        public static Texture2D DoubleCloud { get; set; }

        public static Texture2D TripleCloud { get; set; }

        // Powerups
        public static Texture2D FireBallDown { get; set; }
        public static Texture2D FireBallExplode1 { get; set; }
        public static Texture2D FireBallExplode2 { get; set; }
        public static Texture2D FireBallExplode3 { get; set; }
        public static Texture2D FireBallLeft { get; set; }
        public static Texture2D FireBallRight { get; set; }
        public static Texture2D FireBallUp { get; set; }
        public static Texture2D FireFlower1 { get; set; }
        public static Texture2D FireFlower2 { get; set; }
        public static Texture2D FireFlower3 { get; set; }
        public static Texture2D GrowMushroom1 { get; set; }
        public static Texture2D GrowMushroom2 { get; set; }
        public static Texture2D GrowMushroom3 { get; set; }
        public static Texture2D SuperStar1 { get; set; }
        public static Texture2D SuperStar2 { get; set; }
        public static Texture2D SuperStar3 { get; set; }

        // Mario Big
        public static Texture2D BigBendLeft { get; set; }
        public static Texture2D BigBend { get; set; }
        public static Texture2D BigJumpBackLeft { get; set; }
        public static Texture2D BigJumpBack { get; set; }
        public static Texture2D BigRunLeft { get; set; }
        public static Texture2D BigRun { get; set; }
        public static Texture2D BigStopLeft { get; set; }
        public static Texture2D BigStop { get; set; }
        public static Texture2D BigWalk1Left { get; set; }
        public static Texture2D BigWalk1 { get; set; }
        public static Texture2D BigWalk2Left { get; set; }
        public static Texture2D BigWalk2 { get; set; }
        public static Texture2D BigWalk3Left { get; set; }
        public static Texture2D BigWalk3 { get; set; }
        public static Texture2D LowerThePost1 { get; set; }
        public static Texture2D LowerThePost2 { get; set; }
        public static Texture2D LowerThePostLeft { get; set; }

        // Mario Small
        public static Texture2D SmallDie { get; set; }
        public static Texture2D SmallJumpLeft { get; set; }
        public static Texture2D SmallJump { get; set; }
        public static Texture2D SmallRunLeft { get; set; }
        public static Texture2D SmallRun { get; set; }
        public static Texture2D SmallStopLeft { get; set; }
        public static Texture2D SmallStop { get; set; }
        public static Texture2D SmallWalk1Left { get; set; }
        public static Texture2D SmallWalk1 { get; set; }
        public static Texture2D SmallWalk2Left { get; set; }
        public static Texture2D SmallWalk2 { get; set; }
        public static Texture2D SmallWalk3Left { get; set; }
        public static Texture2D SmallWalk3 { get; set; }

        // Mario Power
        public static Texture2D PowerBendLeft { get; set; }
        public static Texture2D PowerBend { get; set; }
        public static Texture2D PowerJumpLeft { get; set; }
        public static Texture2D PowerJump { get; set; }
        public static Texture2D PowerRunLeft { get; set; }
        public static Texture2D PowerRun { get; set; }
        public static Texture2D PowerStopLeft { get; set; }
        public static Texture2D PowerStop { get; set; }
        public static Texture2D PowerWalk1Left { get; set; }
        public static Texture2D PowerWalk1 { get; set; }
        public static Texture2D PowerWalk2Left { get; set; }
        public static Texture2D PowerWalk2 { get; set; }
        public static Texture2D PowerWalk3Left { get; set; }
        public static Texture2D PowerWalk3 { get; set; }
        public static Texture2D MountainMenu { get; set; }
        public static Texture2D BushMenu { get; set; }
        public static void Load(ContentManager content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content), "ContentManager cannot be null.");
            }
            //blocks
            BlockedBlockBrown = content.Load<Texture2D>("sprites/blocks/blocked_block_1");
            BlockedBlockBlue = content.Load<Texture2D>("sprites/blocks/blocked_block_2");
            BlockedBlockGrey = content.Load<Texture2D>("sprites/blocks/blocked_block_3");
            BrickBlockBrown = content.Load<Texture2D>("sprites/blocks/brick_block_1");
            BrickBlockBlue = content.Load<Texture2D>("sprites/blocks/brick_block_2");
            BrickBlockGrey = content.Load<Texture2D>("sprites/blocks/brick_block_3");
            DuctCrossGreen = content.Load<Texture2D>("sprites/blocks/duct_cross_1");
            DuctCrossOrange = content.Load<Texture2D>("sprites/blocks/duct_cross_2");
            DuctCrossColors = content.Load<Texture2D>("sprites/blocks/duct_cross_3");
            DuctSquareGreen = content.Load<Texture2D>("sprites/blocks/duct_square_1");
            DuctSquareOrange = content.Load<Texture2D>("sprites/blocks/duct_square_2");
            DuctSquareColors = content.Load<Texture2D>("sprites/blocks/duct_square_3");
            DuctVerticalGreen = content.Load<Texture2D>("sprites/blocks/duct_vertical_extension_1");
            DuctVerticalOrange = content.Load<Texture2D>("sprites/blocks/duct_vertical_extension_2");
            DuctVerticalColors = content.Load<Texture2D>("sprites/blocks/duct_vertical_extension_3");
            NoIluminatedBrickBlockBrown = content.Load<Texture2D>("sprites/blocks/no_iluminated_brick_block_1");
            NoIluminatedBrickBlockBlue = content.Load<Texture2D>("sprites/blocks/no_iluminated_brick_block_2");
            NoIluminatedBrickBlockGrey = content.Load<Texture2D>("sprites/blocks/no_iluminated_brick_block_3");
            PolishedStoneBlockBrown = content.Load<Texture2D>("sprites/blocks/polished_stone_block_1");
            PolishedStoneBlockBlue = content.Load<Texture2D>("sprites/blocks/polished_stone_block_2");
            PolishedStoneBlockGrey = content.Load<Texture2D>("sprites/blocks/polished_stone_block_3");
            QuestionBlockBrown = content.Load<Texture2D>("sprites/blocks/question_block_1");
            QuestionBlockBlue = content.Load<Texture2D>("sprites/blocks/question_block_2");
            QuestionBlockGrey = content.Load<Texture2D>("sprites/blocks/question_block_3");
            QuestionBlockAny = content.Load<Texture2D>("sprites/blocks/question_block_any");
            StoneBlockBrown = content.Load<Texture2D>("sprites/blocks/stone_block_1");
            StoneBlockBlue = content.Load<Texture2D>("sprites/blocks/stone_block_2");
            StoneBlockGrey = content.Load<Texture2D>("sprites/blocks/stone_block_3");

            // Enemies
            Goomba1 = content.Load<Texture2D>("sprites/Enemies/Goomba/Goomba1");
            Goomba2 = content.Load<Texture2D>("sprites/Enemies/Goomba/Goomba2");
            Goomba3 = content.Load<Texture2D>("sprites/Enemies/Goomba/Goomba3");
            Koopa1 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa1");
            Koopa2 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa2");
            Koopa3 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa3");
            Koopa4 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa4");
            Koopa5 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa5");
            Koopa6 = content.Load<Texture2D>("sprites/Enemies/Koopa/Koopa6");
            PiranhaPlant1 = content.Load<Texture2D>("sprites/Enemies/PiranhaPlant/Plant1");
            PiranhaPlant2 = content.Load<Texture2D>("sprites/Enemies/PiranhaPlant/Plant2");

            // Props
            CastleBrown = content.Load<Texture2D>("sprites/props/castle_1");
            CastleBlue = content.Load<Texture2D>("sprites/props/castle_2");
            CoinIcon = content.Load<Texture2D>("sprites/props/coin_icon_1");
            WinFlagGreen = content.Load<Texture2D>("sprites/props/win_flag_1");
            WinFlagBrown = content.Load<Texture2D>("sprites/props/win_flag_2");
            WinFlagWhite = content.Load<Texture2D>("sprites/props/win_flag_3");
            WinFlag = content.Load<Texture2D>("sprites/props/flag");
            BigBush1 = content.Load<Texture2D>("sprites/props/big_bush_1");
            BigBush2 = content.Load<Texture2D>("sprites/props/big_bush_2");
            SimpleBush = content.Load<Texture2D>("sprites/props/simple_bush");
            DoubleBush = content.Load<Texture2D>("sprites/props/double_bush");
            SimpleCloud = content.Load<Texture2D>("sprites/props/simple_cloud");
            DoubleCloud = content.Load<Texture2D>("sprites/props/double_cloud");
            TripleCloud = content.Load<Texture2D>("sprites/props/triple_cloud");

            // Powerups
            FireBallDown = content.Load<Texture2D>("sprites/powerups/fire_ball_down");
            FireBallExplode1 = content.Load<Texture2D>("sprites/powerups/fire_ball_explode_1");
            FireBallExplode2 = content.Load<Texture2D>("sprites/powerups/fire_ball_explode_2");
            FireBallExplode3 = content.Load<Texture2D>("sprites/powerups/fire_ball_explode_3");
            FireBallLeft = content.Load<Texture2D>("sprites/powerups/fire_ball_left");
            FireBallRight = content.Load<Texture2D>("sprites/powerups/fire_ball_rigth");
            FireBallUp = content.Load<Texture2D>("sprites/powerups/fire_ball_up");
            FireFlower1 = content.Load<Texture2D>("sprites/powerups/fire_flower_1");
            FireFlower2 = content.Load<Texture2D>("sprites/powerups/fire_flower_2");
            FireFlower3 = content.Load<Texture2D>("sprites/powerups/fire_flower_3");
            GrowMushroom1 = content.Load<Texture2D>("sprites/powerups/grow_mushroom_1");
            GrowMushroom2 = content.Load<Texture2D>("sprites/powerups/grow_mushroom_2");
            GrowMushroom3 = content.Load<Texture2D>("sprites/powerups/grow_mushroom_3");
            SuperStar1 = content.Load<Texture2D>("sprites/powerups/super_star_1");
            SuperStar2 = content.Load<Texture2D>("sprites/powerups/super_star_2");
            SuperStar3 = content.Load<Texture2D>("sprites/powerups/super_star_3");

            // Mario Big
            BigBendLeft = content.Load<Texture2D>("sprites/Mario/Big/Bend_l");
            BigBend = content.Load<Texture2D>("sprites/Mario/Big/Bend");
            BigJumpBackLeft = content.Load<Texture2D>("sprites/Mario/Big/Jump_b_l");
            BigJumpBack = content.Load<Texture2D>("sprites/Mario/Big/Jump_b");
            BigRunLeft = content.Load<Texture2D>("sprites/Mario/Big/RunB_l");
            BigRun = content.Load<Texture2D>("sprites/Mario/Big/RunB");
            BigStopLeft = content.Load<Texture2D>("sprites/Mario/Big/StoppedB_l");
            BigStop = content.Load<Texture2D>("sprites/Mario/Big/StoppedB");
            BigWalk1Left = content.Load<Texture2D>("sprites/Mario/Big/Walk_1_l");
            BigWalk1 = content.Load<Texture2D>("sprites/Mario/Big/Walk_1");
            BigWalk2Left = content.Load<Texture2D>("sprites/Mario/Big/Walk_2_l");
            BigWalk2 = content.Load<Texture2D>("sprites/Mario/Big/Walk_2");
            BigWalk3Left = content.Load<Texture2D>("sprites/Mario/Big/Walk_3_l");
            BigWalk3 = content.Load<Texture2D>("sprites/Mario/Big/Walk_3");
            LowerThePost1 = content.Load<Texture2D>("sprites/Mario/Big/LowerThePost_1");
            LowerThePost2 = content.Load<Texture2D>("sprites/Mario/Big/LowerThePost_2");
            LowerThePostLeft = content.Load<Texture2D>("sprites/Mario/Big/LowerThePost_l");

            // Mario Small
            SmallDie = content.Load<Texture2D>("sprites/Mario/Small/Die");
            SmallJumpLeft = content.Load<Texture2D>("sprites/Mario/Small/Jump_l");
            SmallJump = content.Load<Texture2D>("sprites/Mario/Small/Jump");
            SmallRunLeft = content.Load<Texture2D>("sprites/Mario/Small/Run_l");
            SmallRun = content.Load<Texture2D>("sprites/Mario/Small/Run");
            SmallStopLeft = content.Load<Texture2D>("sprites/Mario/Small/stopped_l");
            SmallStop = content.Load<Texture2D>("sprites/Mario/Small/Stopped");
            SmallWalk1Left = content.Load<Texture2D>("sprites/Mario/Small/Walk1_l");
            SmallWalk1 = content.Load<Texture2D>("sprites/Mario/Small/Walk1");
            SmallWalk2Left = content.Load<Texture2D>("sprites/Mario/Small/Walk2_l");
            SmallWalk2 = content.Load<Texture2D>("sprites/Mario/Small/Walk2");
            SmallWalk3Left = content.Load<Texture2D>("sprites/Mario/Small/Walk3_l");
            SmallWalk3 = content.Load<Texture2D>("sprites/Mario/Small/Walk3");

            // Mario Power
            PowerBendLeft = content.Load<Texture2D>("sprites/Mario/Power/Bend_l");
            PowerBend = content.Load<Texture2D>("sprites/Mario/Power/Bend");
            PowerJumpLeft = content.Load<Texture2D>("sprites/Mario/Power/Jump_p_l");
            PowerJump = content.Load<Texture2D>("sprites/Mario/Power/jump_p");
            PowerRunLeft = content.Load<Texture2D>("sprites/Mario/Power/Run_l");
            PowerRun = content.Load<Texture2D>("sprites/Mario/Power/Run");
            PowerStopLeft = content.Load<Texture2D>("sprites/Mario/Power/Stopped_l");
            PowerStop = content.Load<Texture2D>("sprites/Mario/Power/Stopped");
            PowerWalk1Left = content.Load<Texture2D>("sprites/Mario/Power/Walk-1_l");
            PowerWalk1 = content.Load<Texture2D>("sprites/Mario/Power/Walk-1");
            PowerWalk2Left = content.Load<Texture2D>("sprites/Mario/Power/Walk-2_l");
            PowerWalk2 = content.Load<Texture2D>("sprites/Mario/Power/Walk-2");
            PowerWalk3Left = content.Load<Texture2D>("sprites/Mario/Power/Walk-3_l");
            PowerWalk3 = content.Load<Texture2D>("sprites/Mario/Power/Walk-3");

            MountainMenu = content.Load<Texture2D>("sprites/props/mountain");
            BushMenu = content.Load<Texture2D>("sprites/props/bush");
        }
    }
}
