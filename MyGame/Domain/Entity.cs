using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    public class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }

        public bool IsMoving { get; set; }

        public Image CurrentSprite { get; set; }

        public Entity(int x, int y, Image sprite)
        {
            X = x;
            Y = y;
            CurrentSprite = sprite;
        }

        public void Move()
        {
            X += DirectionX;
            Y += DirectionY;
        }
    }
}
