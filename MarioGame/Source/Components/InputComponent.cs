using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.Xna.Framework.Input;

using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Components
{
    public class InputComponent : BaseComponent
    {
        public InputList A { get; set; }
        public InputList B { get; set; }
        public InputList DOWN { get; set; }
        public InputList LEFT { get; set; }
        public InputList RIGHT { get; set; }

        public InputComponent()
        {
            A = new InputList();
            B = new InputList();
            DOWN = new InputList();
            LEFT = new InputList();
            RIGHT = new InputList();
            LoadDefaultInputSettings();
        }

        private void LoadDefaultInputSettings()
        {
            A.actions = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Z),
                 new DPadInput(Buttons.A),
                });
            B.actions = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.X),
                 new DPadInput(Buttons.B),
                });
            DOWN.actions = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Down),
                 new JoystickInput(DirectionJoystick.DOWN),
                 new DPadInput(Buttons.DPadDown),
                });
            LEFT.actions = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Left),
                 new JoystickInput(DirectionJoystick.LEFT),
                 new DPadInput(Buttons.DPadLeft),
                });
            RIGHT.actions = new ReadOnlyCollection<IActionInput>(
                new List<IActionInput> {
                 new KeyboardInput(Keys.Right),
                 new JoystickInput(DirectionJoystick.RIGHT),
                 new DPadInput(Buttons.DPadRight),
                });
        }

    }
}