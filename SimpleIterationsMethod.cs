using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixEquations
{
    public static class SimpleIterationsMethod
    {
        public static Vector Solve(SquareMatrix coefficientMatrix, Vector freeTerms, float calculationError)
        {
            List<Vector> x = new List<Vector>();
            x.Add((Vector)freeTerms.Clone());

            for (int iteration = 1; iteration < 10000; iteration++)
            {
                x.Add(new Vector(x[iteration - 1].Size));

                for (int i = 0; i < coefficientMatrix.Size; i++)
                {
                    float sum = 0;
                    for (int j = 0; j < coefficientMatrix.Size; j++)
                        sum += coefficientMatrix[i, j] * x[iteration - 1][j];
                    x[iteration][i] = freeTerms[i] + sum;
                }

                if ((x[iteration - 1] - x[iteration]).CalcDefaultNorm() <= calculationError)
                    break;
            }

            return x.Last();
        }
        public static (SquareMatrix,Vector) TransformSystemForIterativeMethod(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            SquareMatrix alpha = new SquareMatrix(coefficientMatrix.Size);
            Vector beta = new Vector(freeTerms.Size);

            for (int row = 0; row < coefficientMatrix.RowCount; row++)
            {
                for (int column = 0; column < coefficientMatrix.ColumnCount; column++)
                    if (row != column)
                        alpha[row, column] = -coefficientMatrix[row, column] / coefficientMatrix[row, row];
                beta[row] = freeTerms[row] / coefficientMatrix[row, row];
            }
            return (alpha, beta);
        }
    }
}
