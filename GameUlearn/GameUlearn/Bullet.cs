using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    class Bullet
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
            rectangle = new Rectangle((int)position.X, (int)position.Y, 10, 10);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Position += new Vector2(_speed * (float)Math.Cos(Direction), _speed * (float)Math.Sin(Direction));
            rectangle.X += (int)(_speed * (float)Math.Cos(Direction));
            rectangle.Y += (int)(_speed * (float)Math.Sin(Direction));

            spriteBatch.Draw(Image, rectangle, null,Color.White, Direction, new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 1f);
        }

        public bool IsNeedToDelete(List<Box> boxes)
        {
            return rectangle.X > 1980 || rectangle.X < 0 || rectangle.Y > 1080 || rectangle.Y < 0 || Intersected(boxes);
        }

        private bool Intersected(List<Box> boxs)
        {
/*            foreach (var zombie in zombies)
            {
                if (rectangle.Intersects(zombie.Rectangle))
                    return true;
            }*/

            foreach (var box in boxs)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;
            }
            return false;
            
        }
    }
}
