namespace SuperMarioBros.Utils
{
    public class DefaultSettingsInput
    {
        public static IActionInput A = new KeyboardInput(Keys.A);
        public static IActionInput B = new KeyboardInput(Keys.S);
        public static IActionInput UP = new KeyboardInput(Keys.W);
        public static IActionInput DOWN = new KeyboardInput(Keys.S);
        public static IActionInput LEFT = new KeyboardInput(Keys.A);
        public static IActionInput RIGHT = new KeyboardInput(Keys.D);
    }
}