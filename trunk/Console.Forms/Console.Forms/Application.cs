using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Crsouza.Console.Forms
{

    /// <summary>
    /// Provides static methods and properties to manage an console application,
    /// such as methods to start and stop an application, to process console
    /// keys, and properties to get information about an console application.
    /// 
    /// This class cannot be inherited.
    /// </summary>
    public static class Application
    {
        private static bool m_exit;


        /// <summary>
        ///  Begins running a standard application message loop on the
        ///  current thread, and makes the specified form visible.
        /// </summary>
        /// <param name="form"></param>
        public static void Run(Form form)
        {
            Run(new ApplicationContext(form));
        }

        /// <summary>
        ///   Begins running a standard application message loop on the
        ///   current thread, with an ApplicationContext.
        /// </summary>
        /// <param name="applicationContext"></param>
        public static void Run(ApplicationContext applicationContext)
        {
            applicationContext.ThreadExit +=  new EventHandler(ApplicationContext_ThreadExit);
            applicationContext.MainForm.Show();

            ConsoleKeyInfo key;

            while (!m_exit)
            {
                key = System.Console.ReadKey(true);
                applicationContext.ProcessKey(key);         
            }
        }


        
        /// <summary>
        ///   Informs all message pumps that they must terminate, and then
        ///   closes all application windows after the messages have been processed.
        /// </summary>
        public static void Exit()
        {
            m_exit = true;
        }




        private static void ApplicationContext_ThreadExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
