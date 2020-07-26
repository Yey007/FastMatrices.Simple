
namespace FastMatrixOperations.Samples.GPU
{
    static class GPUSub
    {
        static void Subtract()
        {
            GPUOperator op = new GPUOperator();

            //two 5*3 matrices
            FastMatrix one = new FastMatrix(5, 3);
            FastMatrix two = new FastMatrix(5, 3);

            Utilities.FillMatrix(one, 5);
            Utilities.FillMatrix(two, 10);

            FastMatrix result = op.Subtract(one, two);
        }
    }
}