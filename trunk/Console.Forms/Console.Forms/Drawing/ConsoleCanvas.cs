using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Crsouza.Console.Forms.Drawing
{
    public class ConsoleCanvas
    {
        public static readonly ConsoleCanvas Instance = new ConsoleCanvas();

        private ConsoleElement[,] drawingBuffer;

        public ConsoleElement[,] DrawingBuffer
        {
            get { return drawingBuffer; }
        }
        private ConsoleElement[,] consoleBuffer;

        public ConsoleElement[,] ConsoleBuffer
        {
            get { return consoleBuffer; }
        }

        public ConsoleElement this[int i, int j]
        {
            set { drawingBuffer[i, j] = value; }
            get { return drawingBuffer[i, j]; }
        }



        public void Invalidate()
        {
            for (int i = 0; i < consoleBuffer.GetLength(0); i++)
                for (int j = 0; j < consoleBuffer.GetLength(1); j++)
                    consoleBuffer[i, j] = ConsoleElement.Empty;
        }

        public void Invalidate(Rectangle region)
        {
            for (int i = region.X; i < region.Width; i++)
                for (int j = region.Y; j < region.Height; j++)
                    consoleBuffer[i, j] = ConsoleElement.Empty;
        }

        private ConsoleCanvas()
        {
            drawingBuffer = new ConsoleElement[80, 25];
            consoleBuffer = new ConsoleElement[80, 25];
        }

        public void Update()
        {
            int cursorLeft = System.Console.CursorLeft;
            int cursorTop = System.Console.CursorTop;
            bool cursorvisibility = System.Console.CursorVisible;

            System.Console.CursorVisible = false;


            for (int j = 0; j < consoleBuffer.GetLength(1); j++)
                for (int i = 0; i < consoleBuffer.GetLength(0); i++)
                    if (consoleBuffer[i, j] != drawingBuffer[i, j])
                    {
                        ConsoleElement conel = drawingBuffer[i, j];

                        consoleBuffer[i, j] = conel;
                        System.Console.SetCursorPosition(i, j);
                        System.Console.BackgroundColor = conel.Background;
                        System.Console.ForegroundColor = conel.Foreground;
                        System.Console.Write(conel.Character);
                    }

            System.Console.CursorLeft = cursorLeft;
            System.Console.CursorTop = cursorTop;
            System.Console.CursorVisible = cursorvisibility;

            System.Console.WindowTop = 0;
        }
    }
}
