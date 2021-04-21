using MyGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    public partial class Form1 : Form
    {
        public Entity Player { get; set; }
        public Form1()
        {
            InitializeComponent();
            InitializeEntities();

            DoubleBuffered = true;

            Paint += (sender, args) =>
            {
                args.Graphics.DrawImage(Player.CurrentSprite, Player.X, Player.Y);
            };

            var timer = new Timer();
            var currentTick = 0;
            var maxTick = 4;
            timer.Interval = 50;
            timer.Tick += (sender, args) =>
            {
                if (Player.IsMoving)
                    Player.Move();
                Invalidate();
                currentTick = (currentTick + 1) % maxTick;
            };
            timer.Start();            

            KeyDown += (sender, args) =>
            {
                var speed = 10;
                Player.IsMoving = true;
                MoveCharacter(args, currentTick, speed, maxTick);
            };

            KeyUp += (sender, args) =>
            {
                Player.DirectionX = 0;
                Player.DirectionY = 0;
                Player.IsMoving = false;
                Player.CurrentSprite = Resource1.DoomGuyStand;
            };
        }

        private void MoveCharacter(KeyEventArgs args, int currentTick, int speed, int maxTick)
        {
            var numberTicks = maxTick / 2;
            switch (args.KeyCode)
            {                
                case Keys.W:
                    Player.DirectionY = -speed;
                    Player.CurrentSprite = currentTick < numberTicks
                    ? Resource1.DoomGuyGoingUpR
                    : Resource1.DoomGuyGoingUpL;
                    break;
                case Keys.S:
                    Player.DirectionY = speed;
                    Player.CurrentSprite = currentTick < numberTicks
                    ? Resource1.DoomGuyGoingDownR
                    : Resource1.DoomGuyGoingDownL;
                    break;
                case Keys.A:
                    Player.DirectionX = -speed;
                    Player.CurrentSprite = currentTick < numberTicks
                    ? Resource1.DoomGuyRunsToLeftR
                    : Resource1.DoomGuyRunsToLeftL;
                    break;
                case Keys.D:
                    Player.DirectionX = speed;
                    Player.CurrentSprite = currentTick < numberTicks
                    ? Resource1.DoomGuyRunsToRightR
                    : Resource1.DoomGuyRunsToRightL;
                    break;
            }
        }

        public void InitializeEntities()
        {
            Player = new Entity(730, 380, new Bitmap(Resource1.DoomGuyShootsDown));
        }
    }
}
