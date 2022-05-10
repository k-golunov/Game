using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameUlearn
{
    class BossLevel1 : Zombie
    {

        public BossLevel1()
        {
            var speed = 0.1f;
        }

        public override void SetRandomPosition()
        {
            Position.X = 500;
            Position.Y = 500;
        }

    }
}
