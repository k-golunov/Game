using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    public class Zombie : Entity, IDraw
    {
        public float Speed { get; set; }
        private string LastMoveDirection = "none";

        public Zombie() { }

        public Zombie(Texture2D image, int bossLevel)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
            Speed = 0.5f;
            Healthy = 10 + 5 * bossLevel;
            MaxHealthy = 10 + 5 * bossLevel;
            Damage = 10 + 5 * bossLevel;
        }

        public void Move(Player player, Map map, int scores)
        {
            SetHitBoxSize();
            FindWayToPlayer(player, map, scores);
        }

        public void FindWayToPlayer(Player player, Map map, int scores)
        {
            var a = Math.Abs(player.Position.X - Position.X);
            var b = Math.Abs(player.Position.Y - Position.Y);
            if (Math.Sqrt(a * a + b * b) < 1000)
            {
                if (player.Position.X >= Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, map, scores) && LastMoveDirection == "DownRight") return;
                    Position.X += Speed;
                    Position.Y += Speed;
                    LastMoveDirection = "DownRight";
                }

                else if (player.Position.X < Position.X && player.Position.Y < Position.Y)
                {
                    if (Intersected(player, map, scores) && LastMoveDirection == "UpLeft") return;
                    Position.X -= Speed;
                    Position.Y -= Speed;
                    LastMoveDirection = "UpLeft";
                }

                else if (player.Position.X < Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, map, scores) && LastMoveDirection == "DownLeft") return;
                    Position.X -= Speed;
                    Position.Y += Speed;
                    LastMoveDirection = "DownLeft";
                }

                else
                {
                    if (Intersected(player, map, scores) && LastMoveDirection == "UpRight") return;
                    Position.X += Speed;
                    Position.Y -= Speed;
                    LastMoveDirection = "UpRight";
                }
            }

        }

        public void SetRandomPosition()
        {
            var rand = new Random();
            Position.X = rand.Next(0, 1920);
            Position.Y = rand.Next(0, 1080);
            SetHitBoxSize();
        }

        public bool Intersected(Player player, Map map, int scores)
        {
            SetHitBoxSize();
            
            if (Rectangle.Intersects(player.Rectangle) && TotalTime - PrevTime >= 2000/* && scores % 20 == 0*/)
            {
                player.Healthy -= Damage;
                PrevTime = TotalTime;
                return true;
            }

            return map.Intersets(Rectangle);
        }

        private void SetHitBoxSize()
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        public bool IntersetsWithBullet(List<Bullet> bullets)
        {
            foreach (var bullet in bullets)
            {
                if (Rectangle.Intersects(bullet.GetRectangle()))
                    return true;
            }

            return false;
        }

        public void ChagneRotation(Player player)
        {
            var playerPos = new Vector2(player.Position.X, player.Position.Y);
            var direction = playerPos - Position;
            direction.Normalize();
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
        }

        public void RaiseSpeed()
        {
            if (Speed <= 5f)
                Speed += 0.01f;
        }
    }

    public class SpeedZombie : Zombie, IDraw
    {
        public SpeedZombie(Texture2D image, int bossLevel)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
            Speed = 5.5f;
            Healthy = 10 + 5 * bossLevel;
            MaxHealthy = 10 + 5 * bossLevel;
        }
    
        public void UpdatePosition()
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }
    }
}

