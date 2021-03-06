using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameUlearn
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player player;
        private Texture2D BulletImg;
        Texture2D simpleZombieImg;
        readonly List<Bullet> bullets = new List<Bullet>();
        readonly List<IDraw> objs = new List<IDraw>();
        private readonly Map map = new Map();
        MouseState previousMouse;
        KeyboardState prevKeyboardState;
        readonly List<Zombie> zombies = new List<Zombie>();
        SpriteFont mainFont;
        GameState gameState = GameState.Menu;
        MenuOptions option = MenuOptions.Play;
        int optionCounter = 1;
        readonly string gameTitle = "2Д Шутер";
        readonly string gamePlay = "Играть";
        readonly string gameScores = "Очки";
        readonly string gameExit = "Выход";
        readonly List<Zombie> deadZombie = new List<Zombie>();
        readonly List<SpeedZombie> speedZombies = new List<SpeedZombie>();
        readonly List<SpeedZombie> deadSpeedZombies = new List<SpeedZombie>();
        Texture2D speedZombieImg;
        int scores = 0;
        int prevScores = 0;
        readonly Dictionary<DateTime, int> timeAndScores = new Dictionary<DateTime, int>();
        private BossLevel1 boss1;
        readonly List<HeartBonus> heartBonuses = new List<HeartBonus>();
        private Texture2D heartImg;
        private readonly TimeEvent timeEvent = new TimeEvent();
        private readonly TimeDraw timeDraw = new TimeDraw();
        private readonly Music menuMusic = new Music();
        private readonly Music gameMusic = new Music();

        private int OptionsCounter
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
            //Window.IsBorderless = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1920; //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = 1080; //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //_graphics.ToggleFullScreen();

            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            player = new Player(new Vector2(1000,1000)); 
            boss1 = new BossLevel1();
            base.Initialize();
        }

        protected override void LoadContent()
        { 
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Image = Content.Load<Texture2D>("soldier1_gun");
            BulletImg = Content.Load<Texture2D>("weapon_gun");
            map.Image[4] = Content.Load<Texture2D>("walls1");
            map.Image[1] = Content.Load<Texture2D>("road_left");
            map.Image[2] = Content.Load<Texture2D>("road_centre");
            map.Image[3] = Content.Load<Texture2D>("road_right");
            map.Image[5] = Content.Load<Texture2D>("grunt");
            map.Image[0] = Content.Load<Texture2D>("grass");
            player.HealthbarFont = Content.Load<SpriteFont>("Arial");
            boss1.HealthbarFont = Content.Load<SpriteFont>("Arial"); ;
            simpleZombieImg = Content.Load<Texture2D>("zoimbie1_hold");
            mainFont = Content.Load<SpriteFont>("Arial");
            boss1.Image = Content.Load<Texture2D>("boss");
            boss1.BulletImg = Content.Load<Texture2D>("weapon_gun");
            heartImg = Content.Load<Texture2D>("heart2");
            menuMusic.Sound = Content.Load<SoundEffect>("menuSound");
            menuMusic.CreateInstance();
            speedZombieImg = Content.Load<Texture2D>("speedZombie");
            boss1.SetSizeHitBox();
            player.SetSizeHitBox();
            gameMusic.Sound = Content.Load<SoundEffect>("GameMusic");
            gameMusic.CreateInstance();
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
                    menuMusic.StopMusic();
                    gameMusic.StartMusic();
                    gameMusic.Instance.IsLooped = true;

                    player.TotalTime = totalTime;
                    player.ChagneRotation();

                    if (keyboardState.IsKeyDown(Keys.A))
                        player.Left(zombies, boss1, map);
                    if (keyboardState.IsKeyDown(Keys.D))
                        player.Right(zombies, boss1, map);
                    if (keyboardState.IsKeyDown(Keys.W))
                        player.Up(zombies, boss1, map);
                    if (keyboardState.IsKeyDown(Keys.S))
                        player.Down(zombies, boss1, map);
                    if (mouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton != ButtonState.Pressed)
                    {
                        bullets.Add(new Bullet(BulletImg, player.Rotation, player.Position));
                        objs.Add(bullets[^1]);
                    }
                        
                    map.ChangeSpeedOnBox(player);
                    timeEvent.SpawnZombie(zombies, simpleZombieImg, speedZombies, speedZombieImg, boss1.Level, objs);

                    foreach (var zombie in speedZombies)
                    {
                        zombie.UpdatePosition();
                        zombie.ChagneRotation(player);
                        zombie.Move(player, map, scores);
                        if (zombie.IntersetsWithBullet(bullets))
                            deadSpeedZombies.Add(zombie);
                    }                   
                    
                    foreach (var zombie in zombies)
                    {
                        zombie.TotalTime = (int)totalTime;
                        timeEvent.RaiseSpeedForZombie(zombie);
                        zombie.ChagneRotation(player);
                        zombie.Move(player, map, scores);
                        if (zombie.IntersetsWithBullet(bullets))
                            deadZombie.Add(zombie);
                    }

                    timeEvent.TotalTime = (int)totalTime;
                    scores = timeEvent.AddScore(scores, deadSpeedZombies.Count, deadZombie.Count);
                    if (deadSpeedZombies.Count != 0)
                    {
                        foreach (var dead in deadSpeedZombies)
                        {
                            speedZombies.Remove(dead);
                            objs.Remove(dead);
                        }
                        deadSpeedZombies.Clear();
                    }

                    timeEvent.AddHeartBonus(heartBonuses, heartImg, objs);

                    for (var i = heartBonuses.Count - 1; i >= 0; i--)
                        if (heartBonuses[i].Intersets(player))
                        {
                            objs.Remove(heartBonuses[i]);
                            heartBonuses.RemoveAt(i);
                        }

                    for (var i = bullets.Count - 1; i >= 0; i--)
                    {
                        if (bullets[i].IsNeedToDelete(zombies, boss1, map))
                        {
                            if (boss1.IntersetsWithBullet(bullets))
                                boss1.Healthy -= player.Damage;
                            objs.Remove(bullets[i]);
                            bullets.RemoveAt(i);
                        }
                    }
                    // снова дублирование
                    if (deadZombie.Count != 0)
                    {
                        foreach (var dead in deadZombie)
                        {
                            zombies.Remove(dead);
                            objs.Remove(dead);
                        }
                        deadZombie.Clear();
                    }

                    if (scores - prevScores >= 1500 /*&& boss1.Alive*/)
                    {
                        boss1.SetSizeHitBox();
                        boss1.Move(player);
                        boss1.Update((int)gameTime.TotalGameTime.TotalMilliseconds, player, map, zombies);
                        if (boss1.IntersetsWithBullet(bullets))
                            boss1.Healthy -= player.Damage;
                        if (boss1.Healthy <= 0)
                        {
                            scores += 10000;
                            //boss1.Alive = false;
                            prevScores = scores;
                            boss1.UpdateFields();
                            player.UpdateFields(boss1.Level, 50);
                            boss1.HideHitBox();
                        }
                    }
                    
                    else
                    
                    if (player.Healthy <= 0)
                        gameState = GameState.GameOver;

                    break;

                case (GameState.Menu):
                    menuMusic.StartMusic();
                    gameMusic.StopMusic();

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
                                gameState = GameState.Scores;
                                break;

                            case MenuOptions.Exit:
                                Exit();

                                break;
                        }
                    }

                    break;

                case (GameState.Scores):
                    menuMusic.StopMusic();

                    break;

                case (GameState.GameOver):
                    if (player.Healthy <= 0)
                    {
                        if (keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
                        {
                            gameState = GameState.Menu;
                            StartNewGame();
                        }
                    }
                        
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
                    timeDraw.Scores = scores;
                    timeDraw.SpriteBatch = _spriteBatch;
                    player.Draw(_spriteBatch);
                    map.Draw(_spriteBatch);
                    timeDraw.DrawBossInformation(mainFont);
                    timeDraw.DrawTraining(mainFont);

                    foreach (var item in objs)
                        item.Draw(_spriteBatch);                    

                    _spriteBatch.DrawString(mainFont, $"Очки: {scores}", new Vector2(20, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

                    if (scores - prevScores >= 1500 /*&& boss1.Alive*/)
                        boss1.Draw(_spriteBatch);

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
                        _spriteBatch.DrawString(mainFont, timeAndScores[key].ToString(), new Vector2(900, (float)y), Color.White);
                    }    

                    break;

                case (GameState.GameOver):
                    _spriteBatch.DrawString(mainFont, "К сожалению, вы проиграли.", new Vector2((int)(1980 * 0.35), (int)(1080 * 0.1)), Color.White);
                    _spriteBatch.DrawString(mainFont, "Нажмите Enter чтобы выйти в меню", new Vector2((int)(1980 * 0.35), (int)(1080 * 0.2)), Color.White);
                    break;
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void StartNewGame() 
        {
            var time = DateTime.Now;
            timeAndScores[time] = scores;
            bullets.Clear();
            player.Position.X = 800;
            player.Position.Y = 800;
            zombies.Clear();
            player.Healthy = 100;
            scores = 0;
            boss1.Alive = true;
            boss1.Healthy = 1000;
            heartBonuses.Clear();
            objs.Clear();
            boss1.Level = 0;
            boss1.Healthy = 1000;
        }
    }
}
