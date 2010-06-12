using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Encog.Util.Logging;
using Encog.Persist;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace Market
{
    public class MarketTrain
    {
        const int tickCount = 10000;

        public void Run()
        {
            Logging.StopConsoleLogging();

            if (!File.Exists(Config.FILENAME))
            {
                Console.WriteLine("Can't read file: " + Config.FILENAME);
                return;
            }

            EncogPersistedCollection encog = new EncogPersistedCollection(Config.FILENAME, FileMode.Open);
            INeuralDataSet trainingSet = (INeuralDataSet)encog.Find(Config.MARKET_TRAIN);

            BasicNetwork network = (BasicNetwork)encog.Find(Config.MARKET_NETWORK);

            // train the neural network
            ITrain train = new ResilientPropagation(network, trainingSet);
            //final Train train = new Backpropagation(network, trainingSet, 0.0001, 0.0);



            int epoch = 1;
            long startTime = DateTime.Now.Ticks;
            int left = 0;

            do
            {
                int running = (int)((DateTime.Now.Ticks - startTime) / (60000*tickCount) );
                left = Config.TRAINING_MINUTES - running;
                train.Iteration();
                Console.WriteLine("Epoch #" + epoch + " Error:" + (train.Error * 100.0) + "%,"
                                + " Time Left: " + left + " Minutes");
                epoch++;
            } while ((left >= 0) && (train.Error > 0.001));

            network.Description = "Trained neural network";
            encog.Add(Config.MARKET_NETWORK, network);
        }
    }
}
