using System;
using System.Collections.Generic;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class MainForm
    {
        Label lbProgramTitle;
        Label lbDocumentTitle;
        TextBox tbDocument;
        Label lbStatus;

        CaptionLabel clHelp;
        CaptionLabel clExit;


        public void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Nano# Text Editor";
            this.Size = new System.Drawing.Size(Console.WindowWidth, Console.WindowHeight);
            this.Location = new System.Drawing.Point(0, 0);
            this.KeyPreview = true;

            lbProgramTitle = new Label();
            lbProgramTitle.Text = "CRS nano# 0.0.1";
            lbProgramTitle.Size = new System.Drawing.Size(Width, 1);
            lbProgramTitle.BackColor = ConsoleColor.Gray;
            lbProgramTitle.ForeColor = ConsoleColor.Black;
            lbProgramTitle.NoBackground = false;
            lbProgramTitle.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            lbProgramTitle.Location = new System.Drawing.Point(0, 0);

            lbDocumentTitle = new Label();
            lbDocumentTitle.Text = "New Buffer";
            lbDocumentTitle.Size = new System.Drawing.Size(Width, 1);
            lbDocumentTitle.BackColor = ConsoleColor.Gray;
            lbDocumentTitle.ForeColor = ConsoleColor.Black;
            lbDocumentTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            lbDocumentTitle.NoBackground = true;
            lbDocumentTitle.Location = new System.Drawing.Point(0, 0);

            tbDocument = new TextBox();
            tbDocument.Size = new System.Drawing.Size(Width, Height - 3);
            tbDocument.Location = new System.Drawing.Point(0, 2);
            tbDocument.Padding = new System.Windows.Forms.Padding(1);

            clHelp = new CaptionLabel();
            clHelp.Text = "Get Help";
            clHelp.Hotkey = "^H";
            clHelp.Location = new System.Drawing.Point(0, Height - 2);
            clHelp.Size = new System.Drawing.Size(Width, 1);

            clExit = new CaptionLabel();
            clExit.Text = "Exit";
            clExit.Hotkey = "^Q";
            clExit.Location = new System.Drawing.Point(0, Height - 1);
            clExit.Size = new System.Drawing.Size(Width, 1);

            lbStatus = new Label();
            lbStatus.Text = "[ Read 0 Lines ]";
            lbStatus.ForeColor = ConsoleColor.Black;
            lbStatus.BackColor = ConsoleColor.Gray;
            lbStatus.Size = new System.Drawing.Size(Width, 1);
            lbStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            lbStatus.Location = new System.Drawing.Point(0, Height - 3);
            lbStatus.NoBackground = true;

            this.Controls.Add(lbProgramTitle);
            this.Controls.Add(lbDocumentTitle);
            this.Controls.Add(tbDocument);
            this.Controls.Add(clHelp);
            this.Controls.Add(clExit);
            this.Controls.Add(lbStatus);
            this.ResumeLayout(true);
        }
    }
}
