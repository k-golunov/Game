﻿using System;
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
            if (Position.Y < 1030) Position.Y += speed; // 450 поменять на высоту окна
        }

        public void Left()
        {
            if (Position.X > 20) Position.X -= speed;
        }

        public void Right()
        {
            if (Position.X < 1900) Position.X += /*direction.X **/ speed; // заменить 800 на ширину окна
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

 /*   class Bullet
    {
        public Vector2 Position;
        public Vector2 Direction;
        public Texture2D Image;

        public void Update()
        {
            if (Position.X < 1980 || Position.X > 0
                || Position.Y > 0 || Position.Y < 1080)
                Position += Direction;
        }

        public Bullet(Vector2 position)
        {
            Position = position;
        }

        public bool Hidden
        {
            get
            {
                return Position.X > 1980 || Position.X < 0
                   || Position.Y < 0 || Position.Y > 1080;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Color.White);
        }
    }

    class Shoot
    {
        static List<Bullet> bullets = new List<Bullet>();

        static Player player = new Player();
        public void Shooting()
        {
            bullets.Add(new Bullet(player.GetPositionForFire()));
        }

        public static void Update()
        {
            for (var i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                if (bullets[i].Hidden)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in bullets)
                bullet.Draw(spriteBatch);
        }
    }*/

}
