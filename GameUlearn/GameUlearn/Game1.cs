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
        private static Player player;
        private Texture2D BulletImg;
        Texture2D simpleZombieImg;
        List<Bullet> bullets = new List<Bullet>();
        private Map map = new Map();
        MouseState previousMouse;
        List<Zombie> zombies = new List<Zombie>();
        GameState gameState = GameState.Menu;
        MenuOptions option = MenuOptions.New;
        int optionCounter = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
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
            map.Image[4] = Content.Load<Texture2D>("glass_vertical");
            map.Image[0] = Content.Load<Texture2D>("grass");
            player.HealthbarFont = Content.Load<SpriteFont>("Arial");
            simpleZombieImg = Content.Load<Texture2D>("zoimbie1_hold");
            map.GenerateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            _graphics.ApplyChanges();
            KeyboardState keyboardState = Keyboard.GetState();
            var totalTime = gameTime.TotalGameTime.TotalMilliseconds;
            MouseState mouse = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.A))
                player.Left(map.boxes, zombies);
            if (keyboardState.IsKeyDown(Keys.D))
                player.Right(map.boxes, zombies);
            if (keyboardState.IsKeyDown(Keys.W))
                player.Up(map.boxes, zombies);
            if (keyboardState.IsKeyDown(Keys.S))
                player.Down(map.boxes, zombies);
            if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                bullets.Add(new Bullet(BulletImg, player.Rotation, player.Position));

            if ((int)totalTime % 10000 == 0)
                zombies.Add(new Zombie(simpleZombieImg));
            foreach (var zombie in zombies)
                zombie.ChagneRotation(player);
            

            player.ChagneRotation();
            for (var i = bullets.Count - 1; i >= 0; i--)
                if (bullets[i].IsNeedToDelete(map.boxes, zombies))
                    bullets.RemoveAt(i);
            

            previousMouse = mouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            player.Draw(_spriteBatch);
            foreach (var bullet in bullets)
                bullet.Draw(_spriteBatch);
            foreach (var box in map.boxes)
                box.Draw(_spriteBatch);            
            foreach (var zombie in zombies)
                zombie.Draw(_spriteBatch);


            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
