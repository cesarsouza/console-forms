using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Padding = System.Windows.Forms.Padding;

namespace Crsouza.Console.Forms
{
    public abstract class Control : IDisposable
    {
        // Variables
        private Size m_size;
        private Point m_location;
        private String m_name;
        private String m_text;
        private Control m_parent;
        private Padding m_padding;
        private bool m_visible = true;
        private bool m_layoutSuspended = false;
        private bool m_shown = false;
        private int m_tabIndex;
        private bool m_tabStop;
        private int m_canSelect;
        private bool m_enabled = true;
        private bool m_selectable = true;
        private ControlCollection m_controls;
        private bool m_disposed;

        //TODO: Nullable console colors to use parent's color?
        private ConsoleColor m_backgroundColor = ConsoleColor.Black;
        private ConsoleColor m_foregroundColor = ConsoleColor.Gray;
        private bool m_noBackground = true;

        // Events
        public event ConsoleKeyPressEventHandler KeyPress;
        public event EventHandler Click;
        public event EventHandler Enter;
        public event EventHandler GotFocus;
        public event EventHandler LostFocus;
        public event EventHandler ControlAdded;
        public event EventHandler ControlRemoved;
        public event EventHandler ParentChanged;
        public event EventHandler VisibleChanged;
        public event EventHandler TabIndexChanged;
        public event EventHandler EnabledChanged;
        public event EventHandler TabStopChanged;
        public event EventHandler ForeColorChanged;
        public event EventHandler BackColorChanged;
        public event EventHandler TextChanged;



        #region Constructors
        public Control() : this(null)
        {
        }

        public Control(Control parent) : this(parent, String.Empty)
        {
        }

        public Control(Control parent, string text)
        {
            m_name = String.Empty;
            m_size = new Size(5,10);
            m_location = new Point(0,0);
            m_parent = parent;
            m_text = text;
        }
        #endregion




        #region Properties
        public Control Parent
        {
            get
            {
                return this.m_parent;
            }
            set
            {
                if (this.m_parent != value)
                {
                    /*
                    if (value != null)
                    {
                        value.Controls.Add(this);
                    }
                    else
                    {
                        this.m_parent.Controls.Remove(this);
                    }
                     */
                    this.m_parent = value;
                }
            }
        }

        public Size Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public Padding Padding
        {
            get { return m_padding; }
            set { m_padding = value; }
        }

        public Point Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        public int Top
        {
            get { return m_location.Y; }
        }

        public int Left
        {
            get { return m_location.X; }
        }

        public int Width
        {
            get { return m_size.Width; }
            set { m_size.Width = value; }
        }

        public int Height
        {
            get { return m_size.Height; }
        }

        public virtual String Text
        {
            get { return m_text; }
            set
            {
                m_text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }

        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public ConsoleColor BackColor
        {
            get { return m_backgroundColor; }
            set
            {
                m_backgroundColor = value;
                OnBackColorChanged(EventArgs.Empty);
            }
        }

        public ConsoleColor ForeColor
        {
            get { return m_foregroundColor; }
            set
            {
                m_foregroundColor = value;
                OnForeColorChanged(EventArgs.Empty);
            }
        }

        public bool NoBackground
        {
            get { return m_noBackground; }
            set { m_noBackground = value; }
        }

        public bool Visible
        {
            get { return m_visible; }
            set
            {
                if (m_visible != value || m_shown == false)
                {
                    if (IsDisposed && value)
                    {
                        throw new InvalidOperationException();
                    }

                    m_visible = value;

                    if (m_visible == false)
                    {
                        if (m_parent == null)
                        {
                            System.Console.ResetColor();
                            System.Console.Clear();
                        }
                        else
                        {
                            Parent.PerformLayout();
                        }
                    }
                    else
                    {
                        m_shown = true;
                        PerformLayout();
                    }
                }
            }
        }

        public bool Enabled
        {
            get
            {
                if (!m_enabled)
                    return false;
                return ((this.Parent == null) || this.Parent.Enabled);
            }
            set
            {
                bool enabled = this.Enabled;
                m_enabled = value;
                if (enabled != value)
                {
                    if (!value)
                    {
                        SelectNextIfFocused();
                    }
                    OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        public virtual bool Focused
        {
            get
            {
                if (m_parent == null)
                    return true;

                else if (Parent is ContainerControl)
                {
                    if ((Parent as ContainerControl).ActiveControl == this)
                        return true;
                }

                return false;
            }
        }

        public int TabIndex
        {
            get { return m_tabIndex; }
            set
            {
                if (m_tabIndex != value)
                {
                    m_tabIndex = value;
                    OnTabIndexChanged(EventArgs.Empty);
                }
            }
        }

        public bool TabStop
        {
            get
            {
                return m_tabStop;
            }
            set
            {
                if (this.TabStop != value)
                {
                    m_tabStop = value;
                    OnTabStopChanged(EventArgs.Empty);
                }
            }
        }

        public ControlCollection Controls
        {
            get
            {
                if (m_controls == null)
                {
                    m_controls = new ControlCollection(this);
                }
                return m_controls;
            }
        }

        public bool ContainsFocus
        {
            get
            {
                if (!this.IsShown)
                {
                    return false;
                }
                return Focused;
            }
        }

        public bool IsDisposed
        {
            get { return m_disposed; }
        }
        #endregion



        #region Public Methods
        public ConsoleGraphics CreateGraphics()
        {
            Point p = getAbsolutePosition(0,0);
            return new ConsoleGraphics(new Rectangle(p, Size));
        }

        public void SetCursorPosition(int x, int y)
        {
            SetCursorPosition(new Point(x, y));
        }
        public void SetCursorPosition(Point point)
        {
            Point absolute = PointToScreen(point);
            System.Console.SetCursorPosition(absolute.X, absolute.Y);
        }

        public Point PointToScreen(Point point)
        {
            return getAbsolutePosition(point);
        }

        public void ResetText()
        {
            m_text = String.Empty;
        }

        public void Focus()
        {
            if (m_parent != null && m_parent is ContainerControl)
            {
                (m_parent as ContainerControl).ActiveControl = this;
            }

            if (IsShown)
                OnGotFocus(EventArgs.Empty);
        }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        /// <summary>
        ///   Retrieves a value indicating whether the specified control is a child of the control.
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public bool Contains(Control ctl)
        {
            // We start in the control and go back on
            //  its inheritance tree searching for the
            //  current control.
            while (ctl != null)
            {
                ctl = ctl.Parent;
                if (ctl == null)
                {
                    return false;
                }
                if (ctl == this)
                {
                    return true;
                }
            }
            return false;
        }

        public bool SelectNextControl(Control ctl, bool forward, bool tabStopOnly, bool nested, bool wrap)
        {
            if (!this.Contains(ctl) || (!nested && (ctl.m_parent != this)))
            {
                ctl = null;
            }
            return true;
        }

        public Control GetNextControl(Control ctl, bool forward)
        {
            return null;  
        }

        public virtual bool CanSelect
        {
            get
            {
                 if (!m_selectable)
                     return false;
                 
                for (Control control = this; control != null; control = control.m_parent)
                {
                    if (!control.Enabled || !control.Visible)
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        public virtual void PerformLayout()
        {
            if (m_layoutSuspended)
                return;

            if (!m_shown)
                return;

            if (Parent == null)
            {
                System.Console.BackgroundColor = BackColor;
                System.Console.ForegroundColor = ForeColor;
                System.Console.Clear();
            }

            if (m_visible)
            {
                ConsolePaintEventArgs e = new ConsolePaintEventArgs(CreateGraphics());
                this.OnPaintBackground(e);
                this.OnPaint(e);

                System.Console.WindowTop = 0;
               // System.Console.SetCursorPosition(0, 0);
            }
        }

        
        public void Select()
        {
            if (CanSelect)
            {
                OnEnter(EventArgs.Empty);
                Focus();
            }
        }


        public void SuspendLayout()
        {
            m_layoutSuspended = true;
        }

        public void ResumeLayout(bool performLayout)
        {
            m_layoutSuspended = false;

            if (performLayout) this.PerformLayout();
        }


        #endregion




        #region Virtual Methods
        public virtual void Invalidate()
        {
        }


        protected virtual void Select(bool directed, bool forward)
        {
            throw new NotImplementedException();
        }

        internal protected virtual Control GetFirstChildControlInTabOrder(bool forward)
        {
            throw new NotImplementedException();
            return null;
        }
        #endregion
        



        #region Private Methods
        private Point getAbsolutePosition(int x, int y)
        {
            return getAbsolutePosition(new Point(x, y));
        }

        private Point getAbsolutePosition(Point point)
        {
            if (m_parent == null)
                return new Point(point.X + Left, point.Y+Top);
            else return Parent.getAbsolutePosition(new Point(point.X + Left, point.Y + Top));
        }

        private void SelectNextIfFocused()
        {
            if (this.ContainsFocus && (this.Parent != null))
            {
                IContainerControl containerControlInternal = this.Parent.GetContainerControl();
                if (containerControlInternal != null)
                {
                    ((Control)containerControlInternal).SelectNextControl(this, true, true, true, true);
                }
            }
        }
        #endregion





        #region Event Triggers
        protected virtual void OnEnter(EventArgs e)
        {
            if (Enter != null)
                Enter(this, e);
        }

        protected virtual void OnTabStopChanged(EventArgs e)
        {
            if (TabStopChanged != null)
                TabStopChanged(this, e);
        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {
            if (EnabledChanged != null)
                EnabledChanged(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (VisibleChanged != null)
                VisibleChanged(this, e);
        }

        protected virtual void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.Tab)
            {
                if (Parent != null)
                Parent.SelectNextControl(this, true, true, true, true);
            }
            else
            {

                if (KeyPress != null)
                    KeyPress(this, e);
            }
        }

       

        protected virtual void OnPaint(ConsolePaintEventArgs e)
        {
            System.Console.ResetColor();
        }

        protected virtual void OnPaintBackground(ConsolePaintEventArgs e)
        {
            if (m_noBackground)
                return;


            e.Graphics.DrawRectangle(new Rectangle(0, 0, Width, Height),' ', m_foregroundColor, m_backgroundColor);
        }

        protected virtual void OnGotFocus(EventArgs e)
        {
            if (GotFocus != null)
                GotFocus(this, e);
        }

        protected virtual void OnLostFocus(EventArgs e)
        {
            if (LostFocus != null)
                LostFocus(this, e);
        }

        protected virtual void OnClick(EventArgs e)
        {
            if (Click != null)
                Click(this, e);
        }

        protected virtual void OnTabIndexChanged(EventArgs e)
        {
            if (TabIndexChanged != null)
                TabIndexChanged(this, e);
        }

        protected virtual void OnForeColorChanged(EventArgs e)
        {
            Invalidate();

            if (ForeColorChanged != null)
                ForeColorChanged(this, e);

            if (m_controls != null)
            {
                for (int i = 0; i < m_controls.Count; i++)
                {
                    m_controls[i].OnParentForeColorChanged(e);
                }
            }
        }

        protected virtual void OnBackColorChanged(EventArgs e)
        {
            Invalidate();

            if (BackColorChanged != null)
                BackColorChanged(this, e);

            if (m_controls != null)
            {
                for (int i = 0; i < m_controls.Count; i++)
                {
                    m_controls[i].OnParentBackColorChanged(e);
                }
            }
        }

        protected virtual void OnParentBackColorChanged(EventArgs e)
        {

        }

        protected virtual void OnParentForeColorChanged(EventArgs e)
        {

        }
        #endregion





        






        #region Internal Methods
        internal virtual void ProcessKey(ConsoleKeyInfo key)
        {
            ConsoleKeyPressEventArgs e = new ConsoleKeyPressEventArgs(key);
            OnKeyPressed(e);
        }


        internal protected bool LayoutSuspended
        {
            get { return m_layoutSuspended; }
        }

        internal protected bool IsShown
        {
            get
            {
                if (m_parent != null)
                    return m_parent.IsShown;
                return m_shown;
            }
        }

        internal protected ContainerControl GetContainerControl()
        {
            if (m_parent != null)
                return m_parent as ContainerControl;
            else return null;
        }
        #endregion







        #region Nested Classes
        public class ControlCollection : List<Control>, ICollection<Control>
        {
            private Control m_owner;

            public ControlCollection(Control owner)
            {
                m_owner = owner;
            }

            public new void Add(Control control)
            {
                control.m_parent = m_owner;
                control.m_shown = true;
                base.Add(control);
            }

            //TODO: This obviously needs some optimizations
            public int GetChildIndex(Control child)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] == child)
                        return i;
                }
                throw new InvalidOperationException();
            }


            

        }
        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Visible = false;
            m_disposed = true;
        }

        #endregion

    }
}
