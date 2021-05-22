using MyGame.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    public partial class Form1 : Form
    {
        public Player Player { get; set; }
        public static Timer Timer { get; set; }
        public Random Random { get; set; }
        static List<Alien> AliensList { get; set; } // new
        static List<Fuel> FuelsList { get; set; } // new

        public Form1()
        {
            #region Инициализация иконок
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
            #endregion

            DoubleBuffered = true;

            Paint += (sender, args) =>
            {
                args.Graphics.DrawImage(Player.CurrentSprite.Image, Player.X, Player.Y);
                foreach (var fuel in FuelsList)
                    args.Graphics.DrawImage(fuel.CurrentSprite.Image, fuel.CurrentSprite.Location);
                foreach (var alien in AliensList)
                    args.Graphics.DrawImage(alien.CurrentSprite.Image, alien.CurrentSprite.Location);
            };

            MakeAliens(3);

            Timer.Interval = 30;
            Timer.Tick += (sender, args) =>
            {
                healthBar.Value = Player.Health;
                if (Player.Health <= 0)
                    Player.CurrentSprite.Image = Resource1.DoomGuyDied;

                if (Player.IsMoving)
                    Player.Move();

                SpawnFuel(); // new
                DirectAliensToPlayer(); // new
                CheckIfTookFuel(fuelBar); // new
                CheckIfAliensHaveBeenShot(); // new

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

        public void InitializeEntities()
        {
            Player = new Player(ClientSize.Width / 2, ClientSize.Height / 2, new Bitmap(Resource1.DoomGuyStand), new Size(200, 200));
            Timer = new Timer();
            Random = new Random();
            AliensList = new List<Alien>();
            FuelsList = new List<Fuel>(); // new
        }

        private void MakeAliens(int numberAliens) // new
        {
            for (int i = 0; i < numberAliens; i++)
            {
                var point = new Point(Random.Next(0, 900), Random.Next(0, 800));
                var alien = new Alien(point);
                AliensList.Add(alien);
            }
        }

        private void SpawnFuel() // new
        {
            if (Random.Next(0, 100) == 1)
            {
                var fuel = new Fuel(this, Random);
                FuelsList.Add(fuel);
            }

        }

        private void DirectAliensToPlayer() // new
        {
            foreach (var alien in AliensList)
                alien.GoToPlayer(Player);
        }
        async void CheckIfTookFuel(ProgressBar fuelBar) // BeginInvoke
        {
            var indexesToDelete = await CheckIfTookFuelInThread(fuelBar);
            await RemoveTakenFuelInThread(indexesToDelete);
        }

        async void CheckIfAliensHaveBeenShot() // new
        {
            foreach (var alien in AliensList)
                await RegisterHitInThread(alien);
        }

        Task<List<int>> CheckIfTookFuelInThread(ProgressBar fuelBar)
        {
            var task = Task.Run
                (
                    () =>
                    {
                        var indexesToDelete = new List<int>();

                        foreach (var fuel in FuelsList)
                        {
                            if (fuel.CurrentSprite.Bounds.IntersectsWith(Player.CurrentSprite.Bounds))
                            {
                                BeginInvoke(new Action(() => fuelBar.Value += 10));
                                indexesToDelete.Add(FuelsList.IndexOf(fuel));
                            }
                        }
                        return indexesToDelete;
                    }
                );
            return task;
        }

        Task RemoveTakenFuelInThread(List<int> indexesToDelete)
        {
            var task = Task.Run
                (
                    () =>
                    {
                        foreach (var index in indexesToDelete)
                        {
                            lock (FuelsList)
                            {
                                FuelsList.RemoveAt(index);
                            }
                        }
                    }
                );
            return task;
        }

        Task RegisterHitInThread(Alien alien)
        {
            var task = Task.Run
                (
                    () =>
                    {
                        foreach (Control control in this.Controls)
                        {
                            if ((string)control.Tag == "bullet"
                                && alien.CurrentSprite.Bounds
                                .IntersectsWith(control.Bounds))
                            {
                                BeginInvoke(new Action(() =>
                                {
                                    alien.Health--;
                                    this.Controls.Remove(control);
                                    control.Dispose();
                                }));

                                if (alien.Health <= 0)
                                {
                                    BeginInvoke(new Action(() =>
                                    {
                                        var point = new Point(Random.Next(0, 900), Random.Next(0, 800));
                                        alien.CurrentSprite.Left = point.X;
                                        alien.CurrentSprite.Top = point.Y;
                                        alien.Health = 3;
                                    }));
                                }
                            }
                        }
                    }
                );
            return task;
        }

        private void MoveOrStopPlayer(Keys keys, int speed, bool isMoving)
        {
            switch (keys)
            {
                case Keys.W:
                    Player.DirectionY = -speed;
                    Player.CurrentMovement[Player.DirectionMovement.Up] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingUp;
                    break;
                case Keys.S:
                    Player.DirectionY = speed;
                    Player.CurrentMovement[Player.DirectionMovement.Down] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingDown;
                    break;
                case Keys.A:
                    Player.DirectionX = -speed;
                    Player.CurrentMovement[Player.DirectionMovement.Left] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingLeft;
                    break;
                case Keys.D:
                    Player.DirectionX = speed;
                    Player.CurrentMovement[Player.DirectionMovement.Right] = isMoving;
                    Player.CurrentSprite.Image = Resource1.DoomGuyGoingRight;
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
            shootBulet.CurrentSprite.Location
                = new Point(Player.X + (Player.CurrentSprite.Width / 2),
                Player.Y + (Player.CurrentSprite.Height / 2));
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
    }
}

#region мусор
//public void CheckIfAliensHaveBeenShot() // del
//{
//    foreach (Control firstControl in this.Controls)
//    {
//        foreach (Control secondControl in this.Controls)
//        {
//            if ((string)secondControl.Tag == "bullet"
//                && (string)firstControl.Tag == "alien"
//                && firstControl.Bounds.IntersectsWith(secondControl.Bounds))
//            {
//                this.Controls.Remove(firstControl);
//                this.Controls.Remove(secondControl);
//                ((PictureBox)firstControl).Dispose();
//                ((PictureBox)secondControl).Dispose();

//                //var alien = AliensList
//                //    .Where(ali => ali.CurrentSprite == (PictureBox)firstControl)
//                //    .Select(ali => ali);
//                // AliensList.Remove((Alien)alien);
//                AliensList.Remove((PictureBox)firstControl);

//                CreateAlien(); // !!!
//            }
//        }
//    }
//}

//private void CreateAlien() // del
//{
//    var alien = new PictureBox();
//    alien.Tag = "alien";
//    alien.Image = Resource1.AlienGoingLeft;
//    alien.Left = Random.Next(0, 900);
//    alien.Top = Random.Next(0, 800);
//    alien.SizeMode = PictureBoxSizeMode.AutoSize;
//    AliensList.Add(alien);
//    this.Controls.Add(alien);
//    Player.CurrentSprite.BringToFront();
//}

//public void DirectAlienToPlayer() // del
//{
//    foreach (Control control in this.Controls)
//    {
//        if (control is PictureBox && (string)control.Tag == "alien")
//        {
//            var speed = 5;
//            if (control.Top > Player.CurrentSprite.Top)
//            {
//                control.Top -= speed;
//                ((PictureBox)control).Image = Resource1.AlienGoingUp;
//            }
//            if (control.Top < Player.CurrentSprite.Top)
//            {
//                control.Top += speed;
//                ((PictureBox)control).Image = Resource1.AlienGoingDown;
//            }
//            if (control.Left > Player.CurrentSprite.Left)
//            {
//                control.Left -= speed;
//                ((PictureBox)control).Image = Resource1.AlienGoingLeft;
//            }
//            if (control.Left < Player.CurrentSprite.Left)
//            {
//                control.Left += speed;
//                ((PictureBox)control).Image = Resource1.AlienGoingRight;
//            }

//        }
//    }
//}
#endregion