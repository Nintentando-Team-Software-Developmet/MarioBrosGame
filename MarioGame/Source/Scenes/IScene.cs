using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Scenes
{
    public interface IScene
    {
        void Load(SpriteData spriteData);
        void Unload();
        void Draw(SpriteData spriteData);
    }
}
