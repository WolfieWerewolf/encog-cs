using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Logging;
using System.IO;
using Encog.Persist;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData.Market;
using Encog.Neural.Data;
using Encog.Neural.NeuralData;
using Encog.Neural.NeuralData.Market.Loader;

namespace Market
{
    public class MarketPredict
    {
        public enum Direction
        {
            up,
            down
        };

        public Direction determineDirection(double d)
        {
            if (d < 0)
                return Direction.down;
            else
                return Direction.up;
        }


        public MarketNeuralDataSet grabData()
        {
            IMarketLoader loader = new YahooFinanceLoader();
            MarketNeuralDataSet result = new MarketNeuralDataSet(
                    loader,
                    Config.INPUT_WINDOW,
                    Config.PREDICT_WINDOW);
            MarketDataDescription desc = new MarketDataDescription(
                    new TickerSymbol(Config.TICKER),
                    MarketDataType.ADJUSTED_CLOSE,
                    true,
                    true);
            result.AddDescription(desc);

            DateTime end = DateTime.Now;
            DateTime begin = end.AddDays(-60);

            result.Load(begin, end);
            result.Generate();

            return result;

        }

        public void Run()
        {
            Logging.StopConsoleLogging();

            if (!File.Exists(Config.FILENAME))
            {
                Console.WriteLine("Can't read file: " + Config.FILENAME);
                return;
            }

            EncogPersistedCollection encog = new EncogPersistedCollection(Config.FILENAME, FileMode.Open);
            BasicNetwork network = (BasicNetwork)encog.Find(Config.MARKET_NETWORK);

            if (network == null)
            {
                Console.WriteLine("Can't find network resource: " + Config.MARKET_NETWORK);
                return;
            }

            MarketNeuralDataSet data = grabData();

            int count = 0;
            int correct = 0;
            foreach (INeuralDataPair pair in data)
            {
                INeuralData input = pair.Input;
                INeuralData actualData = pair.Ideal;
                INeuralData predictData = network.Compute(input);

                double actual = actualData.Data[0];
                double predict = predictData.Data[0];
                double diff = Math.Abs(predict - actual);

                Direction actualDirection = determineDirection(actual);
                Direction predictDirection = determineDirection(predict);

                if (actualDirection == predictDirection)
                    correct++;

                count++;

                Console.WriteLine("Day " + count + ":actual="
                        + (actual*100) + "(" + actualDirection + ")"
                        + ",predict="
                        + (predict*100) + "(" + actualDirection + ")"
                        + ",diff=" + diff);

            }
            double percent = (double)correct / (double)count;
            Console.WriteLine("Direction correct:" + correct + "/" + count);
            Console.WriteLine("Directional Accuracy:" + percent + "%");

        }

    }

}
