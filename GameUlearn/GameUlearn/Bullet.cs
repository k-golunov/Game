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
        public Texture2D Image;
        public float Direction;
        public Vector2 Position;
        private float _speed = 1f;

        public Bullet(Texture2D image, float direction, Vector2 position)
        {
            Image = image;
            Direction = direction;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Position.X -= _speed;
            Position.Y -= _speed;
            spriteBatch.Draw(Image, new Rectangle((int)Position.X, (int)Position.Y, 10, 10), null,Color.White, Direction, new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 0f);
        }
    }
}
