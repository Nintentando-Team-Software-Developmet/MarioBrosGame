using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Components
{
    public class InputComponent : BaseComponent
    {
        public ReadOnlyCollection<IActionInput> A { get; private set; }
        public ReadOnlyCollection<IActionInput> B { get; private set; }
        public ReadOnlyCollection<IActionInput> DOWN { get; private set; }
        public ReadOnlyCollection<IActionInput> LEFT { get; private set; }
        public ReadOnlyCollection<IActionInput> RIGHT { get; private set; }

        public InputComponent()
        {
            LoadDefaultInputSettings();
        }

        private void LoadDefaultInputSettings()
        {
            A = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Z),
                 new DPadInput(Buttons.A),
                });
            B = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.X),
                 new DPadInput(Buttons.B),
                });
            DOWN = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Down),
                 new JoystickInput(DirectionJoystick.DOWN),
                 new DPadInput(Buttons.DPadDown),
                });
            LEFT = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Left),
                 new JoystickInput(DirectionJoystick.LEFT),
                 new DPadInput(Buttons.DPadLeft),
                });
            RIGHT = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Right),
                 new JoystickInput(DirectionJoystick.RIGHT),
                 new DPadInput(Buttons.DPadRight),
                });
        }

    }
}