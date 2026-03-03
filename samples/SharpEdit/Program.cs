using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Forms;

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
