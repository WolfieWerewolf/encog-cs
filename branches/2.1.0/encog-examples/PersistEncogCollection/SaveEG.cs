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
using Encog.Persist;
using System.IO;

namespace PersistEncogCollection
{
    class SaveEG
    {
        public const String FILENAME = "encogexample.eg";

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
            EncogPersistedCollection encog = new EncogPersistedCollection(FILENAME,FileMode.CreateNew);
		    encog.Create();
		    encog.Add("network", network);
        }

        public void LoadAndEvaluate()
        {
            Console.WriteLine("Loading network");
            
            EncogPersistedCollection encog = new EncogPersistedCollection(FILENAME,FileMode.Open);
		    BasicNetwork network = (BasicNetwork)encog.Find("network");
            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);
            double e = network.CalculateError(trainingSet);
            Console.WriteLine("Loaded network's error is(should be same as above): " + e);
        }

        static void Main(string[] args)
        {
            Logging.StopConsoleLogging();

            SaveEG program = new SaveEG();
            program.TrainAndSave();
            program.LoadAndEvaluate();
        }
    }
}
