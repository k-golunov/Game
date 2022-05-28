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
        public Rectangle Rectangle;
        public int Healthy = 100;
        public SpriteFont HealthbarFont;
        public double TotalTime;
        public double prevTime;
        public Keys LastKeyToMove = Keys.None;
    }

    public class Player : Entity
    {
        public float speed = 5f;

        public Player() { }
        public Player(Vector2 position)
        {
            Position = position;
        }

        public void SetSizeHitBox()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Image.Width / 2, Image.Height / 2);
        }

        public void Up(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            if (Position.Y > 20 && !(Intersected(boxes, Rectangle, zombies, boss1) && (LastKeyToMove == Keys.A || LastKeyToMove == Keys.D || LastKeyToMove == Keys.W)))
            {
                Position.Y -= speed;
                LastKeyToMove = Keys.W;
            }
        }

        public void Down(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        {           
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            if (Position.Y < 1030 && !(Intersected(boxes, Rectangle, zombies, boss1) && (LastKeyToMove == Keys.D || LastKeyToMove == Keys.A || LastKeyToMove == Keys.S)))
            {
                Position.Y += speed;
                LastKeyToMove = Keys.S;
            } 
        }

        public void Left(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        { 
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;

            if (Position.X > 20 && !(Intersected(boxes, Rectangle, zombies, boss1) && (LastKeyToMove == Keys.W || LastKeyToMove == Keys.S || LastKeyToMove == Keys.A)))
            {
                Position.X -= speed;
                LastKeyToMove = Keys.A;
            }
        }

        public void Right(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            if (Position.X < 1900 && !(Intersected(boxes, Rectangle, zombies, boss1) && (LastKeyToMove == Keys.W || LastKeyToMove == Keys.S || LastKeyToMove == Keys.D)))
            {
                Position.X += speed;
                LastKeyToMove = Keys.D;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(HealthbarFont, $"Здоровье: {Healthy} / 100", new Vector2(10, 950), Color.Red);
        }

        private bool Intersected(List<Box> boxes, Rectangle rectangle, List<Zombie> zombies, BossLevel1 boss1)
        {
            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;

                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 5)
                    speed = 1f;
                else if (rectangle.Intersects(box.GetRectangle()) && (box.NumberTexture == 1 || box.NumberTexture == 2 || box.NumberTexture == 3))
                    speed = 10f;
                else if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 0)
                    speed = 5f;
            }

            if (boss1.HitBox.Intersects(rectangle))
                return true;

            return IntersetsWithZombie(rectangle, zombies);
        }

        private bool IntersetsWithZombie(Rectangle rectangle, List<Zombie> zombies)
        {
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                {
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

        public void Damage()
        {
                Healthy -= 10;
        }
    }

    public class Zombie : Entity
    {
        private float speed = 0.5f;
        private string LastMoveDirection = "none";

        public Zombie() { }

        public Zombie(Texture2D image)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
        }

        public void Move(Player player, List<Box> boxes)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            FindWayToPlayer(player, boxes);
        }

        public void FindWayToPlayer(Player player, List<Box> boxes)
        {
            var a = Math.Abs(player.Position.X - Position.X);
            var b = Math.Abs(player.Position.Y - Position.Y);
            if (Math.Sqrt(a * a + b * b) < 1000)
            {
                if (player.Position.X >= Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, boxes) && LastMoveDirection == "DownRight") return;
                    Position.X += speed;
                    Position.Y += speed;
                    LastMoveDirection = "DownRight";
                }

                else if (player.Position.X < Position.X && player.Position.Y < Position.Y)
                {
                    if (Intersected(player, boxes) && LastMoveDirection == "UpLeft") return;
                    Position.X -= speed;
                    Position.Y -= speed;
                    LastMoveDirection = "UpLeft";
                }

                else if (player.Position.X < Position.X && player.Position.Y >= Position.Y)
                {
                    if (Intersected(player, boxes) && LastMoveDirection == "DownLeft") return;
                    Position.X -= speed;
                    Position.Y += speed;
                    LastMoveDirection = "DownLeft";
                }

                else
                {
                    if (Intersected(player, boxes) && LastMoveDirection == "UpRight") return;
                    Position.X += speed;
                    Position.Y -= speed;
                    LastMoveDirection = "UpRight";
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

        public bool Intersected(Player player, List<Box> boxes)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
            if (Rectangle.Intersects(new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Image.Width, player.Image.Height)) /*&& box.NumberTexture == 4*/)
                return true;
            foreach (var box in boxes)
                if (Rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4) 
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
            Rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
        }

        public void RaiseSpeed()
        {
            if (speed <= 5f)
                speed += 0.01f;
        }

    }
}
