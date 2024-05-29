namespace SuperMarioBros.Source.Events
{
    public class InputEvent : BaseEvent {
        public string Input { get; set; }

        public InputEvent(string input) : base("Input") {
            Input = input;
        }
    }
}
