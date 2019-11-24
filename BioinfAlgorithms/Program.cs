using BioinfAlgorithms.HMM;
using BioinfAlgorithms.Phylogenetic;
using BioinfAlgorithms.Phylogenetics;
using MathNet.Numerics.LinearAlgebra;
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

        static void PrintMatrix(Matrix<double> matrix)
        {
            for(int i =0; i<matrix.RowCount; i++)
            {
                for(int j = 0;j < matrix.ColumnCount; j++)
                {
                    Console.Write("{0:0.00} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        static void Test(string s1, string s2)
        {
            PairHmmAlignment pHmmAlignment = new PairHmmAlignment();
            var fbRes = pHmmAlignment.FB(s1, s2);

            Console.WriteLine("M:");
            PrintMatrix(fbRes.M);

            Console.WriteLine("X:");
            PrintMatrix(fbRes.X);


            Console.WriteLine("Y:");
            PrintMatrix(fbRes.Y);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Тест 1:");
            Test("AGAGA", "AGAGAGA");

            Console.WriteLine("");
            Console.WriteLine("Тест 2:");
            Test("ATAGCTACGAC", "TGCTAGCTAGC");

            Console.ReadKey();

        }
    }
}
