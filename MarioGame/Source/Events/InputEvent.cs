namespace Events {
    public class InputEvent : Event {
        public string Input { get; set; }

        public InputEvent(string input) : base("Input") {
            Input = input;
        }
    }
}
