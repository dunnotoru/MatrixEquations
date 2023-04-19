using System;

namespace MatrixEquations.MathObjects
{
    public class SquareMatrix : Matrix 
    {
        public int Size { get; private set; }
        public SquareMatrix(int size) : base(size)
        {
            Size = size;
        }

        public SquareMatrix(float[,] matrixArray) : base(matrixArray)
        {
            if (matrixArray.GetLength(0) != matrixArray.GetLength(1))
                throw new ArithmeticException();

            Size = matrixArray.GetLength(0);
        }

        public float CalcDeterminant()
        {
            if (Size == 1)
                return this[0, 0];
            if (Size == 2)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];

            float result = 0;
            for (int i = 0; i < Size; i++)
                result += (i % 2 == 1 ? 1 : -1) * this[1, i]
                    * GetCutMatrix(1, i).CalcDeterminant();

            return result;
        }
        public SquareMatrix GetCutMatrix(int row, int column)
        {
            Matrix m = CutRow(row).CutColumn(column);
            if (m.RowCount != m.ColumnCount)
                throw new InvalidOperationException();
            SquareMatrix result = new SquareMatrix(m.RowCount);
            for (int r = 0; r < result.RowCount; r++)
                for (int c = 0; c < result.ColumnCount; c++)
                    result[r, c] = m[r, c];
            return result;
        }
        public Matrix GetInverseMatrix()
        {
            float determinant = CalcDeterminant();
            if (determinant == 0)
                throw new InvalidOperationException();
            SquareMatrix minorMatrix = GetMinorMatrix();
            SquareMatrix result = new SquareMatrix(Size);
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    result[r, c] = ((r + c) % 2 == 1 ? -1 : 1) *
                    minorMatrix[r, c] / determinant;

            return result.GetTransposedMatrix();
        }

        public float CalcMinor(int r, int c)
            => GetCutMatrix(r, c).CalcDeterminant();

        public SquareMatrix GetMinorMatrix()
        {
            SquareMatrix result = new SquareMatrix(Size);
            float determinant = CalcDeterminant();

            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    result[r, c] = CalcMinor(r, c);

            return result;
        }

        public override object Clone()
        {
            return new SquareMatrix((float[,])Values.Clone());
        }
    }
}
