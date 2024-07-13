using Microsoft.Xna.Framework;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    public interface IScene
    {
        void Load(SpriteData spriteData);
        void Unload();
        void Draw(SpriteData spriteData, GameTime gameTime);
        void Update(GameTime gameTime, SceneManager sceneManager);
        SceneType GetSceneType();
    }
}
