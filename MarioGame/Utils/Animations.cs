using Microsoft.Xna.Framework.Graphics;

using SuperMarioBros.Utils;

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
    }
}