using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixEquations
{
    public class Vector : ICloneable, IEnumerable
    {
        private float[] _values;
        public float[] Values
        {
            get { return _values; }
            private set { _values = value; }
        }
        private int _size;
        public int Size
        {
            get { return _size; }
            private set { _size = value; }
        }

        public float this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value;}
        }
        public Vector(int size)
        {
            Size = size;
            Values = new float[Size];
        }
        public Vector(float[] values)
        {
            Values = values;
            Size = values.GetLength(0);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Size != b.Size)
                throw new ArithmeticException();
            Vector result = new Vector(a.Size);

            for(int index = 0; index < a.Size; index++)
                result[index] = a[index] + b[index];

            return result;
        }
        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Size != b.Size)
                throw new ArithmeticException();
            Vector result = new Vector(a.Size);

            for (int index = 0; index < a.Size; index++)
                result[index] = a[index] - b[index];

            return result;
        }
        public static Vector operator *(Vector a, float b)
        {
            Vector result = new Vector(a.Size);

            for (int index = 0; index < a.Size; index++)
                    result[index] = a[index] * b;

            return result;
        }
        public static Vector operator *(float b, Vector a)
        {
            return a * b;
        }

        public void Swap(int firstIndex, int secondIndex)
        {
            float temp = this[secondIndex];
            this[secondIndex] = this[firstIndex];
            this[firstIndex] = temp;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int index = 0; index < Size; index++)
            {
                result.Append(Math.Round(this[index], 5));
                result.Append('\n');
            }
            return result.ToString();
        }
        public virtual object Clone()
            => new Vector((float[])Values.Clone());
        public float CalcInfiniteNorm()
        {
            float maximum = float.NegativeInfinity;
            for (int index = 0; index < Size; index++)
                maximum = Math.Max(maximum, Math.Abs(this[index]));

            return maximum;
        }
        public float CalcDefaultNorm()
        {
            float sum = 0;
            for (int index = 0; index < Size; index++)
                sum += Math.Abs(this[index]);
            return sum;
        }
        public float CalcEuqlidianNorm()
        {
            float sum = 0;
            for (int index = 0; index < Size; index++)
                sum += Convert.ToSingle(Math.Pow(this[index], 2));
            return Convert.ToSingle(Math.Sqrt(sum));
        }

        public IEnumerator GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
