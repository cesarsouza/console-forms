using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;

using Padding = System.Windows.Forms.Padding;
using System.Collections;

namespace Crsouza.Console.Forms
{

    /// <summary>
    ///   Defines the base class for controls, which are components
    ///   with visual representation.
    /// </summary>
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
        private ConsoleColor? m_backgroundColor = ConsoleColor.Black;
        private ConsoleColor? m_foregroundColor = ConsoleColor.Gray;
        private bool m_noBackground = false;

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
        /// <summary>
        ///   Initializes a new instance of the Control class with default settings.
        /// </summary>
        public Control() : this(null, String.Empty)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the Control class with specific text.
        /// </summary>
        /// <param name="text">The text displayed by the control.</param>
        public Control(String text) : this(null, text)
        {
        }

        /// <summary>
        ///   The Control to be the parent of the control.
        /// </summary>
        /// <param name="parent"></param>
        public Control(Control parent) : this(parent, String.Empty)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the Control class as a child control,
        ///   with specific text;
        /// </summary>
        /// <param name="parent">The Control to be the parent of the control.</param>
        /// <param name="text">The text displayed by the control.</param>
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
        /// <summary>
        ///   Gets or sets the parent container of the control.
        /// </summary>
        /// <remarks>
        ///   Setting the Parent property value to a null reference removes
        ///   the control from the Control.ControlCollection of its current
        ///   parent control.
        /// </remarks>
        public Control Parent
        {
            get { return this.m_parent; }
            set
            {
                if (this.m_parent != value)
                {
                    //FIXME: Parent doesn't add/remove the control from the Control Collection
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

        /// <summary>
        ///   Gets or sets the height and width of the control.
        /// </summary>
        public Size Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        /// <summary>
        ///   Gets or sets the height and width of the client area of the control.
        /// </summary>
        /// <remarks>
        ///   The client area of a control is the bounds of the control, minus the
        ///   nonclient elements such as scroll bars, borders, title bars, and menus.
        /// </remarks>
        public Size ClientSize
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
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
                if (m_text != value)
                {
                    m_text = value;
                    OnTextChanged(EventArgs.Empty);
                }
            }
        }

        public String Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                }
            }
        }

        public ConsoleColor BackColor
        {
            get
            {
                if (m_backgroundColor.HasValue == false)
                {
                    Control parent = Parent;
                    if (parent != null)
                        return parent.BackColor;
                    else return ConsoleColor.Black;
                }
                else return m_backgroundColor.Value;
            }
            set
            {
                m_backgroundColor = value;
                OnBackColorChanged(EventArgs.Empty);
            }
        }

        public ConsoleColor ForeColor
        {
            get
            {
                if (m_foregroundColor.HasValue == false)
                {
                    Control parent = Parent;
                    if (parent != null)
                        return parent.BackColor;
                    else return ConsoleColor.Gray;
                }
                else return m_foregroundColor.Value;
            }
            set
            {
                m_foregroundColor = value;
                OnForeColorChanged(EventArgs.Empty);
            }
        }

        //FIXME: This is a hack and should be removed in future
        public bool NoBackground
        {
            get { return m_noBackground; }
            set { m_noBackground = value; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the control
        ///   and all its child controls are displayed.
        /// </summary>
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

                    OnVisibleChanged(EventArgs.Empty);

                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the control can respond to user interaction.
        /// </summary>
        /// <remarks>
        /// </remarks>
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

        /// <summary>
        ///   Gets a value indicating whether the control has input focus.
        /// </summary>
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
                if (!this.IsHandleCreated)
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

        /// <summary>
        ///   Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="point">The screen coordinate Point to convert.</param>
        /// <returns>A Point that represents the converted Point, p, in client coordinates.</returns>
        public Point PointToScreen(Point point)
        {
            return getAbsolutePosition(point);
        }

        /// <summary>
        ///   Computes the location of the specified screen point into client coordinates.
        /// </summary>
        /// <param name="point">The client coordinate Point to convert.</param>
        /// <returns>A Point that represents the converted Point, p, in screen coordinates.</returns>
        public Point PointToClient(Point point)
        {
            throw new NotImplementedException();
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

            if (IsHandleCreated)
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
            Control next = GetNextControl(this, forward);
            next.Select();

            return true;
        }

        public Control GetNextControl(Control ctl, bool forward)
        {
            Control[] controls = GetChildControlsInTabOrder(true);

            if (controls.Length == 0)
            {
                if (Parent != null)
                    return Parent.GetNextControl(this, forward);
                else return this;
            }

            for (int i = 0; i < controls.Length; i++)
            {
                if (controls[i] == ctl)
                {
                    if (forward)
                    {
                        if (i < controls.Length-1)
                            return controls[i + 1];
                        else
                        {
                            if (Parent != null)
                                return Parent.GetNextControl(this, forward);
                            return controls[0];
                        }
                    }
                    else
                    {
                        if (i > 0)
                            return controls[i - 1];
                        else
                        {
                            if (Parent != null)
                                return Parent.GetNextControl(this, forward);
                            return controls[controls.Length - 1];
                        }
                    }
                }
            }

            throw new InvalidOperationException();
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

        /// <summary>
        ///   Forces the control to apply layout logic to all its child controls.
        /// </summary>
        public virtual void PerformLayout()
        {
            if (m_layoutSuspended)
                return;

            if (!m_shown)
                return;
/*
            if (Parent == null)
            {
                System.Console.BackgroundColor = BackColor;
                System.Console.ForegroundColor = ForeColor;

                //FIXME: The following line is a hack and should be removed in future.
                System.Console.Clear();
            }
*/
            if (m_visible)
            {
                ConsolePaintEventArgs e = new ConsolePaintEventArgs(CreateGraphics());
                this.OnPaintBackground(e);
                this.OnPaint(e);

                if (!LayoutSuspended && IsHandleCreated)
                {
                    foreach (Control control in Controls)
                    {
                        control.PerformLayout();
                    }
                }

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
            PerformLayout();
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

        private List<ControlTabOrderHolder> GetChildControlsTabOrderList()
        {
            List<ControlTabOrderHolder> list = new List<ControlTabOrderHolder>();
            foreach (Control control in this.Controls)
            {
                int newOrder = (control == null) ? -1 : control.TabIndex;
                list.Add(new ControlTabOrderHolder(list.Count, newOrder, control));
            }
            list.Sort(new ControlTabOrderComparer());
            return list;
        }


        #endregion


        #region Static Methods
        public static bool IsMnemonic(char charCode, string text)
        {
#if NO_WINFORMS_DEPENDENCY
            if (charCode == '&')
            {
                return false;
            }
            if (text != null && text.Length > 0)
            {
                int mnemonicCharIndex = -1;
                charCode = char.ToUpper(charCode, CultureInfo.CurrentCulture);

                mnemonicCharIndex = text.IndexOf('&', 0) + 1;

                if ((mnemonicCharIndex <= 0) || (mnemonicCharIndex >= text.Length))
                    return false; // string doesn't have a mnemonic

                char mnemonicChar = Char.ToUpper(text[mnemonicCharIndex], CultureInfo.CurrentCulture);

                if (mnemonicChar == charCode)
                    return true; // yes, charCode is mnemonic in text
                return false;
            }

            return false;
#else
            return System.Windows.Forms.Control.IsMnemonic(charCode, text);
#endif
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
            if (Visible)
            {
                m_shown = true;
                PerformLayout();
                
            }
            else
            {
                if (m_parent != null)
                {
                    Parent.PerformLayout();
                }
            }

            if (VisibleChanged != null)
                VisibleChanged(this, e);
        }

        protected virtual void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            if (e.KeyInfo.Key == ConsoleKey.Tab)
            {
                SelectNextControl(this, !e.Shift, true, true, true);
            }
            else
            {
                if (KeyPress != null)
                    KeyPress(this, e);
            }
        }

       
        /// <summary>
        ///   Raises the Paint event.
        /// </summary>
        /// <remarks>
        ///   The OnPaint method also enables derived classes to handle the event
        ///   without attaching a delegate. This is the preferred technique for
        ///   handling the event in a derived class.
        ///
        ///   Notes to Inheritors: 
        ///   When overriding OnPaint in a derived class, be sure to call the base
        ///   class' OnPaint method so that registered delegates receive the event.
        ///</remarks>
        /// <param name="e">A ConsolePaintEventArgs that contains the event data.</param>
        protected virtual void OnPaint(ConsolePaintEventArgs e)
        {
            System.Console.ResetColor();
        }

        /// <summary>
        ///   Paints the background of the control.
        /// </summary>
        /// <param name="e">A ConsolePaintEventArgs that contains information about the control to paint.</param>
        protected virtual void OnPaintBackground(ConsolePaintEventArgs e)
        {
            if (m_noBackground)
                return;

            e.Graphics.DrawRectangle(new Rectangle(0, 0, Width, Height),' ', ForeColor, BackColor);
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

            // Notify child controls forecolor has changed
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

            // Notify child controls backcolor has changed
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

        /// <summary>
        ///   
        /// </summary>
        /// <remarks>
        ///   This is just an emulation of WinForms behaviour as Console
        ///   Controls doesn't have windows handles.
        /// </remarks>
        internal protected bool IsHandleCreated
        {
            get
            {
                if (m_parent != null)
                    return m_parent.IsHandleCreated;
                return m_shown;
            }
        }

        internal protected ContainerControl GetContainerControl()
        {
            if (m_parent != null)
                return m_parent as ContainerControl;
            else return null;
        }



        internal Control[] GetChildControlsInTabOrder(bool handleCreatedOnly)
        {
            var childControlsTabOrderList = GetChildControlsTabOrderList();
            Control[] controlArray = new Control[childControlsTabOrderList.Count];
            for (int i = 0; i < childControlsTabOrderList.Count; i++)
            {
                controlArray[i] = childControlsTabOrderList[i].control;
            }
            return controlArray;
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

            
            public int GetChildIndex(Control child)
            {
                return this.IndexOf(child);
            }



            

        }

        private class ControlTabOrderHolder
        {
            // Fields
            internal readonly Control control;
            internal readonly int newOrder;
            internal readonly int oldOrder;

            // Methods
            internal ControlTabOrderHolder(int oldOrder, int newOrder, Control control)
            {
                this.oldOrder = oldOrder;
                this.newOrder = newOrder;
                this.control = control;
            }
        }

        private class ControlTabOrderComparer : IComparer<Control.ControlTabOrderHolder>
        {
            // Methods
            int IComparer<Control.ControlTabOrderHolder>.Compare(Control.ControlTabOrderHolder x,
                Control.ControlTabOrderHolder y)
            {
                int d = x.newOrder - y.newOrder;
                if (d == 0)
                {
                    d = x.oldOrder - y.oldOrder;
                }
                return d;
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
