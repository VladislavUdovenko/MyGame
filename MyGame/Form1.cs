using MyGame.Domain;
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
            Initialization();

            DoubleBuffered = true;

            var timer = new Timer();
            var currentTick = 0;
            timer.Interval = 50;
            timer.Tick += (sender, args) =>
            {
                if (Player.IsMoving)
                    Player.Move();
                Invalidate();
                currentTick = (currentTick + 1) % 10;
            };
            timer.Start();

            Paint += (sender, args) =>
            {
                var graphics = args.Graphics;
                graphics.DrawImage(Player.CurrentSprite, Player.X, Player.Y);
            };

            KeyDown += (sender, args) =>
            {
                var speed = 10;
                Player.IsMoving = true;
                switch (args.KeyCode)
                {
                    case Keys.W:
                        Player.DirectionY = -speed;
                        Player.CurrentSprite = currentTick < 5
                        ? Resource1.DoomGuyGoingUpR
                        : Resource1.DoomGuyGoingUpL;
                        break;
                    case Keys.S:
                        Player.DirectionY = speed;
                        Player.CurrentSprite = currentTick < 5
                        ? Resource1.DoomGuyGoingDownR
                        : Resource1.DoomGuyGoingDownL;
                        break;
                    case Keys.A:
                        Player.DirectionX = -speed;
                        Player.CurrentSprite = currentTick < 5
                        ? Resource1.DoomGuyRunsToLeftR
                        : Resource1.DoomGuyRunsToLeftL;
                        break;
                    case Keys.D:
                        Player.DirectionX = speed;
                        Player.CurrentSprite = currentTick < 5
                        ? Resource1.DoomGuyRunsToRightR
                        : Resource1.DoomGuyRunsToRightL;
                        break;
                }                
            };

            KeyUp += (sender, args) =>
            {
                Player.DirectionX = 0;
                Player.DirectionY = 0;
                Player.IsMoving = false;
                Player.CurrentSprite = Resource1.DoomGuyStand;
            };
        }

        public void Initialization()
        {
            Player = new Entity(730, 380, new Bitmap(Resource1.DoomGuyShootsDown));
        }
    }
}
