using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Crsouza.Console.Forms
{

    /// <summary>
    /// Specifies the contextual information about an application thread.
    /// </summary>
    /// <remarks>
    /// You can use the ApplicationContext class to redefine the circumstances
    /// that cause a message loop to exit. By default, the ApplicationContext
    /// listens to the Closed event on the application's main Form, then exits
    /// the thread's message loop.
    /// </remarks>
    public class ApplicationContext
    {
        // Fields
        private Form m_mainForm;
        private object m_userData;
      

        // Events
        /// <summary>
        ///   Occurs when the message loop of the thread should be
        ///   terminated, by calling ExitThread.
        /// </summary>
        public event EventHandler ThreadExit;



        #region Constructor
        /// <summary>
        ///   Initializes a new instance of the ApplicationContext class with no context.
        /// </summary>
        public ApplicationContext()
            : this(null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the ApplicationContext class with the specified Form.
        /// </summary>
        /// <param name="mainForm"></param>
        public ApplicationContext(Form mainForm)
        {
            this.MainForm = mainForm;
        }
        #endregion



        #region Properties
        /// <summary>
        ///   Gets or sets the Form to use as context.
        /// </summary>
        public Form MainForm
        {
            get { return m_mainForm; }
            set
            {
                if (m_mainForm != value)
                {
                    if (m_mainForm != null)
                        m_mainForm.Closed -= OnMainFormClosed;

                    m_mainForm = value;

                    if (m_mainForm != null)
                        m_mainForm.Closed += OnMainFormClosed;
                }
            }
        }

        /// <summary>
        ///   Gets or sets an object that contains data about the control.
        /// </summary>
        public object Tag
        {
            get { return m_userData; }
            set { m_userData = value; }
        }
        #endregion



        /// <summary>
        ///   Calls ExitThreadCore, which raises the ThreadExit event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The EventArgs that contains the event data.</param>
        public virtual void OnMainFormClosed(object sender, EventArgs e)
        {
            ExitThreadCore();

            System.Console.ResetColor();
        }

        /// <summary>
        ///   Terminates the message loop of the thread.
        /// </summary>
        public void ExitThread()
        {
            /*  Note: ExitThread and ExitThreadCore do not actually cause
                the thread to terminate. These methods raise the ThreadExit
                event to which the Application object listens. The Application
                object then terminates the thread. */


            ExitThreadCore();
        }



        protected virtual void ExitThreadCore()
        {
            if (ThreadExit != null)
                ThreadExit(this, EventArgs.Empty);
        }


        public virtual void ProcessKey(ConsoleKeyInfo key)
        {
            if (MainForm != null)
                MainForm.ProcessKey(key);
        }

    }
}


