using Xunit;

namespace FastMatrixOperations.Tests
{
    public class GPUTests : TestBase
    {
        GPUOperator gpu = new GPUOperator();

        [Fact]
        public void Add()
        {
            double[,] expected = MakeResult(size, size, 20);

            FastMatrix one = MakeMatrix(size, size, 15);
            FastMatrix two = MakeMatrix(size, size, 5);
            FastMatrix actual = gpu.Add(one, two);
            VerifyResults(actual, expected);
        }

        [Fact]
        public void Subtract()
        {
            double[,] expected = MakeResult(size, size, 15);

            FastMatrix one = MakeMatrix(size, size, 20);
            FastMatrix two = MakeMatrix(size, size, 5);
            FastMatrix actual = gpu.Subtract(one, two);
            VerifyResults(actual, expected);
        }

        [Fact]
        public void Multiply()
        {
            double[,] expected = MakeResult(size, size, 750);

            FastMatrix one = MakeMatrix(size, size, 15);
            FastMatrix two = MakeMatrix(size, size, 10);
            FastMatrix actual = gpu.Multiply(one, two);
            VerifyResults(actual, expected);
        }

        [Fact]
        public void Transpose()
        {
            double[,] expected = MakeResult(size, size, 10);

            FastMatrix one = MakeMatrix(size, size, 10);

            one[size - 1, 0] = 5;
            expected[0, size - 1] = 5;

            FastMatrix actual = gpu.Transpose(one);
            VerifyResults(actual, expected);
        }
    }
}
