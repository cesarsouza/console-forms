using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Crsouza.Console.Forms
{
    public class Line : Crsouza.Console.Forms.Control
    {
        private Orientation m_orientation = Orientation.Horizontal;
        private char m_lineChar = '-';

        public Line()
        {
        }

        public Orientation Orientation
        {
            get { return m_orientation; }
            set { m_orientation = value; }
        }

        public Char Character
        {
            get { return m_lineChar; }
            set { m_lineChar = value; }
        }


        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            base.OnPaint(e);
            
            if (m_orientation == Orientation.Horizontal)
            {
                for (int i = 0; i < Width; i++)
                    e.Graphics[i, 0] = Character;
            }
        }
    }
}
