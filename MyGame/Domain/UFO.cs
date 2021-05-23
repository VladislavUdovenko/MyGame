using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame.Domain
{
    public class UFO
    {
        public PictureBox CurrentSprite { get; set; }
        public bool IsFueled { get; set; }

        public UFO()
        {
            var resolution = Screen.PrimaryScreen.Bounds.Size;

            CurrentSprite = new PictureBox();
            CurrentSprite.Image = Resource1.UFOWithoutFuel;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            CurrentSprite.Location = new Point(resolution.Width / 3, resolution.Height / 3);

            IsFueled = false;
        }

        public void FlyUp()
        {
            CurrentSprite.Top -= 10;
            CurrentSprite.Left += 30;

            var resolution = Screen.PrimaryScreen.Bounds.Size;
            if (CurrentSprite.Left < 0 || CurrentSprite.Left > resolution.Width || CurrentSprite.Top < 10 || CurrentSprite.Top > resolution.Width)
                CurrentSprite.Dispose();
        }
    }
}
