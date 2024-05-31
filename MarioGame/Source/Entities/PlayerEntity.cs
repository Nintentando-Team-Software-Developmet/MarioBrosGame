using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity(int id) : base(id)
        {
            AddComponent(new TransformBaseComponent());
            AddComponent(new PhysicsBaseComponent());
            AddComponent(new SpriteBaseComponent());
        }
    }
}
