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
        KeyboardState prevKeyboardState;
        List<Zombie> zombies = new List<Zombie>();
        SpriteFont mainFont;
        GameState gameState = GameState.Menu;
        MenuOptions option = MenuOptions.Play;
        int optionCounter = 1;
        string gameTitle = "2Д Шутер";
        string gamePlay = "Играть";
        string gameScores = "Очки";
        string gameExit = "Выход";
        List<Zombie> deadZombie = new List<Zombie>();
        int scores = 0;
        Dictionary<DateTime, int> timeAndScores = new Dictionary<DateTime, int>();
        BossLevel1 boss1;

        int OptionsCounter
        {
            get
            {
                return optionCounter;
            }

            set
            {
                if (value > 3)
                    optionCounter = 3;
                if (value < 1)
                    optionCounter = 1;
                else
                    optionCounter = value;

                if (optionCounter == 1)
                    option = MenuOptions.Play;
                else if (optionCounter == 2)
                    option = MenuOptions.Scores;
                else
                    option = MenuOptions.Exit;
            }
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = /*1920;*/GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = /*1080;*/GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.ToggleFullScreen();
            
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player();
            boss1 = new BossLevel1(BulletImg);
            base.Initialize();
        }

        protected override void LoadContent()
        { 
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Image = Content.Load<Texture2D>("soldier1_gun");
            BulletImg = Content.Load<Texture2D>("weapon_gun");
            map.Image[4] = Content.Load<Texture2D>("walls1");
            map.Image[0] = Content.Load<Texture2D>("grass");
            player.HealthbarFont = Content.Load<SpriteFont>("Arial");
            simpleZombieImg = Content.Load<Texture2D>("zoimbie1_hold");
            mainFont = Content.Load<SpriteFont>("Arial");
            boss1.Image = Content.Load<Texture2D>("boss");
            boss1.BulletImg = Content.Load<Texture2D>("weapon_gun");
            map.GenerateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            _graphics.ApplyChanges();
            KeyboardState keyboardState = Keyboard.GetState();
            var totalTime = gameTime.TotalGameTime.TotalMilliseconds;
            MouseState mouse = Mouse.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape) && prevKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (gameState == GameState.Game)
                    gameState = GameState.Menu;
                else
                    gameState = GameState.Game;

            }

            switch (gameState)
            {
                case (GameState.Game):
                    player.TotalTime = totalTime;
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

                    if ((int)totalTime % 1000 == 0)
                        scores += 10;

                    if ((int)totalTime % 10000 == 0)
                        zombies.Add(new Zombie(simpleZombieImg));
                    foreach (var zombie in zombies)
                    {
                        zombie.ChagneRotation(player);
                        zombie.Move(player);
                        if (zombie.IntersetsWithBullet(bullets))
                            deadZombie.Add(zombie);
                    }

                    scores += deadZombie.Count * 50;

                    for (var i = bullets.Count - 1; i >= 0; i--)
                        if (bullets[i].IsNeedToDelete(map.boxes, zombies))
                            bullets.RemoveAt(i);

                    if (deadZombie.Count != 0)
                    {
                        foreach (var dead in deadZombie)
                            zombies.Remove(dead);
                        deadZombie.Clear();
                    }

                    player.ChagneRotation();
                    player.UpdateRectangle();

                    if (player.Healthy <= 0)
                        StartNewGame();


                    // логика босса поменять время с 30 сек на 2 минуты в конце
                    if ((int)totalTime >= 1800)
                    {
                        boss1.Update((int)totalTime, player, map, zombies);
                        boss1.Move(player);
                    }
                    
                        

                    break;

                case (GameState.Menu):
                    if (keyboardState.IsKeyDown(Keys.Up) && prevKeyboardState.IsKeyUp(Keys.Up))
                        OptionsCounter--;
                    if (keyboardState.IsKeyDown(Keys.Down) && prevKeyboardState.IsKeyUp(Keys.Down))
                        OptionsCounter++;
                    if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                    {
                        switch (option)
                        {
                            case MenuOptions.Play:

                                gameState = GameState.Game;
                                break;

                            case MenuOptions.Scores:
                                // добавить отдельное окно игры с очками
                                gameState = GameState.Scores;
                                break;

                            case MenuOptions.Exit:
                                Exit();

                                break;
                        }
                    }


                    break;

                case (GameState.Scores):
                    // добавить словарь с временем и очками игрока, можно запариться и добавить введение ника игрока

                    break;
                
            }

            prevKeyboardState = keyboardState;
            previousMouse = mouse;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGreen);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            switch (gameState)
            {
                case (GameState.Game):
                    player.Draw(_spriteBatch);
                    foreach (var bullet in bullets)
                        bullet.Draw(_spriteBatch);
                    foreach (var box in map.boxes)
                        box.Draw(_spriteBatch);
                    foreach (var zombie in zombies)
                        zombie.Draw(_spriteBatch);
                    _spriteBatch.DrawString(mainFont, $"Очки: {scores}", new Vector2(20, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

                    if (gameTime.TotalGameTime.TotalMilliseconds >= 18000)
                        boss1.Draw(_spriteBatch);

                    // добавить отдельный класс TimeEvent
                    if (gameTime.TotalGameTime.TotalSeconds <= 10)
                    {
                        _spriteBatch.DrawString(mainFont, "Для перемещения используйте WASD", 
                            new Vector2(650, 1080 * 0.8f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                        _spriteBatch.DrawString(mainFont, "Чтобы стрелять нажмите ЛКМ",
                            new Vector2(650, 1080 * 0.9f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                    }

                    else if (gameTime.TotalGameTime.TotalSeconds <= 18 && gameTime.TotalGameTime.TotalSeconds > 8)
                    {
                        _spriteBatch.DrawString(mainFont, "Цель игры - не погибнуть от зомби и прожить как можно дольше",
                            new Vector2(650, 1080 * 0.8f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                        _spriteBatch.DrawString(mainFont, "Удачи! GL HF",
                            new Vector2(650, 1080 * 0.9f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
                    }
                       

                    break;

                case (GameState.Menu):
                    _spriteBatch.DrawString(mainFont, gameTitle, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.1)), Color.White);

                    if (option == MenuOptions.Play)
                        _spriteBatch.DrawString(mainFont, gamePlay, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.4)), Color.Black);
                    else
                        _spriteBatch.DrawString(mainFont, gamePlay, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.4)), Color.White);

                    if (option == MenuOptions.Scores)
                        _spriteBatch.DrawString(mainFont, gameScores, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.45)), Color.Black);
                    else
                        _spriteBatch.DrawString(mainFont, gameScores, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.45)), Color.White);

                    if (option == MenuOptions.Exit)
                        _spriteBatch.DrawString(mainFont, gameExit, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.5)), Color.Black);
                    else
                        _spriteBatch.DrawString(mainFont, gameExit, new Vector2((int)(1980 * 0.45), (int)(1080 * 0.5)), Color.White);

                    break;

                case (GameState.Scores):
                    var y = 1080 * 0.01;
                    foreach (var key in timeAndScores.Keys)
                    {
                        _spriteBatch.DrawString(mainFont, key.ToString(), new Vector2(400, (float)y), Color.White);
                        _spriteBatch.DrawString(mainFont, timeAndScores[key].ToString(), new Vector2(700, (float)y), Color.White);
                    }    

                    break;
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void StartNewGame() // этот метод нужен для создания новой игры
        {
            var time = DateTime.Now;
            timeAndScores[time] = scores;
            bullets.Clear();
            player.Position.X = 20;
            player.Position.Y = 20;
            zombies.Clear();
            player.Healthy = 100;
            scores = 0;
            // в методе нужно обнулять все значения
        }
    }
}
