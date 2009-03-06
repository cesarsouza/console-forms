using System;
using System.Collections.Generic;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class CaptionLabel : UserControl
    {
        public CaptionLabel()
        {
            InitializeComponent();
        }

        public override String Text
        {
            get { return lbDescription.Text; }
            set { lbDescription.Text = value; }
        }

        public String Hotkey
        {
            get { return lbHotkey.Text; }
            set { lbHotkey.Text = value; }
        }
    }
}
