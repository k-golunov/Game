using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    public class Zombie : Entity
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
        }

        public void Move(Player player, Map map)
        {
            /*            Rectangle.X = (int)Position.X;
                        Rectangle.Y = (int)Position.Y;*/
            SetHitBoxSize();
            FindWayToPlayer(player, map);
        }

        public void FindWayToPlayer(Player player, Map map)
        {
            var a = Math.Abs(player.Position.X - Position.X);
            var b = Math.Abs(player.Position.Y - Position.Y);
            if (Math.Sqrt(a * a + b * b) < 1000)
            {
                if (player.Position.X >= Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, map) && LastMoveDirection == "DownRight") return;
                    Position.X += Speed;
                    Position.Y += Speed;
                    LastMoveDirection = "DownRight";
                }

                else if (player.Position.X < Position.X && player.Position.Y < Position.Y)
                {
                    if (Intersected(player, map) && LastMoveDirection == "UpLeft") return;
                    Position.X -= Speed;
                    Position.Y -= Speed;
                    LastMoveDirection = "UpLeft";
                }

                else if (player.Position.X < Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, map) && LastMoveDirection == "DownLeft") return;
                    Position.X -= Speed;
                    Position.Y += Speed;
                    LastMoveDirection = "DownLeft";
                }

                else
                {
                    if (Intersected(player, map) && LastMoveDirection == "UpRight") return;
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
            /*            Rectangle.X = (int)Position.X;
                        Rectangle.Y = (int)Position.Y;*/
            SetHitBoxSize();
        }

        public bool Intersected(Player player, Map map)
        {
            SetHitBoxSize();
            return map.Intersets(Rectangle) || Rectangle.Intersects(player.Rectangle);
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

    public class SpeedZombie : Zombie
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
    }
}

