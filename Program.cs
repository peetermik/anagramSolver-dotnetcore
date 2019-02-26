using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;

namespace anagramSolver_dotnetcore
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string fileName = args[0];
            string word = args[1];

            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    word += " " + args[i];
                }
            }

            string output = "";
            double originalVector = wordVector(word);
            var originalCharacters = wordChars(word);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var lines = File.ReadLines(fileName, Encoding.GetEncoding(1257));
            foreach (String line in lines)
            {
                if (line.Length == word.Length && !line.Equals(word) && wordVector(line) == originalVector)
                {
                    if (areDictionariesEqual(wordChars(line), originalCharacters))
                    {
                        output += "," + line;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("{0}{1}", stopwatch.ElapsedTicks * 1000000 / Stopwatch.Frequency, output);
        }

        static double wordVector(String word)
        {
            long temp = 0;
            foreach (Char ch in word)
            {
                temp += (int)ch * (int)ch;
            }
            return temp;
        }

        static Dictionary<char, int> wordChars(String word)
        {
            var temp = new Dictionary<char, int>(31);
            int currentCount = 0;
            foreach (Char ch in word)
            {
                temp.TryGetValue(ch, out currentCount);
                temp[ch] = currentCount + 1;
            }
            return temp;
        }

        static bool areDictionariesEqual(Dictionary<char, int> d1, Dictionary<char, int> d2)
        {
            bool temp = true;
            foreach (char key in d1.Keys)
            {
                if (!(d2.ContainsKey(key) && d1[key] == d2[key]))
                {
                    temp = false;
                    break;
                }
            }
            return temp;
        }
    }
}
