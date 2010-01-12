using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Networks;
using Encog.Util.Logging;
using Encog.Neural.Networks.Training;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Training.Genetic;
using Encog.Util.Randomize;
using Encog.Neural.Networks.Training.Anneal;
using ConsoleExamples.Examples;

namespace Encog.Examples.Lunar
{
    public class LunarLander : IExample
    {
        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(LunarLander),
                    "lunar",
                    "Lunar Lander",
                    "Use a genetic algorithm to learn to land a lunar space lander.");
                return info;
            }
        }

        public static BasicNetwork CreateNetwork()
        {
            FeedForwardPattern pattern = new FeedForwardPattern();
            pattern.InputNeurons = 3;
            pattern.AddHiddenLayer(50);
            pattern.OutputNeurons = 1;
            pattern.ActivationFunction = new ActivationTANH();
            BasicNetwork network = pattern.Generate();
            network.Reset();
            return network;
        }

        public void Execute(IExampleInterface app)
        {
            Logging.StopConsoleLogging();
            BasicNetwork network = CreateNetwork();

            ITrain train;

            if (app.Args.Length > 0 && String.Compare(app.Args[0], "anneal", true) == 0)
            {
                train = new NeuralSimulatedAnnealing(
                        network, new PilotScore(app), 10, 2, 100);
            }
            else
            {
                train = new NeuralGeneticAlgorithm(
                        network, new FanInRandomizer(),
                        new PilotScore(app), 500, 0.1, 0.25);
            }

            int epoch = 1;

            for (int i = 0; i < 50; i++)
            {
                train.Iteration();
                app.WriteLine("Epoch #" + epoch + " Score:" + train.Error);
                epoch++;
            }

            app.WriteLine("\nHow the winning network landed:");
            network = train.Network;
            NeuralPilot pilot = new NeuralPilot(app, network, true);
            app.WriteLine("" + pilot.ScorePilot());
        }
    }
}
