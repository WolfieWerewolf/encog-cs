using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Logging;
using Encog.Neural.NeuralData.Market;
using Encog.Neural.NeuralData.Market.Loader;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Persist;
using System.IO;

namespace Encog.Examples.Market
{
    public class MarketBuildTraining
    {
        public void Run()
        {
            Logging.StopConsoleLogging();
            IMarketLoader loader = new YahooFinanceLoader();
            MarketNeuralDataSet market = new MarketNeuralDataSet(
                    loader,
                    Config.INPUT_WINDOW,
                    Config.PREDICT_WINDOW);
            TickerSymbol ticker = new TickerSymbol(Config.TICKER);
            MarketDataDescription desc = new MarketDataDescription(
                    ticker,
                    MarketDataType.ADJUSTED_CLOSE,
                    true,
                    true);
            market.AddDescription(desc);

            DateTime begin = new DateTime(
                Config.TRAIN_BEGIN_YEAR, 
                Config.TRAIN_BEGIN_MONTH, 
                Config.TRAIN_BEGIN_DAY);

            DateTime end = new DateTime(
                Config.TRAIN_END_YEAR,
                Config.TRAIN_END_MONTH,
                Config.TRAIN_END_DAY);

            market.Load(begin, end);
            market.Generate();
            market.Description = "Market data for: " + Config.TICKER;

            // create a network
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(market.InputSize));
            network.AddLayer(new BasicLayer(Config.HIDDEN1_COUNT));
            if (Config.HIDDEN2_COUNT != 0)
                network.AddLayer(new BasicLayer(Config.HIDDEN2_COUNT));
            network.AddLayer(new BasicLayer(market.IdealSize));
            network.Structure.FinalizeStructure();
            network.Reset();

            // save the network and the training
            EncogPersistedCollection encog = new EncogPersistedCollection(Config.FILENAME,FileMode.Create);
            encog.Create();
            encog.Add(Config.MARKET_TRAIN, market);
            encog.Add(Config.MARKET_NETWORK, network);
        }
    }
}
