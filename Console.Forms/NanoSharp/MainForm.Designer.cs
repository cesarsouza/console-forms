using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class MainForm
    {
        Label lbProgramTitle;
        Label lbDocumentTitle;
        Line line2;
        TextBox tbDocument;
        Label lbHelpCaption;
        Label lbHelpKey;


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

            line2 = new Line();
            line2.Size = new System.Drawing.Size(Width, 1);
            line2.Location = new System.Drawing.Point(0, Height-3);
            line2.Character = '_';

            lbHelpKey = new Label();
            lbHelpKey.Text = "^H";
            lbHelpKey.Size = new System.Drawing.Size(Width, 1);
            lbHelpKey.BackColor = ConsoleColor.Gray;
            lbHelpKey.ForeColor = ConsoleColor.Black;
            lbHelpKey.Location = new System.Drawing.Point(0, Height-1);

            lbHelpCaption = new Label();
            lbHelpCaption.Text = "Get Help";
            lbHelpCaption.Size = new System.Drawing.Size(Width, 1);
            lbHelpCaption.NoBackground = true;
            lbHelpCaption.Location = new System.Drawing.Point(3, Height-1);

            this.Controls.Add(lbProgramTitle);
            this.Controls.Add(lbDocumentTitle);
            this.Controls.Add(tbDocument);
            this.Controls.Add(line2);
            this.Controls.Add(lbHelpKey);
            this.Controls.Add(lbHelpCaption);

            this.ResumeLayout(true);

            tbDocument.Focus();
        }
    }
}
