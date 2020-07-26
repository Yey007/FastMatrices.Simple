﻿using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.CPU;
using ILGPU.Runtime.Cuda;
using ILGPU.Runtime.OpenCL;
using System;

namespace FastMatrixOperations.Internal
{
    /// <summary>
    /// The hardware accelerator manager.
    /// </summary>
    public static class HardwareAcceleratorManager
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        private static Context context { get; set; }

        private static Accelerator gpuAccelerator;

        /// <summary>
        /// Gets the gpu accelerator.
        /// </summary>
        public static Accelerator GPUAccelerator
        {
            get
            {
                getGPUAccelerator();
                return gpuAccelerator;
            }
        }

        /// <summary>
        /// gets the GPU accelerator.
        /// </summary>
        /// <returns>An Accelerator.</returns>
        private static void getGPUAccelerator()
        {
            if (gpuAccelerator != null)
                return;
            if (CudaAccelerator.CudaAccelerators.Length > 0)
            {
                if (context == null)
                    context = new Context();
                gpuAccelerator = Accelerator.Create(context, CudaAccelerator.CudaAccelerators[0]);
                return;
            }
            foreach (CLAcceleratorId aid in CLAccelerator.CLAccelerators)
            {
                if (aid.DeviceType == ILGPU.Runtime.OpenCL.API.CLDeviceType.CL_DEVICE_TYPE_GPU)
                {
                    if (context == null)
                        context = new Context();
                    gpuAccelerator = Accelerator.Create(context, aid);
                    return;
                }
            }
            if (CPUAccelerator.CPUAccelerators.Length > 0)
            {
                if (context == null)
                    context = new Context();
                gpuAccelerator = new CPUAccelerator(context, Environment.ProcessorCount);
                return;
            }

        }


        /// <summary>
        /// Is a gpu available?
        /// </summary>
        /// <returns>A bool.</returns>
        public static bool IsGPUAvailable()
        {
            if (gpuAccelerator == null)
                getGPUAccelerator();
            return gpuAccelerator != null;
        }

        /// <summary>
        /// Disposes the accelerator.
        /// </summary>
        public static void Dispose()
        {
            if (context != null) context.Dispose();
            if (GPUAccelerator != null) GPUAccelerator.Dispose();
        }
    }
}