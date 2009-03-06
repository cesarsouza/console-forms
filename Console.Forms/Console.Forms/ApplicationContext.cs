using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Crsouza.Console.Forms
{
    public class ApplicationContext
    {
        // Fields
        private Crsouza.Console.Forms.Form m_mainForm;
        private object m_userData;
      


        // Events
        public event EventHandler ThreadExit;

        // Methods
        public ApplicationContext()
            : this(null)
        {
        }

        public ApplicationContext(Crsouza.Console.Forms.Form mainForm)
        {
            this.MainForm = mainForm;
        }

        public Crsouza.Console.Forms.Form MainForm
        {
            get { return m_mainForm; }
            set { m_mainForm = value; }
        }

        public object Tag
        {
            get { return m_userData; }
            set { m_userData = value; }
        }

        public virtual void ProcessKey(ConsoleKeyInfo key)
        {
            if (MainForm != null)
                MainForm.ProcessKey(key);
        }



    }
}


