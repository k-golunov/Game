using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    public class BossLevel1 
    {
        public List<Bullet> bullets = new List<Bullet>();
        
        private readonly float speed = 0.5f;
        public Texture2D BulletImg;
        public int Healthy = 1000;
        public Vector2 Position;
        public float Rotation;
        public Rectangle HitBox;
        public Texture2D Image;
        public SpriteFont HealthbarFont;
        public bool Alive = true;

        public BossLevel1()
        {
            //BulletImg = bulletImg;            
            Position.X = 250;
            Position.Y = 250;

        }

        public void SetSizeHitBox()
        {
            HitBox.Width = Image.Width * 2;
            HitBox.Height = Image.Height * 2;
        }

        public void SetPosition()
        {

        }

        public void Update(int totalGameTime, Player player, Map map, List<Zombie> zombies)
        {
            HitBox.X = (int)Position.X;
            HitBox.Y = (int)Position.Y;
            if (totalGameTime % 2000 == 0)
                bullets.Add(new Bullet(BulletImg, Rotation, Position));

            //стрельба в игрока раз в 3 секунды, наносит 20 урона
            // почему-то урон проходит только если босс не двигается (уже не так)
/*            foreach (var bullet in bullets)
                if (player.Rectangle.Intersects(bullet.GetRectangle()))
                    player.Healthy -= 20;*/

            // нужна другая проверка для босса, чтобы игроку наносился урон
            for (var i = bullets.Count - 1; i >= 0; i--)
                if (bullets[i].IsNeedToDelete(map.boxes, zombies, player))
                {
                    bullets.RemoveAt(i);
                }
                    
            ChagneRotation(player);
        }

        public void Move(Player player)
        {
            if (Intersected(player)) return;
            HitBox.X = (int)Position.X;
            HitBox.Y = (int)Position.Y;
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

        public void ChagneRotation(Player player)
        {
            var playerPos = new Vector2(player.Position.X, player.Position.Y);
            var direction = playerPos - Position;
            direction.Normalize();
            Rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }

        public bool Intersected(Player player)
        {
            HitBox.X = (int)Position.X;
            HitBox.Y = (int)Position.Y;
            if (HitBox.Intersects(player.Rectangle/*new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Image.Width, player.Image.Height)*/) /*&& box.NumberTexture == 4*/)
                return true;
            return false;
        }

        public bool IntersetsWithBullet(List<Bullet> bullets)
        {
            foreach (var bullet in bullets)
            {
                if (HitBox.Intersects(bullet.GetRectangle()))
                    return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in bullets)
                bullet.Draw(spriteBatch);
            

            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(HealthbarFont, $"Босс: {Healthy} / 1000",
                            new Vector2(550, 20), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
        }


    }
}
