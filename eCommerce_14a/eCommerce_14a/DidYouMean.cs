using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce_14a
{
    class DidYouMean
    {
        /* 
         * <param name="orig" the original string </param>
         * <param name=fileName"> the file name of the suggestions file </param>
         * <returns>  the closest correction found based on the edit distance, or the original 
         * string if no suggestions were found </returns>
         */
        public static string suggest(string orig, string fileName)
        {
            string[] suggestions = parse(fileName);

            if (suggestions.Length == 0)
            {
                return orig;
            }

            var bestOffer = Tuple.Create(orig, int.MaxValue);

            foreach (string suggestion in suggestions)
            {
                int distance = computeDistance(orig, suggestion, int.MaxValue);

                if (distance < bestOffer.Item2) 
                    bestOffer = Tuple.Create(suggestion, distance);
            }

            return bestOffer.Item1;

        }

        /*
         * parses a suggestions file into array of strings
         * </summary>
         * <param name="fileName"> a file name with the suggested corrections, seperated by spaces </param>
         * <returns> array of parsed strings found in the input file </returns>
         */
        private static string[] parse(string fileName)
        {
            try
            {
                string text = System.IO.File.ReadAllText("resources\\" + fileName);
                return text.Split(' ');
            }
            catch (Exception e)
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Computes the Damerau-Levenshtein Distance between two strings.
        /// <returns>Int.MaxValue if threshhold exceeded; otherwise the Damerau-Leveshteim distance between the strings</returns>
        private static int computeDistance(string source, string target, int threshold)
        {

            int length1 = source.Length;
            int length2 = target.Length;

            // Return trivial case - difference in string lengths exceeds threshhold
            if (Math.Abs(length1 - length2) > threshold) { return int.MaxValue; }

            // Ensure arrays [i] / length1 use shorter length 
            if (length1 > length2)
            {
                Swap(ref target, ref source);
                Swap(ref length1, ref length2);
            }

            int maxi = length1;
            int maxj = length2;

            int[] dCurrent = new int[maxi + 1];
            int[] dMinus1 = new int[maxi + 1];
            int[] dMinus2 = new int[maxi + 1];
            int[] dSwap;

            for (int i = 0; i <= maxi; i++) { dCurrent[i] = i; }

            int jm1 = 0, im1 = 0, im2 = -1;

            for (int j = 1; j <= maxj; j++)
            {

                // Rotate
                dSwap = dMinus2;
                dMinus2 = dMinus1;
                dMinus1 = dCurrent;
                dCurrent = dSwap;

                // Initialize
                int minDistance = int.MaxValue;
                dCurrent[0] = j;
                im1 = 0;
                im2 = -1;

                for (int i = 1; i <= maxi; i++)
                {

                    int cost = source[im1] == target[jm1] ? 0 : 1;

                    int del = dCurrent[im1] + 1;
                    int ins = dMinus1[i] + 1;
                    int sub = dMinus1[im1] + cost;

                    //Fastest execution for min value of 3 integers
                    int min = (del > ins) ? (ins > sub ? sub : ins) : (del > sub ? sub : del);

                    if (i > 1 && j > 1 && source[im2] == target[jm1] && source[im1] == target[j - 2])
                        min = Math.Min(min, dMinus2[im2] + cost);

                    dCurrent[i] = min;
                    if (min < minDistance) { minDistance = min; }
                    im1++;
                    im2++;
                }
                jm1++;
                if (minDistance > threshold) { return int.MaxValue; }
            }

            int result = dCurrent[maxi];
            return (result > threshold) ? int.MaxValue : result;
        }
        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}
