using SuperMarioBros;

namespace MarioGame.Source.Scenes
{
    public interface IScene
    {
        void Load();
        void Unload();
        
        void Draw(SpriteData spriteData);
    }
}