namespace SuperMarioBros.Source.Components
{
    public class PlayerStateComponent : BaseComponent
    {
        public bool IsInvincible { get; set; }
        public bool IsBig { get; set; }
        public bool HasFirePower { get; set; }
        public float InvulnerabilityTimer { get; set; }

        public PlayerStateComponent()
        {
            IsInvincible = false;
            IsBig = false;
            HasFirePower = false;
            InvulnerabilityTimer = 0f;
        }
    }
}
