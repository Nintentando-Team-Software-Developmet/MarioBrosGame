using SuperMarioBros.Utils.DataStructures;

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
        public bool MayTeleport { get; set; }
        public bool IsBig { get; set; }
        public StatusMario statusMario { get; set; }
        public int PlayerPositionX { get; set; }
        public int PlayerPositionY { get; set; }

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
            MayTeleport = false;
            statusMario = StatusMario.SmallMario;
            PlayerPositionX = 100;
            PlayerPositionY = 645;
        }
    }
}
