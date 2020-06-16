using System;
using System.Linq;
using System.Collections.Generic; // Allows lists to be used. Important!

namespace Functions
{
    class PermutationCalculator
    {
        static void Main(string[] args)
        {
            // Taking an input as a string here which can easily be turned into a ReadLine input.
            String raw = "12";
            // Regardless of this input method the variable can be put into the extractor to return a list
            String input = raw;
            List<string> storage = new List<string>();
            List<String> output = Permute(raw,storage,0,raw.Length -1);
            // If you want to print the result then you easily can here
            Console.Clear();
            foreach (string element in output)
            {
                Console.WriteLine(element);
            }
        }

        private static List<String> Permute(String input_list, List<string> storage, int l, int r)
        {
            if (l == r)
            {
                // I don't know if I need to check this or not but 
                if (!storage.Contains(input_list))
                {
                    storage.Add(input_list);
                }
                
            }
            else
            {
                for (int i = l; i <= r; i++)
                {
                    input_list = Swapper(input_list, l, i);
                    Permute(input_list, storage, l + 1, r);
                    input_list = Swapper(input_list, l, i);
                }
            }
            return storage;

        }

        public static String Swapper(String a, int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }
    }
}
