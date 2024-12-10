using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Sound
{
    public class SoundEffectPlayer
    {
        protected SoundEffectInstance soundEffect;

        // value between 0 (muted) and 1 (highest), default is 1
        public float Volume
        {
            get
            {
                return soundEffect.Volume;
            }
            set
            {
                soundEffect.Volume = value;
            }
        }

        // if set to true, sound effect will keep looping
        // either set this to false or call stop method to make sound effect stop looping
        public bool Loop
        {
            get
            {
                return soundEffect.IsLooped;
            }
            set
            {
                soundEffect.IsLooped = value;
            }
        }

        // raises or lowers to pitch of the actual sound
        // default is 0, higher vlaue == the faster and higher pitch; lower value == slower and lower pitch
        // pitch of 1.0f makes pitch normal speed, it's the same as it being 0, idk why
        // range is between -1.0f and 1.0f 
        public float Pitch
        {
            get
            {
                return soundEffect.Pitch;
            }
            set
            {
                soundEffect.Pitch = value;
            }
        }

        // controls the left-to-right panning of the sound (closer to -1.0f sends sound to left, closer to 1.0f sends sound to the right)
        public float Pan
        {
            get
            {
                return soundEffect.Pan;
            }
            set
            {
                soundEffect.Pan = value;
            }
        }

        public SoundEffectPlayer(SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect.CreateInstance();
        }

        public void Play()
        {
            soundEffect.Play();
        }

        // if sound effect is set to loop, this is the only way to make it stop
        public void Stop()
        {
            soundEffect.Stop();
        }
    }
}
