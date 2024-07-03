using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using SuperMarioBros.Utils.DataStructures;

namespace SuperMarioBros.Source.Managers;

public class SoundEffectManager
{
    private static SoundEffectManager _instance;
    private Dictionary<SoundEffectType, SoundEffect> _soundEffects;

    private SoundEffectManager()
    {
        _soundEffects = new Dictionary<SoundEffectType, SoundEffect>();
    }

    public static SoundEffectManager Instance => _instance ??= new SoundEffectManager();

    public void LoadSoundEffect(ContentManager content, SoundEffectType type, string path)
    {
        if (content != null)
        {
            var soundEffect = content.Load<SoundEffect>(path);
            if (!_soundEffects.ContainsKey(type))
            {
                _soundEffects.Add(type, soundEffect);
            }
        }
    }

    public SoundEffect GetSoundEffect(SoundEffectType type)
    {
        return _soundEffects.ContainsKey(type) ? _soundEffects[type] : null;
    }
}
