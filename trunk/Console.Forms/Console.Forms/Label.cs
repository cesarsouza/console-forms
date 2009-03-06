using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;

namespace Crsouza.Console.Forms
{
    public class Label : Control
    {

         private HorizontalAlignment m_alignment = HorizontalAlignment.Left;

         public HorizontalAlignment TextAlign
         {
             get { return m_alignment; }
             set
             {
                 m_alignment = value;
                 OnTextChanged(EventArgs.Empty);
             }
         }

        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            base.OnPaint(e);

            Point location;
            switch (m_alignment)
            {
                case HorizontalAlignment.Left:
                    location = new Point(0, 0);
                    break;
                case HorizontalAlignment.Right:
                    location = new Point(Width - Text.Length, 0);
                    break;
                case HorizontalAlignment.Center:
                    location = new Point((Width - Text.Length) / 2, 0);
                    break;
                default:
                    goto case HorizontalAlignment.Left;
            }

            e.Graphics.DrawText(Text, location, ForeColor, BackColor);
        }
    }
}
