using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace System {
   public class SystemManager
    {
        private List<ISystem> systems;

        public SystemManager()
        {
            systems = new List<ISystem>();
        }

        public void Update()
        {
            foreach (ISystem system in systems)
            {
                system.Update();
            }
        }
    }
}