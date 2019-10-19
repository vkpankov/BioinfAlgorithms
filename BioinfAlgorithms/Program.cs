using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    class Program
    {
        static string SGen(int length, Random rnd)
        {
            string s = "";
            for (int i = 0; i < length; i++)
            {
                double rndDouble = rnd.NextDouble();
                if (rndDouble < 0.25)
                    s += "A";
                else if (rndDouble < 0.50)
                    s += "C";
                else if (rndDouble < 0.75)
                    s += "G";
                else if (rndDouble < 1)
                    s += "T";

            }
            return s;
        }
        static void Main(string[] args)
        {

            string affGapTS1 = "TCCCAGTTATGTCAGGGGACACGAGCATGCAGAGAC";//"TCCCAGTTATGTCAGGGGACACGAGCATGCAGAGAC";
            string affGapTS2 = "AATTGCCGCCGTCGTTTTCAGCAGTTATGTCAGATC";//"AATTGCCGCCGTCGTTTTCAGCAGTTATGTCAGATC";

            var ag = new AffinityGapsAlignment();
            Console.WriteLine("Тест 1 (gap=-1, mismatch=-1, open = 0):");
            ag.MismatchPenalty = -1;
            ag.GapPenalty = -1;
            ag.GapOpenPenalty = 0;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));

            Console.WriteLine("Тест 2 (gap=-1, mismatch=-1):");
            ag.MismatchPenalty = -1;
            ag.GapPenalty = -0.01;
            ag.GapOpenPenalty = -100;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));


            Console.WriteLine("Тест 3 (gap=-1, mismatch=-1):");
            ag.MismatchPenalty = -1;
            ag.GapPenalty = -0.3;
            ag.GapOpenPenalty = 0.5;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));


            Console.ReadKey();
        }
    }
}
