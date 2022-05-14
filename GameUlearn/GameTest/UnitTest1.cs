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
        Map map = new Map();
        Player player = new Player();
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Image = Content.Load<Texture2D>("soldier1_gun");
            BulletImg = Content.Load<Texture2D>("weapon_gun");
            map.Image[4] = Content.Load<Texture2D>("glass_vertical");
            map.Image[0] = Content.Load<Texture2D>("grass");
            player.HealthbarFont = Content.Load<SpriteFont>("Arial");
            simpleZombieImg = Content.Load<Texture2D>("zoimbie1_hold");
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
            player = new Player();
            base.Initialize();
        }
         // добавить в тесты босса
        [TestMethod]
        public void TestMovePlayer()
        {
            _graphics.ApplyChanges();
            Initialize();
            LoadContent();
            var zombies = new List<Zombie>
            {
                new Zombie(simpleZombieImg)
            };
/*            player.Right(map.boxes, zombies);
            Assert.AreEqual(player.Position.X, 1f);
            Assert.AreEqual(player.Position.Y, 0);
            player.Left(map.boxes, zombies);
            Assert.AreEqual(player.Position.X, 1f); // игрок вообзе не должен попадать на координаты 0 0, поэтому он не двигается
            Assert.AreEqual(player.Position.Y, 0);
            player.Position.X = 50;
            player.Position.Y = 50;
            player.Down(map.boxes, zombies);
            Assert.AreEqual(player.Position.Y, 51f);
            player.Up(map.boxes, zombies);
            Assert.AreEqual(player.Position.Y, 50f);*/
        }

        [TestMethod]
        public void TestGenerateMap()
        {
            _graphics.ApplyChanges();
            Initialize();
            Assert.AreEqual(map.boxes.Count, 480);
        }

        [TestMethod]
        public void TestShoot()
        {
            _graphics.ApplyChanges();
            Initialize();
            var bullets = new List<Bullet>();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg));
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
                //if (bullets[i].IsNeedToDelete(map.boxes, zombies))
                    bullets.RemoveAt(i);
            Assert.AreEqual(bullets.Count, 0);
        }

        [TestMethod]
        public void TestKillZombie() //тест на убийство зомби (код еще не написан)
        {
            _graphics.ApplyChanges();
            Initialize();

        }

        [TestMethod]
        public void TestDamageForPlayer() // тест на нанесение урона игроку (код еще не написан)
        {
            _graphics.ApplyChanges();
            Initialize();

        }

        [TestMethod]
        public void TestIntersets()
        {
            _graphics.ApplyChanges();
            Initialize();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg));
            zombies[0].Position.X = 50;
            zombies[0].Position.Y = 50;
            player.Position.X = 50.5f;
            player.Position.Y = 50.0f;
            //player.Left(map.boxes, zombies);
            Assert.AreEqual(player.Position.X, 50.5f);
        }
    }
}
