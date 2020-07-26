using System;
using Xunit;

namespace FastMatrixOperations.Tests
{
    public class TestBase
    {
        protected const int size = 5;

        protected double[,] MakeResult(int rows, int columns, double value)
        {
            double[,] resultArray = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resultArray[i, j] = value;
                }
            }

            return resultArray;
        }

        protected FastMatrix MakeMatrix(int rows, int columns, double value)
        {
            double[,] resultArray = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    resultArray[i, j] = value;
                }
            }

            return new FastMatrix(resultArray);
        }

        protected void VerifyResults(FastMatrix matrix, double[,] expected)
        {
            Assert.Equal(matrix.GetSize(0), expected.GetLength(0));
            Assert.Equal(matrix.GetSize(1), expected.GetLength(1));
            for (int i = 0; i < matrix.GetSize(0); i++)
            {
                for (int j = 0; j < matrix.GetSize(1); j++)
                {
                    Assert.Equal(expected[i, j], matrix[i, j]);
                }
            }
        }
    }
}
