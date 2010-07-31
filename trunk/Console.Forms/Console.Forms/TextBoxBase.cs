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

        protected Point m_selectionStart;
        protected Point m_selectStop;


        private bool m_wordWrap;

        private int m_showLineNumbers;

        protected int m_currentLineIndex;
        protected int m_currentLinePosition;

        protected int m_firstVisibleLineIndex;
        protected int m_firstVisiblePositionIndex;

        protected int m_visibleLines = 19;

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

                /*        if (line < 0)
                            line = 0;
                        if (line >= LineCount)
                            return;
                  */
                if (line > m_visibleLines + m_firstVisibleLineIndex)
                {
                    m_firstVisibleLineIndex = m_firstVisibleLineIndex - (m_visibleLines - line);
                    OnTextChanged(EventArgs.Empty);
                    line = m_visibleLines;
                    
                }

                else if (line < m_firstVisibleLineIndex)
                {
                    if (m_firstVisibleLineIndex == 0)
                    {
                        line = 0;
                        return;
                    }
                    else
                    {
                        m_firstVisibleLineIndex = m_firstVisibleLineIndex + line;
                        OnTextChanged(EventArgs.Empty);
                        line = 0;
                        //setCaretPosition(0, 0);
                    }
                }

                if (col < 0)
                {
                    if (line == 0)
                        return;

                    // Column is negative
                    line = line - 1;
                    col = m_textLines[line].Length + col + 1;

                }

                if (line < m_textLines.Count && line >= 0 && col > m_textLines[line].Length)
                {
                    // Column is greater than line length
                    col = col - m_textLines[line].Length - 1;
                    line = line + 1;
                }


                if (line >= 0 && line < LineCount)
                {
                    m_currentLineIndex = line;
                    changed = true;
                }

                if (col >= 0 && line < LineCount && line > 0 && col <= m_textLines[line].Length)
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

        protected StringBuilder getCurrentLine()
        {
            return m_textLines[m_currentLineIndex];
        }


        protected bool m_selecting;
        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);

            char ch = e.KeyInfo.KeyChar;

            if (e.Shift)
            {
                if (!m_selecting)
                {
                    m_selecting = true;
                    m_selectionStart = getCursorPosition();
                    m_selectStop = getCursorPosition();
                }
                else
                {
                    m_selectStop = getCursorPosition();
                    OnTextChanged(EventArgs.Empty);
                }
            }
            else
            {
                if (m_selecting)
                    m_selecting = false;
            }

            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    if (m_currentLineIndex > 0 && m_currentLinePosition > m_textLines[m_currentLineIndex - 1].Length)
                        m_currentLinePosition = m_textLines[m_currentLineIndex - 1].Length;
                    setCaretPosition(m_currentLineIndex - 1, m_currentLinePosition);
                    return;

                case ConsoleKey.DownArrow:
                    if (m_currentLineIndex < LineCount - 1 && m_currentLinePosition > m_textLines[m_currentLineIndex + 1].Length)
                        m_currentLinePosition = m_textLines[m_currentLineIndex + 1].Length;
                    setCaretPosition(m_currentLineIndex + 1, m_currentLinePosition);
                    return;

                case ConsoleKey.LeftArrow:
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition - 1);
                    return;

                case ConsoleKey.RightArrow:
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition + 1);
                    return;

                case ConsoleKey.End:
                    setCaretPosition(m_currentLineIndex, getCurrentLine().Length);
                    return;

                case ConsoleKey.Home:
                    setCaretPosition(m_currentLineIndex, 0);
                    return;

                default:
                    break;
            }
            if (e.KeyInfo.Key == ConsoleKey.Enter)
            {
                var currentLine = getCurrentLine();
                string toend = currentLine.ToString().Substring(m_currentLinePosition);
                currentLine.Remove(m_currentLinePosition, currentLine.Length - m_currentLinePosition);
                m_textLines.Insert(m_currentLineIndex + 1, new StringBuilder(toend));
                OnTextChanged(EventArgs.Empty);
                setCaretPosition(m_currentLineIndex + 1, 0);
                return;
            }

            else if (e.KeyInfo.Key == ConsoleKey.Backspace)
            {
                if (m_currentLinePosition > 0)
                {
                    getCurrentLine().Remove(m_currentLinePosition - 1, 1);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition - 1);
                }
                else
                {
                    if (m_currentLineIndex == 0) return;

                    // Copy the current line to the end of the previous line
                    string sl = getCurrentLine().ToString();
                    m_textLines[m_currentLineIndex - 1].Append(sl);
                    m_textLines.RemoveAt(m_currentLineIndex);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex - 1, m_textLines[m_currentLineIndex - 1].Length - sl.Length);

                }
                return;
            }

            else if (e.KeyInfo.Key == ConsoleKey.Delete)
            {
                if (m_currentLinePosition < getCurrentLine().Length)
                {
                    getCurrentLine().Remove(m_currentLinePosition, 1);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition);
                }
                else
                {
                    // Copy the next line to the end of this line
                    string sl = m_textLines[m_currentLineIndex + 1].ToString();
                    getCurrentLine().Append(sl);
                    m_textLines.RemoveAt(m_currentLineIndex + 1);
                    OnTextChanged(EventArgs.Empty);
                    setCaretPosition(m_currentLineIndex, m_currentLinePosition);
                }
                return;
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
