using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameUlearn
{
    class TimeEvent
    {
        public int TotalTime { get; set; }

        public int AddScore(int scores, int countSpeedDeadZombies,  int countDeadZombies)
        {
            if (TotalTime % 1000 == 0)
                scores += 10;
            scores += countSpeedDeadZombies * 100;
            scores += countDeadZombies * 50;
            return scores;
        }

        public void SpawnZombie(List<Zombie> zombies, Texture2D simpleZombieImg, List<SpeedZombie> speedZombies, Texture2D speedZombieImg)
        {
            if (TotalTime % 5000 == 0 && TotalTime > 1000)
                zombies.Add(new Zombie(simpleZombieImg));

            if (TotalTime % 15000 == 0 && TotalTime > 1000)
                speedZombies.Add(new SpeedZombie(speedZombieImg));
        }

        public void RaiseSpeedForZombie(Zombie zombie)
        {
            if (TotalTime % 2000 == 0)
                zombie.RaiseSpeed();
        }
    }
}
