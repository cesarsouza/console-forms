using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Crsouza.Console.Forms.TextControl
{
    internal class Document
    {
        
        private int m_showLineNumbers;

        private int m_currentLine;
        private int m_currentLinePosition;

        public event EventHandler CurrentPositionChanged;
        public event EventHandler CurrentLineChanged;

        private List<StringBuilder> m_lines;

        public Document()
        {
            m_lines = new List<StringBuilder>();
            m_lines.Add(new StringBuilder());
        }

        public int CurrentLineIndex
        {
            get { return m_currentLine; }
            set
            {
                if (value < 0 || value > LineCount-1)
                    return;
             
                m_currentLine = value;

                OnCurrentPositionChanged(EventArgs.Empty);
            }
        }

        protected StringBuilder CurrentLine
        {
            get { return m_lines[m_currentLine]; }
        }

        protected virtual void OnCurrentPositionChanged(EventArgs e)
        {
            if (CurrentPositionChanged != null)
                CurrentPositionChanged(this, e);
        }

        protected virtual void OnCurrentLineChanged(EventArgs e)
        {
            if (CurrentLineChanged != null)
                CurrentLineChanged(this, e);
        }

        public int CurrentLinePositionIndex
        {
            get { return m_currentLinePosition; }
            set {
                if (value < 0 || value > CurrentLine.Length)
                    return;

                m_currentLinePosition = value;

                OnCurrentPositionChanged(EventArgs.Empty);
            }
        }


        public String Text
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (StringBuilder line in m_lines)
                {
                    sb.AppendLine(line.ToString());
                }
                return sb.ToString();
            }
            set
            {
                String[] lines = value.Split('\n');
                m_lines = new List<StringBuilder>();
                foreach (String line in lines)
                {
                    m_lines.Add(new StringBuilder(line));
                }
            }
        }

        public string[] GetTextWindow(int startingLine, int startingCol, int maxWidth, int lineCount)
        {
            string[] lines = new string[lineCount];
            for (int i = startingLine; i < lineCount; i++)
            {
                string line = lines[i] = m_lines[i].ToString();
                if (m_lines[i].Length < maxWidth)
                    lines[i] = line;
                else  lines[i] = line.Substring(0, maxWidth);
            }
            return lines;
        }

        public void Write(char c)
        {
            CurrentLine.Insert(m_currentLinePosition, c);
            OnCurrentLineChanged(EventArgs.Empty);
        }

        public void GoToWordEnd()
        {

        }

        public void OverwriteSelection(string text)
        {
        }

        public int LineCount
        {
            get { return m_lines.Count; }
        }
    }
}
