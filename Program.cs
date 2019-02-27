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
            string output = "";

            //If more than two arguments are provided, then add those to the search word.
            if (args.Length > 2)
            {
                for (int i = 2; i < args.Length; i++)
                {
                    word += " " + args[i];
                }
            }
            
            //Calculate the vector lenght and character count of the original word.
            double originalVector = wordVector(word);
            var originalCharacters = wordChars(word);

            //Read all lines from the anagram list. 'lemmad.txt' is encoded as windows-1257, let's use the same.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var lines = File.ReadLines(fileName, Encoding.GetEncoding(1257));

            foreach (String line in lines)
            {
                // If original word has the same lenght as current line on the anagram list, and
                // word vectors have the same value, and the words aren't the same, then we might have an anagram.
                if (line.Length == word.Length && wordVector(line) == originalVector && !line.Equals(word))
                {
                    // More detailed (and time expensive) check if the current line is an anagram of the search word.
                    if (areDictionariesEqual(wordChars(line), originalCharacters))
                    {
                        //We have an anagram, add to output string.
                        output += "," + line;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine("{0}{1}", stopwatch.ElapsedTicks * 1000000 / Stopwatch.Frequency, output);
        }
        
        // Calculate vector lenght of a string: square root of the sum of all character values squared.
        static double wordVector(String word)
        {
            long temp = 0;
            foreach (Char ch in word)
            {
                temp += (int)ch * (int)ch;
            }
            return Math.Sqrt(temp);
        }

        // Create a <char, int> dictionary containing the count of every character in the input string.
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

        // Check, if two <char, int> dictionaries are the same, e.g. they contain the same key-value pairs.
        // No need to check for key counts, as this method will be called after checking string lenght equality.
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