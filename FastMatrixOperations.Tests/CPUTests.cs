using Xunit;

namespace FastMatrixOperations.Tests
{
    public class CPUTests : TestBase
    {
        SingleThreadedOperator cpu = new SingleThreadedOperator();

        [Fact]
        public void Add()
        {
            double[,] expected = MakeResult(size, size, 20);
            FastMatrix matrix = MakeMatrix(size, size, 15);
            FastMatrix matrix2 = MakeMatrix(size, size, 5);
            VerifyResults(cpu.Add(matrix, matrix2), expected);
        }

        [Fact]
        public void Subtract()
        {
            double[,] expected = MakeResult(size, size, 10);
            FastMatrix matrix = MakeMatrix(size, size, 15);
            FastMatrix matrix2 = MakeMatrix(size, size, 5);
            VerifyResults(cpu.Subtract(matrix, matrix2), expected);
        }

        [Fact]
        public void Multiply()
        {
            double[,] expected = MakeResult(size, size, 375);
            FastMatrix matrix = MakeMatrix(size, size, 15);
            FastMatrix matrix2 = MakeMatrix(size, size, 5);
            VerifyResults(cpu.Multiply(matrix, matrix2), expected);
        }

        [Fact]
        public void Transpose()
        {
            double[,] expected = MakeResult(size, size, 10);
            FastMatrix matrix = MakeMatrix(size, size, 10);

            matrix[0, size - 1] = 5;
            expected[size - 1, 0] = 5;

            matrix = cpu.Transpose(matrix);

            VerifyResults(matrix, expected);
        }
    }
}
