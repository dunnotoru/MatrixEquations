using System.Collections;
using System.Text;
using System;

namespace MatrixEquations.MathObjects
{
    public class Matrix : ICloneable, IEnumerable
    {
        private float[,] _values;
        public float[,] Values
        {
            get { return _values; }
            private set { _values = value; }
        }

        private int _columnCount;
        public int ColumnCount
        {
            get { return _columnCount;}
            private set { _columnCount = value; }
        }

        private int _rowCount;
        public int RowCount
        {
            get { return _rowCount;}
            private set { _rowCount = value; }
        }
        public float this[int row, int column]
        {
            get { return Values[row, column]; }
            set { Values[row, column] = value; }
        }

        public Matrix(int size)
        {
            RowCount = size;
            ColumnCount = size;
            Values = new float[RowCount, ColumnCount];
        }
        public Matrix(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Values = new float[RowCount, ColumnCount];
        }
        public Matrix(float[,] matrixArray)
        {
            RowCount = matrixArray.GetLength(0);
            ColumnCount = matrixArray.GetLength(1);

            Values = matrixArray;
        }
        public Matrix(float[] array) //вектор столбец
        {
            RowCount = array.GetLength(0);
            ColumnCount = 1;
            Values = new float[RowCount, 1];
            for (int i = 0; i < RowCount; i++)
                Values[i, 0] = array[i];
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.RowCount != b.RowCount)
                throw new ArithmeticException();
            if (a.ColumnCount != b.ColumnCount)
                throw new ArithmeticException();

            Matrix result = new Matrix(a.RowCount, a.ColumnCount);

            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row, col] = a[row, col] + b[row, col];

            return result;
        }
        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.RowCount != b.RowCount)
                throw new ArithmeticException();
            if (a.ColumnCount != b.ColumnCount)
                throw new ArithmeticException();

            Matrix result = new Matrix(a.RowCount, a.ColumnCount);

            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row, col] = a[row, col] - b[row, col];

            return result;
        }
        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.RowCount != b.ColumnCount)
                throw new ArithmeticException();
            if (a.ColumnCount != b.RowCount)
                throw new ArithmeticException();

            Matrix result = new Matrix(a.RowCount, a.ColumnCount);

            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row, col] = MultiplyRowByColumn(a.GetRow(row), b.GetColumn(col));
            return result;
        }
        public static Matrix operator *(Matrix a, float b)
        {
            Matrix result = new Matrix(a.RowCount,a.ColumnCount);
            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row, col] = a[row, col] * b;

            return result;
        }
        public static Matrix operator *(float b, Matrix a)
        {
            Matrix result = new Matrix(a.RowCount, a.ColumnCount);
            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row, col] = a[row, col] * b;

            return result;
        }

        public static Vector operator *(Matrix a, Vector b)
        {
            if (b.Size != a.ColumnCount)
                throw new ArithmeticException();

            Vector result = new Vector(b.Size);
            for (int row = 0; row < a.RowCount; row++)
                for (int col = 0; col < a.ColumnCount; col++)
                    result[row] += a[row, col] * b[col];

            return result;
        }

        public static explicit operator Matrix(Vector vector)
        {
            Matrix result = new Matrix(vector.Size, 1);
            for (int row = 0; row < result.RowCount; row++)
                result[row, 0] = vector[row];
            return result;
        }

        public static float MultiplyRowByColumn(float[] row, float[] column)
        {
            if (row.Length != column.Length)
                throw new ArithmeticException();

            float result = 0;
            for (int i = 0; i < row.Length; i++)
                result += row[i] * column[i];
            return result;
        }
        public Matrix Pow(int n)
        {
            Matrix result = (Matrix)this.Clone();
            for (int i = 0; i < n - 1; i++)
                result *= this;

            return result;
        }
        public Matrix GetTransposedMatrix()
        {
            Matrix result = new Matrix(ColumnCount,RowCount);
            for (int row = 0; row < result.RowCount; row++)
                result.SetRow(GetColumn(row), row);

            return result;
        }
        public Matrix CutRow(int rowNumber)
        {
            Matrix result = new Matrix(RowCount-1, ColumnCount);
            for (int r = 0; r < result.RowCount; r++)
                for (int c = 0; c < result.ColumnCount; c++)
                    result[r, c] = r < rowNumber ? Values[r, c] : Values[r+1, c];
            return result;
        }
        public Matrix CutColumn(int columnNumber)
        {
            Matrix result = new Matrix(RowCount, ColumnCount-1);
            for (int r = 0; r < result.RowCount; r++)
                for (int c = 0; c < result.ColumnCount; c++)
                    result[r, c] = c < columnNumber ? Values[r, c] : Values[r, c + 1];
            return result;
        }

        public void SwapRows(int sourceRowIndex, int destinationRowIndex)
        {
            float[] temp = GetRow(destinationRowIndex);
            SetRow(GetRow(sourceRowIndex), destinationRowIndex);
            SetRow(temp, sourceRowIndex);
        }
        public void SwapColumns(int sourceColumnIndex, int destinationColumnIndex)
        {
            float[] temp = GetColumn(destinationColumnIndex);
            SetColumn(GetColumn(sourceColumnIndex), destinationColumnIndex);
            SetColumn(temp, sourceColumnIndex);
        }

        public float[] GetRow(int number)
        {
            if (number >= RowCount)
                throw new ArgumentOutOfRangeException();

            float[] row = new float[RowCount];
            for (int column = 0; column < ColumnCount; column++)
                row[column] = Values[number, column];

            return row;
        }
        public float[] GetColumn(int number)
        {
            if (number >= ColumnCount)
                throw new ArgumentOutOfRangeException();

            float[] column = new float[ColumnCount];
            for (int row = 0; row < RowCount; row++)
                column[row] = Values[row, number];

            return column;
        }
        private void SetRow(float[] row, int number)
        {
            if (number >= RowCount)
                throw new ArgumentOutOfRangeException();

            for (int column = 0; column < ColumnCount; column++)
                Values[number, column] = row[column];
        }
        private void SetColumn(float[] column, int number)
        {
            if (number >= ColumnCount)
                throw new ArgumentOutOfRangeException();

            for (int row = 0; row < RowCount; row++)
                Values[row, number] = column[row];
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    result.Append(Math.Round(this[row, col], 5));
                    result.Append(' ');
                }
                result.Append('\n');
            }
            return result.ToString();
        }
        public virtual object Clone()
            => new Matrix((float[,])Values.Clone());

        public float CalcEuqlidianNorm()
        {
            float sum = 0;
            for (int row = 0; row < RowCount; row++)
                for (int column = 0; column < ColumnCount; column++)
                    sum += Convert.ToSingle(Math.Pow(this[row,column], 2));
            return Convert.ToSingle(Math.Sqrt(sum));
        }
        public float CalcInfiniteNorm()
        {
            float maximumSum = float.NegativeInfinity;
            for (int row = 0; row < RowCount; row++)
            {
                float sumInRow = 0;
                for (int column = 0; column < ColumnCount; column++)
                    sumInRow += Convert.ToSingle(Math.Abs(this[row,column]));
                maximumSum = Math.Max(maximumSum, sumInRow);
            }
            return maximumSum;
        }
        public float CalcDefaultNorm()
        {
            float maximumSum = float.NegativeInfinity;
            for (int column = 0; column < ColumnCount; column++)
            {
                float sumIntColumn = 0;
                for (int row = 0; row < RowCount; row++)
                    sumIntColumn += Convert.ToSingle(Math.Abs(this[row, column]));
                maximumSum = Math.Max(maximumSum, sumIntColumn);
            }
            return maximumSum;
        }

        public IEnumerator GetEnumerator()
        {
            return Values.GetEnumerator();
        }



    }
}
