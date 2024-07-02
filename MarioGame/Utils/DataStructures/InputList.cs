using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SuperMarioBros.Utils
{
    public class InputList 
    {
        public ReadOnlyCollection<IActionInput> actions { get; set; }
        public bool IsPressed { get; set; }
        public bool IsReleased { get; set; } = true;
        public bool IsHeld { get; private set; }

        public void setHeld(bool held)
        {
            
            IsHeld = held;
            
        }
    }
}