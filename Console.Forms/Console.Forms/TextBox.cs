using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Scrollbars = System.Windows.Forms.ScrollBars;


namespace Crsouza.Console.Forms
{
    public class TextBox : TextBoxBase
    {

        private Scrollbars m_scrollBars;


        public TextBox()
        {
        }



        public Scrollbars Scrollbars
        {
            get { return m_scrollBars; }
            set { m_scrollBars = value; }
        }

        public void ScrollToCaret()
        {
        }

        public void Select(int start, int length)
        {
        }

        public void SelectAll()
        {
        }

        
        public void Copy()
        {
        }

        public void Paste()
        {
        }

        public void Cut()
        {
        }

        public string SelectedText
        {
            get { throw new NotImplementedException(); }
            set {
                // overwrite selected text
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            SetCursorPosition(getCursorPosition());
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            Invalidate();
        }

        protected override void OnCaretPositionChanged(EventArgs e)
        {
            base.OnCaretPositionChanged(e);

            Point p = getCursorPosition();
            SetCursorPosition(p);
        }

        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            base.OnPaint(e);

            string line;
            for (int i = 0; i < LineCount; i++)
            {
                line = m_textLines[m_firstVisibleLineIndex + i].ToString();
                e.Graphics.DrawText(line, new Point(0, i), ForeColor, BackColor);
                if (line.Length < Width)
                    e.Graphics.DrawLine(' ', new Point(line.Length, i), new Point(Width, i), ForeColor, BackColor);
            }

        }


    }
}
