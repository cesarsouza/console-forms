using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Forms
{
    public interface IContainerControl
    {
        // Methods
        bool ActivateControl(Control active);

        // Properties
        Control ActiveControl { get; set; }
    }
}
