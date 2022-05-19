using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    public class Bullet
    {
        private Texture2D Image;
        private float Direction;
        //private Vector2 Position;
        private Rectangle rectangle;
        private float _speed = 20f;

        public Bullet(Texture2D image, float direction, Vector2 position)
        {
            Image = image;
            Direction = direction;
            //Position = position;
            rectangle = new Rectangle((int)position.X/* + (int)(_speed*3 * (float)Math.Cos(Direction))*/, (int)position.Y /*+ (int)(_speed*3 * (float)Math.Sin(Direction))*/, 10, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Position += new Vector2(_speed * (float)Math.Cos(Direction), _speed * (float)Math.Sin(Direction));
            rectangle.X += (int)(_speed * (float)Math.Cos(Direction));
            rectangle.Y += (int)(_speed * (float)Math.Sin(Direction));

            spriteBatch.Draw(Image, rectangle, null,Color.White, Direction, new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 1f);
        }

        public bool IsNeedToDelete(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        {
            return rectangle.X > 1980 || rectangle.X < 0 || rectangle.Y > 1080 || rectangle.Y < 0 || Intersected(boxes, zombies, boss1);
        }

        public bool IsNeedToDelete(List<Box> boxes, List<Zombie> zombies, Player player)
        {
            return rectangle.X > 1980 || rectangle.X < 0 || rectangle.Y > 1080 || rectangle.Y < 0 || Intersected(boxes, zombies, player);
        }

        private bool Intersected(List<Box> boxes, List<Zombie> zombies, Player player)
        {
/*            if (zombies.Count == 0)
                return false;*/
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }

            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;
            }

            if (rectangle.Intersects(player.Rectangle))
            {
                player.Healthy -= 20;
                return true;
            }

            return false;
        }
        private bool Intersected(List<Box> boxes, List<Zombie> zombies, BossLevel1 boss1)
        {

            /*            if (zombies.Count == 0)
                            return false;
                        foreach (var zombie in zombies)
                        {
                            if (rectangle.Intersects(zombie.Rectangle))
                                return true;
                        }

                        foreach (var box in boxes)
                        {
                            if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                                return true;
                        }

                        if (rectangle.Intersects(player.Rectangle))
                        {
                            player.Healthy -= 20;
                            return true;
                        }*/
            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }

            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;
            }

/*            if (rectangle.Intersects(player.Rectangle))
            {
                player.Healthy -= 20;
                return true;
            }*/

            //return false;
        
            if (rectangle.Intersects(boss1.HitBox))
                return true;
            //return Intersected(boxes, zombies, player);
            return false;
            
        }

        public Rectangle GetRectangle() => rectangle;
        
    }
}
