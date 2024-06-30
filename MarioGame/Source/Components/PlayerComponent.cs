using SuperMarioBros.Utils;

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
        public bool ShouldProcessDeath { get; set; }
        public bool IsTimeOver { get; set; }
        //TODO: provitional Refactor 
        public PlayerState State { get; set; }

        public PlayerComponent()
        {
            IsAlive = true;
            Lives = 3;
            colition = false;
            HasReachedEnd = false;
            IsDying = false;
            DeathTimer = 0;
            DeathAnimationComplete = false;
            ShouldProcessDeath = false;
            IsTimeOver = false;
            //TODO: Refactor
            State = PlayerState.SMALL;
        }
    }
}
