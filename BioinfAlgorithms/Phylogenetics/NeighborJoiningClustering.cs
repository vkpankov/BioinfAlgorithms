using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioinfAlgorithms.Phylogenetics
{
    public static class NeighborJoiningClustering
    {
        private static (int i, int j) GetMinIndex(Matrix<double> matrix)
        {
            int curI = 0, curJ = 1;
            for (int k = 0; k < matrix.RowCount; k++)
            {
                for (int l = 0; l < matrix.ColumnCount; l++)
                {
                    if (matrix[curI, curJ] > matrix[k, l] && k != l)
                    {
                        curI = k;
                        curJ = l;
                    }
                }
            }
            return (curI, curJ);
        }

        private static Matrix<double> CalcQMatrix(Matrix<double> D)
        {
            int n = D.RowCount;
            Matrix<double> Q = Matrix<double>.Build.Dense(n, n);
            Vector<double> columnSums = D.ColumnSums();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                        Q[i, j] = (n - 2) * D[i, j] - columnSums[i] - columnSums[j];
                }
            }
            return Q;
        }

        public static string BuildTree(Matrix<double> D, string[] objectNames)
        {
        
            var names = new List<string>();
            names.AddRange(objectNames);
            List<double> nodeLengths = new List<double>();
            nodeLengths.AddRange(Enumerable.Repeat(0.0, D.RowCount));

            Vector<double> newRow = null;
            while (D.RowCount>2)
            {
                int n = D.RowCount;
                Vector<double> dColumnSums = D.ColumnSums();
                var Q = CalcQMatrix(D);
                (int minI, int minJ) = GetMinIndex(Q);
                int firstRow = minI < minJ ? minI : minJ;
                int secondRow = minJ > minI ? minJ - 1 : minI - 1;
                double ijValQ = Q[minI, minJ];
                double ijValD = D[minI, minJ];
                newRow = Vector<double>.Build.Dense(Q.RowCount - 2);

                if (Q.RowCount > 2)
                {
                    int j = 0;
                    for (int i = 0; i < Q.RowCount; i++)
                    {
                        if (i != minI && i != minJ)
                            newRow[j++] = 1 / 2.0 * (D[minI, i] + D[minJ, i] - D[minI, minJ]);
                    }
                    D = D.RemoveRow(firstRow).RemoveColumn(firstRow)
                        .RemoveRow(secondRow).RemoveColumn(secondRow);
                    D = D.InsertRow(D.RowCount, newRow);
                    D = D.InsertColumn(D.ColumnCount,
                        Vector<double>.Build.DenseOfArray(newRow.Concat(new double[] { 0 }).ToArray()));

                }

                double fuDist = 1 / 2.0 * ijValD + 1 / (2.0 * (n - 2)) * (dColumnSums[minI] - dColumnSums[minJ]);
                double guDist = ijValD - fuDist;
                

                string nameI = names[minI];
                string nameJ = names[minJ];
                names.RemoveAt(firstRow);
                names.RemoveAt(secondRow);


                names.Add($"({nameI}:{fuDist},{nameJ}:{guDist})");
                nodeLengths.RemoveAt(firstRow);
                nodeLengths.RemoveAt(secondRow);
                nodeLengths.Add(ijValQ / 2);
            }



            string f = names[1].TrimEnd(')') + "," + names[0] + ":" + newRow[0] + ")";
            return f;

        }
    }
}
