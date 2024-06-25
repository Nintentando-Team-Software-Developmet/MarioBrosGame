using SuperMarioBros.Source.Entities;
using SuperMarioBros.Utils;

namespace SuperMarioBros.Source.Events;

public class PowerUpEvent : BaseEvent
{
    public Entity Player { get; }
    public Entity PowerUp { get; }
    public PowerUpType PowerUpType { get;  }

    public PowerUpEvent(Entity player, Entity powerUp, PowerUpType powerUpType) : base(powerUpType.ToString() + " PowerUp Event")
    {
        Player = player;
        PowerUp = powerUp;
        PowerUpType = powerUpType;
    }
}
