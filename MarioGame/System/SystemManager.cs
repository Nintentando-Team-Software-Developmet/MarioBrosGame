using System.Collections.Generic;

namespace System {
   public class SystemManager
    {
        private List<ISystem> systems;

        public SystemManager()
        {
            systems = new List<ISystem>();
        }
    }
}