
using System;

using Microsoft.Xna.Framework.Graphics;
using SuperMarioBros.Source.Systems;
using SuperMarioBros.Source.Scenes;
using Microsoft.Xna.Framework;
using SuperMarioBros;

namespace MarioGame
{
    /** 
    * This class is responsible for managing the game world.
    * It contains a SystemManager and a SceneManager.
    */
    public class WorldGame
    {
        private SystemManager systemManager;
        private SceneManager sceneManager;

        public WorldGame(SpriteData spriteData)
        {
            systemManager = new SystemManager();
            sceneManager = new SceneManager(spriteData);
        }

        public void Initialize()
        {   ;
            sceneManager.AddScene("Menu", new MenuScene());
            sceneManager.setScene("Menu");
        }

        public void Draw()

        {
            sceneManager.DrawScene();
            systemManager.Update();
        }
    }
}