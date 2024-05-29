using SuperMarioBros.Source.Components;

namespace SuperMarioBros.Source.Entities
{
    public class EnemyEntity : Entity
    {
        public EnemyEntity(int id) : base(id)
        {
            AddComponent(new TransformBaseComponent());
            AddComponent(new PhysicsBaseComponent());
            AddComponent(new SpriteBaseComponent());
        }
    }
}
