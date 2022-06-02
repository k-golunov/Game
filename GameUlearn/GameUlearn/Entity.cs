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
        public int Healthy = 100;
        public SpriteFont HealthbarFont { get; set; }
        public double TotalTime { get; set; }
        public double prevTime { get; set; }
        public Keys LastKeyToMove = Keys.None;
    }

    public class Player : Entity
    {
        public float Speed { get; set; }

        public Player() { }
        public Player(Vector2 position)
        {
            Position = position;
            Speed = 5f;
        }

        public void SetSizeHitBox()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Image.Width / 2, Image.Height / 2);
        }

        public void Move(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1, int border, float position)
        {
            SetHitBoxPosition();
            // подумать над логикой, как ее передавать в метод, чтобы все делать в одном методе
        }

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
            spriteBatch.DrawString(HealthbarFont, $"Здоровье: {Healthy} / 100", new Vector2(10, 950), Color.Red);
        }

        private bool Intersected(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
/*            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;

                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 5)
                    speed = 1f;
                else if (rectangle.Intersects(box.GetRectangle()) && (box.NumberTexture == 1 || box.NumberTexture == 2 || box.NumberTexture == 3))
                    speed = 10f;
                else if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 0)
                    speed = 5f;
            }*/

/*            if (boss1.HitBox.Intersects(rectangle))
                return true;*/

            return IntersetsWithZombie(zombies) || map.Intersets(Rectangle) || boss1.HitBox.Intersects(Rectangle);
        }

        private bool IntersetsWithZombie(List<Zombie> zombies)
        {
            foreach (var zombie in zombies)
            {
                if (Rectangle.Intersects(zombie.Rectangle))
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
        public float Speed { get; set; }
        private string LastMoveDirection = "none";

        public Zombie() { }

        public Zombie(Texture2D image)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
            Speed = 0.5f;
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
        public SpeedZombie(Texture2D image)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
            Rectangle.Width = Image.Width;
            Rectangle.Height = Image.Height;
            Speed = 6f;  
        }
    }
}
