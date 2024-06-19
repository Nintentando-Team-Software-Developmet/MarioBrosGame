using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class EnemyComponent : BaseComponent
    {
        public bool IsAlive { get; set; }
        public EntitiesName DiedName { get; set; }
        public EnemyComponent( EntitiesName diedName)
        {
            IsAlive = true;
            DiedName = diedName;
        }
    }
}
