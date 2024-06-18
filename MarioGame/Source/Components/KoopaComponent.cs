using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Components;

public class KoopaComponent : BaseComponent
{
    public bool IsKnocked { get; set; }
    public bool IsReviving { get; set; }
    public float KnockedTime { get; set; }
    public float RevivingTime { get; set; }

    public KoopaComponent()
    {
        IsKnocked = false;
        IsReviving = false;
        KnockedTime = GameConstants.KoopaKnockedTime;
        RevivingTime = GameConstants.KoopaReviveTime;
    }
}
