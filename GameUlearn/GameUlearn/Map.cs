using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameUlearn
{
    public class Map
    {
        public Texture2D[] Image = new Texture2D[10];
        private List<Box> boxes = new List<Box>();
        readonly int[,] map = {
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,4 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,4,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,4,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,4,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,4,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            { 0,0,1,2,3,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5 },
            { 0,0,1,2,3,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,5,5 },
            { 0,0,1,2,3,5,5,5,5,0,0,0,0,0,0,0,0,0,4,4,4,4,0,0,0,0,0,0,5,5 },
            { 0,0,1,2,3,5,5,5,5,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
        };

        public void GenerateMap()
        {
            var x = 0;
            var y = 0;
            

            for (var i = 0; i < map.GetLength(0); i++)
            { 
                for (var j = 0; j < map.GetLength(1); j++)
			    {
                    var rect = new Rectangle(x, y, 64, 64);
                    boxes.Add(new Box(Image[map[i,j]], rect, map[i, j]));
                    x += 64;
			    }
                x = 0;
                y += 64;
            }
        }

        public bool Intersets(Rectangle hitBox)
        {
            foreach (var box in boxes)
            {
                if (box.GetRectangle().Intersects(hitBox) && box.NumberTexture == 4)
                    return true;
            }
            return false;
        }

        public void ChangeSpeedOnBox(Player player)
        {
            foreach (var box in boxes)
            {
                if (player.Rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 5)
                    player.Speed = 1f;
                else if (player.Rectangle.Intersects(box.GetRectangle()) && (box.NumberTexture == 1 || box.NumberTexture == 2 || box.NumberTexture == 3))
                    player.Speed = 10f;
                else if (player.Rectangle.Intersects(box.GetRectangle()) && box.NumberTexture == 0)
                    player.Speed = 5f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var box in boxes)
                box.Draw(spriteBatch);
        }
    }

    public class Box
    {
        readonly Texture2D Image;
        readonly Rectangle Rectangle;
        public int NumberTexture { get; set; }

        public Box(Texture2D image, Rectangle rect, int numberTexture)
        {
            Image = image;
            Rectangle = rect;
            NumberTexture = numberTexture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Rectangle, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public Rectangle GetRectangle() => Rectangle;
    }

}
