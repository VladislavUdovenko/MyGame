using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    public class Player
    {
        public enum DirectionMovement
        {
            Up,
            Down,
            Left,
            Right
        }
        public Dictionary<DirectionMovement, bool> CurrentMovement { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int DirectionX { get; set; }
        public int DirectionY { get; set; }

        public bool IsMoving { get; set; }
        public int Health { get; set; }

        public PictureBox CurrentSprite { get; set; }

        public Player(int x, int y, Image sprite, Size size)
        {
            X = x;
            Y = y;

            Health = 100;

            CurrentSprite = new PictureBox();
            CurrentSprite.Image = sprite;
            //CurrentSprite.SizeMode = PictureBoxSizeMode.StretchImage;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            //CurrentSprite.Size = CurrentSprite.Image.Size;
            CurrentSprite.Location = new Point(X, Y);

            CurrentMovement = new Dictionary<DirectionMovement, bool>();
            CurrentMovement[DirectionMovement.Up] = false;
            CurrentMovement[DirectionMovement.Down] = false;
            CurrentMovement[DirectionMovement.Right] = false;
            CurrentMovement[DirectionMovement.Left] = false;
        }

        public void Move()
        {
            // var resolution = Screen.PrimaryScreen.Bounds.Size;
            var resolution = new Size(940, 700);
            if (X + DirectionX < resolution.Width - 30 && X + DirectionX > 0)
                X += DirectionX;
            if (Y + DirectionY < resolution.Height - 55 && Y + DirectionY > 20)
                Y += DirectionY;
            CurrentSprite.Location = new Point(X, Y);
        }
    }
}
