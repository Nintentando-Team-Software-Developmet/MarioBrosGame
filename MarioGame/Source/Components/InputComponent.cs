using Microsoft.Xna.Framework.Input;


namespace SuperMarioBros.Source.Components;

public class InputComponent : BaseComponent
{

    public KeyboardState CurrentKeyboardState { get; set; }
    public GamePadState CurrentGamePadState { get; set; }
}
