using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms
{
    class RNAFolding
    {
        public int StemLoopMinLength { get; set; }
        public bool IsComplimentar(char x, char y)
        {
            if (x == 'A' && y == 'U' || x == 'U' && y == 'A' || x == 'G' && y == 'C' || x == 'C' && y == 'G')
                return true;
            else
                return false;
        }

        private int GetPairedMaxScore(string rnaSeq, int i, int j, int[,] matrix)
        {
            if (j <= i + StemLoopMinLength)
            {
                return 0;
            }
            int scoreWithoutPair = matrix[i, j - 1];
            int pairedMaxScore = 0;
            for (int t = i; t < j; t++)
            {
                if (IsComplimentar(rnaSeq[t], rnaSeq[j]))
                {
                    if (matrix[i, t] == -1)
                        matrix[i, t] = GetPairedMaxScore(rnaSeq, i, t, matrix);

                    if (matrix[t + 1, j - 1] == -1)
                        matrix[t + 1, j - 1] = GetPairedMaxScore(rnaSeq, t + 1, j - 1, matrix);

                    int tMax = 1 
                        + matrix[i,t]
                        + matrix[t+1,j-1];

                    if (tMax > pairedMaxScore)
                    {
                        pairedMaxScore = tMax;
                    }
                }
            }
            return Math.Max(scoreWithoutPair, pairedMaxScore);
        }

        public int[,] FoldRNA(string rnaSeq)
        {
            int n = rnaSeq.Length;
            int[,] nussinovMatrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = i + StemLoopMinLength; j < n; j++)
                {
                    nussinovMatrix[i, j] = -1;
                }
            }
            GetPairedMaxScore(rnaSeq, 0, n - 1, nussinovMatrix);
            return nussinovMatrix;
        }

        public string Traceback(string S, int[,] M)
        {
            int n = M.GetLength(1);
            StringBuilder result = new StringBuilder(new string('.', n));
            var ijStack = new Stack<(int i, int j)>();
            ijStack.Push((0, n - 1));
            List<(int i, int j)> pairs = new List<(int i, int j)>();
            while (ijStack.Count > 0)
            {
                (int i, int j) = ijStack.Pop();
                if (i + StemLoopMinLength >= j) continue;
                {
                    if (IsComplimentar(S[i], S[j]) && M[i + 1, j - 1] + 1 == M[i, j])
                    {
                        result[i] = '(';
                        result[j] = ')';
                        pairs.Add((i, j));
                        ijStack.Push((i + 1, j - 1));
                    }
                    else
                    {
                        for (int k = i; k < j; k++)
                        {
                            if (M[i, k] + M[k + 1, j] == M[i, j])
                            {
                                ijStack.Push((k + 1, j));
                                ijStack.Push((i, k));
                                break;
                            }
                        }
                    }
                }
            }
            return result.ToString();
        }
    }
}
