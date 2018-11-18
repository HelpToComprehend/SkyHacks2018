using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Pattern
{
    class Program
    {
        private static String input = @"C:\Users\User\Desktop\sh1-2b\sh1-1\training\amr-release-2.0-amrs-training-xinhua.txt";
        private static String output = @"C:\Users\User\Desktop\sh1-2b\sh1-1\training\amr-release-2.0-amrs-training-xinhua-reg.txt";

        static void Main(string[] args)
        {
            String tmp = "";
            try
            {
                using (StreamReader sr = new StreamReader(input))
                {
                    // Read a line of text
                    tmp = sr.ReadToEnd();
                }

                // Extract sentences
                Regex sen = new Regex(@"# ::snt ");
                tmp = sen.Replace(tmp, "");

                // Delete comments
                Regex hash = new Regex(@"#.+\n");
                tmp = hash.Replace(tmp, "\n");

                // Single \n
                Regex single = new Regex(@"\n+");
                tmp = single.Replace(tmp, "\n");

                // Delete first line newline
                Regex first = new Regex(@"^\n");
                tmp = first.Replace(tmp, "");

                // Delete two or more spaces
                Regex space = new Regex(@" +");
                tmp = space.Replace(tmp, " ");

                // Delete \n if starting with space
                Regex newline = new Regex(@"\n ");
                tmp = newline.Replace(tmp, "");


                using (StreamWriter sw = new StreamWriter(output))
                {
                    sw.Write(tmp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            System.Console.ReadKey();
        }
    }
}
