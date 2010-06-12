using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util;
using Encog.Util.Logging;

namespace PersistSerial
{

    public class Serial
    {

        public const String FILENAME = "encogexample.ser";

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

        public void TrainAndSave()
        {
            Console.WriteLine("Training XOR network to under 1% error rate.");
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(2));
            network.AddLayer(new BasicLayer(2));
            network.AddLayer(new BasicLayer(1));
            network.Structure.FinalizeStructure();
            network.Reset();

            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);

            // train the neural network
            ITrain train = new ResilientPropagation(network, trainingSet);

            do
            {
                train.Iteration();
            } while (train.Error > 0.009);

            double e = network.CalculateError(trainingSet);
            Console.WriteLine("Network traiined to error: " + e);

            Console.WriteLine("Saving network");
            SerializeObject.Save(FILENAME, network);
        }

        public void LoadAndEvaluate()
        {
            Console.WriteLine("Loading network");
            BasicNetwork network = (BasicNetwork)SerializeObject.Load(FILENAME);
            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);
            double e = network.CalculateError(trainingSet);
            Console.WriteLine("Loaded network's error is(should be same as above): " + e);
        }

        static void Main(string[] args)
        {
            Logging.StopConsoleLogging();

            Serial program = new Serial();
            program.TrainAndSave();
            program.LoadAndEvaluate();
        }
    }
}
