using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BioinfAlgorithms.AffineGapsAlignment;

namespace BioinfAlgorithms
{
    class Program
    {

        public static void Print2DArray<T>(T[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            string rnaStr = "GGACC";
            var rnaFolding = new RNAFolding();
            rnaFolding.StemLoopMinLength = 3;
            var m = rnaFolding.FoldRNA(rnaStr);
            int score = m[0, rnaStr.Length - 1];
            string pairs = rnaFolding.Traceback(rnaStr, m);
            Console.WriteLine("Тест 1: ");
            Console.WriteLine("Число спаренных оснований: " + score);
            Console.WriteLine(pairs);
            Console.WriteLine(rnaStr);
            Console.WriteLine("");
            rnaStr = "AAACAUGAGGAUUACCCAUGU";
            m = rnaFolding.FoldRNA(rnaStr);
            score = m[0, rnaStr.Length - 1];
            pairs = rnaFolding.Traceback(rnaStr, m);
            Console.WriteLine("Тест 2: ");
            Console.WriteLine("Число спаренных оснований: " + score);
            Console.WriteLine(pairs);
            Console.WriteLine(rnaStr);

            Console.ReadKey();
        }
    }
}
