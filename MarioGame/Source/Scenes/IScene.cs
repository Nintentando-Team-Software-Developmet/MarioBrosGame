using SuperMarioBros;

namespace MarioGame.Source.Scenes
{
    public interface IScene
    {
        void Load(SpriteData spriteData);
        void Unload();

        void Draw(SpriteData spriteData);
    }
}
