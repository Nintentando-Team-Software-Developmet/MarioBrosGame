using SuperMarioBros;
using SuperMarioBros.Utils.DataStructures;

namespace MarioGame.Source.Scenes
{
    public interface IScene
    {
        void Load();
        void Unload();

        void Draw(SpriteData spriteData);
    }
}
