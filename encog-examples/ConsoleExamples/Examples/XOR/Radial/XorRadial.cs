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
using Encog.Neural.Networks.Pattern;
using Encog.Util.Simple;

namespace Encog.Examples.XOR.Radial
{
    /// <summary>
    /// XOR: This example is essentially the "Hello World" of neural network
    /// programming.  This example shows how to construct an Encog neural
    /// network to predict the output from the XOR operator.  This example
    /// uses a radial network.
    /// </summary>
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

            RadialBasisPattern pattern = new RadialBasisPattern();
            pattern.InputNeurons = 2;
            pattern.AddHiddenLayer(4);
            pattern.OutputNeurons = 1;
            BasicNetwork network = pattern.Generate();
            RadialBasisFunctionLayer rbfLayer = (RadialBasisFunctionLayer)network.GetLayer(RadialBasisPattern.RBF_LAYER);
            network.Reset();
            rbfLayer.RandomizeGaussianCentersAndWidths(0, 1);


            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);

            // train the neural network
            ITrain train = new ResilientPropagation(network, trainingSet);

            EncogUtility.TrainToError(network, trainingSet, 0.01);

            // test the neural network
            EncogUtility.Evaluate(network, trainingSet);
        }
    }
}
