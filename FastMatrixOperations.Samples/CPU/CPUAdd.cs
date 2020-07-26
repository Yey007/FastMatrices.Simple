
namespace FastMatrixOperations.Samples.CPU
{
    static class CPUAdd
    {
        static void Add()
        {
            //process is same for parallel
            SingleThreadedOperator op = new SingleThreadedOperator();

            //two 5*3 matrices
            FastMatrix one = new FastMatrix(5, 3);
            FastMatrix two = new FastMatrix(5, 3);

            Utilities.FillMatrix(one, 5);
            Utilities.FillMatrix(two, 10);

            FastMatrix result = op.Add(one, two);
            result.Print();
        }
    }
}
