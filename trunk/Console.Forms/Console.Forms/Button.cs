using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using DialogResult = System.Windows.Forms.DialogResult;
using HorizontalAlignment = System.Windows.Forms.HorizontalAlignment;

namespace Crsouza.Console.Forms
{
    public class Button : Control
    {
        
        private DialogResult m_dialogResult;
        private HorizontalAlignment m_textAlign;
        

        public Button()
        {

        }

        public Button(Control parent) : base(parent)
        {
            
        }

        public HorizontalAlignment TextAlign
        {
            get { return m_textAlign; }
            set { m_textAlign = value; }
        }

        public DialogResult DialogResult
        {
            get { return m_dialogResult; }
            set { m_dialogResult = value; }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            SetCursorPosition(0, 0);
        }


        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

       
        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);

            if (e.KeyInfo.Key == ConsoleKey.Enter ||
                e.KeyInfo.Key == ConsoleKey.Spacebar)
            {
                PerformClick();
            }

        }

        protected override void OnClick(EventArgs e)
        {
            if (Parent != null && Parent is Form)
            {
                (Parent as Form).DialogResult = this.m_dialogResult;
            }

            base.OnClick(e);
        }

        

        protected override void OnPaint(ConsolePaintEventArgs e)
        {
            e.Graphics.DrawText(Text, new Point(0, 0), ForeColor, BackColor);
        }
    }
}
