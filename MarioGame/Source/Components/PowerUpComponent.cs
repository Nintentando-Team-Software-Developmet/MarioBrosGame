using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Components;

public class PowerUpComponent : BaseComponent
{
    public PowerUpType PowerUpType { get; private set; }


    public PowerUpComponent(Utils.PowerUpType powerUpType)
    {
        PowerUpType = (PowerUpType)powerUpType;
    }
}
