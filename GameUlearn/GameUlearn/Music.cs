using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameUlearn
{
    public class Music
    {
        public SoundEffect Sound;
        public SoundEffectInstance Instance;
        public bool MusicIsStart;

        public void StartMusic()
        {
            if (MusicIsStart == false)
            {
                Instance.Play();
                Instance.Volume = 0.05f;
                MusicIsStart = true;
            }
        }

        public void StopMusic()
        {
            if (MusicIsStart == true)
            {
                Instance.Stop(true);
                MusicIsStart = false;
            }
        }

        public void CreateInstance()
        {
            Instance = Sound.CreateInstance();
        }
    }

}
