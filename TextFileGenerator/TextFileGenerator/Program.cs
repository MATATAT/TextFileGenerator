using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TextFileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var error = false;
            Console.WriteLine("File generator can use random words from the provided dictionary or an editable file.");
            Console.WriteLine(@"The editable file can be found in TextFiles/Original/original.txt, note that the file must retain this name.");
            Console.WriteLine("1. Use Random Words");
            Console.WriteLine("2. Use Editable File");
            var input = Console.ReadKey();
            Console.WriteLine(string.Empty);

            while (input.Key != ConsoleKey.D1 && input.Key != ConsoleKey.D2 && input.Key != ConsoleKey.NumPad1 && input.Key != ConsoleKey.NumPad2)
            {
                if (error)
                {
                    Console.WriteLine("Error with input.");
                }

                Console.WriteLine("File generator can use random words from the provided dictionary or an editable file.");
                Console.WriteLine("1. Use Random Words");
                Console.WriteLine("2. Use Editable File");
                input = Console.ReadKey();
                Console.WriteLine(string.Empty);

                error = true;
            }

            var randomFileName = new ConsoleKeyInfo();
            error = false;
            while (randomFileName.Key != ConsoleKey.D1 && randomFileName.Key != ConsoleKey.D2 && randomFileName.Key != ConsoleKey.NumPad1 && randomFileName.Key != ConsoleKey.NumPad2)
            {
                if (error)
                {
                    Console.WriteLine("Error with input.");
                }

                Console.WriteLine("Use random word for text file?");
                Console.WriteLine("1. Use Random Word");
                Console.WriteLine("2. Use 'textfile' prefix with number");

                randomFileName = Console.ReadKey();
                Console.WriteLine(string.Empty);
                error = true;
            }

            int numberOfFiles;
            promptUserInput("Generate how many files?", out numberOfFiles);

            string[] dictionary = null;
            int wordsPerFile = -1;
            string fileText = string.Empty;
            if (input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1)
            {
                dictionary = buildDictionary();
                promptUserInput("How many words per file?", out wordsPerFile);
            }
            else
            {
                using (var openFile = new StreamReader("TextFiles/Original/original.txt"))
                {
                    fileText = openFile.ReadToEnd();
                }
            }

            Console.WriteLine("Generating files...");

            var newWriteDir = "TextFiles/" + DateTime.Now.ToString("MM-dd-yy-hhmmss");
            System.IO.Directory.CreateDirectory(newWriteDir);
            var random = new Random((int)DateTime.Now.Ticks);
            for (int i = 1; i <= numberOfFiles; i++)
            {
                var filename = (randomFileName.KeyChar == '1') ? dictionary[random.Next(dictionary.Length - 1)] : ("textfile" + i);
                // If exists just add the integer to have it not overwrite
                filename = Directory.Exists(newWriteDir + "/" + filename + ".txt") ? filename + i : filename;
                using (var newFile = new StreamWriter(newWriteDir + "/" + filename + ".txt"))
                {
                    try
                    {
                        if (input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2)
                        {
                            newFile.Write(fileText);
                        }
                        else
                        {
                            newFile.Write(buildRandomString(wordsPerFile, dictionary, random));
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Error generating files. Terminating.");
                        return;
                    }
                }
            }

            Console.WriteLine("Done!");
        }

        private static void promptUserInput(string promptText, out int count)
        {
            Console.WriteLine(promptText);
            var numberOfFilesString = Console.ReadLine();

            while (!int.TryParse(numberOfFilesString, out count))
            {
                Console.WriteLine("Error with input.");

                // Retry
                Console.WriteLine(promptText);
                numberOfFilesString = Console.ReadLine();
            }
        }

        private static string buildRandomString(int wordsPerFile, string[] dictionary, Random random)
        {
            var retString = new StringBuilder();

            for (int i = 0; i < wordsPerFile; i++)
            {
                retString.Append(dictionary[random.Next(dictionary.Length - 1)] + " ");
            }

            return retString.ToString();
        }

        private static string[] buildDictionary()
        {
            var dictionary = new List<string>();
            using (var openFile = new StreamReader(@"TextFiles/Words/english-words.95"))
            {
                while (!openFile.EndOfStream)
                {
                    dictionary.Add(openFile.ReadLine());
                }
            }

            return dictionary.ToArray();
        }
    }
}
