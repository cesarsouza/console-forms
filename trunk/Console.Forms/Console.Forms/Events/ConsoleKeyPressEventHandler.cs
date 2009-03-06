using System;
using System.Collections.Generic;
using System.Text;

namespace Crsouza.Console.Forms
{

    public delegate void ConsoleKeyPressEventHandler(object sender, ConsoleKeyPressEventArgs e);

    public class ConsoleKeyPressEventArgs : EventArgs
    {
        private ConsoleKeyInfo m_consoleKey;


        public ConsoleKeyPressEventArgs(ConsoleKeyInfo key)
        {
            m_consoleKey = key;
        }

        public ConsoleKeyInfo KeyInfo
        {
            get { return m_consoleKey; }
        }


        public bool Control
        {
            get
            {
                return (m_consoleKey.Modifiers & ConsoleModifiers.Control) == ConsoleModifiers.Control;
            }
        }

        public bool Shift
        {
            get
            {
                return (m_consoleKey.Modifiers & ConsoleModifiers.Shift) == ConsoleModifiers.Shift;
            }
        }

        public bool Alt
        {
            get
            {
                return (m_consoleKey.Modifiers & ConsoleModifiers.Alt) == ConsoleModifiers.Alt;
            }
        }

    }
}
