using System;
using System.Collections.Generic;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class CaptionLabel
    {
        Label lbHotkey;
        Label lbDescription;

        public void InitializeComponent()
        {
            lbHotkey = new Label();
            lbDescription = new Label();

            this.SuspendLayout();

            lbHotkey.Size = new System.Drawing.Size(2, 1);
            lbHotkey.Location = new System.Drawing.Point(0, 0);
            lbHotkey.BackColor = ConsoleColor.Gray;
            lbHotkey.ForeColor = ConsoleColor.Black;

            lbDescription.Size = new System.Drawing.Size(10, 1);
            lbDescription.Location = new System.Drawing.Point(3, 0);
            lbDescription.NoBackground = true;
            lbDescription.BackColor = ConsoleColor.Black;
            lbDescription.ForeColor = ConsoleColor.Gray;

            this.Controls.Add(lbHotkey);
            this.Controls.Add(lbDescription);
            this.ResumeLayout(true);

            this.PerformLayout();
        }
    }
}
