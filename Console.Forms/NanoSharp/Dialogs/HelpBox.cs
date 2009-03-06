using System;
using System.Collections.Generic;
using System.Text;


using Crsouza.Console.Forms;

namespace NanoSharp
{
   partial class HelpBox : Crsouza.Console.Forms.Form
    {
        public HelpBox()
        {
            InitializeComponent();

            this.ActiveControl = btnOk;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
