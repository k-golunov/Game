using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    public class Bullet : IDraw
    {
        private readonly Texture2D Image;
        private readonly float Direction;
        private Rectangle rectangle;
        private readonly float _speed = 20f;

        public Bullet(Texture2D image, float direction, Vector2 position)
        {
            Image = image;
            Direction = direction;
            rectangle = new Rectangle((int)position.X, (int)position.Y, 10, 10);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            rectangle.X += (int)(_speed * (float)Math.Cos(Direction));
            rectangle.Y += (int)(_speed * (float)Math.Sin(Direction));

            spriteBatch.Draw(Image, rectangle, null,Color.White, Direction, 
                new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 1f);
        }

        public bool IsNeedToDelete(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            return rectangle.X > 1980 || rectangle.X < 0 || rectangle.Y > 1080 || rectangle.Y < 0 || Intersected(zombies, boss1, map);
        }

        public bool IsNeedToDelete(List<Zombie> zombies, Player player, Map map, int damage)
        {
            return rectangle.X > 1980 || rectangle.X < 0 || rectangle.Y > 1080 || rectangle.Y < 0 || Intersected(zombies, player, map, damage);
        }

        private bool Intersected(List<Zombie> zombies, Player player, Map map, int damage)
        {
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }

            if (rectangle.Intersects(player.Rectangle))
            {
                player.Healthy -= damage;
                return true;
            }

            return map.Intersets(rectangle);
        }
        private bool Intersected(List<Zombie> zombies, BossLevel1 boss1, Map map)
        {
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }
        
            if (rectangle.Intersects(boss1.HitBox))
                return true;
            return map.Intersets(rectangle);
            
        }

        public Rectangle GetRectangle() => rectangle;
        
    }
}
