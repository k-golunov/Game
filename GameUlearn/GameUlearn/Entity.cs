using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GameUlearn
{
    class Entity
    {
        public Texture2D Image;
        public Vector2 Position;
        public float Rotation;
        public int Healthy = 100;
    }

    class Player : Entity
    {
        readonly float speed = 1f;

        public Player() { }
        public Player(Texture2D image, Vector2 position)
        {
            Image = image;
            Position = position;
        }

        // если добавить direction, то передвижение будет зависить от положения мышки
        public void Up()
        {
            if (Position.Y > 1) Position.Y -= /*direction.Y **/ speed;
        }

        public void Down()
        {
            if (Position.Y < 450) Position.Y += /*direction.Y **/ speed; // 450 поменять на высоту окна
        }

        public void Left()
        {
            if (Position.X > 0) Position.X -= /*direction.X **/ speed;
        }

        public void Right()
        {
            if (Position.X < 800) Position.X += /*direction.X **/ speed; // заменить 800 на ширину окна
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
        }
    }

    class Zombie : Entity
    {

    }
}
