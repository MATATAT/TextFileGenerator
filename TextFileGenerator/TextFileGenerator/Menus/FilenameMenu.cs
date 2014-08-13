using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileGenerator.Menus
{
    class FilenameMenu : Menu
    {
        public FilenameMenu()
        {
            // Set menu text
            base.menuText = new string[] { 
                "Use random word for text file?"
            };

            // Set menu items
            base.menuItems = new string[] { 
                "Use random word",
                "Use 'textfile' prefix with number"
            };
        }
    }
}
