using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.NeuralData.Market;

namespace Encog.Examples.Market
{
    public class Config
    {
        public const String FILENAME = "marketdata.eg";
        public const String MARKET_NETWORK = "market-network";
        public const String MARKET_TRAIN = "market-train";
        public const int TRAINING_MINUTES = 15;
        public const int HIDDEN1_COUNT = 20;
        public const int HIDDEN2_COUNT = 0;

        public const int TRAIN_BEGIN_YEAR = 2000;
        public const int TRAIN_BEGIN_MONTH = 1;
        public const int TRAIN_BEGIN_DAY = 1;

        public const int TRAIN_END_YEAR = 2008;
        public const int TRAIN_END_MONTH = 12;
        public const int TRAIN_END_DAY = 31;

        public const int INPUT_WINDOW = 10;
        public const int PREDICT_WINDOW = 1;
        public const String TICKER = "AAPL";
    }
}
