using System;
using System.Collections.Generic;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    partial class MainForm
    {
        
        public void InitializeComponent()
        {
            this.lbStatus = new Label();
            this.lbProgramTitle = new Label();
            this.lbDocumentTitle = new Label();
            this.tbDocument = new TextBox();
            this.clHelp = new CaptionLabel();
            this.clExit = new CaptionLabel();


            this.SuspendLayout();
            this.Text = "Nano# Text Editor";
            this.Size = new System.Drawing.Size(Console.WindowWidth, Console.WindowHeight);
            this.Location = new System.Drawing.Point(0, 0);
            this.KeyPreview = true;
            
            this.lbProgramTitle.Text = "CRS nano# 0.0.1";
            this.lbProgramTitle.Size = new System.Drawing.Size(Width, 1);
            this.lbProgramTitle.BackColor = ConsoleColor.Gray;
            this.lbProgramTitle.ForeColor = ConsoleColor.Black;
            this.lbProgramTitle.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.lbProgramTitle.Location = new System.Drawing.Point(0, 0);

            this.lbDocumentTitle.Text = "New Buffer";
            this.lbDocumentTitle.Size = new System.Drawing.Size(Width, 1);
            this.lbDocumentTitle.BackColor = ConsoleColor.Gray;
            this.lbDocumentTitle.ForeColor = ConsoleColor.Black;
            this.lbDocumentTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lbDocumentTitle.NoBackground = true;
            this.lbDocumentTitle.Location = new System.Drawing.Point(0, 0);

            this.tbDocument.Size = new System.Drawing.Size(Width, Height - 5);
            this.tbDocument.Location = new System.Drawing.Point(0, 2);
            this.tbDocument.Padding = new System.Windows.Forms.Padding(1);

            this.clHelp.Text = "Get Help";
            this.clHelp.Hotkey = "^H";
            this.clHelp.Location = new System.Drawing.Point(0, Height - 2);
            this.clHelp.Size = new System.Drawing.Size(Width, 1);

            this.clExit.Text = "Exit";
            this.clExit.Hotkey = "^Q";
            this.clExit.Location = new System.Drawing.Point(0, Height - 1);
            this.clExit.Size = new System.Drawing.Size(Width, 1);

            this.lbStatus.Text = "[ Read 0 Lines ]";
            this.lbStatus.ForeColor = ConsoleColor.Black;
            this.lbStatus.BackColor = ConsoleColor.Gray;
            this.lbStatus.Size = new System.Drawing.Size(Width, 1);
            this.lbStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lbStatus.Location = new System.Drawing.Point(0, Height - 3);
            this.lbStatus.NoBackground = true;


            this.Controls.Add(lbProgramTitle);
            this.Controls.Add(lbDocumentTitle);
            this.Controls.Add(tbDocument);
            this.Controls.Add(clHelp);
            this.Controls.Add(clExit);
            this.Controls.Add(lbStatus);
            this.ResumeLayout(true);
        }


        private Label lbProgramTitle;
        private Label lbDocumentTitle;
        private TextBox tbDocument;
        private Label lbStatus;
        private CaptionLabel clHelp;
        private CaptionLabel clExit;
    }
}
