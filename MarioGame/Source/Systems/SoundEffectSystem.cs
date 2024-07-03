using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SuperMarioBros.Source.Entities;
using SuperMarioBros.Source.Events;
using SuperMarioBros.Source.Managers;
using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Systems;

public class SoundEffectSystem : BaseSystem
{
    public SoundEffectSystem()
    {
        EventDispatcher.Instance.Subscribe<SoundEffectEvent>(OnSoundEffectEvent);
    }

    private void OnSoundEffectEvent(object eventArgs)
    {
        var soundEffectEvent = eventArgs as SoundEffectEvent;
        Console.WriteLine("Playing sound effect... " + soundEffectEvent.SoundEffectType.ToString());
        if (soundEffectEvent != null) PlaySoundEffect(soundEffectEvent.SoundEffectType);
    }

    private static void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        Console.WriteLine("Playing sound effect... " + soundEffectType.ToString());
        var soundEffect = SoundEffectManager.Instance.GetSoundEffect(soundEffectType);
        soundEffect?.CreateInstance().Play();
    }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
        // No need to update anything per frame
    }
}
