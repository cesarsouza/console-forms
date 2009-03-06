using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Crsouza.Console.Forms;

namespace NanoSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Default;
            
            Application.Run(new MainForm());
        }
    }
}
