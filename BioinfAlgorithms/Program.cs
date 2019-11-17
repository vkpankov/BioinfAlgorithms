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

        static void Main(string[] args)
        {

            Dictionary<char, int> indexes = new Dictionary<char, int>()
            {
                {'О',0 },
                {'Р', 1}
            };

            //Transition matrix
            var test1A = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { 0.8,0.2 },
                    { 0.8,0.2 }
                });

            //Emission probabilities matrix
            var test1B = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { 0.5,0.5 },
                    { 0.1,0.9 }
                });

            var test2A = Matrix<double>.Build.DenseOfArray(
    new double[,]
    {
                    { 0.5,0.5 },
                    { 0.5,0.5 }
    });
            var test2B = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { 0.5,0.5 },
                    { 0.51,0.49 }
                });




            var test3A = Matrix<double>.Build.DenseOfArray(
new double[,]
{
                    { 0.3 ,0.3, 0.4 },
                    { 0.1, 0.45, 0.45 },
                    {0.2,0.3,0.5 }
}).Transpose();
            var test3B = Matrix<double>.Build.DenseOfArray(
                new double[,]
                {
                    { 1, 0 },
                    { 0.8, 0.2 },
                    {0.3, 0.7 }
                });



            var observations = "ОРОРОРООРРРРРРРРРРОООООООО".Select(x => (double)indexes[x]).ToArray();

            Vector<double> Y = Vector<double>.Build.Dense(observations);
            Vector<double> Y3 = Vector<double>.Build.Dense(new double[] { 1,1,0,1,0,0,1,0,1,1,0,0,0,1});
            var test3Pi = Vector<double>.Build.DenseOfArray(new double[] { 0, 0.2, 0.8 });

            

            var testPi = Vector<double>.Build.DenseOfArray(new double[] { 0.5, 0.5 });

            Console.WriteLine("Viterbi:");
            Console.WriteLine("\r\n");
            Console.WriteLine("Тест 1: ");
            ViterbiAlgorithm vb = new ViterbiAlgorithm();
            Console.WriteLine(vb.Compute(testPi, test1A, test1B, Y));
            Console.WriteLine("\r\n");
            Console.WriteLine("Тест 2: ");
            Console.WriteLine(vb.Compute(testPi, test2A, test2B, Y));




            ForwardBackward fb = new ForwardBackward();


            Console.WriteLine("\r\n");
            Console.WriteLine("Forward-Backward:");
            Console.WriteLine("\r\n");
            Console.WriteLine("Тест 1: ");

            Console.WriteLine(fb.Compute(testPi, test1A, test1B, Y).ToMatrixString(26,26));
            Console.WriteLine("\r\n");
            Console.WriteLine("Тест 2: ");
            Console.WriteLine(fb.Compute(testPi, test2A, test2B, Y).ToMatrixString(26, 26));

            Console.ReadKey();

            var ms = fb.Compute(testPi, test1A, test1B, Y).ToMatrixString();

        }
    }
}
