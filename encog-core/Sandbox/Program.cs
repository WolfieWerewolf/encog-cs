﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.CL;
using Encog.Util;
using Encog.Neural.NeuralData;
using Encog.Util.Banchmark;
using Encog.Neural.Networks;
using Encog.Util.Simple;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Networks.Flat;
using Encog.Neural.Data.Basic;
using Cloo;
using Encog.Util.CL.Kernels;

namespace Sandbox
{
    class Program
    {

        private static string kernelSource = @"
kernel void SingleNetworkCalculate(
    global read_only int inputSize,
    global read_only int outputSize,
    global read_only int layerCount,
    global read_only int neuronCount,
    global read_only int *layerIndex,
    global read_only int *layerCounts,
    global read_only int *weightIndex,
    global read_only float* input,
    global read_only float* weights,
    global write_only float *layerOutput
    )
{
    int sourceIndex = neuronCount - inputSize;

    for(int i=0;i<inputSize;i++)
        layerOutput[sourceIndex+i] = input[i];

    for (int currentLayer = layerCount - 1; currentLayer > 0; currentLayer--)
    {
      int inputIndex = layerIndex[currentLayer];
      int outputIndex = layerIndex[currentLayer - 1];
      int inputSize = layerCounts[currentLayer];
      int outputSize = layerCounts[currentLayer - 1];
      int index = weightIndex[currentLayer - 1];

      for (int i = 0; i < outputSize; i++)
      {
        layerOutput[i + outputIndex] = weights[index++];
      }

      for (int x = 0; x < outputSize; x++)
      {
        float sum = 0;
        for (int y = 0; y < inputSize; y++)
        {
          float value = layerOutput[inputIndex + y];
          value = -1 + (2 / (1 + exp(-2 * value)));
          sum += weights[index++] * value;
        }
        
        layerOutput[outputIndex + x] += sum;

        layerOutput[outputIndex + x] = layerOutput[outputIndex + x];
        
      }
    }
}
";

        /// <summary>
        /// Input for the XOR function.
        /// </summary>
        public static double[][] XOR_INPUT ={
            new double[2] { 0.0, 0.0 },
            new double[2] { 1.0, 0.0 },
			new double[2] { 0.0, 1.0 },
            new double[2] { 1.0, 1.0 } };

        /// <summary>
        /// Ideal output for the XOR function.
        /// </summary>
        public static double[][] XOR_IDEAL = {                                              
            new double[1] { 0.0 }, 
            new double[1] { 1.0 }, 
            new double[1] { 1.0 }, 
            new double[1] { 0.0 } };

        public static void stress()
        {
            INeuralDataSet trainingData = RandomTrainingFactory.Generate(100000, 100, 50, -1, 1);
            BasicNetwork network = EncogUtility.SimpleFeedForward(trainingData.InputSize, 50, 50, trainingData.IdealSize, true);
            EncogUtility.TrainDialog(network, trainingData);
            //EncogUtility.TrainConsole(network, trainingData, 10);
        }

        public static void CalculateGPU(FlatNetwork flat, double[] input, double[] output)
        {
            ComputeContextPropertyList cpl = new ComputeContextPropertyList(ComputePlatform.Platforms[0]);
            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Default, cpl, null, IntPtr.Zero);

            float[] inputArray = new float[flat.InputCount];
            float[] outputArray = new float[flat.OutputCount];
            float[] weightArray = new float[flat.Weights.Length];

            Random rand = new Random();

            for (int i = 0; i < inputArray.Length; i++)
                inputArray[i] = (float)input[i];
            for (int i = 0; i < flat.Weights.Length; i++)
                weightArray[i] = (float)flat.Weights[i];

            ComputeBuffer<float> a = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, inputArray);
            ComputeBuffer<float> b = new ComputeBuffer<float>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, weightArray);
            ComputeBuffer<float> c = new ComputeBuffer<float>(context, ComputeMemoryFlags.WriteOnly, flat.LayerOutput.Length);

            ComputeBuffer<int> d = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, flat.LayerIndex);
            ComputeBuffer<int> e = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, flat.LayerCounts);
            ComputeBuffer<int> f = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, flat.WeightIndex);

            ComputeProgram program = new ComputeProgram(context, new string[] { kernelSource });
            program.Build(null, null, null, IntPtr.Zero);

            ComputeKernel kernel = program.CreateKernel("SingleNetworkCalculate");

            kernel.SetValueArgument<int>(0, flat.InputCount);
            kernel.SetValueArgument<int>(1, flat.OutputCount);
            kernel.SetValueArgument<int>(2, flat.LayerCounts.Length);
            kernel.SetValueArgument<int>(3, flat.LayerOutput.Length);

            kernel.SetMemoryArgument(4, d);
            kernel.SetMemoryArgument(5, e);
            kernel.SetMemoryArgument(6, f);

            kernel.SetMemoryArgument(7, a);
            kernel.SetMemoryArgument(8, b);
            kernel.SetMemoryArgument(9, c);

            ComputeCommandQueue commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            ComputeEventList events = new ComputeEventList();

            commands.Execute(kernel, null, new long[] { 1 }, null, events);

            outputArray = commands.Read(c, true, 0, flat.LayerOutput.Length, events);

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = outputArray[i];
            }
        }

        public static void linear()
        {
            BasicNetwork network = EncogUtility.SimpleFeedForward(2, 3, 0, 1, true);
            FlatNetwork flat = new FlatNetwork(network);
            BasicNeuralDataSet training = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);
            TrainFlatNetwork train = new TrainFlatNetwork(flat, training);
            for (int i = 0; i < 100; i++)
            {
                train.Iteration();
                Console.WriteLine(train.Error);
            }

            /*ComputeContextPropertyList cpl = new ComputeContextPropertyList(ComputePlatform.Platforms[0]);
            ComputeContext context = new ComputeContext(ComputeDeviceTypes.Default, cpl, null, IntPtr.Zero);

            KernelSingleNetworkCalculate k = new KernelSingleNetworkCalculate(context, "Encog.Resources.KernelSingleNetCalculate.txt");
            k.compile();*/

            Encog.Encog.Instance.InitGPU();

            long start = Environment.TickCount;

            for (int j = 0; j < 100; j++)
            {

                double[] output = new double[1];
                for (int i = 0; i < XOR_INPUT.Length; i++)
                {
                    flat.Compute(XOR_INPUT[i], output);
                    //Console.WriteLine(XOR_INPUT[i][0] + ":" + XOR_INPUT[i][1] + ":" + output[0]);
                }
            }

            long stop = Environment.TickCount;
            Console.WriteLine("Time: " + (stop - start));

            Console.WriteLine("Done");
        }

        static void Main(string[] args)
        {
            //try
            {
                ///testCL();
                //stress();
                linear();
                //testBuffer();
            }
            //catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
            }

        }
    }
}
