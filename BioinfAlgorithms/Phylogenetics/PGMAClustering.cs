using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace BioinfAlgorithms.Phylogenetic
{
    public static class PGMAClustering
    {
        public enum ClusteringType { WPGMA, UPGMA};
        private static (int i, int j) GetMinIndex(Matrix<double> matrix)
        {
            int curI = 0, curJ = 1;
            for (int k = 0; k < matrix.RowCount; k++)
            {
                for (int l = 0; l < matrix.ColumnCount; l++)
                {
                    if (matrix[curI, curJ] > matrix[k, l] && k!=l)
                    {
                        curI = k;
                        curJ = l;
                    }
                }
            }
            return (curI, curJ);
        }
        public static string BuildTree(Matrix<double> D, string[] objectNames, ClusteringType clusteringType)
        {
            var names = new List<(string name, int size)>();
            names.AddRange(objectNames.Select(x => (x, 1)));
            List<double> nodeLengths = new List<double>();
            nodeLengths.AddRange(Enumerable.Repeat(0.0, D.RowCount));
            double prev1 = 0;
            double prev2 = 0;
            while (names.Count>1)
            {
                (int minI, int minJ) = GetMinIndex(D);
                int firstRow = minI < minJ ? minI : minJ;
                int secondRow = minJ > minI ? minJ - 1 : minI - 1;
                double ijVal = D[minI, minJ];
                if (D.RowCount > 2)
                {
                    var newRow = Vector<double>.Build.Dense(D.RowCount - 2);
                    int j = 0;
                    for (int i = 0; i < D.RowCount; i++)
                    {
                        int iSize = names[minI].size;
                        int jSize = names[minJ].size;
                        if (clusteringType == ClusteringType.WPGMA)
                        {
                            iSize = 1;
                            jSize = 1;
                        }
                        if (i != minI && i != minJ)
                            newRow[j++] = (D[minI, i] * iSize + D[minJ, i] * jSize) / (iSize + jSize);
                    }
                    D = D.RemoveRow(firstRow).RemoveColumn(firstRow)
                        .RemoveRow(secondRow).RemoveColumn(secondRow);
                    D = D.InsertRow(D.RowCount, newRow);
                    D = D.InsertColumn(D.ColumnCount,
                        Vector<double>.Build.DenseOfArray(newRow.Concat(new double[] { 0 }).ToArray()));

                }
                string nameI = names[minI].name;
                string nameJ = names[minJ].name;
                names.RemoveAt(firstRow);
                names.RemoveAt(secondRow);
                names.Add(($"({nameI}:{ijVal/2 - nodeLengths[minI]},{nameJ}:{ijVal/2-nodeLengths[minJ]})", 2));
                nodeLengths.RemoveAt(firstRow);
                nodeLengths.RemoveAt(secondRow);
                nodeLengths.Add(ijVal/2);

            }
            return names.First().name;

        }
        


    }
}
