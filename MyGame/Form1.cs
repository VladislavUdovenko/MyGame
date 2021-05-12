using MyGame;
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
        public Player Player { get; set; }
        public static Timer Timer { get; set; }
        //public Random randNum = new Random(); // !
        //List<PictureBox> AliensList = new List<PictureBox>(); // !

        public Form1()
        {
            var healthLabel = new Label();
            healthLabel.Text = "Health:";
            healthLabel.Size = new Size(45, 20);

            var healthBar = new ProgressBar();
            healthBar.Size = new Size(180, 20);
            healthBar.Value = 100;

            var fuelLabel = new Label();
            fuelLabel.Text = "Fuel:";
            fuelLabel.Size = new Size(45, 20);

            var fuelBar = new ProgressBar();
            fuelBar.Size = new Size(180, 20);
            fuelBar.Value = 0;

            InitializeComponent();
            InitializeEntities();
            Controls.Add(healthLabel);
            Controls.Add(healthBar);
            Controls.Add(fuelLabel);
            Controls.Add(fuelBar);

            DoubleBuffered = true;

            Paint += (sender, args) =>
            {
                args.Graphics.DrawImage(Player.CurrentSprite.Image, Player.X, Player.Y);
            };

            Timer = new Timer();
            Timer.Interval = 30;
            Timer.Tick += (sender, args) =>
            {
                //if (Player.Health < 1)
                //    timer.Stop();
                if (Player.IsMoving)
                    Player.Move();
                Invalidate();
            };
            Timer.Start();

            SizeChanged += (sender, args) =>
            {
                healthBar.Location = new Point(ClientSize.Width - healthBar.Size.Width, 0);
                healthLabel.Location = new Point(healthBar.Location.X - healthLabel.Size.Width, 0);
                fuelLabel.Location = new Point(0, 0);
                fuelBar.Location = new Point(fuelLabel.Width, 0);
            };

            KeyDown += (sender, args) =>
            {
                Player.IsMoving = true;
                MoveOrStopPlayer(args.KeyCode, 10, true);
                Shoot(args);
            };

            KeyUp += (sender, args) =>
            {
                MoveOrStopPlayer(args.KeyCode, 0, false);
                CheckIfPlayerIsMoving();
            };            
        }

        private void MoveOrStopPlayer(Keys keys, int speed, bool isMoving)
        {
            switch (keys)
            {
                case Keys.W:
                    Player.DirectionY = -speed;
                    Player.CurrentMovement[Player.DirectionMovement.Up] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingUpR;
                    break;
                case Keys.S:
                    Player.DirectionY = speed;
                    Player.CurrentMovement[Player.DirectionMovement.Down] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingDownR;
                    break;
                case Keys.A:
                    Player.DirectionX = -speed;
                    Player.CurrentMovement[Player.DirectionMovement.Left] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingLeftR;
                    break;
                case Keys.D:
                    Player.DirectionX = speed;
                    Player.CurrentMovement[Player.DirectionMovement.Right] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingRightL;
                    break;
            }
        }

        private void Shoot(KeyEventArgs args)
        {
            switch (args.KeyCode)
            {
                case Keys.Right:
                    Player.CurrentSprite.Image = Resource1.DoomGuyShootsRight;
                    Shot(Keys.Right);
                    break;
                case Keys.Left:
                    Player.CurrentSprite.Image = Resource1.DoomGuyShootsLeft;
                    Shot(Keys.Left);
                    break;
                case Keys.Up:
                    Player.CurrentSprite.Image = Resource1.DoomGuyShootsUp;
                    Shot(Keys.Up);
                    break;
                case Keys.Down:
                    Player.CurrentSprite.Image = Resource1.DoomGuyShootsDown;
                    Shot(Keys.Down);
                    break;
            }
        }

        private void Shot(Keys direction)
        {
            var shootBulet = new Bullet();
            shootBulet.Direction = direction;
            shootBulet.X = Player.X + (Player.CurrentSprite.Width / 2);
            shootBulet.Y = Player.Y + (Player.CurrentSprite.Height / 2);
            shootBulet.MakeBullet(this);
        }

        private void CheckIfPlayerIsMoving()
        {
            var movement = Player.CurrentMovement
                            .Where(x => x.Value == true)
                            .Select(x => x.Key);
                            
            if (movement.Count() == 0)
            {
                Player.CurrentSprite.Image = Resource1.DoomGuyStand;
                Player.IsMoving = false;
            }
            else if (movement.First() == Player.DirectionMovement.Up)
                MoveOrStopPlayer(Keys.W, 10, true);
            else if (movement.First() == Player.DirectionMovement.Down)
                MoveOrStopPlayer(Keys.S, 10, true);
            else if (movement.First() == Player.DirectionMovement.Right)
                MoveOrStopPlayer(Keys.D, 10, true);
            else if (movement.First() == Player.DirectionMovement.Left)
                MoveOrStopPlayer(Keys.A, 10, true);
        }

        //private void CreateAliens(int numberAliens)
        //{
        //    for (int i = 0; i < numberAliens; i++)
        //    {
        //        CreateAlien();
        //    }
        //}

        //private void CreateAlien() // !
        //{
        //    var alien = new PictureBox();
        //    alien.Image = Resource1.AlienGoingLeftL;
        //    alien.Left = randNum.Next(0, 900);
        //    alien.Top = randNum.Next(0, 800);
        //    alien.SizeMode = PictureBoxSizeMode.AutoSize;
        //    AliensList.Add(alien);
        //    this.Controls.Add(alien);
        //    Player.CurrentSprite.BringToFront();
        //}

        public void InitializeEntities()
        {
            Player = new Player(ClientSize.Width / 2, ClientSize.Height / 2, new Bitmap(Resource1.DoomGuyStand), new Size(200, 200));
        }
    }
}