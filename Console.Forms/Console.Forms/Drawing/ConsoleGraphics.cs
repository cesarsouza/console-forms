using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
                System.Console.SetCursorPosition(i + m_area.Left, j + m_area.Top);
                System.Console.Write(value);
              //  System.Console.WindowTop = 0;
            }
        }

        public void DrawRectangle(Rectangle rectangle, char ch, ConsoleColor foreground, ConsoleColor background)
        {
            System.Console.CursorVisible = false;
            System.Console.ForegroundColor = foreground;
            System.Console.BackgroundColor = background;

            string line = new string(ch, rectangle.Width);
            for (int i = rectangle.Y; i < rectangle.Height; i++)
            {
                System.Console.SetCursorPosition(m_area.Left, m_area.Top+i);
                System.Console.Write(line);
                System.Console.WindowTop = 0;
            }
         //   System.Console.WindowTop = 0;
            System.Console.CursorVisible = true;
        }

        public void DrawText(string text, Point location, ConsoleColor foreground, ConsoleColor background)
        {
            System.Console.CursorVisible = false;
            List<String> lines = new List<string>();

            int lineSize = m_area.Width;
            
            while (text.Length > m_area.Width)
            {
                lines.Add(text.Substring(0, lineSize));
                text = text.Substring(lineSize, text.Length - lineSize);
            }
            lines.Add(text);

            System.Console.ForegroundColor = foreground;
            System.Console.BackgroundColor = background;

            for (int i = 0; i < lines.Count; i++)
            {
                System.Console.SetCursorPosition(location.X + m_area.Left, location.Y + m_area.Top + i);
                System.Console.WriteLine(lines[i]);
            }

            System.Console.CursorVisible = true;   
        }

    }
}
