using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Crsouza.Console.Forms
{
    public abstract class TextBoxBase : Control
    {
        // Fields
        private bool m_acceptsTab;
        private bool m_hideSelection;
        private bool m_integralHeightAdjust;
        private int m_maxLength;
        private bool m_modified;
        private bool m_multiline;
        private bool m_readOnly;
        private int m_selectionLength;
        private int m_selectionStart;
        private bool m_wordWrap;

        private int m_showLineNumbers;

        protected int m_currentLineIndex;
        protected int m_currentLinePosition;

        protected int m_firstVisibleLineIndex;
        protected int m_firstVisiblePositionIndex;

        protected List<StringBuilder> m_textLines;



        public TextBoxBase()
        {
            m_textLines = new List<StringBuilder>();
            m_firstVisibleLineIndex = 0;
            m_currentLinePosition = 0;
            m_currentLineIndex = 0;
        }


        public override string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (StringBuilder line in m_textLines)
                {
                    sb.AppendLine(line.ToString());
                }
                return sb.ToString();
            }
            set
            {
                String[] lines = value.Split('\n');
                m_textLines = new List<StringBuilder>();
                foreach (String line in lines)
                {
                    m_textLines.Add(new StringBuilder(line));
                }

                OnTextChanged(EventArgs.Empty);
            }
        }

        public Point CaretPosition
        {
            get { return new Point(m_currentLineIndex, m_currentLinePosition); }
            set { setCaretPosition(value.Y, value.X); }
        }

        private void setCaretPosition(int line, int col)
        {
            if (line != m_currentLineIndex || col != m_currentLinePosition)
            {
                bool changed = false;
                if (line >= 0 && line < LineCount)
                {
                    m_currentLineIndex = line;
                    changed = true;
                }

                if (col >= 0 && col < getCurrentLine().Length)
                {
                    m_currentLinePosition = col;
                    changed = true;
                }
                if (changed)
                {
                    OnCaretPositionChanged(EventArgs.Empty);
                }
            }
        }

        public int LineCount
        {
            get { return m_textLines.Count; }
        }

        protected virtual void OnCaretPositionChanged(EventArgs e)
        {

        }

        protected Point getCursorPosition()
        {
            return new Point(m_currentLinePosition - m_firstVisiblePositionIndex,
               m_currentLineIndex - m_firstVisibleLineIndex);
        }
/*
        protected string[] getTextWindow(int startingLine, int startingCol, int maxWidth, int lineCount)
        {
            string[] lines = new string[lineCount];
            for (int i = startingLine; i < lineCount; i++)
            {
                string line = lines[i] = m_textLines[i].ToString();
                if (m_textLines[i].Length < maxWidth)
                    lines[i] = line;
                else lines[i] = line.Substring(0, maxWidth);
            }
            return lines;
        }
*/
        protected StringBuilder getCurrentLine()
        {
            return m_textLines[m_currentLineIndex];
        }



        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);

            char ch = e.KeyInfo.KeyChar;

            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    setCaretPosition(m_currentLineIndex - 1, m_currentLinePosition);
                    return;
                    break;

                case ConsoleKey.DownArrow:
                    setCaretPosition(m_currentLineIndex + 1,m_currentLinePosition);
                    return;
                    break;

                case ConsoleKey.LeftArrow:
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition - 1);
                    return;
                    break;

                case ConsoleKey.RightArrow:
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition + 1);
                    return;
                    break;

                default:
                    break;
            }

            if (e.KeyInfo.Key == ConsoleKey.Backspace)
            {
                if (m_currentLinePosition > 0)
                {
                    getCurrentLine().Remove(m_currentLinePosition - 1, 1);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition - 1);
                    return;
                }
            }

            else if (e.KeyInfo.Key == ConsoleKey.Delete)
            {
                if (m_currentLinePosition < getCurrentLine().Length)
                {
                    getCurrentLine().Remove(m_currentLinePosition, 1);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition);
                    return;
                }
            }
            else if (char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))
            {
                getCurrentLine().Insert(m_currentLinePosition, e.KeyInfo.KeyChar);
                OnTextChanged(EventArgs.Empty);
                setCaretPosition(m_currentLineIndex, m_currentLinePosition + 1);
            }
        }
    }
}
