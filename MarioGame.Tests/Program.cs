using System;

using MarioGame;

namespace MarioGame.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using var game = new Game1();
            game.Run();
        }
    }
}
