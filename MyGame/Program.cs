using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyGame
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var resolution = Screen.PrimaryScreen.Bounds.Size;
            Application.Run(new Form1 { ClientSize = new Size(resolution.Width, resolution.Height),
                                        WindowState = FormWindowState.Maximized });
        }
    }
}
