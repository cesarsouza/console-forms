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
            m_firstVisibleLineIndex = 1;
            m_currentLinePosition = 0;
            m_currentLineIndex = 1;
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

        public Point CarretPosition
        {
            get { return new Point(m_currentLineIndex, m_currentLinePosition); }
            set
            {
                m_currentLineIndex = value.X;
                m_currentLinePosition = value.Y;
            }
        }

        public int LineCount
        {
            get { return m_textLines.Count; }
        }


        protected Point getCursorPosition()
        {
            return new Point(m_currentLineIndex - m_firstVisibleLineIndex,
                m_currentLinePosition - m_firstVisibleLineIndex);
        }

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



        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);


            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                  //  m_document.CurrentLineIndex--;
                    break;

                case ConsoleKey.DownArrow:
                  //  m_document.CurrentLineIndex++;
                    break;

                case ConsoleKey.LeftArrow:
                  //  m_document.CurrentLinePositionIndex--;
                    break;

                case ConsoleKey.RightArrow:
                  //  m_document.CurrentLinePositionIndex++;
                    break;

                default:
                    break;
            }


            if (!Char.IsControl(e.KeyInfo.KeyChar))
            {
             //   m_document.Write(e.KeyInfo.KeyChar);
                return;
            }


        }
    }
}
