using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUlearn;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace GameTest
{
    [TestClass]
    public class UnitTest1 : Game
    {
        private Texture2D BulletImg;
        Texture2D simpleZombieImg;
        readonly Map map = new();
        Player player = new();
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        readonly BossLevel1 boss1 = new();
        private Texture2D bonusImg;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Image = Content.Load<Texture2D>("soldier1_gun");
            BulletImg = Content.Load<Texture2D>("weapon_gun");
            map.Image[4] = Content.Load<Texture2D>("walls1");
            map.Image[0] = Content.Load<Texture2D>("grass");
            player.HealthbarFont = Content.Load<SpriteFont>("Arial");
            boss1.HealthbarFont = Content.Load<SpriteFont>("Arial"); ;
            simpleZombieImg = Content.Load<Texture2D>("zoimbie1_hold");
            boss1.Image = Content.Load<Texture2D>("boss");
            boss1.BulletImg = Content.Load<Texture2D>("weapon_gun");
            bonusImg = Content.Load<Texture2D>("heart2");
            boss1.SetSizeHitBox();
            map.GenerateMap();
        }

        public UnitTest1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1980;
            _graphics.PreferredBackBufferHeight = 1080;

            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player(new Vector2(0,0));
            base.Initialize();
        }

        [TestMethod]
        public void TestMovePlayer()
        {
            _graphics.ApplyChanges();
            Initialize();
            LoadContent();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            player.Position.X = 100;
            player.Position.Y = 100;
            player.Down(zombies, boss1, map);
            Assert.AreEqual(player.Position.Y, 105f);
        }

// для теста надо сделать публичную переменную с боксами
/*        [TestMethod]
        public void TestGenerateMap()
        {
            _graphics.ApplyChanges();
            Initialize();
            Assert.AreEqual(map.boxes.Count, 510);
        }*/

        [TestMethod]
        public void TestMovePlayerToWall()
        {
            _graphics.ApplyChanges();
            Initialize();
            LoadContent();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            player.Left(zombies, boss1, map);
            Assert.AreEqual(player.Position.X, 0f); 
            Assert.AreEqual(player.Position.Y, 0f);
        }

        [TestMethod]
        public void TestMoveToWalls()
        {
            _graphics.ApplyChanges();
            Initialize();
            LoadContent();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            player.Right(zombies, boss1, map);
            Assert.AreEqual(player.Position.X, 5f);
            Assert.AreEqual(player.Position.Y, 0);
        }

        [TestMethod]
        public void TestDeleteBullets()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            for (var i = 0; i < 10; i++)
                bullets.Add(new Bullet(BulletImg, 10f, new Vector2(50, 50)));
            Assert.AreEqual(bullets.Count, 10);
            _spriteBatch.Begin();
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            _spriteBatch.End();

            for (var i = bullets.Count - 1; i >= 0; i--)
                if (bullets[i].IsNeedToDelete(zombies, boss1, map))
                    bullets.RemoveAt(i);
            Assert.AreEqual(bullets.Count, 0);
        }

        [TestMethod]
        public void TestShoot()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>();
            for (var i = 0; i < 10; i++)
                bullets.Add(new Bullet(BulletImg, 10f, new Vector2(50, 50)));
            Assert.AreEqual(bullets.Count, 10);
        }

        [TestMethod]
        public void TestKillZombie()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>
            {
                new Bullet(BulletImg, 0f, new Vector2(50, 50))
            };
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            zombies[0].Position.X = 100;
            zombies[0].Position.Y = 50;
            zombies[0].Rectangle.X = 100;
            zombies[0].Rectangle.Y = 50;
            zombies[0].Rectangle.Width = simpleZombieImg.Width;
            zombies[0].Rectangle.Height = simpleZombieImg.Height;
            var countDeadZombie = 0;
            _spriteBatch.Begin();
            while (!zombies[0].IntersetsWithBullet(bullets))
            {
                bullets[0].Draw(_spriteBatch);
                if (zombies[0].IntersetsWithBullet(bullets))
                    countDeadZombie++;
            }
            _spriteBatch.End();
            Assert.AreEqual(countDeadZombie, 1);

        }

        [TestMethod]
        public void TestDamageForPlayer()
        {
            _graphics.ApplyChanges();
            Initialize();
            player.Healthy = 100;
            player.Healthy -= 10;
            Assert.AreEqual(player.Healthy, 90);
        }

        [TestMethod]
        public void TestIntersetsForMovePlayerInZombie()
        {
            _graphics.ApplyChanges();
            Initialize();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg, 0)
            };
            zombies[0].Position.X = 50;
            zombies[0].Position.Y = 50;
            player.Position.X = 20.5f;
            player.Position.Y = 50.0f;
            player.Right(zombies, boss1, map);
            player.Right(zombies, boss1, map);
            Assert.AreEqual(player.Position.X, 25.5f);
        }

        [TestMethod]
        public void TestDamageForBoss()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>();
            boss1.Position.X = 500;
            boss1.Position.Y = 500;
            boss1.SetSizeHitBox();
            bullets.Add(new Bullet(BulletImg, 180f, new Vector2(400, 500)));
            _spriteBatch.Begin();
            while (!boss1.IntersetsWithBullet(bullets))
            {
                bullets[0].Draw(_spriteBatch);
                if (boss1.IntersetsWithBullet(bullets))
                    boss1.Healthy -= 10;
            }
            _spriteBatch.End();
            Assert.AreEqual(boss1.Healthy, 990);
        }

        [TestMethod]
        public void TestKillBoss()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>();
            boss1.Position.X = 500;
            boss1.Position.Y = 500;
            boss1.SetSizeHitBox();
            boss1.Healthy = 10;
            bullets.Add(new Bullet(BulletImg, 180f, new Vector2(400, 500)));
            _spriteBatch.Begin();
            while (!boss1.IntersetsWithBullet(bullets))
            {
                bullets[0].Draw(_spriteBatch);
                if (boss1.IntersetsWithBullet(bullets))
                    boss1.Healthy -= 10;
            }
            _spriteBatch.End();
            if (boss1.Healthy <= 0)
                boss1.Alive = false;
            Assert.AreEqual(boss1.Alive, false);
        }

        [TestMethod]

        public void TestRaiseSpeedZombie()
        {
            _graphics.ApplyChanges();
            Initialize();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg, 1));
            zombies[0].RaiseSpeed();
            Assert.AreEqual(zombies[0].Speed, 0.51f);
        }

        [TestMethod]
        public void TestChangeFieldsPlayer()
        {
            _graphics.ApplyChanges();
            Initialize();
            player.UpdateFields(1, 50);
            Assert.AreEqual(player.Damage, 20);
            Assert.AreEqual(player.MaxHealthy, 150);
            Assert.AreEqual(player.Healthy, 150);
        }

        [TestMethod]
        public void TestChangeFieldZombie()
        {
            _graphics.ApplyChanges();
            Initialize();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg, 2));
            Assert.AreEqual(zombies[0].MaxHealthy, 20);
            Assert.AreEqual(zombies[0].Healthy, 20);
            Assert.AreEqual(zombies[0].Damage, 20);
        }

        [TestMethod]
        public void TestChangeFieldBoss()
        {
            _graphics.ApplyChanges();
            Initialize();
            boss1.UpdateFields();
            Assert.AreEqual(boss1.Damage, 30);
            Assert.AreEqual(boss1.Healthy, 1100);
        }

        [TestMethod]
        public void TestMoveOnRoad()
        {
            _graphics.ApplyChanges();
            Initialize();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg, 2));
            player.Position.X = 200;
            player.Position.Y = 100;
            player.Right(zombies, boss1, map);
            map.ChangeSpeedOnBox(player);
            Assert.AreEqual(player.Speed, 10f);
        }

        [TestMethod]
        public void TestMoveInSand()
        {
            _graphics.ApplyChanges();
            Initialize();
            player.Position.X = 350;
            player.Position.Y = 100;
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg, 2));
            player.Right(zombies, boss1, map);
            map.ChangeSpeedOnBox(player);
            Assert.AreEqual(player.Speed, 1f);
        }

        [TestMethod]
        public void TestHeartBonus()
        {
            _graphics.ApplyChanges();
            Initialize();
            player.Position.X = 500;
            player.Position.Y = 100;
            player.SetSizeHitBox();
            player.Healthy = 30;
            var bonus = new HeartBonus(bonusImg);
            bonus.hitbox.X = 510;
            bonus.hitbox.Y = 100;
            bonus.hitbox.Width = bonusImg.Width;
            bonus.hitbox.Height = bonusImg.Height;
            var a = bonus.Intersets(player);
            Assert.AreEqual(a, true);
            Assert.AreEqual(player.Healthy, 60);
        }

        [TestMethod]
        public void TestHeartBonusWhenHPPreMax()
        {
            _graphics.ApplyChanges();
            Initialize();
            player.Position.X = 500;
            player.Position.Y = 100;
            player.SetSizeHitBox();
            player.Healthy = 80;
            var bonus = new HeartBonus(bonusImg);
            bonus.hitbox.X = 510;
            bonus.hitbox.Y = 100;
            bonus.hitbox.Width = bonusImg.Width;
            bonus.hitbox.Height = bonusImg.Height;
            var a = bonus.Intersets(player);
            Assert.AreEqual(a, true);
            Assert.AreEqual(player.Healthy, 100);
            bonus.Intersets(player);
            Assert.AreEqual(player.Healthy, 100);
        }
    }
}
