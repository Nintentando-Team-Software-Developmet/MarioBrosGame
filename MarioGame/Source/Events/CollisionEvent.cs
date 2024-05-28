namespace Events
{
    public class CollisionEvent : Event
    {
        public int Entity1 { get; set; }
        public int Entity2 { get; set; }

        public CollisionEvent(int entity1, int entity2) : base("Collision")
        {
            Entity1 = entity1;
            Entity2 = entity2;
        }
    }
}
