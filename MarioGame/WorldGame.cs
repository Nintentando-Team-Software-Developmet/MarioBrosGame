
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarioGame
{
    public class WorldGame
    {
        private SystemManager systemManager;

        public WorldGame()
        {
            systemManager = new SystemManager();
        }

        public void Draw()
        {
            systemManager.Update();
            // TODO: Draw the world
        }
    }
}