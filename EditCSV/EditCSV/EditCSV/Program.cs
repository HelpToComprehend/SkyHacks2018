using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditCSV
{
    class Program
    {
        private void EditCSV(string sourceCsvFile, string destCsvFile)
        {
            // int sbc0013files = 2252; //sbc00132252 files
            // int sbc0013files = //3438-2253

            int filesNumber = 3438;
            string filename = "";

            string[] files = Directory.GetFileSystemEntries("Transcripts");
            List<string> dataLines = new List<string>();
            //  var data = Directory.GetFiles(file, "*.csv"); //FILE name needed
           for (int j = 0; j < filesNumber; j++)
           {
                if(j >= 2253)
                {
                    filename = "sbc0014" + j + "recognized.txt";
                }
                else
                {
                    filename = "sbc0013" + j + "recognized.txt";
                }

               
                if(File.Exists(filename))
                {
                    dataLines.Add(File.ReadAllText(filename));
                }
               
           }

            List<String> csvLines = new List<String>();
            
            using (var streamReader = File.OpenText(sourceCsvFile))
            {
                String line;
                int i = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Contains(","))
                    {
                        String[] split = line.Split(new[] { ',' }, 2);
                        if (i >= dataLines.Count())
                        {                          
                            Console.WriteLine("ERROR: LESS RECOGNIZED DATA THEN AUTHORS!!!");
                            csvLines.Add(split[0] + ", ");
                        }
                        else
                        {
                            char[] charData = dataLines[i].ToCharArray();
                            string newLine = new string(charData.Where(c => !char.IsPunctuation(c)).ToArray());
                            csvLines.Add(split[0] + ", " + newLine);
                        }
                        i++; ;
                    }



                }
            }
            using (StreamWriter writer = new StreamWriter(destCsvFile, false))
            {
                foreach (String line in csvLines)
                {
                    writer.WriteLine(line);
                }
                   
            }


        }

        static void Main(string[] args)
        {
            if (!Directory.Exists("Transcripts"))
            {
                Directory.CreateDirectory("Transcripts");
                Console.WriteLine("Created Transcripts folder! Add data first!");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            Program program = new Program();
            string sourceCSV = args[0];
            string destCSV = args[1];
            program.EditCSV(sourceCSV, destCSV);
            Console.WriteLine("DATA WRITTEN.");
            Console.ReadLine();
        }
    }
}
