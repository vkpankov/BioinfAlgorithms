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

            string affGapTS1 = "TCCCAGTTATGTCAGGGGACACGAGCATGCAGAGAC";
            string affGapTS2 = "AATTGCCGCCGTCGTTTTCAGCAGTTATGTCAGATC";

            var ag = new AffineGapsAlignment();
            Console.WriteLine("Тест 1 (gap=-1, mismatch=-1, open gap = 0):");
            ag.MismatchPenalty = -5;
            ag.GapPenalty = -1;
             ag.GapOpenPenalty = 0;
            var al = ag.CalcAlignment(affGapTS1, affGapTS2);
            Console.WriteLine(al);

            Console.WriteLine("Тест 2 (gap=-0.01, mismatch=-1, open gap = -100):");
            ag.MismatchPenalty = -1;
            ag.GapPenalty = -0.01;
            ag.GapOpenPenalty = -100;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));


            Console.WriteLine("Тест 3 (gap=-0.3, mismatch=-1, open gap = 0.5):");
            ag.MismatchPenalty = -1;
            ag.GapPenalty = -0.3;
            ag.GapOpenPenalty = 0.5;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));


            Console.ReadKey();
        }
    }
}
