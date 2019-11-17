using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms.HMM
{
    class ForwardBackward
    {
        public Matrix<double> Compute(Vector<double> pi, Matrix<double> A, Matrix<double> B, Vector<double> Y)
        {
            /*
             * У нас есть O - N наблюдений, S - K состояний, Y - последовательность наблюдений: y_1, y_2,..., y_T
             * Матрица A переходов из i-го состояния в j-е размером KxK
             * Матрица эмиссии размером KxN, которая определяет вероятность наблюдения O_j из состояния S_i
             */
            int K = pi.Count;
            int T = Y.Count;

            Matrix<double> alpha = Matrix<double>.Build.Dense(K, T);
            for(int i = 0; i<K; i++)
            {
                alpha[i, 0] = pi[i] * B[i, (int)Y[0]];
            }

            for(int t = 1; t<T; t++)
            {
                for (int j = 0; j < K; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < K; k++)
                    {
                        sum += alpha[k, t - 1] * A[k, j];
                    }
                    alpha[j, t] = sum* B[j, (int)Y[t]];
                }
            }

            Matrix<double> beta = Matrix<double>.Build.Dense(K, T);
            for (int i = 0; i < K; i++)
            {
                beta[i, T - 1] = 1;
            }

            for (int t = T-2; t >=0; t--)
            {
                for (int j = 0; j < K; j++)
                {
                    double sum = 0;
                    for (int i = 0; i < K; i++)
                    {
                        sum += beta[i, t + 1] * A[i, j] * B[j, (int)Y[t+1]];
                    }
                    beta[j, t] = sum;
                }
            }

            var lc = alpha.Column(T-1);

            Matrix<double> X = Matrix<double>.Build.Dense(K,T);

            for (int t = 0; t < T; t++)
            {
                double norm = 0;
                for (int i = 0; i < K; i++)
                    norm += alpha[i, t] * beta[i, t];

                for (int j = 0; j < K; j++)
                {



                    X[j, t] += alpha[j, t] * beta[j, t] / norm;
                }
            }

           

            return X;
            


        }

    }
}
