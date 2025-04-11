using System;
using System.Windows.Forms;

namespace IP3000_Control
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (var mutex = new System.Threading.Mutex(true, "IP3000", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    MessageBox.Show("There is already an instace running");
                }
            }
        }

        public static Keys ExcludeUnnecessaryKey(Keys key)
        {
            if ((key & Keys.Control) == Keys.Control)
            {
                key ^= Keys.Control;
            }
            if ((key & Keys.Alt) == Keys.Alt)
            {
                key ^= Keys.Alt;
            }
            if ((key & Keys.Shift) == Keys.Shift)
            {
                key ^= Keys.Shift;
            }

            return key;
        }
    }
}
