using Components;

namespace Entities
{
    public class EnemyEntity : Entity
    {
        public EnemyEntity(int id) : base(id)
        {
            AddComponent(new TransformComponent());
            AddComponent(new PhysicsComponent());
            AddComponent(new SpriteComponent());
        }
    }
}
