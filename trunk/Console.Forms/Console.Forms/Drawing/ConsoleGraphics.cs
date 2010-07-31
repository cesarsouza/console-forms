using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Crsouza.Console.Forms.Drawing;


namespace Crsouza.Console.Forms
{
    public sealed class ConsoleGraphics
    {
        private Rectangle m_area;




        public ConsoleGraphics(Rectangle area)
        {
            m_area = area;
        }


        public char this[int i, int j]
        {
            set
            {
                if (i < 0 || i > m_area.Width)
                    throw new IndexOutOfRangeException();

                ConsoleCanvas.Instance[m_area.X + i, m_area.Y + j] = new ConsoleElement(value, ConsoleColor.Black, ConsoleColor.White);
            }
        }

        public void DrawRectangle(Rectangle rectangle, char ch, ConsoleColor foreground, ConsoleColor background)
        {
            string line = new string(ch, rectangle.Width);
            for (int i = rectangle.Y; i < rectangle.Height; i++)
                for (int j = rectangle.X; j < rectangle.Width; j++)
                    ConsoleCanvas.Instance[m_area.X + j, m_area.Y + i] = new ConsoleElement(ch, background, foreground);
        }

        public void DrawRectangle(Rectangle rectangle, ConsoleColor foreground, ConsoleColor background)
        {
            
            for (int i = rectangle.Y; i < rectangle.Height; i++)
            {
                for (int j = rectangle.X; j < rectangle.Width; j++)
                {
                    ConsoleCanvas.Instance[m_area.X + j, m_area.Y + i] = new ConsoleElement(ConsoleCanvas.Instance[m_area.X + j, m_area.Y + i].Character, background, foreground);
                }
            }
        }


        public void DrawText(string text, Point location, ConsoleColor foreground, ConsoleColor background)
        {
            List<String> lines = new List<string>();
            int lineSize = m_area.Width;

            while (text.Length > m_area.Width)
            {
                lines.Add(text.Substring(0, lineSize - 1) + "$");
                text = text.Substring(lineSize, text.Length - lineSize);
            }
            lines.Add(text);

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = location.X; j < lines[i].Length; j++)
                    ConsoleCanvas.Instance[m_area.X + j, m_area.Y + i + location.Y] = new ConsoleElement(lines[i][j], background, foreground);
            }

        }

        public void DrawLine(char ch, Point p1, Point p2, ConsoleColor foreground, ConsoleColor background)
        {
            //TODO: support vertical or diagonal lines
            string line = new string(ch, p2.X - p1.X);

            for (int i = p1.X; i < p2.X; i++)
                ConsoleCanvas.Instance[m_area.X + i, m_area.Y + p1.Y] = new ConsoleElement(ch, background, foreground);

        }



    }
}
