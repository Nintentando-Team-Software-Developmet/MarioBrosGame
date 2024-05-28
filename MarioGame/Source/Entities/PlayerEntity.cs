using Components;

namespace Entities
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity(int id) : base(id)
        {
            AddComponent(new TransformComponent());
            AddComponent(new PhysicsComponent());
            AddComponent(new SpriteComponent());
        }
    }
}
