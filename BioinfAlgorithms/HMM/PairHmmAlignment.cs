using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms.HMM
{
    class PairHmmAlignment
    {

        /// <summary>
        /// Вероятность открытия гэпа
        /// </summary>
        public double Delta { get; set; } = 0.1;
        /// <summary>
        /// Вероятность продолжения гэпа
        /// </summary>
        public double Epsilon { get; set; } = 0.3;
        public double Tau { get; set; } = 0.1;

        private Matrix<double> TrM { get; set; }

        private double MatchEmProb { get; set; } = 0.9;

        public PairHmmAlignment()
        {
            TrM =
             Matrix<double>.Build.DenseOfArray(
                 new double[,]
                 {    //M               X     Y   End
                    { 1-2*Delta - Tau,Delta,Delta,Tau }, //M
                    { 1-Epsilon -Tau,Epsilon,0,Tau },    //X
                    { 1-Epsilon-Tau,0,Epsilon,Tau},      //Y
                    { 0,0,0,1}                           //End
                 });
        }

        public (Matrix<double> M, Matrix<double> X, Matrix<double> Y) Forward(string seq1, string seq2)
        {
            int n = seq1.Length;
            int m = seq2.Length;

            var M = Matrix<double>.Build.Dense(n+1, m+1);
            var X = Matrix<double>.Build.Dense(n+1, m+1);
            var Y = Matrix<double>.Build.Dense(n+1, m+1);

            M[0, 0] = 1;
            X[0, 0] = 0;
            Y[0, 0] = 0;

            for(int i = 0; i<=n; i++)
            {
                for(int j = 0; j<=m; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (i == 0 || j == 0)
                    {
                        M[i, j] = 0;
                        X[i, j] = 0;
                        Y[i, j] = 0;
                    }
                    else
                    {
                        double pij = 1 - MatchEmProb;
                        if (seq1[i-1] == seq2[j-1])
                            pij = MatchEmProb;
                        M[i, j] = pij * ((1 - 2 * Delta - Tau) * M[i - 1, j - 1] + 
                            (1 - Epsilon - Tau) * (X[i - 1, j - 1] + Y[i - 1, j - 1]));
                    }
                    if (i == 0)
                    {
                        X[i,j] = 0;
                    }
                    else
                    {
                        X[i, j] = 1 * (Delta * M[i - 1, j] + Epsilon * X[i - 1, j]);
                    }
                    if (j == 0)
                    {
                        Y[i, j] = 0;
                    }
                    else
                    {
                        Y[i, j] = 1 * (Delta * M[i, j - 1] + Epsilon * Y[i, j - 1]);
                    }
                }
            }
            return (M.SubMatrix(1, M.RowCount-1,1,M.ColumnCount-1),
                X.SubMatrix(1, X.RowCount - 1, 1, X.ColumnCount - 1), 
                Y.SubMatrix(1, Y.RowCount - 1, 1, Y.ColumnCount - 1));
        }

        public (Matrix<double> M, Matrix<double> X, Matrix<double> Y) Backward(string seq1, string seq2)
        {
            int n = seq1.Length;
            int m = seq2.Length;

            var M = Matrix<double>.Build.Dense(n+1, m+1);
            var X = Matrix<double>.Build.Dense(n+1, m+1);
            var Y = Matrix<double>.Build.Dense(n+1, m+1);

            M[n-1, m-1] = Tau;
            X[n-1, m-1] = Tau;
            Y[n-1, m-1] = Tau;

            for (int i = n; i > 0; i--)
            {
                for (int j = m; j > 0; j--)
                {
                    if (i == n && j == m)
                        continue;

                    double pij = 1 - MatchEmProb;

                    if (i + 1 > n || j + 1 >m)
                        pij = 0;
                    else
                        if (seq1[i] == seq2[j])
                            pij = MatchEmProb;

                    double mij = (1 - 2 * Delta - Tau) * pij * M[i, j] +
                        Delta * (1 * X[i, j-1] + 1 * Y[i-1, j]);
                    double xij = pij*(1 - Epsilon - Tau) * M[i, j] + Epsilon * 1 * X[i, j-1];
                    double yij = pij*(1 - Epsilon - Tau) * M[i, j] + Epsilon * 1 * Y[i-1, j];
                    M[i-1, j-1] = mij;
                    X[i-1, j-1] = xij;
                    Y[i-1, j-1] = yij; //i, j+1 ?
                }
            }
            return (M.SubMatrix(0,M.RowCount -1, 0, M.ColumnCount-1),
                X.SubMatrix(0, X.RowCount - 1, 0, X.ColumnCount - 1),
                Y.SubMatrix(0, Y.RowCount - 1, 0, Y.ColumnCount - 1));
        }

        public (Matrix<double> M, Matrix<double> X, Matrix<double> Y) FB(string seq1, string seq2)
        {
            int n = seq1.Length;
            int m = seq2.Length;
            var forward = Forward(seq1, seq2);
            var backward = Backward(seq1, seq2);

            var fullProb = Tau * (forward.M[n - 1, m - 1] + forward.X[n - 1, m - 1] + forward.Y[n - 1, m - 1]);

            return (
                forward.M.PointwiseMultiply(backward.M) / fullProb,
                forward.X.PointwiseMultiply(backward.X) / fullProb,
                forward.Y.PointwiseMultiply(backward.Y) / fullProb);

        }
    }
}
