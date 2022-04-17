using System;
using System.Collections.Generic;
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
        private Vector2 Position;
        private float _speed = 2f;

        public Bullet(Texture2D image, float direction, Vector2 position)
        {
            Image = image;
            Direction = direction;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Position += new Vector2(_speed * (float)Math.Cos(Direction), _speed * (float)Math.Sin(Direction));
            spriteBatch.Draw(Image, new Rectangle((int)Position.X, (int)Position.Y, 10, 10), null,Color.White, Direction, new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
