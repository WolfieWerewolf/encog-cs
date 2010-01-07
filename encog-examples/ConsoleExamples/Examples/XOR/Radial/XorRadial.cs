using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Logging;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Synapse;
using Encog.Neural.Networks.Training;
using Encog.Neural.Data;
using ConsoleExamples.Examples;

namespace Encog.Examples.XOR.Radial
{
    /**
 * XOR: This example is essentially the "Hello World" of neural network
 * programming.  This example shows how to construct an Encog neural
 * network to predict the output from the XOR operator.  This example
 * uses backpropagation to train the neural network.
 * 
 * @author $Author$
 * @version $Revision$
 */
    public class XorRadial : IExample
    {
        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(XorRadial),
                    "xor-radial",
                    "XOR Operator with a RBF Network",
                    "Use a RBF network to learn the XOR operator.");
                return info;
            }
        }
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
        public void Execute(IExampleInterface app)
        {

            Logging.StopConsoleLogging();

            RadialBasisFunctionLayer rbfLayer;
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(), false, 2));
            network.AddLayer(rbfLayer = new RadialBasisFunctionLayer(4), SynapseType.Direct);
            network.AddLayer(new BasicLayer(1));
            network.Structure.FinalizeStructure();
            network.Reset();
            rbfLayer.RandomizeGaussianCentersAndWidths(0, 1);


            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);

            // train the neural network
            ITrain train = new ResilientPropagation(network, trainingSet);


            int epoch = 1;

            do
            {
                train.Iteration();
                Console.WriteLine("Epoch #" + epoch + " Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.01);

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
