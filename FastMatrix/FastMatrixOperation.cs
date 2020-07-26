using System;
using System.Threading.Tasks;
using ILGPU;
using ILGPU.Runtime;
using FastMatrixOperations.Internal;
using System.Diagnostics;
using ILGPU.Runtime.Cuda;

namespace FastMatrixOperations
{
    /// <summary>
    /// The base class for CPU operators
    /// </summary>
    public interface IMatrixOperator
    {
        public FastMatrix Add(FastMatrix one,
            FastMatrix two);
        public FastMatrix Subtract(FastMatrix one,
            FastMatrix two);
        public FastMatrix Multiply(FastMatrix one,
            FastMatrix two);
        public FastMatrix Transpose(
            FastMatrix matrix);
    }

    /// <summary>
    /// Accesses the CPU for operations
    /// </summary>
    public class SingleThreadedOperator : IMatrixOperator
    {
        /// <summary>
        /// Adds two matrices on the CPU using a single thread
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the addition</returns>
        public FastMatrix Add(FastMatrix one, FastMatrix two)
        {
            if(one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }
            FastMatrix fastMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));
            for (int i = 0; i < one.GetSize(0); i++)
            {
                for (int j = 0; j < one.GetSize(1); j++)
                {
                    fastMatrix[i, j] = one[i, j] + two[i, j];
                }
            }
            return fastMatrix;
        }

        /// <summary>
        /// Multiplies two matrices on the CPU using a single thread
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the multiplication</returns>
        public FastMatrix Multiply(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if (one.GetSize(1) != two.GetSize(0))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }
            FastMatrix returnMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));

            for (int i = 0; i < returnMatrix.GetSize(0); i++)
            {
                for (int j = 0; j < returnMatrix.GetSize(1); j++)
                {
                    double sum = 0;
                    for (int k = 0; k < one.GetSize(0); k++)
                    {
                        sum += one[i, k] * two[k, j];
                    }
                    returnMatrix[i, j] = sum;
                }
            }
            return returnMatrix;
        }

        /// <summary>
        /// Subtracts two matrices on the CPU using a single thread
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the subtraction (one - two)</returns>
        public FastMatrix Subtract(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }
            FastMatrix fastMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));

            for (int i = 0; i < one.GetSize(0); i++)
            {
                for (int j = 0; j < one.GetSize(1); j++)
                {
                    fastMatrix[i, j] = one[i, j] - two[i, j];
                }
            }
            return fastMatrix;
        }

        /// <summary>
        /// Transposes a matrix on the CPU using a single thread
        /// </summary>
        /// <param name="matrix">The matrix</param>
        /// <returns>The result of the transpose</returns>
        public FastMatrix Transpose(FastMatrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException();
            }
            FastMatrix returnMatrix = new FastMatrix(matrix.GetSize(1), matrix.GetSize(0));
            for (int i = 0; i < matrix.GetSize(0); i++)
            {
                for (int j = 0; j < matrix.GetSize(1); j++)
                {
                    returnMatrix[j, i] = matrix[i, j];
                }
            }
            return returnMatrix;
        }
    }

    /// <summary>
    /// Accesses the CPU for operations, but operations run using multiple threads
    /// </summary>
    public class MultiThreadedOperator : IMatrixOperator
    {
        /// <summary>
        /// Adds two matrices on the CPU using multiple threads
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the addition</returns>
        public FastMatrix Add(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }
            FastMatrix fastMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));
            Parallel.For(0, one.GetSize(0), i =>
            {
                for (int j = 0; j < one.GetSize(1); j++)
                {
                    fastMatrix[i, j] = one[i, j] + two[i, j];
                }
            });
            return fastMatrix;
        }

        /// <summary>
        /// Multiplies two matrices on the CPU using multiple threads
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the multiplication</returns>
        public FastMatrix Multiply(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if (one.GetSize(1) != two.GetSize(0))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }

            FastMatrix returnMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));

            Parallel.For(0, returnMatrix.GetSize(0), (i) =>
            {
                for (int j = 0; j < returnMatrix.GetSize(1); j++)
                {
                    double sum = 0;
                    for (int k = 0; k < one.GetSize(0); k++)
                    {
                        sum += one[i, k] * two[k, j];
                    }
                    returnMatrix[i, j] = sum;
                }
            });
            return returnMatrix;
        }

        /// <summary>
        /// Subtracts two matrices on the CPU using multiple threads
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the subtraction (one - two)</returns>
        public FastMatrix Subtract(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }
            FastMatrix fastMatrix = new FastMatrix(one.GetSize(0), two.GetSize(1));
            Parallel.For(0, one.GetSize(0), i =>
            {
                for (int j = 0; j < one.GetSize(1); j++)
                {
                    fastMatrix[i, j] = one[i, j] - two[i, j];
                }
            });
            return fastMatrix;
        }

        /// <summary>
        /// Transposes a matrix on the CPU using multiple threads
        /// </summary>
        /// <param name="matrix">The matrix</param>
        /// <returns>The transposed matrix</returns>
        public FastMatrix Transpose(FastMatrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException();
            }
            FastMatrix returnMatrix = new FastMatrix(matrix.GetSize(1), matrix.GetSize(0));

            Parallel.For(0, matrix.GetSize(0), (i) =>
            {
                for (int j = 0; j < matrix.GetSize(1); j++)
                {
                    returnMatrix[j, i] = matrix[i, j];
                }
            });

            return returnMatrix;
        }
    }

    /// <summary>
    /// Accesses the GPU for operations
    /// </summary>
    /// <remarks>
    /// Note: This is not always faster. There is a lot of overhead in copying information.
    /// </remarks>
    public class GPUOperator : IMatrixOperator
    {
        private static Accelerator accelerator;

        #region Preloaded kernels
        private static Action<Index2, ArrayView2D<double>, ArrayView2D<double>, ArrayView2D<double>>
            GPUAddKernel;

        private static Action<Index2, ArrayView2D<double>, ArrayView2D<double>, ArrayView2D<double>>
             GPUSubKernel;

        private static Action<Index2, ArrayView2D<double>, ArrayView2D<double>, ArrayView2D<double>>
            GPUMultKernel;

        private static Action<Index2, ArrayView2D<double>, ArrayView2D<double>> GPUTransposeKernel;
        #endregion

        static GPUOperator()
        {
            accelerator = HardwareAcceleratorManager.GPUAccelerator;
            GPUAddKernel = accelerator.LoadAutoGroupedStreamKernel<Index2, ArrayView2D<double>, 
                ArrayView2D<double>, ArrayView2D<double>>(GPUAdd);
            GPUSubKernel = accelerator.LoadAutoGroupedStreamKernel<Index2, ArrayView2D<double>,
                ArrayView2D<double>, ArrayView2D<double>>(GPUSub);
            GPUMultKernel = accelerator.LoadAutoGroupedStreamKernel<Index2, ArrayView2D<double>,
                ArrayView2D<double>, ArrayView2D<double>>(GPUMult);
            GPUTransposeKernel = accelerator.LoadAutoGroupedStreamKernel<Index2, 
                ArrayView2D<double>, ArrayView2D<double>>(GPUTranspose);
        }

        /// <summary>
        /// Adds two matrices on the GPU
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the addition</returns>
        public FastMatrix Add(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }

            Stopwatch watch = Stopwatch.StartNew();
            MemoryBuffer2D<double> resultBuffer;

            one.CopyToGPU();
            two.CopyToGPU();
            Console.WriteLine($"Copy: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            resultBuffer = accelerator.Allocate<double>(one.GetSize(0), one.GetSize(1));
            Console.WriteLine($"Allocate: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            one.WaitForCopy(); //this function call is currently not required, 
                               //will come up with a better solution later but for now I'm just
                               //gonna leave it here
            two.WaitForCopy();
            Console.WriteLine($"Finish copy: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            GPUAddKernel(resultBuffer.Extent, one.buffer.View, two.buffer.View, resultBuffer.View);

            accelerator.Synchronize();
            Console.WriteLine($"Execution: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            var tempArray = resultBuffer.GetAs2DArray();
            accelerator.Synchronize();
            Console.WriteLine($"Copy back: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            FastMatrix returnMatrix = new FastMatrix(tempArray);
            return returnMatrix;
        }

        /// <summary>
        /// Subtracts two matrices on the GPU
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the subtraction (one - two) </returns>
        public FastMatrix Subtract(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if ((one.GetSize(0) != two.GetSize(0)) || (one.GetSize(1) != two.GetSize(1)))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }

            MemoryBuffer2D<double> resultBuffer;

            //start tasks
            one.CopyToGPU();
            two.CopyToGPU();

            resultBuffer = accelerator.Allocate<double>(one.GetSize(0), one.GetSize(1));
            
            one.WaitForCopy();
            two.WaitForCopy();

            GPUSubKernel(resultBuffer.Extent, one.buffer.View, two.buffer.View, resultBuffer.View);

            accelerator.Synchronize();

            var tempArray = resultBuffer.GetAs2DArray();
            accelerator.Synchronize();

            FastMatrix returnMatrix = new FastMatrix(tempArray);
            return returnMatrix;
        }

        /// <summary>
        /// Multiplies two matrices on the GPU
        /// </summary>
        /// <param name="one">The first matrix</param>
        /// <param name="two">The second matrix</param>
        /// <returns>The result of the multiplication</returns>
        public FastMatrix Multiply(FastMatrix one, FastMatrix two)
        {
            if (one == null || two == null)
            {
                throw new ArgumentNullException();
            }
            if (one.GetSize(1) != two.GetSize(0))
            {
                throw new BadDimensionException(one.GetSize(0), one.GetSize(1), two.GetSize(0),
                    two.GetSize(1));
            }

            Stopwatch watch = Stopwatch.StartNew();
            MemoryBuffer2D<double> resultBuffer;

            //start tasks
            one.CopyToGPU();
            two.CopyToGPU();
            Console.WriteLine($"Copy: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            resultBuffer = accelerator.Allocate<double>(one.GetSize(0), two.GetSize(1));
            Console.WriteLine($"Alloc: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            one.WaitForCopy();
            two.WaitForCopy();
            Console.WriteLine($"Finish copy: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            GPUMultKernel(resultBuffer.Extent, one.buffer.View, two.buffer.View, 
                resultBuffer.View);

            accelerator.Synchronize();
            Console.WriteLine($"Execute: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            var tempArray = resultBuffer.GetAs2DArray();
            accelerator.Synchronize();
            Console.WriteLine($"Copy back: {watch.ElapsedMilliseconds}ms");
            watch.Restart();

            FastMatrix returnMatrix = new FastMatrix(tempArray);
            return returnMatrix;
        }

        /// <summary>
        /// Transposes a matrix on the GPU
        /// </summary>
        /// <param name="matrix">The matrix</param>
        /// <returns>The transposed matrix</returns>
        public FastMatrix Transpose(FastMatrix matrix)
        {
            if (matrix == null)
            {
                throw new ArgumentNullException();
            }
            Accelerator accelerator;
            MemoryBuffer2D<double> resultBuffer;

            matrix.CopyToGPU();

            accelerator = HardwareAcceleratorManager.GPUAccelerator;
            resultBuffer = accelerator.Allocate<double>(matrix.GetSize(1), matrix.GetSize(0));
            var kernel = GPUTransposeKernel;
            matrix.WaitForCopy();

            kernel(resultBuffer.Extent, matrix.buffer.View, resultBuffer.View);
            accelerator.Synchronize();

            var tempArray = resultBuffer.GetAs2DArray();
            accelerator.Synchronize();

            FastMatrix returnMatrix = new FastMatrix(tempArray);
            return returnMatrix;
        }

        //kernels
        #region Kernels
        private static void GPUAdd(Index2 index, ArrayView2D<double> aView, ArrayView2D<double> bView, 
            ArrayView2D<double> resView)
        {
            resView[index] = aView[index] + bView[index];
        }
        private static void GPUSub(Index2 index, ArrayView2D<double> aView, ArrayView2D<double> bView, 
            ArrayView2D<double> resView)
        {
            resView[index] = aView[index] - bView[index];
        }
        private static void GPUMult(Index2 index, ArrayView2D<double> aView, ArrayView2D<double> bView, 
            ArrayView2D<double> resView)
        {
            int x = index.X; //matrix one row
            int y = index.Y; //matrix two column

            double sum = 0;
            for (int i = 0; i < aView.Height; i++)
            {
                sum += aView[x, i] * bView[i, y];
            }
            resView[index] = sum;
        }
        private static void GPUTranspose(Index2 index, ArrayView2D<double> originalView, 
            ArrayView2D<double> result)
        {
            result[index.Y, index.X] = originalView[index.X, index.Y];
        }
        #endregion
    }
}
