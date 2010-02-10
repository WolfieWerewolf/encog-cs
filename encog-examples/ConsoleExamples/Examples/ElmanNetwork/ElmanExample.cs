using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Synapse;
using Encog.Util.Randomize;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Anneal;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Strategy;
using Encog.Util.Logging;
using Encog.Examples.Util;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Activation;

namespace Encog.Examples.ElmanNetwork
{
    public class ElmanExample: IExample
    {
        private IExampleInterface app;

        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(ElmanExample),
                    "xor-elman",
                    "Elman Temporal XOR",
                    "Uses a temporal sequence, made up of the XOR truth table, as the basis for prediction.  Compares Elman to traditional feedforward.");
                return info;
            }
        }

        private BasicNetwork CreateElmanNetwork()
        {
            // construct an Elman type network
            ElmanPattern pattern = new ElmanPattern();
            pattern.ActivationFunction = new ActivationTANH();
            pattern.InputNeurons = 1;
            pattern.AddHiddenLayer(2);
            pattern.OutputNeurons = 1;
            return pattern.Generate();
        }

        private BasicNetwork CreateFeedforwardNetwork()
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

        private double TrainNetwork(String what, BasicNetwork network, INeuralDataSet trainingSet)
        {
            // train the neural network
            ICalculateScore score = new TrainingSetScore(trainingSet);
            ITrain trainAlt = new NeuralSimulatedAnnealing(
                    network, score, 10, 2, 100);


            ITrain trainMain = new Backpropagation(network, trainingSet, 0.00001, 0.0);

            StopTrainingStrategy stop = new StopTrainingStrategy();
            trainMain.AddStrategy(new Greedy());
            trainMain.AddStrategy(new HybridStrategy(trainAlt));
            trainMain.AddStrategy(stop);

            int epoch = 0;
            while (!stop.ShouldStop())
            {
                trainMain.Iteration();
                app.WriteLine("Training " + what + ", Epoch #" + epoch + " Error:" + trainMain.Error);
                epoch++;
            }
            return trainMain.Error;
        }

        public void Execute(IExampleInterface app)
        {
            this.app = app;
            Logging.StopConsoleLogging();
            TemporalXOR temp = new TemporalXOR();
            INeuralDataSet trainingSet = temp.Generate(100);

            BasicNetwork elmanNetwork = CreateElmanNetwork();
            BasicNetwork feedforwardNetwork = CreateFeedforwardNetwork();

            double elmanError = TrainNetwork("Elman", elmanNetwork, trainingSet);
            double feedforwardError = TrainNetwork("Feedforward", feedforwardNetwork, trainingSet);

            app.WriteLine("Best error rate with Elman Network: " + elmanError);
            app.WriteLine("Best error rate with Feedforward Network: " + feedforwardError);
            app.WriteLine("Elman should be able to get into the 30% range,\nfeedforward should not go below 50%.\nThe recurrent Elment net can learn better in this case.");
            app.WriteLine("If your results are not as good, try rerunning, or perhaps training longer.");
        }

    }
}
