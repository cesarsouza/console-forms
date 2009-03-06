using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Crsouza.Console.Forms
{
    public abstract class TextBoxBase : Control
    {
        // Fields
        private TextControl.Document m_document;
        private bool m_acceptsTab;
        private bool m_hideSelection;
        private bool m_integralHeightAdjust;
        private int m_maxLength;
        private bool m_modified;
        private bool m_multiline;
        private bool m_readOnly;
        private int m_selectionLength;
        private int m_selectionStart;
        private bool m_wordWrap;



        private int m_firstVisibleLineIndex;
        private int m_firstVisiblePositionIndex;


        public TextBoxBase()
        {
            m_document = new Crsouza.Console.Forms.TextControl.Document();
            m_document.CurrentPositionChanged += new EventHandler(m_document_CurrentPositionChanged);
            m_document.CurrentLineChanged += new EventHandler(m_document_CurrentLineChanged);
        }

        void m_document_CurrentLineChanged(object sender, EventArgs e)
        {
            PerformLayout();
        }

        void m_document_CurrentPositionChanged(object sender, EventArgs e)
        {
            SetCursorPosition(GetCursorPositionFromDocument());
        }

        public override string Text
        {
            get { return m_document.Text; }
            set
            {
                m_document.Text = value;
                // reset selection;
            }
        }


        public Point CarretPosition
        {
            get { return new Point(m_document.CurrentLineIndex, m_document.CurrentLinePositionIndex); }
            set
            {
                m_document.CurrentLineIndex = value.X;
                m_document.CurrentLinePositionIndex = value.Y;
            }
        }

        public Point GetCursorPositionFromDocument()
        {
            Point p = new Point(m_document.CurrentLineIndex - m_firstVisibleLineIndex,
                m_document.CurrentLinePositionIndex - m_firstVisibleLineIndex);
            return p;
        }


        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);


            switch (e.KeyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    m_document.CurrentLineIndex--;
                    break;

                case ConsoleKey.DownArrow:
                    m_document.CurrentLineIndex++;
                    break;

                case ConsoleKey.LeftArrow:
                    m_document.CurrentLinePositionIndex--;
                    break;

                case ConsoleKey.RightArrow:
                    m_document.CurrentLinePositionIndex++;
                    break;

                default:
                    break;
            }


            if (!Char.IsControl(e.KeyInfo.KeyChar))
            {
                m_document.Write(e.KeyInfo.KeyChar);
                return;
            }


        }
    }
}
