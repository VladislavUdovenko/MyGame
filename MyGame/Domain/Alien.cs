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
        public int Health { get; set; }
        public PictureBox CurrentSprite { get; set; }
        public bool AlienCanGo { get; set; }

        public Alien(Random random)
        {
            Health = 2;

            CurrentSprite = new PictureBox();
            CurrentSprite.Image = Resource1.AlienGoingDown;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            CurrentSprite.Location = GetCoordinate(random);

            AlienCanGo = true;
        }

        public void GoToPlayer(Player player)
        {
            if (!AlienCanGo)
                return;
            var speed = 5;
            if (CurrentSprite.Top > player.CurrentSprite.Top)
            {
                CurrentSprite.Top -= speed;
                CurrentSprite.Image = Resource1.AlienGoingUp;
            }
            if (CurrentSprite.Top < player.CurrentSprite.Top)
            {
                CurrentSprite.Top += speed;
                CurrentSprite.Image = Resource1.AlienGoingDown;
            }
            if (CurrentSprite.Left > player.CurrentSprite.Left)
            {
                CurrentSprite.Left -= speed;
                CurrentSprite.Image = Resource1.AlienGoingLeft;
            }
            if (CurrentSprite.Left < player.CurrentSprite.Left)
            {
                CurrentSprite.Left += speed;
                CurrentSprite.Image = Resource1.AlienGoingRight;
            }
        }

        public static Point GetCoordinate(Random random)
        {
            var resolution = Screen.PrimaryScreen.Bounds.Size;
            Point point;
            if (random.Next(0, 2) == 0)
            {
                if (random.Next(0, 2) == 0)
                    point = new Point(resolution.Width + 100, random.Next(-100, resolution.Height + 100)); // right wall
                else
                    point = new Point(-100, random.Next(-100, resolution.Height + 100)); // left wall
            }
            else
            {

                if (random.Next(0, 2) == 0)
                    point = new Point(random.Next(-100, resolution.Width + 100), resolution.Height + 100); // lower wall
                else
                    point = new Point(random.Next(-100, resolution.Width + 100), -100); // upper wall
            }

            return point;
        }
    }
}
