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

        static void Main(string[] args)
        {
           var namesTest1 = new string[] { "A", "B", "C", "D" };
            var test1 = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    {0, 16, 16, 10 },
                    {16, 0, 8, 8 },
                    {16,8, 0, 4 },
                    {10, 8, 4, 0 }
                });

            var namesTest2 = new string[] { "A", "B", "C", "D", "E", "F" };
            var test2 = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    {0, 5, 4, 7, 6, 8 },
                    {5, 0, 7, 10, 9, 11 },
                    {4, 7, 0, 7, 6, 8 },
                    {7,10,7,0,5,9 },
                    {6,9,6,5,0,8},
                    {8,11,8,9,8,0 }
                });

           /* var namesWikiTest = new string[] { "A", "B", "C", "D", "E" };
            var wikiTest = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { 0, 17,  21,  31,  23},
                    {17, 0,   30,  34,  21},
                    { 21,    30,  0,   28,  39},
                    {31, 34,  28,  0,   43 },
                    {23, 21,  39,  43,  0 }
                }
                );

            var njTest = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    {0,  5,   9,   9,   8 },
                    {5,  0,   10,  10,  9 },
                    {9,  10,  0,   8,   7 },
                    {9,  10,  8,   0,   3 },
                    {8 , 9,   7,   3,   0 }
                });

            var njTest2 = Matrix<double>.Build.DenseOfArray(
    new double[,]
    {
                    {0,5,4,7,6,8 },
                    {5,0,7,10,9,11 },
                    {4,7,0,7,6,8 },
                    {7,10,7,0,5,9 },
                    {6,9,6,5,0,8 },
                    {8,11,8,9,8,0 }

    });
    */



            var test1NJ = NeighborJoiningClustering.BuildTree(test1, namesTest1);
            var test2NJ = NeighborJoiningClustering.BuildTree(test2, namesTest2);

            Console.WriteLine("Тест 1:");
            Console.WriteLine("NJ: "+ test1NJ);

            Console.WriteLine("");
            Console.WriteLine("Тест 2:");
            Console.WriteLine("NJ: " + test2NJ);
            Console.WriteLine("");
            Console.ReadKey();

        }
    }
}
