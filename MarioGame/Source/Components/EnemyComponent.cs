using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class EnemyComponent : BaseComponent
    {
        public bool IsAlive { get; set; }
        public EntitiesName KilledName { get; set; }
        public EnemyComponent( EntitiesName killedName)
        {
            IsAlive = true;
            KilledName = killedName;
        }
    }
}
