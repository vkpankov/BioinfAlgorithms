using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Глобальное выравнивание без матрицы весов:");
            string testS1 = "CCCACCGCGTAGTGTAGAGACTAGAT", testS2 = "GCGCGATAGCTAGCTTAGCGCTAAGT";
            var ndm = new NeedlemanWunschAlgorithm();
            Console.WriteLine("Тест 1 (gap=-1, mismatch=-1):");
            ndm.MismatchPenalty = -1;
            ndm.GapPenalty = -1;
            var alignment = ndm.CalcAlignment(testS1, testS2);
            Console.WriteLine(alignment);

            Console.WriteLine("");
            Console.WriteLine("Тест 2 (gap=-0.499, mismatch=-1):");
            ndm.MismatchPenalty = -1;
            ndm.GapPenalty = -0.499;
            alignment = ndm.CalcAlignment(testS1, testS2);
            Console.WriteLine(alignment);

            Console.WriteLine("");
            Console.WriteLine("Глобальное выравнивание с матрицей весов:");
            ndm.SimilarityMatrix = new double[,] {
                     {10,-1,-3,-4 },
                     {-1,7,-5,-3 },
                     {-3,-5,9,0 },
                     {-4,-3,0,8 } };
            ndm.GapPenalty = -1;
            ndm.LetterIndexPairs = new Dictionary<char, int>() {
             {'A',0 },{'C',1},{'G',2},{'T',3} };
            alignment = ndm.CalcAlignment(testS1, testS2);
            Console.WriteLine(alignment);

            Console.WriteLine("");
            Console.WriteLine("Заменили вес совпадения буквы С с 10 на 100:");
            ndm.SimilarityMatrix[1, 1] = 100;
            alignment = ndm.CalcAlignment(testS1, testS2);
            Console.WriteLine(alignment);
            ndm.SimilarityMatrix[1, 1] = 10;

            Console.WriteLine("");
            Console.WriteLine("Локальное выравнивание:");
            string localTestS1 = "AATTGCCGCCGTCGTTTTCACAGTTATGTCAGGATC";
            string localTestS2 = "TCCAGTTATGTCAGGGGGACACGAGCATGCAGAGAC";

            var smw = new SmithWatermanAlgorithm();
            //smw.SimilarityMatrix = ndm.SimilarityMatrix;
            //smw.LetterIndexPairs = ndm.LetterIndexPairs;
            smw.GapPenalty = -1;
            smw.MismatchPenalty = -1;
            alignment = smw.CalcAlignment(localTestS1, localTestS2);
            Console.WriteLine(alignment);

            Console.WriteLine("Тест: глобальное выравнивание для этого же случая:");
            ndm.GapPenalty = -1;
            ndm.MismatchPenalty = -1;
            ndm.SimilarityMatrix = null;
            alignment = ndm.CalcAlignment(localTestS1, localTestS2);
            Console.WriteLine(alignment);

            Console.ReadKey();
        }
    }
}
