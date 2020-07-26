
namespace FastMatrixOperations.Samples.GPU
{
    static class GPUAdd
    {
        static void Add()
        {
            //common wrappers are included in this library by default
            //see DefaultTypeOperators.cs under FastMatrixOperations
            GPUOperator op = new GPUOperator();

            //two 5*3 matrices
            FastMatrix one = new FastMatrix(5, 3);
            FastMatrix two = new FastMatrix(5, 3);

            Utilities.FillMatrix(one, 5);
            Utilities.FillMatrix(two, 10);

            FastMatrix result = op.Add(one, two);
        }
    }
}
