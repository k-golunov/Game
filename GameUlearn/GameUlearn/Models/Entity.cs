using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace GameUlearn
{
    public class Entity
    {
        public Texture2D Image { get; set; }
        public Vector2 Position;
        public float Rotation { get; set; }
        public Rectangle Rectangle;
        public int Healthy { get; set; }
        public int MaxHealthy { get; set; }
        public SpriteFont HealthbarFont { get; set; }
        public double TotalTime { get; set; }
        public double PrevTime { get; set; }
        public int Damage { get; set; }
        public Keys LastKeyToMove = Keys.None;

        public void UpdateFields(int bossLevel, int coef)
        {
            MaxHealthy += coef;
            Healthy = MaxHealthy;
            Damage += 10;
        }
    }

    public class Player : Entity
    {
        public float Speed { get; set; }

        public Player() { }
        public Player(Vector2 position)
        {
            Position = position;
            Speed = 5f;
            Healthy = 100;
            MaxHealthy = 100;
            Damage = 10;
        }

        public void SetSizeHitBox()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Image.Width / 2, Image.Height / 2);
        }

        /*        public void Move(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1, int border, float position)
                {
                    SetHitBoxPosition();
                    // подумать над логикой, как ее передавать в метод, чтобы все делать в одном методе
                }*/

        public void Up(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            SetHitBoxPosition();
            if (Position.Y > 20 && !(Intersected(zombies, boss1, map) && (LastKeyToMove == Keys.A || LastKeyToMove == Keys.D || LastKeyToMove == Keys.W)))
            {
                Position.Y -= Speed;
                LastKeyToMove = Keys.W;
            }
        }

        public void Down(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            SetHitBoxPosition();
            if (Position.Y < 1030 && !(Intersected(zombies, boss1, map) && (LastKeyToMove == Keys.D || LastKeyToMove == Keys.A || LastKeyToMove == Keys.S)))
            {
                Position.Y += Speed;
                LastKeyToMove = Keys.S;
            }
        }

        public void Left(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            SetHitBoxPosition();
            if (Position.X > 20 && !(Intersected(zombies, boss1, map) && (LastKeyToMove == Keys.W || LastKeyToMove == Keys.S || LastKeyToMove == Keys.A)))
            {
                Position.X -= Speed;
                LastKeyToMove = Keys.A;
            }
        }

        public void Right(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            SetHitBoxPosition();
            if (Position.X < 1900 && !(Intersected(zombies, boss1, map) && (LastKeyToMove == Keys.W || LastKeyToMove == Keys.S || LastKeyToMove == Keys.D)))
            {
                Position.X += Speed;
                LastKeyToMove = Keys.D;
            }
        }

        private void SetHitBoxPosition()
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(HealthbarFont, $"Здоровье: {Healthy} / {MaxHealthy}", new Vector2(10, 950), Color.Red);
        }

        private bool Intersected(List<Zombie> zombies, BossLevel1 boss1, Map map) => IntersetsWithZombie(zombies)
            || map.Intersets(Rectangle)
            || boss1.HitBox.Intersects(Rectangle);


        private bool IntersetsWithZombie(List<Zombie> zombies)
        {
            foreach (var zombie in zombies)
            {
                if (Rectangle.Intersects(zombie.Rectangle))
                {
                    if (TotalTime - PrevTime <= 2000 && TotalTime - PrevTime >= 200)
                    {
                        Healthy -= zombie.Damage;
                        PrevTime = TotalTime;
                    }

                    return true;
                }
            }

            return false;
        }

        public void ChagneRotation()
        {
            MouseState mouse = Mouse.GetState();
            var _mousePos = new Vector2(mouse.X, mouse.Y);
            var direction = _mousePos - Position;
            direction.Normalize();
            Rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }
    }
}
