using System;

namespace MatrixEquations
{
    public static class GaussMethod
    {
        public static Vector Solve(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            SquareMatrix matrix = (SquareMatrix)coefficientMatrix.Clone();
            Vector b = (Vector)freeTerms.Clone();

            if (b.Size != matrix.Size)
                throw new ArithmeticException();

            for (int step = 0; step < matrix.Size; step++)
            {
                float mainElement = matrix[step, step];
                for (int i = step+1; i < matrix.RowCount; i++)
                {
                    float mu = matrix[i, step] / mainElement;
                    for (int column = 0; column < matrix.ColumnCount; column++)
                        matrix[i, column] -= mu * matrix[step, column];

                    b[i] -= mu * b[step];
                }
            }

            
            return CalcReversalProcess(matrix, b);
        }
        public static Vector SolveWithMainElementSelectionInColumn(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            SquareMatrix matrix = (SquareMatrix)coefficientMatrix.Clone();
            Vector b = (Vector)freeTerms.Clone();

            if (b.Size != matrix.RowCount)
                throw new ArithmeticException();

            for (int step = 0; step < matrix.Size; step++)
            {
                int rowIndex = GetMainElementIndexInColumn(matrix, step);
                matrix.SwapRows(step,rowIndex);
                b.Swap(step,rowIndex);
                float mainElement = matrix[step, step];
                for (int i = step + 1; i < matrix.RowCount; i++)
                {
                    float mu = matrix[i, step] / mainElement;
                    for (int column = 0; column < matrix.ColumnCount; column++)
                        matrix[i, column] -= mu * matrix[step, column];

                    b[i] -= mu * b[step];
                }
            }

            return CalcReversalProcess(matrix,b);
        }
        private static int GetMainElementIndexInColumn(Matrix matrix,int columnIndex)
        {
            int mainElementRowIndex = columnIndex;
            for (int row = columnIndex; row < matrix.RowCount; row++)
                if(Math.Abs(matrix[mainElementRowIndex, columnIndex]) < Math.Abs(matrix[row, columnIndex]))
                    mainElementRowIndex = row;

            return mainElementRowIndex;
        }
        public static Vector SolveWithMainElementSelectionInRow(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            SquareMatrix matrix = (SquareMatrix)coefficientMatrix.Clone();
            Vector b = (Vector)freeTerms.Clone();
            float[] p = new float[freeTerms.Size];
            Vector places = new Vector(freeTerms.Size);
            for (int i = 0; i < freeTerms.Size; i++)
                places[i] = i;
            if (b.Size != matrix.RowCount)
                throw new ArithmeticException();

            for (int step = 0; step < matrix.Size; step++)
            {
                int columnIndex = GetMainElementIndexInRow(matrix, step);
                matrix.SwapColumns(step, columnIndex);
                places.Swap(step, columnIndex);

                float mainElement = matrix[step, step];
                for (int i = step + 1; i < matrix.RowCount; i++)
                {
                    float mu = matrix[i, step] / mainElement;
                    for (int column = 0; column < matrix.ColumnCount; column++)
                        matrix[i, column] -= mu * matrix[step, column];

                    b[i] -= mu * b[step];
                }
            }


            Vector temp = CalcReversalProcess(matrix, b) ;
            Vector result = new Vector(b.Size);
            for (int i = 0; i < places.Size; i++)
            {
                result[i] = temp[(int)places[i]];
            }

            return result;
        }
        private static int GetMainElementIndexInRow(Matrix matrix, int rowIndex)
        {
            int mainElementColumnIndex = rowIndex;
            for (int column = rowIndex; column < matrix.ColumnCount; column++)
                if (Math.Abs(matrix[rowIndex, mainElementColumnIndex]) < Math.Abs(matrix[rowIndex, column]))
                    mainElementColumnIndex = column;

            return mainElementColumnIndex;
        }
        private static Vector CalcReversalProcess(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            Vector result = new Vector(freeTerms.Size);
            for (int row = coefficientMatrix.RowCount - 1; row >= 0; row--)
            {
                float sum = 0;
                for (int column = coefficientMatrix.ColumnCount - 1; column >= 0; column--)
                    if (row != column)
                        sum += coefficientMatrix[row, column] * result[column];

                result[row] = (freeTerms[row] - sum) / coefficientMatrix[row, row];
            }
            return result;
        } 
    }
}
