using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameUlearn
{
    class TimeEvent
    {
        public int TotalTime { get; set; }
        public int Scores { get; set; }

        public int AddScore(int scores, int countSpeedDeadZombies,  int countDeadZombies)
        {
            if (TotalTime % 1000 == 0)
                scores += 10;
            scores += countSpeedDeadZombies * 100;
            scores += countDeadZombies * 50;
            return scores;
        }

        public void SpawnZombie(List<Zombie> zombies, Texture2D simpleZombieImg, List<SpeedZombie> speedZombies, Texture2D speedZombieImg, int bossLevel, List<IDraw> draws)
        {
            if (TotalTime % 5000 == 0 && TotalTime > 1000)
            {
                zombies.Add(new Zombie(simpleZombieImg, bossLevel));
                draws.Add(zombies[^1]);
            }


            if (TotalTime % 15000 == 0 && TotalTime > 1000)
            {
                speedZombies.Add(new SpeedZombie(speedZombieImg, bossLevel));
                draws.Add(speedZombies[^1]);
            }
        }

        public void RaiseSpeedForZombie(Zombie zombie)
        {
            if (TotalTime % 2000 == 0)
                zombie.RaiseSpeed();
        }

        public void AddHeartBonus(List<HeartBonus> heartBonuses, Texture2D heartImg, List<IDraw> draws)
        {
            if (TotalTime % 30000 == 0)
            {
                heartBonuses.Add(new HeartBonus(heartImg));
                draws.Add(heartBonuses[^1]);
            } 
        }
    }

    class TimeDraw
    {
        public SpriteBatch SpriteBatch { get; set; }
        public int Scores { get; set; }

        public void DrawTraining(SpriteFont mainFont)
        {
            if (Scores <= 70)
            {
                SpriteBatch.DrawString(mainFont, "Для перемещения используйте WASD",
                    new Vector2(650, 1080 * 0.8f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                SpriteBatch.DrawString(mainFont, "Чтобы стрелять нажмите ЛКМ",
                    new Vector2(650, 1080 * 0.9f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }

            else if (Scores >= 70 && Scores <= 150)
            {
                SpriteBatch.DrawString(mainFont, "Цель игры - не погибнуть от зомби и прожить как можно дольше",
                    new Vector2(650, 1080 * 0.8f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                SpriteBatch.DrawString(mainFont, "Удачи! GL HF",
                    new Vector2(650, 1080 * 0.9f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }
        }

        public void DrawBossInformation(SpriteFont mainFont)
        {
            if (Scores >= 1500 && Scores <= 1700)
            {
                SpriteBatch.DrawString(mainFont, "ВНИМАНИЕ! Появился босс! Босс бросается камнями.",
                    new Vector2(650, 900), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                SpriteBatch.DrawString(mainFont, "Он часто промахивается, но если попадет, то вы потеряете очень много здоровья",
                    new Vector2(650, 950), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }

            if (Scores >= 1700 && Scores <= 2000)
            {
                SpriteBatch.DrawString(mainFont, "Ваша задача уничтожить босса, у него 1000 здоровья!",
                    new Vector2(650, 900), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
            }
        }
    }
}
