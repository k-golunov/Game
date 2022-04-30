using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameUlearn;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        [TestMethod]
        public void TestMovePlayer()
        {
            _graphics.ApplyChanges();
            Initialize();
            LoadContent();
            var zombies = new List<Zombie>();
            zombies.Add(new Zombie(simpleZombieImg));
            player.Right(map.boxes, zombies);
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
            Assert.AreEqual(player.Position.Y, 50f);
        }

        [TestMethod]
        public void TestGenerateMap()
        {
            _graphics.ApplyChanges();
            Initialize();
            Assert.AreEqual(map.boxes.Count, 480);
        }
    }
}
