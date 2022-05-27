using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    class HeartBonus
    {
        private Vector2 position;
        public Rectangle hitbox;
        private readonly Texture2D Image;
        public bool IsNeedToDelete = false;

        public HeartBonus(Texture2D image)
        {
            SetRandomPosition();
            Image = image;
            hitbox.Width = Image.Width;
            hitbox.Height = Image.Height;
        }

        private void SetRandomPosition()
        {
            var rand = new Random();
            position.X = rand.Next(0, 1920);
            position.Y = rand.Next(0, 1080);
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
        }

        public bool Intersets(Player player)
        {
            if (hitbox.Intersects(player.Rectangle))
            {
                if (player.Healthy <= 70)
                    player.Healthy += 30;
                else
                    player.Healthy = 100;
                IsNeedToDelete = true;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, hitbox, null, Color.White, 0f, new Vector2(Image.Width / 2, Image.Height / 2), SpriteEffects.None, 1f);
        }
    }
}
