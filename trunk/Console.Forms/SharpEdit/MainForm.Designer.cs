using System;
using System.Collections.Generic;
using System.Text;
using Crsouza.Console.Forms;
using System.Drawing;

namespace SharpEdit
{
    partial class MainForm
    {
        public void InitializeComponent()
        {
             this.lbStatus = new Label();
            this.lbProgramTitle = new Label();
            this.lbDocumentTitle = new Label();
            this.tbDocument = new TextBox();

            this.SuspendLayout();
            this.Text = "Nano# Text Editor";
            this.Size = new Size(Console.WindowWidth, Console.WindowHeight);
            this.Location = new Point(0, 0);
            this.KeyPreview = true;
            
            this.lbProgramTitle.Text = "CRS nano# 0.0.1";
            this.lbProgramTitle.Size = new Size(Width, 1);
            this.lbProgramTitle.BackColor = ConsoleColor.Gray;
            this.lbProgramTitle.ForeColor = ConsoleColor.Black;
            this.lbProgramTitle.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.lbProgramTitle.Location = new Point(0, 0);

            this.lbDocumentTitle.Text = "New Buffer";
            this.lbDocumentTitle.Size = new Size(Width, 1);
            this.lbDocumentTitle.BackColor = ConsoleColor.Gray;
            this.lbDocumentTitle.ForeColor = ConsoleColor.Black;
            this.lbDocumentTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lbDocumentTitle.NoBackground = true;
            this.lbDocumentTitle.Location = new System.Drawing.Point(0, 0);

            this.tbDocument.Size = new Size(Width, Height - 5);
            this.tbDocument.Location = new Point(0, 2);
            this.tbDocument.Padding = new System.Windows.Forms.Padding(1);

            this.lbStatus.Text = "[ Read 0 Lines ]";
            this.lbStatus.ForeColor = ConsoleColor.Black;
            this.lbStatus.BackColor = ConsoleColor.Gray;
            this.lbStatus.Size = new Size(Width, 1);
            this.lbStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.lbStatus.Location = new Point(0, Height - 3);
            this.lbStatus.NoBackground = true;


            this.Controls.Add(lbProgramTitle);
            this.Controls.Add(lbDocumentTitle);
            this.Controls.Add(tbDocument);
            this.Controls.Add(lbStatus);
            this.ResumeLayout(true);
        }


        private Label lbProgramTitle;
        private Label lbDocumentTitle;
        private TextBox tbDocument;
        private Label lbStatus;
        
    }
}
