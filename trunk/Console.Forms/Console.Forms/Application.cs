using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crsouza.Console.Forms
{
    public static class Application
    {
        private static bool m_exit;


        public static void Run(Crsouza.Console.Forms.Form form)
        {
            Run(new ApplicationContext(form));
        }

        public static void Run(Crsouza.Console.Forms.ApplicationContext applicationContext)
        {
            applicationContext.MainForm.Closed += new EventHandler(MainForm_Closed);
            applicationContext.MainForm.Show();

            ConsoleKeyInfo key;

            while (!m_exit)
            {
                key = System.Console.ReadKey(true);
                applicationContext.ProcessKey(key);         
            }
        }

        static void MainForm_Closed(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static void Exit()
        {
            m_exit = true;
            System.Console.ResetColor();
        }


    }
}
