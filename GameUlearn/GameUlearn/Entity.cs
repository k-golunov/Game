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
        public Texture2D Image;
        public Vector2 Position;
        public float Rotation;
        //public Rectangle Rectangle;
        public int Healthy = 100;
        public SpriteFont HealthbarFont;
        public double TotalTime;
        public double prevTime;
    }

    public class Player : Entity
    {
        readonly float speed = 5f;
        

        public Player() { }
        public Player(Vector2 position)
        {
            Position = position;
        }

        // можно добавить в перемещение также проверку по второй координате, тогда, скорее всего,
        //
        // уйдет проблема с невозможностью двигаться в две стороны, ведь по сути граница должна запрещать движение в одну сторону
        public void Up(List<Box> boxes, List<Zombie> zombies)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X + 0.5 * speed), (int)(Position.Y - 0.5 * speed), Image.Width, Image.Height), zombies)) return;
            if (Position.Y > 20) Position.Y -= speed;
        }

        public void Down(List<Box> boxes, List<Zombie> zombies)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X - 0.5 * speed), (int)(Position.Y + 0.5 * speed), Image.Width, Image.Height), zombies)) return;
            if (Position.Y < 1030) Position.Y += speed;
        }

        public void Left(List<Box> boxes, List<Zombie> zombies)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X - 0.5 * speed), (int)(Position.Y + 0.5 * speed), Image.Width, Image.Height), zombies)) return;
            if (Position.X > 20) Position.X -= speed;
        }

        public void Right(List<Box> boxes, List<Zombie> zombies)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X + 0.5 * speed), (int)(Position.Y - 0.5 * speed), Image.Width, Image.Height), zombies)) return;
            if (Position.X < 1900) Position.X += speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(HealthbarFont, $"Healthy: {Healthy} / 100", new Vector2(10, 950), Color.Pink);
        }

        private bool Intersected(List<Box> boxes, Rectangle rectangle, List<Zombie> zombies) // try create field rectangle for player or/and entity
        {
            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;
            }

            return IntersetsWithZombie(rectangle, zombies);

/*            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }

            return false;*/
        }

        private bool IntersetsWithZombie(Rectangle rectangle, List<Zombie> zombies)
        {
            //var prevTime = -1.0;
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                {
                    // урон проходит, но зомби всегда должен пытаться двигаться на игрока
                    // чтобы исправить, надо вызывать метод не только при движении, или наносить урон другим методом, вызывая его постоянно
                    if (TotalTime - prevTime >= 2000)
                    {
                        Damage();
                        prevTime = TotalTime;
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

        private void HealthBar()
        {
            
        }

        private void Damage(/*Object source, ElapsedEventArgs e*/)
        {
                Healthy -= 10;
        }
    }

    public class Zombie : Entity
    {
        private readonly float speed = 0.5f;
        public Rectangle Rectangle;

        public Zombie(Texture2D image)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
        }

        public void Move(Player player)
        {
            if (Intersected(player)) return;
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            FindWayToPlayer(player);
        }

        private void FindWayToPlayer(Player player)
        {
            var a = Math.Abs(player.Position.X - Position.X);
            var b = Math.Abs(player.Position.Y - Position.Y);
            if (Math.Sqrt(a * a + b * b) < 400)
            {
                if (Intersected(player)) return; 
                if (player.Position.X >= Position.X && player.Position.Y >= Position.Y)
                {
                    Position.X += speed;
                    Position.Y += speed;
                }

                else if (player.Position.X < Position.X && player.Position.Y < Position.Y)
                {
                    Position.X -= speed;
                    Position.Y -= speed;
                }

                else if (player.Position.X < Position.X && player.Position.Y >= Position.Y)
                {
                    Position.X -= speed;
                    Position.Y += speed;
                }

                else
                {
                    Position.X += speed;
                    Position.Y -= speed;
                }
            }


        }

        public void SetRandomPosition()
        {
            var rand = new Random();
            Position.X = rand.Next(0, 1920);
            Position.Y = rand.Next(0, 1080);
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        public bool Intersected(Player player)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            if (Rectangle.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Image.Width, player.Image.Height)) /*&& box.NumberTexture == 4*/)
                return true;
            return false;
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
            Rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
        }

    }
}
