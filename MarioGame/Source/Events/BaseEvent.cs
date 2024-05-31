
namespace SuperMarioBros.Source.Events
{
    public class BaseEvent
    {
        public string Name { get; set; }

        public BaseEvent(string name)
        {
            Name = name;
        }
    }
}
