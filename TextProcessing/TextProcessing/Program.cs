using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextProcessing
{
    class Program
    {


        private void ProcessFiles()
        {
            string[] filePaths = Directory.GetFiles(Directory.GetCurrentDirectory().ToString() + "\\Transcripts", "*.trn");

            for(int i = 0; i < filePaths.Count(); i++)
            {
                string[] lines = System.IO.File.ReadAllLines(filePaths[i]);
                string allLines = string.Join(Environment.NewLine, lines);
                //allLines = Regex.Replace(allLines, @"\((.*?)\) |\< (.*?)\>|[\d -]", string.Empty);
                //(\w+:) autora
                //\((.*?)\)|\< (.*?)\> //nawiasy
                // (=)
                allLines = allLines.ToLower();
                string[] splitted = allLines.Split(':');
                string newLines = "";
                for(int j = 0; j < splitted.Count()-1; j++)
                {
                    // pobranie ostatniego wyrazu jako autor i wyrzucenie go z tej treści
                    string author = splitted[j].Split(' ').Last();
                    // czyszczenie
                    string text = splitted[j + 1];
                    author = Regex.Replace(author, @"\((.*?)\)|\<(.*?)\>|[\d-]|([^a-zA-Z ])|([xX]+)", string.Empty);
                    text = Regex.Replace(text, @"\((.*?)\)|\<(.*?)\>|[\d-]|([^a-zA-Z ])|([xX]+)", string.Empty);
                    text = text.Replace(text.Split(' ').Last(), "");
                    author = Regex.Replace(author, @"[ ]{2,}", " ");
                    text = Regex.Replace(text, @"[ ]{2,}", " ");
                    text = Regex.Replace(text, @"^\s+", "");
                    text = Regex.Replace(text, @"\s+$", "");
                    author = Regex.Replace(author, @"^\s+", "");
                    author = Regex.Replace(author, @"\s+$", "");
                    newLines += author + ", " + text + Environment.NewLine;
                    
                    // newLines = newLines.Replace(@"\s+", " ");
                }
                
                //allLines = new string(allLines.Where(c => !char.IsPunctuation(c)).ToArray());
                // filePaths[i] = filePaths[i].Substring(filePaths[i].Length, filePaths[i].Length - 4);
                filePaths[i] = "ProcessedFiles/" + System.IO.Path.GetFileNameWithoutExtension(filePaths[i]);
               // File.Create(filePaths[i] + "processed.smp"); // change extension and name 
                File.WriteAllText(filePaths[i] + "processed.smp", newLines);
            }
            //string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");



        }


        static void Main(string[] args)
        {
            Program program = new Program();
            program.ProcessFiles();
        }
    }
}
