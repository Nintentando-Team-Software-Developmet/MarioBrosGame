using System;
using System.Collections.Generic;

using MarioGame.Source.Scenes;

namespace SuperMarioBros.Source.Scenes
{
    public class SceneManager
    {
        private Dictionary<string, IScene> _scenes = new();
        private IScene _currentScene;
        private SpriteData _spriteData;

        public SceneManager(SpriteData spriteData)
        {
            this._spriteData = spriteData;
        }

        public void AddScene(string name, IScene scene)
        {
            _scenes[name] = scene;

        }

        public void ChangeScene(string name)
        {
            _currentScene?.Unload();
            _currentScene = _scenes[name];
            _currentScene.Load(this._spriteData);
        }

        public void LoadScene(string name)
        {
            _currentScene = _scenes[name];
            _currentScene.Load(this._spriteData);
        }

        public void DrawScene(){
             _currentScene.Draw(_spriteData);
        }

        public void setScene(string name){
            _currentScene = _scenes[name];
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
