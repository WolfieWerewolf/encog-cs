using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Layers;
using Encog.Util.Banchmark;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Networks.Training.Propagation.Multi;
using Encog.Util.Logging;

namespace MultiBench
{
    public class MultiBench
    {
        public const int INPUT_COUNT = 40;
        public const int HIDDEN_COUNT = 60;
        public const int OUTPUT_COUNT = 20;

        public static BasicNetwork generateNetwork()
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(MultiBench.INPUT_COUNT));
            network.AddLayer(new BasicLayer(MultiBench.HIDDEN_COUNT));
            network.AddLayer(new BasicLayer(MultiBench.OUTPUT_COUNT));
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        public static INeuralDataSet generateTraining()
        {
            INeuralDataSet training = RandomTrainingFactory.Generate(50000,
                    INPUT_COUNT, OUTPUT_COUNT, -1, 1);
            return training;
        }

        public static double evaluateRPROP(BasicNetwork network, INeuralDataSet data)
        {

            ResilientPropagation train = new ResilientPropagation(network, data);
            long start = DateTime.Now.Ticks;
            Console.WriteLine("Training 20 Iterations with RPROP");
            for (int i = 1; i <= 20; i++)
            {
                train.Iteration();
                Console.WriteLine("Iteration #" + i + " Error:" + train.Error);
            }
            //train.FinishTraining();
            long stop = DateTime.Now.Ticks;
            double diff = (stop - start) / Evaluate.TICKS / Evaluate.MILIS;
            Console.WriteLine("RPROP Result:" + diff + " seconds.");
            Console.WriteLine("Final RPROP error: " + network.CalculateError(data));
            return diff;
        }

        public static double evaluateMPROP(BasicNetwork network, INeuralDataSet data)
        {

            MultiPropagation train = new MultiPropagation(network, data);
            long start = DateTime.Now.Ticks;
            Console.WriteLine("Training 20 Iterations with MPROP");
            for (int i = 1; i <= 20; i++)
            {
                train.Iteration();
                Console.WriteLine("Iteration #" + i + " Error:" + train.Error);
            }
            //train.finishTraining();
            long stop = DateTime.Now.Ticks;
            double diff = (stop - start) / Evaluate.TICKS / Evaluate.MILIS;
            Console.WriteLine("MPROP Result:" + diff + " seconds.");
            Console.WriteLine("Final MPROP error: " + network.CalculateError(data));
            return diff;
        }
        static void Main(string[] args)
        {
            Logging.StopConsoleLogging();
            BasicNetwork network = generateNetwork();
            INeuralDataSet data = generateTraining();

            double rprop = evaluateRPROP(network, data);
            double mprop = evaluateMPROP(network, data);
            double factor = rprop / mprop;
            Console.WriteLine("Factor improvement:" + factor);
        }
    }
}
