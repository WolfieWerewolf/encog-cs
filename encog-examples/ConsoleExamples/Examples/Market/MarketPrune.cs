using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Encog.Persist;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Prune;

namespace Encog.Examples.Market
{
    public class MarketPrune
    {
        public void Run()
        {
            String file = Config.FILENAME;

            if (!File.Exists(file))
            {
                Console.WriteLine("Can't read file: " + file);
                return;
            }

            EncogPersistedCollection encog = new EncogPersistedCollection(file, FileMode.Open);

            INeuralDataSet training = (INeuralDataSet)encog
                    .Find(Config.MARKET_TRAIN);
            FeedForwardPattern pattern = new FeedForwardPattern();
            pattern.InputNeurons = training.InputSize;
            pattern.OutputNeurons = training.IdealSize;
            pattern.ActivationFunction = new ActivationTANH();

            PruneIncremental prune = new PruneIncremental(training, pattern, 100,
                    new ConsoleStatusReportable());

            prune.AddHiddenLayer(5, 50);
            prune.AddHiddenLayer(0, 50);

            prune.Process();

            encog.Add(Config.MARKET_NETWORK, prune.BestNetwork);

        }
    }
}
