using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Activation;
using Encog.Neural.Data.Basic;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Training;
using Encog.Neural.Data;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace XORResilient
{
    /// <summary>
    /// XOR: This example is essentially the "Hello World" of neural network
    /// programming.  This example shows how to construct an Encog neural
    /// network to predict the output from the XOR operator.  This example
    /// uses RPROP to train the neural network.
    /// </summary>
    public class XORResilient
    {

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

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Not used.</param>
        static void Main(string[] args)
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 2));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 3));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 1));
            network.Structure.FinalizeStructure();
            network.Reset();

            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);

            // train the neural network
            // train the neural network
            ITrain train = new ResilientPropagation(network, trainingSet);

            int epoch = 1;

            do
            {
                train.Iteration();
                Console.WriteLine("Epoch #" + epoch + " Error:" + train.Error);
                epoch++;
            } while ((epoch < 5000) && (train.Error > 0.001));

            // test the neural network
            Console.WriteLine("Neural Network Results:");
            foreach (INeuralDataPair pair in trainingSet)
            {
                INeuralData output = network.Compute(pair.Input);
                Console.WriteLine(pair.Input[0] + "," + pair.Input[1]
                        + ", actual=" + output[0] + ",ideal=" + pair.Ideal[0]);
            }

        }
    }
}