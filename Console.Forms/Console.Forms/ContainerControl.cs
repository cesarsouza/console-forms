using System;
using System.Collections.Generic;
using System.Text;

namespace Crsouza.Console.Forms
{
    public abstract class ContainerControl : ScrollableControl, IContainerControl
    {
        private ControlCollection m_controlCollection;
        private Control m_activeControl;

        public ContainerControl()
        {
            m_controlCollection = new ControlCollection(this);
        }

        public override void PerformLayout()
        {
            base.PerformLayout();

            if (!LayoutSuspended && IsShown)
            {
                foreach (Control control in m_controlCollection)
                {
                    control.PerformLayout();
                }
            }

            ActivateControl(ActiveControl);
        }
        


        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (m_activeControl != null)
                m_activeControl.Focus();
        }

        public ControlCollection Controls
        {
            get { return m_controlCollection; }
        }

        public Control ActiveControl
        {
            get { return m_activeControl; }
            set
            {
                if (m_activeControl != value)
                {
                    if (!Contains(value))
                        return;

                    m_activeControl = value;
                    m_activeControl.Focus();
                }
                
            }
        }

        public bool ActivateControl(Control control)
        {
            if (!Contains(control))
            {
                return false;
            }
            else
            {
                ActiveControl = control;
                control.Select();
                return true;
            }
        }


        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);

            
        }

        protected virtual void OnActiveControlChanged(EventArgs e)
        {
        }

        internal override void ProcessKey(ConsoleKeyInfo key)
        {
            if (ActiveControl != null)
                ActiveControl.ProcessKey(key);
            else base.ProcessKey(key);
        }
    }
}
