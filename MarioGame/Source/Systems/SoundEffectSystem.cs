using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

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
        if (soundEffectEvent != null) PlaySoundEffect(soundEffectEvent.SoundEffectType);
    }

    private static void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        var soundEffect = SoundEffectManager.Instance.GetSoundEffect(soundEffectType);
        var instance = soundEffect.CreateInstance();
        instance.Volume = 0.5f;
        instance.Play();
        instance = null;
    }

    public override void Update(GameTime gameTime, IEnumerable<Entity> entities)
    {
    }
}
