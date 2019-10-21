using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    public partial class AffineGapsAlignment
    {

        public double[,] SimilarityMatrix { get; set; }

        //Фиксированные штрафы; используются, если не задана матрица весов
        public double MismatchPenalty { get; set; }
        public double GapOpenPenalty { get; set; }
        public double GapPenalty { get; set; } 
        public Dictionary<char, int> LetterIndexPairs { get; set; }

        private AlignmentResult GetAlignmentByAncestorsMatrix(string S1, string S2, StepType[,] pointersM, StepType[,] pointersGaps1,
            StepType[,] pointersGaps2, StepType startStep)
        {
            int k = S1.Length;
            int l = S2.Length;

            AlignmentResult result = new AlignmentResult();

            StepType curStep = startStep;

            while (k > 0 || l > 0)
            {
                if (curStep == StepType.M && k >= 0 && l >= 0)
                {
                    result.S1 = S1[k-1] + result.S1;
                    result.S2 = S2[l-1] + result.S2;
                    curStep = pointersM[k, l];
                    k--;
                    l--;
                }
                else
                if ((k > 0 && curStep == StepType.Gaps2)||(k>0 && l<=0))
                {
                    result.S1 = S1[k-1] + result.S1;
                    result.S2 = "-" + result.S2;
                    curStep = pointersGaps2[k, Math.Max(0,l)];
                    k--;

                }
                else if ((l > 0 && curStep == StepType.Gaps1)||(l>0&&k<=0))
                {
                    result.S1 = "-" + result.S1;
                    result.S2 = S2[l-1] + result.S2;
                    curStep = pointersGaps1[Math.Max(k,0), l];
                    l--;

                }
            }
            return result;
            
        }

        public AlignmentResult CalcAlignment(string S1, string S2)
        {
            double[,] M = new double[S1.Length + 1, S2.Length + 1];
            double[,] Gaps1 = new double[S1.Length + 1, S2.Length + 1];
            double[,] Gaps2 = new double[S1.Length + 1, S2.Length + 1];

            StepType[,] MPointers = new StepType[S1.Length + 1, S2.Length + 1]; //0
            StepType[,] Gaps1Pointers = new StepType[S1.Length + 1, S2.Length + 1]; //1
            StepType[,] Gaps2Pointers = new StepType[S1.Length + 1, S2.Length + 1]; //2


            for (int i = 1; i <= S1.Length; i++)
                M[i, 0] = double.NegativeInfinity;
            for (int j = 1; j <= S2.Length; j++)
                M[0, j] = double.NegativeInfinity;


            for (int i = 0; i <= S1.Length; i++)
            {
                Gaps1[i, 0] = GapOpenPenalty + i * GapPenalty;
                Gaps2[i, 0] = double.NegativeInfinity;
            }

            for (int i = 0; i <= S2.Length; i++)
            {
                Gaps1[0, i] = double.NegativeInfinity;
                Gaps2[0, i] = GapOpenPenalty + i * GapPenalty;

            }

            for (int i = 1; i <= S1.Length; i++)
            {
                for (int j = 1; j <= S2.Length; j++)
                {

                    double score1 = 0;
                    double score2 = 0;
                    double score3 = 0;

                    double pairMatch;
                    if (SimilarityMatrix != null)
                    {
                        pairMatch = SimilarityMatrix[LetterIndexPairs[S1[i-1]], LetterIndexPairs[S2[j-1]]];
                    }
                    else
                    {
                        pairMatch = (S1[i-1] == S2[j-1] ? 1 : MismatchPenalty);
                    }
                    score1 = M[i - 1, j - 1] + pairMatch;
                    score2 = Gaps1[i - 1, j - 1] + pairMatch;
                    score3 = Gaps2[i - 1, j - 1] + pairMatch;

                    if (score1 >= score2 && score1 >= score3)
                    {
                        M[i, j] = score1;
                        MPointers[i, j] = StepType.M;
                        //Pointer matrix for M to M
                    }
                    else if (score2 >= score1 && score2 >= score3)
                    {
                        M[i, j] = score2;
                        MPointers[i, j] = StepType.Gaps1;
                        //Pointer matrix for M to Gaps1
                    }
                    else
                    {
                        M[i, j] = score3;
                        MPointers[i, j] = StepType.Gaps2;
                        //Pointer matrix for M to Gaps2
                    }

                    score1 = GapOpenPenalty + GapPenalty + M[i, j - 1];
                    score2 = GapPenalty + Gaps1[i, j - 1];
                    score3 = GapOpenPenalty + GapPenalty + Gaps2[i, j - 1];


                    if (score1 >= score2)
                    {
                        Gaps1[i, j] = score1;
                        Gaps1Pointers[i, j] = StepType.M;
                        //Pointer matrix for Gaps1 to M
                    }
                    else if (score2 >= score1 && score2 >= score3)
                    {
                        Gaps1[i, j] = score2;
                        Gaps1Pointers[i, j] = StepType.Gaps1;
                        //Pointer matrix for Gaps1 to Gaps1
                    }
                    else
                    {
                        Gaps1[i, j] = score3;
                        Gaps1Pointers[i, j] = StepType.Gaps2;
                        //Pointer matrix for Gaps1 to Gaps2
                    }

                    score1 = GapOpenPenalty + GapPenalty + M[i - 1, j];
                    score2 = GapPenalty + Gaps2[i - 1, j];
                    score3 = GapOpenPenalty + GapPenalty + Gaps1[i - 1, j];

                    if (score1 >= score2 && score1 >= score3)
                    {
                        Gaps2[i, j] = score1;
                        Gaps2Pointers[i, j] = StepType.M;
                        //Pointer matrix for Gaps2 to M
                    }
                    else if (score2 >= score1 && score2 >= score3)
                    {
                        Gaps2[i, j] = score2;
                        Gaps2Pointers[i, j] = StepType.Gaps2;
                        //Pointer matrix for Gaps2 to Gaps2
                    }
                    else
                    {
                        Gaps2[i, j] = score3;
                        Gaps2Pointers[i, j] = StepType.Gaps1;
                        //Pointer matrix for Gaps2 to Gaps1
                    }



                }
            }

            double s1 = Gaps1[S1.Length, S2.Length];
            double s2 = Gaps2[S1.Length, S2.Length];
            double s3 = M[S1.Length, S2.Length];
            double maxScore = 0;

            var stepType = StepType.M;
            if (s1 >= s2 && s1 >= s3)
            {
                maxScore = s1;
                stepType = StepType.Gaps1;

            }
            else if (s2 >= s1 && s2 >= s3)
            {
                maxScore = s2;
                stepType = StepType.Gaps2;
            }
            else
            {
                maxScore = s3;
                stepType = StepType.M;
            }

            var alignmentResult = GetAlignmentByAncestorsMatrix(S1, S2, MPointers, Gaps1Pointers, Gaps2Pointers, stepType);
            alignmentResult.Score = maxScore;
            return alignmentResult;
        }


    }
}
