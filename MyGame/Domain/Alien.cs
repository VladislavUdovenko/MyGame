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
        public int Health { get; set; }
        public PictureBox CurrentSprite { get; set; }

        public Alien(Point point, List<Alien> aliensList, Form form)
        {

            CurrentSprite = new PictureBox();

            CurrentSprite.Left = point.X;
            CurrentSprite.Top = point.Y;

            CurrentSprite.Image = Resource1.AlienGoingDown;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            CurrentSprite.Tag = "alien";

            Health = 3;

            aliensList.Add(this);
            form.Controls.Add(CurrentSprite);
        }

        public void GoToPlayer(Player player)
        {
            var speed = 3;
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

        //public static Point GetCoordinate()
        //{
        //    var rnd = new Random();
        //    var x = rnd.Next(0, 940);
        //    var y = rnd.Next(0, 700);
        //    return new Point(x, y);
        //}

        //public static Point GetCoordinate()
        //{
        //    var resolution = Screen.PrimaryScreen.Bounds.Size;
        //    var rnd = new Random();
        //    var point = new Point();
        //    if (rnd.Next(0, 2) == 0)
        //    {
        //        rnd = new Random();
        //        if (rnd.Next(0, 2) == 0)
        //            point = new Point(resolution.Width, rnd.Next(0, resolution.Height));
        //        else
        //            point = new Point(0, rnd.Next(0, resolution.Height));
        //    }
        //    else
        //    {

        //        if (rnd.Next(0, 2) == 0)
        //            point = new Point(rnd.Next(0, resolution.Width), resolution.Height);
        //        else
        //            point = new Point(rnd.Next(0, resolution.Width), 0);

        //    }

        //    return point;
        //}
    }
}
