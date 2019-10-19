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
            Console.WriteLine("Тест 1 (gap=-1, mismatch=-1):");
            ag.MismatchPenalty = -5;
            ag.GapPenalty = -1;
            Console.WriteLine(ag.CalcAlignment(affGapTS1, affGapTS2));

            Console.ReadKey();
        }
    }
}
