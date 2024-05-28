using System.Collections.Generic;

namespace Scenes
{
    public class SceneManager
    {
        private Dictionary<string, Scene> _scenes = new();
        private Scene _currentScene;

        public void AddScene(string name, Scene scene)
        {
            _scenes[name] = scene;
        }

        public void ChangeScene(string name)
        {
            _currentScene?.Unload();
            _currentScene = _scenes[name];
            _currentScene.Load();
        }

        public void LoadScene(string name)
        {
            _currentScene = _scenes[name];
            _currentScene.Load();
        }
    }
}
