using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using TextFileGenerator.Menus;

namespace TextFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            (new TextFileGenerator()).Run();
        }
    }
}
