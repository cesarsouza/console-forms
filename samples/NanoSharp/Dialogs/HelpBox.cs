using System;
using System.Collections.Generic;
using System.Text;


using Terminal.Forms;

namespace NanoSharp
{
   partial class HelpBox : Terminal.Forms.Form
    {
        public HelpBox()
        {
            InitializeComponent();
            ActiveControl = btnOk;
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
