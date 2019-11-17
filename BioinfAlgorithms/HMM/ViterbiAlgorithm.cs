using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace BioinfAlgorithms.HMM
{
    class ViterbiAlgorithm
    {
        public Vector<double> Compute(Vector<double> pi, Matrix<double> A, Matrix<double> B, Vector<double> Y)
        {
            /*
             * У нас есть O - N наблюдений, S - K состояний, Y - последовательность наблюдений: y_1, y_2,..., y_T
             * Матрица A переходов из i-го состояния в j-е размером KxK
             * Матрица эмиссии размером KxN, которая определяет вероятность наблюдения O_j из состояния S_i
             */
            int K = pi.Count;
            int T = Y.Count;
            Matrix<double> state = Matrix<double>.Build.Dense(K, T);
            Matrix<double> index = Matrix<double>.Build.Dense(K, T);
            for (int i = 0; i < K; i++)
                state[i, 0] = pi[i] * B[i, (int)Y[0]];
            //state.SetColumn(0, pi.PointwiseMultiply(B.Column(0)));


            for(int t = 1; t<T; t++)
            {
                for(int j = 0; j<K; j++)
                {

                    int maxK = 0;
                    double prevMax = double.MinValue;
                    for(int k = 0; k<K; k++)
                    {
                        double val = state[k, t - 1] * A[k, j] * B[j, (int)Y[t]];
                        if (val > prevMax)
                        {
                            prevMax = val;
                            maxK = k;
                        }
                    }
                    index[j, t] = maxK;
                    state[j, t] = state[(int)index[j, t], t - 1] * A[maxK, j] * B[j, (int)Y[t]];
                }
            }
            Vector<double> X = Vector<double>.Build.Dense(T);
            int maxI = 0;
            for(int i =0; i<K; i++)
            {
                if (index[i, T - 1] > index[maxI, T - 1])
                    maxI = i;
            }
            X[T - 1] = maxI;
            for(int i =T-1; i>0; i--)
            {
                X[i - 1] = (int)index[(int)X[i], i];
            }
            return X;

        }
    }
}
