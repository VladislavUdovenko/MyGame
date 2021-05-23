using MyGame.Domain;
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
        public bool InsideUFO { get; set; }
        private int health;
        public int Health
        {
            get => health;
            set
            {
                if (value <= 0)
                    health = 0;
                else
                    health = value;
            }
        }

        public PictureBox CurrentSprite { get; set; }

        public Player()
        {
            var resolution = Screen.PrimaryScreen.Bounds.Size;
            X = resolution.Width / 2;
            Y = resolution.Height / 2;

            InsideUFO = false;
            health = 100;

            CurrentSprite = new PictureBox();
            CurrentSprite.Image = Resource1.DoomGuyStand;
            CurrentSprite.SizeMode = PictureBoxSizeMode.AutoSize;
            CurrentSprite.Location = new Point(X, Y);

            CurrentMovement = new Dictionary<DirectionMovement, bool>();
            CurrentMovement[DirectionMovement.Up] = false;
            CurrentMovement[DirectionMovement.Down] = false;
            CurrentMovement[DirectionMovement.Right] = false;
            CurrentMovement[DirectionMovement.Left] = false;
        }

        public void Move(UFO ufo)
        {
            var resolution = Screen.PrimaryScreen.Bounds.Size;

            var piboxX = CurrentSprite;
            piboxX.Left += DirectionX;

            var piboxY = CurrentSprite;            
            piboxY.Top += DirectionY;

            if (X + DirectionX < resolution.Width - 30 && X + DirectionX > 0
                && (!piboxX.Bounds.IntersectsWith(ufo.CurrentSprite.Bounds) || ufo.IsFueled))
                X += DirectionX;
            if (Y + DirectionY < resolution.Height - 120 && Y + DirectionY > 20
                && (!piboxY.Bounds.IntersectsWith(ufo.CurrentSprite.Bounds) || ufo.IsFueled))
                Y += DirectionY;
            CurrentSprite.Location = new Point(X, Y);
        }
    }
}
