namespace MarioGame.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var game = new MarioGame.Game1();
        Assert.NotNull(game);
    }
}
