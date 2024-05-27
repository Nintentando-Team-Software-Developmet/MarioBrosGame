using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MarioGame
{
    public static class Sprites
    {
        //blocks
        public static Texture2D BlockedBlockBrown;
        public static Texture2D BlockedBlockBlue;
        public static Texture2D BlockedBlockGrey;
        public static Texture2D BrickBlockBrown;
        public static Texture2D BrickBlockBlue;
        public static Texture2D BrickBlockGrey;
        public static Texture2D DuctCrossGreen;
        public static Texture2D DuctCrossOrange;
        public static Texture2D DuctCrossColors;
        public static Texture2D DuctSquareGreen;
        public static Texture2D DuctSquareOrange;
        public static Texture2D DuctSquareColors;
        public static Texture2D DuctVerticalGreen;
        public static Texture2D DuctVerticalOrange;
        public static Texture2D DuctVerticalColors;
        public static Texture2D NoIluminatedBrickBlockBrown;
        public static Texture2D NoIluminatedBrickBlockBlue;
        public static Texture2D NoIluminatedBrickBlockGrey;
        public static Texture2D PolishedStoneBlockBrown;
        public static Texture2D PolishedStoneBlockBlue;
        public static Texture2D PolishedStoneBlockGrey;
        public static Texture2D QuestionBlockBrown;
        public static Texture2D QuestionBlockBlue;
        public static Texture2D QuestionBlockGrey;
        public static Texture2D QuestionBlockAny;
        public static Texture2D StoneBlockBrown;
        public static Texture2D StoneBlockBlue;
        public static Texture2D StoneBlockGrey;
        //Enemies
        public static Texture2D Goomba1;
        public static Texture2D Goomba2;
        public static Texture2D Goomba3;
        public static Texture2D Koopa1;
        public static Texture2D Koopa2;
        public static Texture2D Koopa3;
        public static Texture2D Koopa4;
        public static Texture2D Koopa5;
        public static Texture2D Koopa6;
        public static Texture2D PiranhaPlant1;
        public static Texture2D PiranhaPlant2;
        // Props
        public static Texture2D CastleBrown;
        public static Texture2D CastleBlue;
        public static Texture2D WinFlagGreen;
        public static Texture2D WinFlagBrown;
        public static Texture2D WinFlagWhite;

        // Powerups
        public static Texture2D FireBallDown;
        public static Texture2D FireBallExplode1;
        public static Texture2D FireBallExplode2;
        public static Texture2D FireBallExplode3;
        public static Texture2D FireBallLeft;
        public static Texture2D FireBallRight;
        public static Texture2D FireBallUp;
        public static Texture2D FireFlower1;
        public static Texture2D FireFlower2;
        public static Texture2D FireFlower3;
        public static Texture2D GrowMushroom1;
        public static Texture2D GrowMushroom2;
        public static Texture2D GrowMushroom3;
        public static Texture2D SuperStar1;
        public static Texture2D SuperStar2;
        public static Texture2D SuperStar3;

        // Mario Big
        public static Texture2D BigBendLeft;
        public static Texture2D BigBend;
        public static Texture2D BigJumpBackLeft;
        public static Texture2D BigJumpBack;
        public static Texture2D BigRunLeft;
        public static Texture2D BigRun;
        public static Texture2D BigStopLeft;
        public static Texture2D BigStop;
        public static Texture2D BigWalk1Left;
        public static Texture2D BigWalk1;
        public static Texture2D BigWalk2Left;
        public static Texture2D BigWalk2;
        public static Texture2D BigWalk3Left;
        public static Texture2D BigWalk3;

        // Mario Small
        public static Texture2D SmallDie;
        public static Texture2D SmallJumpLeft;
        public static Texture2D SmallJump;
        public static Texture2D SmallRunLeft;
        public static Texture2D SmallRun;
        public static Texture2D SmallStopLeft;
        public static Texture2D SmallStop;
        public static Texture2D SmallWalk1Left;
        public static Texture2D SmallWalk1;
        public static Texture2D SmallWalk2Left;
        public static Texture2D SmallWalk2;
        public static Texture2D SmallWalk3Left;
        public static Texture2D SmallWalk3;

        // Mario Power
        public static Texture2D PowerBendLeft;
        public static Texture2D PowerBend;
        public static Texture2D PowerJumpLeft;
        public static Texture2D PowerJump;
        public static Texture2D PowerRunLeft;
        public static Texture2D PowerRun;
        public static Texture2D PowerStopLeft;
        public static Texture2D PowerStop;
        public static Texture2D PowerWalk1Left;
        public static Texture2D PowerWalk1;
        public static Texture2D PowerWalk2Left;
        public static Texture2D PowerWalk2;
        public static Texture2D PowerWalk3Left;
        public static Texture2D PowerWalk3;

        public static void Load(ContentManager content)
        {
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
            WinFlagGreen = content.Load<Texture2D>("sprites/props/win_flag_1");
            WinFlagBrown = content.Load<Texture2D>("sprites/props/win_flag_2");
            WinFlagWhite = content.Load<Texture2D>("sprites/props/win_flag_3");

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
        }
    }
}
