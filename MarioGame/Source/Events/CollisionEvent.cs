using SuperMarioBros.Source.Entities;

namespace SuperMarioBros.Source.Events
{
    public class CollisionEvent : BaseEvent
    {
        public Entity Entity1 { get; set; }
        public Entity Entity2 { get; set; }

        public CollisionEvent(Entity entity1, Entity entity2) : base("Collision")
        {
            Entity1 = entity1;
            Entity2 = entity2;
        }
    }
}
