using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;



namespace Crsouza.Console.Forms
{
    public class TextBox : TextBoxBase
    {

        private ScrollBars m_scrollBars;


        public TextBox()
        {
        }



        public ScrollBars Scrollbars
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
            set
            {
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

        protected override void OnPaintBackground(ConsolePaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            base.OnPaint(e);

            string line;
            for (int i = 0; i < LineCount-m_firstVisibleLineIndex; i++)
            {
                line = m_textLines[m_firstVisibleLineIndex + i].ToString();
                e.Graphics.DrawText(line, new Point(0, i), ForeColor, BackColor, false);
            }

            if (m_selecting)
            {
                e.Graphics.DrawRectangle(
                    new Rectangle(m_selectionStart.X, m_selectionStart.Y,
                    m_selectStop.X - m_selectionStart.X,
                    m_selectStop.Y - m_selectionStart.Y),
                    BackColor, ForeColor);
            }

        }


    }
}
