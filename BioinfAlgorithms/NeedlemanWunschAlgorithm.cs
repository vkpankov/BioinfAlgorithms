using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    public class NeedlemanWunschAlgorithm
    {

        public double[,] SimilarityMatrix { get; set; }

        //Фиксированные штрафы; используются, если не задана матрица весов
        public double MismatchPenalty { get; set; }
        public double GapPenalty { get; set; } 
        public Dictionary<char, int> LetterIndexPairs { get; set; }

        private AlignmentResult GetAlignmentByAncestorsMatrix(string S1, string S2, int[,] ancestors)
        {
            int k = S1.Length - 1;
            int l = S2.Length - 1;

            AlignmentResult result = new AlignmentResult();

            while (k >= 0 && l >= 0)
            {
                if (ancestors[k, l] == 0)
                {
                    result.S1 = S1[k] + result.S1;
                    result.S2 = S2[l] + result.S2;
                    k = k - 1;
                    l = l - 1;
                }
                else if (ancestors[k, l] == 1)
                {
                    result.S1 = S1[k] + result.S1;
                    result.S2 = "-" + result.S2;
                    k = k - 1;
                }
                else
                {
                    result.S1 = "-" + result.S1;
                    result.S2 = S2[l] + result.S2;
                    l = l - 1;
                }

            }
            if (l > 0)
            {
                result.S1 = new string('-', l) + result.S1;
                result.S2 = S2.Substring(0, l) + result.S2;
            }
            if (k > 0)
            {
                result.S1 = S1.Substring(0, k) + result.S1;
                result.S2 = new string('-', k) + result.S2;
            }
            return result;
            
        }

        public AlignmentResult CalcAlignment(string S1, string S2)
        {
            double[,] M = new double[S1.Length, S2.Length];
            int[,] ancestors = new int[S1.Length, S2.Length];

            for (int i = 0; i < S1.Length; i++)
            {
                M[i, 0] = GapPenalty * i;
            }
            for (int j = 0; j < S2.Length; j++)
            {
                M[0, j] = GapPenalty * j;
            }
            for (int i = 1; i < S1.Length; i++)
            {
                for (int j = 1; j < S2.Length; j++)
                {


                    double matchScore = 0;
                    if (SimilarityMatrix != null)
                    {
                        matchScore = M[i - 1, j - 1] +
                        SimilarityMatrix[LetterIndexPairs[S1[i]], LetterIndexPairs[S2[j]]];
                    } else {
                        matchScore = M[i - 1, j - 1] +
                            (S1[i] == S2[j] ? 1 : MismatchPenalty);
                    }
                    double deleteScore = M[i - 1, j] + GapPenalty;
                    double insertScore = M[i, j - 1] + GapPenalty;
                    if(matchScore>=deleteScore && matchScore >= insertScore)
                    {
                        M[i, j] = matchScore; //пришли из соседней ячейки по диагонали
                        ancestors[i, j] = 0;
                    } else if (deleteScore >= insertScore)
                    {
                        M[i, j] = deleteScore; //пришли из левой ячейки
                        ancestors[i, j] = 1;
                    }
                    else
                    {
                        M[i, j] = insertScore; //пришли из правой ячейки
                        ancestors[i, j] = 2;
                    }
                }
            }

            var alignmentResult = GetAlignmentByAncestorsMatrix(S1, S2, ancestors);
            alignmentResult.Score = M[S1.Length - 1, S2.Length - 1];
            return alignmentResult;
        }


    }
}
