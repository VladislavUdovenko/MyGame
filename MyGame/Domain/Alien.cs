using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame.Domain
{
    public class Alien
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Image CurrentSprite { get; set; }

        public Alien(int x, int y, Image sprite)
        {
            X = x;
            Y = y;
            CurrentSprite = sprite;
        }

        public static Point GetCoordinate()
        {
            var resolution = Screen.PrimaryScreen.Bounds.Size;
            var rnd = new Random();
            var point = new Point();
            if (rnd.Next(0, 2) == 0)
            {
                rnd = new Random();
                if (rnd.Next(0, 2) == 0)
                    point = new Point(resolution.Width, rnd.Next(0, resolution.Height));
                else
                    point = new Point(0, rnd.Next(0, resolution.Height));
            }
            else
            {

                if (rnd.Next(0, 2) == 0)
                    point = new Point(rnd.Next(0, resolution.Width), resolution.Height);
                else
                    point = new Point(rnd.Next(0, resolution.Width), 0);
                
            }
                
            return point;
        }
    }
}
