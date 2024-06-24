namespace SuperMarioBros.Source.Components
{
    public class PlayerComponent : BaseComponent
    {
        public bool IsAlive { get; set; }
        public int Lives { get; set; }
        public bool colition { get; set; }
        public bool HasReachedEnd { get; set; }
        public bool IsDying { get; set; }
        public float DeathTimer { get; set; }
        public bool DeathAnimationComplete { get; set; }

        public PlayerComponent()
        {
            IsAlive = true;
            Lives = 3;
            colition = false;
            HasReachedEnd = false;
            IsDying = false;
            DeathTimer = 0;
            DeathAnimationComplete = false;
        }
    }
}
