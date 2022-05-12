using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    class BossLevel1 : Zombie // надо убирать наследование и переписывать с нуля, иначе методы не работают
    {
        List<Bullet> bullets = new List<Bullet>();
        
        private float speed = 0.1f;
        public Texture2D BulletImg;
/*        public Vector2 Position;
        public float Rotation;
        public Rectangle HitBox;
        public Texture2D Image;*/

        public BossLevel1(Texture2D bulletImg)
        {
            BulletImg = bulletImg;            
            Position.X = 250;
            Position.Y = 250;
        }

        public void SetPosition()
        {

        }

        public void Update(int totalGameTime, Player player, Map map, List<Zombie> zombies)
        {
            if (totalGameTime % 2000 == 0)
                bullets.Add(new Bullet(BulletImg, Rotation, Position));
            
            //стрельба в игрока раз в 3 секунды, наносит 20 урона           
            foreach (var bullet in bullets)
                if (bullet.GetRectangle().Intersects(player.Rectangle))
                    player.Healthy -= 20;

            // нужна другая проверка для босса, чтобы игроку наносился урон
            for (var i = bullets.Count - 1; i >= 0; i--)
                if (bullets[i].IsNeedToDelete(map.boxes, zombies)) 
                    bullets.RemoveAt(i);
            ChagneRotation(player);
        }

/*        public void Move(Player player)
        {
            //if (Intersected(player)) return;
            HitBox.X = (int)Position.X;
            HitBox.Y = (int)Position.Y;
            FindWayToPlayer(player);
        }*/

/*        private void FindWayToPlayer(Player player)
        {
            var a = Math.Abs(player.Position.X - Position.X);
            var b = Math.Abs(player.Position.Y - Position.Y);
            if (Math.Sqrt(a * a + b * b) < 400)
            {
                //if (Intersected(player)) return;
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
        }*/


        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in bullets)
                bullet.Draw(spriteBatch);
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 2f, SpriteEffects.None, 1f);
        }

    }
}
