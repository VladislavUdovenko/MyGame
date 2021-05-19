using System;
using System.Collections.Generic;
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
            CurrentSprite.Left = random.Next(10, form.ClientSize.Width - CurrentSprite.Width);
            CurrentSprite.Top = random.Next(60, form.ClientSize.Height - CurrentSprite.Height);
            CurrentSprite.Tag = "fuel";
            form.Controls.Add(CurrentSprite);

            //CurrentSprite.BringToFront();
        }
    }
}
