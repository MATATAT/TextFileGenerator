using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileGenerator.Menus
{
    public class MainMenu : Menu
    {
        public MainMenu()
        {
            // set menu text
            base.menuText = new string[] {
                "File generator can use random words from the provided dictionary or an editable file.",
                @"The editable file can be found in TextFiles/Original/original.txt, note that the file must retain this name."
            };

            // set menu items
            base.menuItems = new string[] {
                "Use Random Words",
                "Use Editable File",
                "Use Hipster Ipsum"
            };
        }
    }
}
