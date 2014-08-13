using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TextFileGenerator.Menus;

namespace TextFileGenerator
{
    class TextFileGenerator
    {
        private const string hipsterIpsumUrl = @"http://hipsterjesus.com/api/";

        public void Run()
        {
            var mainMenuInput = (new MainMenu()).DisplayMenu();

            var filenameInput = (new FilenameMenu()).DisplayMenu();

            int numberOfFiles;
            promptUserInput("Generate how many files?", out numberOfFiles);

            string[] dictionary = null;
            int wordsPerFile = -1, paragraphCount = 0;
            string fileText = string.Empty;
            if (mainMenuInput == 1)
            {
                dictionary = buildDictionary();
                promptUserInput("How many words per file?", out wordsPerFile);
            }
            else if (mainMenuInput == 3)
            {
                promptUserInput("How many paragraphs?", out paragraphCount);
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
                var filename = (filenameInput == 1) ? dictionary[random.Next(dictionary.Length - 1)] : ("textfile" + i);
                // If exists just add the integer to have it not overwrite
                filename = Directory.Exists(newWriteDir + "/" + filename + ".txt") ? filename + i : filename;
                using (var newFile = new StreamWriter(newWriteDir + "/" + filename + ".txt"))
                {
                    try
                    {
                        switch (mainMenuInput)
                        {
                            case 1:
                                newFile.Write(fileText);
                                break;
                            case 2:
                                newFile.Write(buildRandomString(wordsPerFile, dictionary, random));
                                break;
                            case 3:
                                newFile.Write(requestHipsterIpsumText(paragraphCount));
                                break;
                            default:
                                break;
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

        private void promptUserInput(string promptText, out int count)
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

        private string buildRandomString(int wordsPerFile, string[] dictionary, Random random)
        {
            var retString = new StringBuilder();

            for (int i = 0; i < wordsPerFile; i++)
            {
                retString.Append(dictionary[random.Next(dictionary.Length - 1)] + " ");
            }

            return retString.ToString();
        }

        private string[] buildDictionary()
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

        private string requestHipsterIpsumText(int paragraphs)
        {
            /*
            endpoint: http://hipsterjesus.com/api/
            parameters: 
            paras = [1 - 99] (default 4)
            type = ['hipster-latin', 'hipster-centric'] (default 'hipster-latin')
            html = ['false', 'true'] ( default 'true') - strips html from output, replaces p tags with newlines
            */

            var uriBuilder = new UriBuilder(hipsterIpsumUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            // add paragraphs
            query["paras"] = paragraphs.ToString();
            // set type 
            query["html"] = bool.FalseString.ToLower();

            // convert back
            uriBuilder.Query = query.ToString();

            using (var client = new WebClient())
            {
                return client.DownloadString(uriBuilder.ToString());
            }
        }
    }
}
