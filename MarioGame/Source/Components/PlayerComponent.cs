namespace SuperMarioBros.Source.Components
{
    public class PlayerComponent : BaseComponent
    {
        public bool IsAlive { get; set; }
        public int Lives { get; set; }
        public bool colition { get; set; }
        public PlayerComponent()
        {
            IsAlive = true;
            Lives = 3;
            colition = false;
        }
    }
}
