using System;
using System.Collections.Generic;
using System.Text;
using Crsouza.Console.Forms;

namespace SharpEdit
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
