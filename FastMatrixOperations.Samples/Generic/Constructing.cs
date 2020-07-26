using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastMatrixOperations.Samples.Generic
{
    static class Constructing
    {
        static void Construct()
        {
            //constructs a 10 row by 3 column matrix
            FastMatrix withSize = new FastMatrix(10, 3);

            double[,] array = new double[10, 3];
            //constructs a 10 * 3 matrix from the multidimensional array
            FastMatrix with2DArray = new FastMatrix(array);

            double[][] jaggedArray = new double[10][];
            for(int i = 0, n = jaggedArray.Length; i < n; i++)
            {
                jaggedArray[i] = new double[3];
            }
            //constructs a 10 * 3 matrix from the jagged array
            FastMatrix withJaggedArray = new FastMatrix(jaggedArray);

            double[][] badJaggedArray = new double[10][];
            for (int i = 0, n = jaggedArray.Length; i < n; i++)
            {
                if (i == 0)
                {
                    badJaggedArray[i] = new double[4];
                }
                else
                {
                    badJaggedArray[i] = new double[3];
                }
            }
            //throws an exception
            //FastMatrix withBadJaggedArray = new FastMatrix(badJaggedArray);
        }
    }
}
