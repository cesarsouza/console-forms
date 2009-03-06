using System;
using System.Collections.Generic;
using System.Text;
using Crsouza.Console.Forms;


namespace NanoSharp
{
    partial class MainForm : Crsouza.Console.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();

            this.ActiveControl = tbDocument;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tbDocument.Text = 
                "Lorem ipsum dolor sit amet, consectetuer\n" +
                              "adipiscing elit, sed diam nonummy nibh euismod\n" +
                              "tincidunt ut laoreet dolore magna aliquam erat\n"+
                              "volutpat. Ut wisi enim ad minim veniam, quis nostrud\n"+
                              "exerci tation ullamcorper suscipit lobortis nisl ut\n"+
                              "aliquip ex ea commodo consequat. Duis autem vel eum\n" +
                              "iriure dolor in hendrerit in vulputate velit esse\n"+
                              "molestie consequat, vel illum dolore eu feugiat nulla\n"+
                              "facilisis at vero eros et accumsan et iusto odio dignissim\n" +
                              "qui blandit praesent luptatum zzril delenit augue duis\n" +
                              "dolore te feugait nulla facilisi.";
        }

        protected override void OnPreviewKeypress(ConsolePreviewKeyPressEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyInfo.Key == ConsoleKey.H)
                {
                    HelpBox box = new HelpBox();
                    box.ShowDialog(this);
                }
                else if (e.KeyInfo.Key == ConsoleKey.Q)
                {
                    Close();
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            this.ActiveControl = tbDocument;
        }

        protected override void OnKeyPressed(ConsoleKeyPressEventArgs e)
        {
            base.OnKeyPressed(e);


        }
    }
}
