using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crsouza.Console.Forms
{

    public delegate void ConsolePaintEventHandler(object sender, ConsolePaintEventArgs e);

    public sealed class ConsolePaintEventArgs : EventArgs
    {
        private ConsoleGraphics m_graphicsObject;

        public ConsolePaintEventArgs(ConsoleGraphics graphics)
        {
            m_graphicsObject = graphics;
        }

        public ConsoleGraphics Graphics
        {
            get { return m_graphicsObject; }
        }
    }
}
