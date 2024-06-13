namespace SuperMarioBros.Source.Components
{
    public class EnemyComponent : BaseComponent
    {
        public bool IsAlive { get; set; }

        public EnemyComponent()
        {
            IsAlive = true;
        }
    }
}
