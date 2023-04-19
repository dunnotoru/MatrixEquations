using System;
using MatrixEquations.MathObjects;

namespace MatrixEquations.Methods
{
    public class RunThroughMethod
    {
        public SquareMatrix CoefficientMatrix { get; private set; }
        public Vector FreeTerms { get; private set; }

        public RunThroughMethod(SquareMatrix coefficientMatrix, Vector freeTerms)
        {
            CoefficientMatrix = coefficientMatrix;
            FreeTerms = freeTerms;
        }
        
        public Vector Solve()
        {
            SquareMatrix matrix = (SquareMatrix)CoefficientMatrix.Clone();
            Vector d = (Vector)FreeTerms.Clone();
            
            int size = FreeTerms.Size;
            int m = size - 1;
            
            Vector alpha = new Vector(size);
            Vector beta = new Vector(size);

            alpha[0] = -GetC(0) / GetB(0);
            beta[0] = d[0] / GetB(0);

            for (int i = 1; i < m; i++)
            {
                float y = GetB(i) + GetA(i) * alpha[i-1];
                alpha[i] = - GetC(i) / y;
                beta[i] = (d[i] - GetA(i) * beta[i-1]) / y;
            }

            beta[m] = (d[m] - GetA(m) * beta[m-1]) / GetB(m) + GetA(m) * alpha[m-1];
            
            Vector result = new Vector(size);
            result[m] = beta[m];

            for (int i = m-1; i >= 0; i--)
            {
                result[i] = alpha[i] * result[i + 1] + beta[i];
            }

            return result;
        }

        private float GetC(int i)
        {
            if (i >= 0 && i < FreeTerms.Size - 1)
                return CoefficientMatrix[i, i+1];
            
            throw new ArithmeticException();
        }

        private float GetA(int i)
        {
            if(i >= 1 && i < FreeTerms.Size)
                return CoefficientMatrix[i, i-1];

            throw new ArithmeticException();
        }

        private float GetB(int i)
            => CoefficientMatrix[i, i];

    }
}
