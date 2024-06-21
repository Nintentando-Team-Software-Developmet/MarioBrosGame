using SuperMarioBros.Utils;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Components
{
    public class EnemyComponent : BaseComponent
    {
        public bool IsAlive { get; set; }
        public string KilledName { get; set; }
        public float KillTime { get; set; }
        public EnemyComponent(string killedName)
        {
            IsAlive = true;
            KilledName = killedName;
            KillTime = GameConstants.EnemyKillTime;
        }
    }
}
