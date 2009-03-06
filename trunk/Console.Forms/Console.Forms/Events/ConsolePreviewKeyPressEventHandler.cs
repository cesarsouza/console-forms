using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crsouza.Console.Forms
{
    public delegate void ConsolePreviewKeyPressEventHandler(object sender, ConsolePreviewKeyPressEventArgs e);

    public class ConsolePreviewKeyPressEventArgs : EventArgs
    {
        private ConsoleKeyInfo m_consoleKey;
        private bool m_handled;


        public ConsolePreviewKeyPressEventArgs(ConsoleKeyInfo key)
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

        public bool Handled
        {
            get { return m_handled; }
            set { m_handled = value; }
        }
    }
}
