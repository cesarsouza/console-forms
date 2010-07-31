using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Crsouza.Console.Forms
{
    public class Scrollbar : Control
    {

        ScrollOrientation m_orientation;

        int m_value;
        int m_maximum;
        int m_minimum;
        int m_smallChange;
        int m_largeChange;

        public Scrollbar()
        {

        }

        public Scrollbar(Control parent) : base(parent)
        {

        }

        public int SmallChange
        {
            get { 
                return Math.Min(this.m_smallChange, this.LargeChange);
            }
            set { this.m_smallChange = value; }
        }

        public int LargeChange
        {
            get
            {
                return Math.Min(m_largeChange, (m_maximum - m_minimum) + 1);
            }
            set { this.m_largeChange = value; }
        }

        public int Maximum
        {
            get { return m_maximum; }
            set
            {
                if (m_maximum != value)
                {
                    if (m_minimum > value)
                    {
                        m_minimum = value;
                    }
                    if (value < m_value)
                    {
                        Value = value;
                    }
                    m_maximum = value;
                }
            }
        }

        public int Minimum
        {
            get { return m_minimum; }
            set
            {
                if (m_minimum != value)
                {
                    if (m_maximum < value)
                    {
                        m_maximum = value;
                    }
                    if (value > m_value)
                    {
                        m_value = value;
                    }
                    m_minimum = value;
                }
            }
        }

        public int Value
        {
            get { return m_value; }
            set
            {
                if (m_value != value)
                {
                    if ((value < m_minimum) || (value > m_maximum))
                    {
                        throw new ArgumentOutOfRangeException("Value", "Value must be between Minimum and Maximum.");
                    }
                    m_value = value;

                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        private void DoScroll(ScrollEventType type)
        {
            int newValue = this.m_value;
            int oldValue = this.m_value;

            switch (type)
            {
                case ScrollEventType.SmallDecrement:
                    newValue = Math.Max(m_value - this.SmallChange, this.m_minimum);
                    break;

                case ScrollEventType.SmallIncrement:
                    newValue = Math.Min((int)(this.m_value + this.SmallChange), (int)((this.m_maximum - this.LargeChange) + 1));
                    break;

                case ScrollEventType.LargeDecrement:
                    newValue = Math.Max(this.m_value - this.LargeChange, this.m_minimum);
                    break;

                case ScrollEventType.LargeIncrement:
                    newValue = Math.Min((int)(this.m_value + this.LargeChange), (int)((this.m_maximum - this.LargeChange) + 1));
                    break;
/*
                case ScrollEventType.ThumbPosition:
                case ScrollEventType.ThumbTrack:
                    {
                        NativeMethods.SCROLLINFO si = new NativeMethods.SCROLLINFO();
                        si.fMask = 0x10;
                        SafeNativeMethods.GetScrollInfo(new HandleRef(this, base.Handle), 2, si);
                        if (this.RightToLeft != RightToLeft.Yes)
                        {
                            newValue = si.nTrackPos;
                            break;
                        }
                        newValue = this.ReflectPosition(si.nTrackPos);
                        break;
                    }
 */ 
                case ScrollEventType.First:
                    newValue = this.m_minimum;
                    break;

                case ScrollEventType.Last:
                    newValue = (this.m_maximum - this.LargeChange) + 1;
                    break;
            }

            ScrollEventArgs se = new ScrollEventArgs(type, oldValue, newValue, this.m_orientation);
            this.OnScroll(se);

            this.Value = se.NewValue;
        }



        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        protected virtual void OnScroll(ScrollEventArgs e)
        {
        }

        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            int trackPosition;

            if (m_orientation == ScrollOrientation.HorizontalScroll)
            {
          //      System.Console.SetCursorPosition(Left, Top);
          //      for (int i = 0; i < Width; i++)
         //           System.Console.Write('-');

                trackPosition = (m_value / m_maximum) * Width + Top;
          //      System.Console.SetCursorPosition(Left, trackPosition);
          //      System.Console.Write('+');
            }
            else
            {
                for (int i = 0; i < Height; i++)
                {
             //       System.Console.SetCursorPosition(Left, Top + i);
            //        System.Console.Write('|');
                }

                trackPosition = (m_value / m_maximum) * Height + Left;
           //     System.Console.SetCursorPosition(trackPosition, Top);
              //  System.Console.Write('+');
            }
        }
    }
}
