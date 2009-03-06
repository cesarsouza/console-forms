using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crsouza.Console.Forms
{
    public interface IContainerControl
    {
        // Methods
        bool ActivateControl(Control active);

        // Properties
        Control ActiveControl { get; set; }
    }
}
