using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros.Utils
{
    public class KeyboardInput : IActionInput
    {
        private Keys _key{ get; set;}

        public KeyboardInput(Keys key)
        {
            _key = key;
        }
        public bool IsPressed()
        {
            return Keyboard.GetState().IsKeyDown(_key);
        }

        public bool IsReleased()
        {
            return Keyboard.GetState().IsKeyUp(_key);
        }
    }
}