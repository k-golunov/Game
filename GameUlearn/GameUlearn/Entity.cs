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
        //public Rectangle Rectangle;
        public int Healthy = 100;
        public SpriteFont HealthbarFont;
    }

    class Player : Entity
    {
        readonly float speed = 1f;
        

        public Player() { }
        public Player(Vector2 position)
        {
            Position = position;
        }

        public void Up(List<Box> boxes)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X), (int)(Position.Y - 2 * speed), Image.Width, Image.Height))) return;
            if (Position.Y > 20) Position.Y -= speed;
        }

        public void Down(List<Box> boxes)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X), (int)(Position.Y + 2 * speed), Image.Width, Image.Height))) return;
            if (Position.Y < 1030) Position.Y += speed;
        }

        public void Left(List<Box> boxes)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X - 2* speed), (int)(Position.Y), Image.Width, Image.Height))) return;
            if (Position.X > 20) Position.X -= speed;
        }

        public void Right(List<Box> boxes)
        {
            if (Intersected(boxes, new Rectangle((int)(Position.X + 2 * speed), (int)(Position.Y), Image.Width, Image.Height))) return;
            if (Position.X < 1900) Position.X += speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(HealthbarFont, $"Health: {Healthy} / 100", new Vector2(10, 950), Color.Pink);
        }

        private bool Intersected(List<Box> boxes, Rectangle rectangle) // try create field rectangle for player or/and entity
        {
            foreach (var box in boxes)
            {
                if (rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 4)
                    return true;
            }
            return false;
        }

        public void ChagneRotation()
        {
            MouseState mouse = Mouse.GetState();
            var _mousePos = new Vector2(mouse.X, mouse.Y);
            var direction = _mousePos - Position;
            direction.Normalize();
            Rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);
        }

        private void HealthBar()
        {

        }

        private void Damage()
        {

        }
    }

    class Zombie : Entity
    {
        private readonly float speed = 0.5f;

        public Zombie(Texture2D image)
        {
            Image = image;
            SetRandomPosition();
            Rotation = 1f;
        }

        public void Move(Vector2 playerPosition)
        {

        }

        private void FindWayToPlayer(Vector2 playerPosition)
        {

        }

        public void SetRandomPosition()
        {
            var rand = new Random();
            Position.X = (float)rand.Next(0, 1980);
            Position.Y = (float)rand.Next(0, 1080);
        }

        public bool Intersected()
        {
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, null, Color.White,
                Rotation, new Vector2(Image.Width / 2, Image.Height / 2), 1f, SpriteEffects.None, 1f);
        }

    }
}
