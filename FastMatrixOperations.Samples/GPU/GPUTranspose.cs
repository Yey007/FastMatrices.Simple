﻿
namespace FastMatrixOperations.Samples.GPU
{
    static class GPUTranspose
    { 
        static void Transpose()
        {
            GPUOperator op = new GPUOperator();

            //5*3 matrix
            FastMatrix one = new FastMatrix(5, 3);

            Utilities.FillMatrix(one, 5);

            //10 will start at the bottom left and go to the top right
            one[0, one.GetSize(0) - 1] = 10;
            FastMatrix result = op.Transpose(one);
        }
    }
}