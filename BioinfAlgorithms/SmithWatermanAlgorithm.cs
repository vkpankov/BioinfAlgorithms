using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    public class SmithWatermanAlgorithm
    {
        public double[,] SimilarityMatrix { get; set; }
        public double MismatchPenalty { get; set; }
        public double GapPenalty { get; set; }
        public Dictionary<char, int> LetterIndexPairs { get; set; }

        private AlignmentResult GetAlignmentByAncestorsMatrix(string S1, string S2, int[,] ancestorsMatrix, double[,] scoringMatrix, int maxScoreI, int maxScoreJ)
        {
            int k = maxScoreI;
            int l = maxScoreJ;

            AlignmentResult result = new AlignmentResult();

            while (scoringMatrix[k,l]>0)
            {
                if (ancestorsMatrix[k, l] == 0)
                {
                    result.S1 = S1[k] + result.S1;
                    result.S2 = S2[l] + result.S2;
                    k = k - 1;
                    l = l - 1;
                }
                else if (ancestorsMatrix[k, l] == 1)
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

            if (k < l)
            {
                result.S1 = new string('-', l - k) + S1.Substring(0, k) + "***"+result.S1+"***";
                result.S2 = S2.Substring(0, l) + "***" + result.S2 + "***";
            }
            else
            {
                result.S1 = S1.Substring(0, k) + "***" + result.S1 + "***";
                result.S2 = new string('-', k - l) + S2.Substring(0, l) + "***" + result.S2 + "***";
            }
            result.S1 = result.S1 + S1.Substring(maxScoreI, S1.Length - maxScoreI);
            result.S2 = result.S2 + S2.Substring(maxScoreJ, S2.Length - maxScoreJ);
            if (result.S1.Length < result.S2.Length)
            {
                result.S1 = result.S1 += new string('-', result.S2.Length - result.S1.Length);
            } else {
                result.S2 = result.S2 += new string('-', result.S1.Length - result.S2.Length);
            }

            return result;

        }

        public AlignmentResult CalcAlignment(string S1, string S2)
        {
            double[,] M = new double[S1.Length, S2.Length];
            int[,] ancestors = new int[S1.Length, S2.Length];

            double maxScore = 0;
            int maxScoreI = -1, maxScoreJ = -1;
            for (int i = 1; i < S1.Length; i++)
            {
                for (int j = 1; j < S2.Length; j++)
                {

                    double matchScore = 0;
                    if (SimilarityMatrix != null)
                    {
                        matchScore = M[i - 1, j - 1] +
                        SimilarityMatrix[LetterIndexPairs[S1[i]], LetterIndexPairs[S2[j]]];
                    }
                    else
                    {
                        matchScore = M[i - 1, j - 1] +
                            (S1[i] == S2[j] ? 1 : MismatchPenalty);
                    }
                    matchScore = Math.Max(0, matchScore);
                    double deleteScore = Math.Max(0, M[i - 1, j] + GapPenalty);
                    double insertScore = Math.Max(0, M[i, j - 1] + GapPenalty);
                    if (matchScore >= deleteScore && matchScore >= insertScore)
                    {
                        M[i, j] = matchScore; //пришли из соседней ячейки по диагонали
                        ancestors[i, j] = 0;
                    }
                    else if (deleteScore >= insertScore)
                    {
                        M[i, j] = deleteScore; //пришли из левой ячейки
                        ancestors[i, j] = 1;
                    }
                    else
                    {
                        M[i, j] = insertScore; //пришли из правой ячейки
                        ancestors[i, j] = 2;
                    }
                    if (M[i, j] > maxScore)
                    {
                        maxScoreI = i;
                        maxScoreJ = j;
                        maxScore = M[i, j];
                    }
                }
            }

            var alignmentResult = GetAlignmentByAncestorsMatrix(S1, S2, ancestors, M, maxScoreI, maxScoreJ);
            alignmentResult.Score = maxScore;
            return alignmentResult;
        }

    }
}
