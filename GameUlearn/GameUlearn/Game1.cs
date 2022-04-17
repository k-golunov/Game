using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameUlearn
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 _mousePos;
        private static Player player;
        private Texture2D BulletImg;
        List<Bullet> bullets = new List<Bullet>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1980;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 1080;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.ToggleFullScreen();
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Image = Content.Load<Texture2D>("soldier1_gun");
            BulletImg = Content.Load<Texture2D>("weapon_gun");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _mousePos = new Vector2(mouse.X, mouse.Y);
            var direction = _mousePos - player.Position;
            direction.Normalize();
            player.Rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            // логика перемещения персонажа (игрока)
            if (keyboardState.IsKeyDown(Keys.A))
                player.Left();
            if (keyboardState.IsKeyDown(Keys.D))
                player.Right();
            if (keyboardState.IsKeyDown(Keys.W))
                player.Up();
            if (keyboardState.IsKeyDown(Keys.S))
                player.Down();
            if (keyboardState.IsKeyDown(Keys.LeftControl)) bullets.Add(new Bullet(BulletImg, player.Rotation, player.Position));


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            player.Draw(_spriteBatch);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
