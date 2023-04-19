using System;
using System.Text;

namespace MatrixEquations.MathObjects
{
    public class LinearSystem
    {
        private SquareMatrix _coefficientMatrix;
        public SquareMatrix CoefficientMatrix
        {
            get { return _coefficientMatrix; }
            private set { _coefficientMatrix = value; }
        }

        private Vector _freeTerms;
        public Vector FreeTerms
        {
            get { return _freeTerms;}
            private set { _freeTerms = value; }
        }

        private int _size;
        public int Size
        {
            get { return _size; }
            private set { _size = value; }
        }
        public LinearSystem(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            if (coefficientMatrix.Size != freeTerms.Size)
                throw new ArgumentException();

            CoefficientMatrix = coefficientMatrix;
            FreeTerms = freeTerms;
            Size = freeTerms.Size;
        }

        public void SwapRows(int source, int destination)
        {
            CoefficientMatrix.SwapRows(source, destination);
            FreeTerms.Swap(source,destination);
        }

        public void SwapColumns(int source, int destination)
        {
            CoefficientMatrix.SwapColumns(source, destination);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int row = 0; row < CoefficientMatrix.RowCount; row++)
            {
                for (int column = 0; column < CoefficientMatrix.ColumnCount; column++)
                {
                    result.Append(Math.Round(CoefficientMatrix[row, column],3));
                    result.Append($" ");
                }
                result.Append("= ");
                result.Append(Math.Round(FreeTerms[row],3));
                result.Append("\n");
            }
            return result.ToString();
        }
    }
}
