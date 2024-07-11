using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Events;

public class SoundEffectEvent : BaseEvent
{
    public SoundEffectType SoundEffectType { get; }

    public SoundEffectEvent(SoundEffectType soundEffectType) : base(soundEffectType.ToString())
    {
        SoundEffectType = soundEffectType;
    }
}
