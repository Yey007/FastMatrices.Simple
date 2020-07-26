
namespace FastMatrixOperations.Samples.CPU
{
    static class CPUMult
    {
        static void Multiply()
        {
            //process is same for parallel
            SingleThreadedOperator op = new SingleThreadedOperator();

            //two 5*3 matrices
            FastMatrix one = new FastMatrix(5, 3);
            FastMatrix two = new FastMatrix(3, 5);

            Utilities.FillMatrix(one, 5);
            Utilities.FillMatrix(two, 10);

            FastMatrix result = op.Multiply(one, two);
            result.Print();
        }
    }
}