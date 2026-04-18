using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace ChessEngine.Audio
{
    internal class AudioManager
    {
        private static Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();

        public static void AddSoundEffect(string name, SoundEffect effect)
        {
            _soundEffects.Add(name, effect);
        }

        public static void PlaySoundEffect(string name)
        {
            _soundEffects[name].Play();
        }
    }
}
