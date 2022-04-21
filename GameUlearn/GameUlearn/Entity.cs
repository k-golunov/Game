using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    class Entity
    {
        public Texture2D Image;
        public Vector2 Position;
        public float Rotation;
        public Rectangle Rectangle;
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

        public void Up()
        {
            if (Position.Y > 20) Position.Y -= speed;
        }

        public void Down()
        {
            if (Position.Y < 1030) Position.Y += speed;
        }

        public void Left()
        {
            if (Position.X > 20) Position.X -= speed;
        }

        public void Right()
        {
            if (Position.X < 1900) Position.X += speed;
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
