using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class HelpBox
    {
        Label lbDialogTitle;
        Label lbAbout;
        Button btnOk;
        Button btnCancel;


        public void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Nano# Text Editor - Help";
            this.Size = new System.Drawing.Size(50, 6);
            this.Location = new System.Drawing.Point(2, 2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = ConsoleColor.DarkBlue;
            this.ForeColor = ConsoleColor.Blue;
            this.NoBackground = false;

            lbDialogTitle = new Label();
            lbDialogTitle.Text = "Help";
            lbDialogTitle.Size = new System.Drawing.Size(Width, 1);
            lbDialogTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            lbDialogTitle.BackColor = ConsoleColor.Gray;
            lbDialogTitle.ForeColor = ConsoleColor.Black;
            lbDialogTitle.NoBackground = false;
            lbDialogTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 0);
            lbDialogTitle.Location = new System.Drawing.Point(0, 0);

            lbAbout = new Label();
            lbAbout.Text = "This is a simple description text to test multiline labels (which currently looks horrible).";
            lbAbout.Size = new System.Drawing.Size(Width-2, 2);
            lbAbout.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            lbAbout.BackColor = ConsoleColor.DarkGreen;
            lbAbout.ForeColor = ConsoleColor.Green;
            lbAbout.NoBackground = false;
            lbAbout.Padding = new System.Windows.Forms.Padding(0);
            lbAbout.Location = new System.Drawing.Point(1, 2);

            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Size = new System.Drawing.Size(2, 1);
            btnOk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            btnOk.BackColor = ConsoleColor.DarkGreen;
            btnOk.ForeColor = ConsoleColor.Green;
            btnOk.NoBackground = false;
            btnOk.Location = new System.Drawing.Point(1, Height - 1);
            btnOk.Click += new EventHandler(btnOk_Click);
            btnOk.TabIndex = 1;

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new System.Drawing.Size(6, 1);
            btnCancel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            btnCancel.BackColor = ConsoleColor.DarkRed;
            btnCancel.NoBackground = false;
            btnCancel.ForeColor = ConsoleColor.Red;
            btnCancel.Location = new System.Drawing.Point(Width-btnCancel.Width-1, Height - 1);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnCancel.TabIndex = 2;

            this.Controls.Add(lbDialogTitle);
            this.Controls.Add(lbAbout);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.ResumeLayout(true);

        }

    }
}
