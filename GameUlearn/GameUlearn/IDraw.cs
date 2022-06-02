using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameUlearn
{
    interface IDraw
    {

        void DrawAllElemts(List<IDraw> list, SpriteBatch spriteBatch)
        {
            foreach (var item in list)
                item.Draw(spriteBatch);
        }

        /*oid DrawAllElements(SpriteBatch spriteBatch) { }*/
        public void Draw(SpriteBatch spriteBatch) { }
    }
}
