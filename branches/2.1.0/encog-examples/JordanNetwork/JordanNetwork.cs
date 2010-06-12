using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Util.Logging;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Anneal;
using Encog.Neural.Networks.Training.Strategy;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Synapse;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Training.Propagation.Back;

namespace JordanNetwork
{
    class JordanNetwork
    {
        public class JordanXOR
        {

            static BasicNetwork createJordanNetwork()
            {
                // construct an Jordan type network
                ILayer hidden, output;
                ILayer context = new ContextLayer(1);
                BasicNetwork network = new BasicNetwork();
                network.AddLayer(new BasicLayer(1));
                network.AddLayer(hidden = new BasicLayer(2));
                network.AddLayer(output = new BasicLayer(1));

                output.AddNext(context, SynapseType.OneToOne);
                context.AddNext(hidden);
                network.Structure.FinalizeStructure();
                network.Reset();
                return network;
            }

            static BasicNetwork createFeedforwardNetwork()
            {
                // construct a feedforward type network

                BasicNetwork network = new BasicNetwork();
                network.AddLayer(new BasicLayer(1));
                network.AddLayer(new BasicLayer(2));
                network.AddLayer(new BasicLayer(1));
                network.Structure.FinalizeStructure();
                network.Reset();
                return network;
            }

            public static double trainNetwork(String what, BasicNetwork network, INeuralDataSet trainingSet)
            {
                // train the neural network
                NeuralSimulatedAnnealing trainAlt = new NeuralSimulatedAnnealing(
                       network, trainingSet, 10, 2, 100);

                ITrain trainMain = new Backpropagation(network, trainingSet, 0.00001, 0.0);

                StopTrainingStrategy stop = new StopTrainingStrategy();
                trainMain.AddStrategy(new Greedy());
                trainMain.AddStrategy(new HybridStrategy(trainAlt));
                trainMain.AddStrategy(stop);

                int epoch = 0;
                while (!stop.ShouldStop())
                {
                    trainMain.Iteration();
                    Console.WriteLine("Training " + what + ", Epoch #" + epoch + " Error:" + trainMain.Error);
                    epoch++;
                }
                return trainMain.Error;
            }

            static void Main(string[] args)
            {
                Logging.StopConsoleLogging();
                TemporalXOR temp = new TemporalXOR();
                INeuralDataSet trainingSet = temp.Generate(100);

                BasicNetwork jordanNetwork = createJordanNetwork();
                BasicNetwork feedforwardNetwork = createFeedforwardNetwork();

                double jordanError = trainNetwork("Jordan", jordanNetwork, trainingSet);
                double feedforwardError = trainNetwork("Feedforward", feedforwardNetwork, trainingSet);

                Console.WriteLine("Best error rate with Jordan Network: " + jordanError);
                Console.WriteLine("Best error rate with Feedforward Network: " + feedforwardError);
                Console.WriteLine("Jordan should be able to get into the 40% range,\nfeedforward should not go below 50%.\nThe recurrent Elment net can learn better in this case.");
                Console.WriteLine("If your results are not as good, try rerunning, or perhaps training longer.");
            }
        }
    }
}

