using SuperMarioBros;

namespace MarioGame.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        using (var game = new Game1())
        {
            Assert.NotNull(game);
        }
    }
}
