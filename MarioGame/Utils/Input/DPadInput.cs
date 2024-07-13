using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros.Utils
{
    public class DPadInput : IActionInput
    {
        private Buttons _button;

        public DPadInput(Buttons button)
        {
            _button = button;
        }

        public bool IsPressed()
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            return state.IsButtonDown(_button);
        }

        public bool IsReleased()
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            return state.IsButtonUp(_button);
        }
    }
}