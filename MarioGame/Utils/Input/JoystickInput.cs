using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioBros.Utils
{
    public class JoystickInput : IActionInput
    {
        private bool _isReleased { get; set; } = true;
        private DirectionJoystick _direction { get; set; }

        public JoystickInput(DirectionJoystick direction)
        {
            _direction = direction;
        }
        public bool IsPressed()
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            return _direction switch
            {
                DirectionJoystick.UP => gamePadState.ThumbSticks.Left.Y > 0.5f,
                DirectionJoystick.DOWN => gamePadState.ThumbSticks.Left.Y < -0.5f,
                DirectionJoystick.LEFT => gamePadState.ThumbSticks.Left.X < -0.5f,
                DirectionJoystick.RIGHT => gamePadState.ThumbSticks.Left.X > 0.5f,
                _ => false,
            };
        }

        public bool IsReleased()
        {
            return _isReleased;
        }
    }

    public enum DirectionJoystick
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}