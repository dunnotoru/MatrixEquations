using System;
using MatrixEquations.MathObjects;

namespace MatrixEquations.Methods
{
    public class GaussMethod
    {
        public SquareMatrix CoefficientMatrix { get; private set; }
        public Vector FreeTerms { get; private set; }

        public GaussMethod(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            CoefficientMatrix = (SquareMatrix)coefficientMatrix.Clone(); ;
            FreeTerms = (Vector)freeTerms.Clone();
        }

        public Vector Solve()
        {
            SquareMatrix matrix = (SquareMatrix)CoefficientMatrix.Clone();
            Vector terms = (Vector)FreeTerms.Clone();

            if (terms.Size != matrix.Size)
                throw new ArithmeticException();

            for (int step = 0; step < matrix.Size; step++)
            {
                float mainElement = matrix[step, step];
                for (int i = step+1; i < matrix.RowCount; i++)
                {
                    float mu = matrix[i, step] / mainElement;
                    for (int column = 0; column < matrix.ColumnCount; column++)
                        matrix[i, column] -= mu * matrix[step, column];

                    terms[i] -= mu * terms[step];
                }
            }

            
            return CalcReversalProcess(matrix, terms);
        }
        public Vector SolveWithMainElementSelectionInColumn()
        {
            SquareMatrix matrix = (SquareMatrix)CoefficientMatrix.Clone();
            Vector terms = (Vector)FreeTerms.Clone();

            if (terms.Size != matrix.RowCount)
                throw new ArithmeticException();

            for (int step = 0; step < matrix.Size; step++)
            {
                int rowIndex = GetMainElementIndexInColumn(matrix, step);
                matrix.SwapRows(step,rowIndex);
                terms.Swap(step,rowIndex);
                float mainElement = matrix[step, step];
                for (int i = step + 1; i < matrix.RowCount; i++)
                {
                    float mu = matrix[i, step] / mainElement;
                    for (int column = 0; column < matrix.ColumnCount; column++)
                        matrix[i, column] -= mu * matrix[step, column];

                    terms[i] -= mu * terms[step];
                }
            }

            return CalcReversalProcess(matrix,terms);
        }
        private int GetMainElementIndexInColumn(Matrix matrix,int columnIndex)
        {
            int mainElementRowIndex = columnIndex;
            for (int row = columnIndex; row < matrix.RowCount; row++)
                if(Math.Abs(matrix[mainElementRowIndex, columnIndex]) < Math.Abs(matrix[row, columnIndex]))
                    mainElementRowIndex = row;

            return mainElementRowIndex;
        }
        public Vector SolveWithMainElementSelectionInRow()
        {
            SquareMatrix matrix = (SquareMatrix)CoefficientMatrix.Clone();
            Vector terms = (Vector)FreeTerms.Clone();

            Vector places = new Vector(FreeTerms.Size);
            for (int i = 0; i < FreeTerms.Size; i++)
                places[i] = i;
            if (terms.Size != matrix.RowCount)
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

                    terms[i] -= mu * terms[step];
                }
            }

            Vector temp = CalcReversalProcess(matrix, terms) ;
            Vector result = new Vector(terms.Size);
            for (int i = 0; i < places.Size; i++)
            {
                result[i] = temp[(int)places[i]];
            }

            return result;
        }
        private int GetMainElementIndexInRow(Matrix matrix, int rowIndex)
        {
            int mainElementColumnIndex = rowIndex;
            for (int column = rowIndex; column < matrix.ColumnCount; column++)
                if (Math.Abs(matrix[rowIndex, mainElementColumnIndex]) < Math.Abs(matrix[rowIndex, column]))
                    mainElementColumnIndex = column;

            return mainElementColumnIndex;
        }
        private Vector CalcReversalProcess(SquareMatrix coefficientMatrix, Vector freeTerms)
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
