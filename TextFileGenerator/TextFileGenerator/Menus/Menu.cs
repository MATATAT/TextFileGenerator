using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace TextFileGenerator.Menus
{
    public abstract class Menu : IMenu
    {
        protected string[] menuText;
        public string[] MenuText
        {
            get { return menuText; }
        }

        protected string[] menuItems;
        public string[] MenuItems
        {
            get { return menuItems; }
        }

        public virtual int DisplayMenu()
        {
            int menuNumber = 0;
            writeMenuToConsole();
            var input = Console.ReadKey();
            Console.WriteLine(string.Empty);

            while (!int.TryParse(input.KeyChar.ToString(), out menuNumber) && menuNumber <= 0 && menuNumber > MenuItems.Length)
            {
                // Write error message
                Console.WriteLine("Error with input.");

                // Rewrite menu
                writeMenuToConsole();

                input = Console.ReadKey();
            }

            return menuNumber;
        }

        private void writeMenuToConsole()
        {
            if (MenuText != null)
            {
                foreach (var line in MenuText)
                {
                    Console.WriteLine(line);
                } 
            }

            if (MenuItems != null)
            {
                for (int i = 0; i < MenuItems.Length; i++)
                {
                    Console.WriteLine(string.Format("{0}. {1}", i + 1, MenuItems[i]));
                } 
            }
        }
    }

    interface IMenu
    {
        /// <summary>
        /// Text to display with <see cref="MenuItems"/>. Each index will be a new line.
        /// </summary>
        string[] MenuText { get; }
        
        /// <summary>
        /// Items to display for menu
        /// </summary>
        string[] MenuItems { get; }
        
        /// <summary>
        /// Displays menu and requests input from user.
        /// </summary>
        /// <returns>Item user selected.</returns>
        int DisplayMenu();
    }
}
