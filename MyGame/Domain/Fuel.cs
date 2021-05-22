using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame.Domain
{
    class Fuel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PictureBox CurrentSprite { get; set; }

        public Fuel(Form form, Random random)
        {
            CurrentSprite = new PictureBox();
            CurrentSprite.Image = Resource1.Fuel;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            
            X = random.Next(10, form.ClientSize.Width - CurrentSprite.Width);
            Y = random.Next(60, form.ClientSize.Height - CurrentSprite.Height);
            CurrentSprite.Location = new Point(X, Y);
        }
    }
}
