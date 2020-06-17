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

// A second class that is arranged specifically to deal with an array of integers as an input.
// This was created for the Google KickStart Round A Plates problem
class PermutationCalculatorIntArray
    {
        public static bool checker;
        public static List<int[]> Permutations(int stacks, int height, int length)
        {
            // We can ask permute to give us the different arrangements of a set of numbers
            // We need to pass in all options that would give us enough plates
            // First we cycle through all of the possible stack numbers
            checker = true;
            int[] querey = new int[stacks];
            int remaining = length;
            int counter = 0;
            // Finding the starting possition
            while (checker)
            {
                // Checking if we can satisfy the problem within a single stack
                if (height >= remaining)
                {
                    querey[counter] = remaining;
                    checker = false;
                }
                else
                {
                    querey[counter] = height;
                    remaining -= height;
                    counter++;
                }
            }
            // We now have a starting combination and set checker back to true for the new loop
            checker = true;
            // Regardless of this input method the variable can be put into the extractor to return a list of integer arrays
            List<int[]> storage = new List<int[]>();
            // Adding the first querey to our list of options
            int indexer = 1;
            while (checker)
            {
                storage = Permute(querey,storage,0,stacks);
                // Checking if the first value is the largest
                if (querey[0] == length - (stacks - 1))
                {
                    querey[0]--;
                    querey[indexer]++;
                    indexer++;
                }
                else
                {
                    checker = false;
                }

                
                
            }
            

            return storage;
        }

        private static List<int[]> Permute(int[] input_list, List<int[]> output, int l, int r)
        {
            if (l == r)
            {
                // I don't know if I need to check this or not but 
                if (!output.Contains(input_list))
                {
                    output.Add(input_list);
                    checker = true;
                }
                
            }
            else
            {
                for (int i = l; i < r; i++)
                {
                    input_list = Swapper(input_list, l, i);
                    Permute(input_list, output, l + 1, r);
                    input_list = Swapper(input_list, l, i);
                }
            }
            return output;

        }

        public static int[] Swapper(int[] a, int i, int j)
        {
            char temp;
            string str = string.Join("", a);
            char[] charArray = str.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            int[] Aint = Array.ConvertAll(charArray, c => (int)Char.GetNumericValue(c));
            return Aint;
        }
    }
