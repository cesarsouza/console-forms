using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;



namespace Crsouza.Console.Forms
{
    public class Form : ContainerControl
    {

        private DialogResult m_dialogResult;
        private FormStartPosition m_startPosition;
        private bool m_keyPreview;
        private bool m_setConsoleTitle = true;
        private Form m_owner;

        public event EventHandler Load;
        public event EventHandler Closed;
        public event CancelEventHandler Closing;
        public event EventHandler Shown;
        public event ConsolePreviewKeyPressEventHandler PreviewKeyPress;
        public event EventHandler Activated;


        public Form()
        {

        }


        #region Public Properties
        public Form Owner
        {
            get { return m_owner; }
            set { m_owner = value; }
        }

        public bool ShowTitleInConsole
        {
            get { return m_setConsoleTitle; }
            set
            {
                m_setConsoleTitle = value;
                updateTitle();
            }
        }

        public FormStartPosition StartPosition
        {
            get { return m_startPosition; }
            set { m_startPosition = value; }
        }

        public bool KeyPreview
        {
            get { return m_keyPreview; }
            set { m_keyPreview = value; }
        }

        public DialogResult DialogResult
        {
            get { return m_dialogResult; }
            set { m_dialogResult = value; }
        }
        #endregion


        #region Public Methods
        public void Show(Form owner)
        {
          //  this.Parent = owner;
            this.Owner = owner;

            switch (m_startPosition)
            {
                case FormStartPosition.Manual:
                    break;

                case FormStartPosition.CenterScreen:
                    this.Location = new System.Drawing.Point(
                        System.Console.WindowLeft + (System.Console.WindowWidth - this.Width) / 2,
                        System.Console.WindowTop + (System.Console.WindowHeight - this.Height) / 2);
                    break;

                case FormStartPosition.CenterParent:
                    this.Location = new Point(Owner.Left + (Owner.Width - this.Width) / 2,
                      Owner.Top + (Owner.Height - this.Height) / 2);
                    break;

                default:
                    throw new InvalidOperationException();

            }

            Show();
            Focus();

            OnShown(EventArgs.Empty);
        }


        public DialogResult ShowDialog(Form owner)
        {
            Show(owner);

            ConsoleKeyInfo key;

            while (!IsDisposed)
            {
                key = System.Console.ReadKey(true);
                ProcessKey(key);
            }

            return DialogResult;
        }

        /// <summary>
        ///    Activates the form and gives it focus.
        /// </summary>
        /// <remarks>
        ///    Activating a form brings it to the front if this is the active application,
        ///    or it flashes the window caption if this is not the active application. The
        ///    form must be visible for this method to have any effect. To determine the
        ///    active form in an application, use the ActiveForm property or the ActiveMdiChild
        ///    property if your forms are in a Multiple-document interface (MDI) application.
        /// </remarks>
        public void Activate()
        {
            if (Visible)
            {
                updateTitle();
                Focus();

                OnActivated(EventArgs.Empty);
            }
        }

        public void Close()
        {
            CancelEventArgs e = new CancelEventArgs();
            OnClosing(e);

            if (!e.Cancel)
            {
                Hide();
                Dispose();
            }

            if (Owner != null)
                Owner.Activate();
            OnClosed(EventArgs.Empty);
        }



        protected override void OnTextChanged(EventArgs e)
        {
            updateTitle();

            base.OnTextChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                if (!IsHandleCreated)
                    OnLoad(EventArgs.Empty);
            }
            else
            {
                if (Owner != null)
                {
                    Owner.PerformLayout();
                }
            }

            base.OnVisibleChanged(e);
        }
        #endregion



        #region Event Triggers
        protected virtual void OnShown(EventArgs e)
        {
            if (Shown != null)
                Shown(this, e);
        }

        protected virtual void OnPreviewKeypress(ConsolePreviewKeyPressEventArgs e)
        {
            if (PreviewKeyPress != null)
                PreviewKeyPress(this, e);
        }

        protected virtual void OnActivated(EventArgs e)
        {
            if (Activated != null)
                Activated(this, e);
        }

        protected virtual void OnClosing(CancelEventArgs e)
        {
            if (Closing != null)
                Closing(this, e);
        }

        protected virtual void OnClosed(EventArgs e)
        {
            if (Closed != null)
                Closed(this, e);
        }

        protected virtual void OnLoad(EventArgs e)
        {
            if (Load != null)
                Load(this, e);
        }
        #endregion






        #region Internal Methods
        internal override void ProcessKey(ConsoleKeyInfo key)
        {
            if (!m_keyPreview)
                base.ProcessKey(key);

            else
            {
                ConsolePreviewKeyPressEventArgs e = new ConsolePreviewKeyPressEventArgs(key);
                OnPreviewKeypress(e);

                if (e.Handled) return;
                else base.ProcessKey(key);
            }
        }
        #endregion





        #region Private Methods
        private void updateTitle()
        {
            if (m_setConsoleTitle)
                System.Console.Title = Text;
        }
        #endregion


    }
}
