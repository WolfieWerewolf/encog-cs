using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.NeuralData.Market.Loader;
using Encog.Neural.NeuralData.Market;
using Encog.Neural.NeuralData.Temporal;
using Encog.Neural.NeuralData;

namespace encog_test.Data.Market
{
    [TestFixture]
    public class TestMarketData
    {
        [Test]
        public void MarketData()
        {
            IMarketLoader loader = new YahooFinanceLoader();
            TickerSymbol tickerAAPL = new TickerSymbol("AAPL", null);
            TickerSymbol tickerMSFT = new TickerSymbol("MSFT", null);
            MarketNeuralDataSet marketData = new MarketNeuralDataSet(loader, 5, 1);
            marketData.AddDescription(new MarketDataDescription(tickerAAPL, MarketDataType.CLOSE, true, true));
            marketData.AddDescription(new MarketDataDescription(tickerMSFT, MarketDataType.CLOSE, true, false));
            marketData.AddDescription(new MarketDataDescription(tickerAAPL, MarketDataType.VOLUME, true, false));
            marketData.AddDescription(new MarketDataDescription(tickerMSFT, MarketDataType.VOLUME, true, false));
            DateTime begin = new DateTime(2008, 7, 1);
            DateTime end = new DateTime(2008, 7, 31);
            marketData.Load(begin, end);
            marketData.Generate();
            Assert.AreEqual(22, marketData.Points.Count);

            // first test the points
            IEnumerator<TemporalPoint> itr = marketData.Points.GetEnumerator();
            itr.MoveNext();
            TemporalPoint point = itr.Current;
            Assert.AreEqual(0, point.Sequence);
            Assert.AreEqual(4, point.Data.Length);
            Assert.AreEqual(174.68, point.Data[0]);
            Assert.AreEqual(26.87, point.Data[1]);
            Assert.AreEqual(39, (int)(point.Data[2] / 1000000));
            Assert.AreEqual(100, (int)(point.Data[3] / 1000000));

            itr.MoveNext();
            point = itr.Current;
            Assert.AreEqual(1, point.Sequence);
            Assert.AreEqual(4, point.Data.Length);
            Assert.AreEqual(168.18, point.Data[0]);
            Assert.AreEqual(25.88, point.Data[1]);
            Assert.AreEqual(29, (int)(point.Data[2] / 1000000));
            Assert.AreEqual(84, (int)(point.Data[3] / 1000000));

            itr.MoveNext();
            point = itr.Current;
            Assert.AreEqual(2, point.Sequence);
            Assert.AreEqual(4, point.Data.Length);
            Assert.AreEqual(170.12, point.Data[0]);
            Assert.AreEqual(25.98, point.Data[1]);
            Assert.AreEqual(18, (int)(point.Data[2] / 1000000));
            Assert.AreEqual(37, (int)(point.Data[3] / 1000000));

            // now check the actual data
            Assert.AreEqual(16, marketData.Data.Count);
            Assert.AreEqual(20, marketData.InputNeuronCount);
            Assert.AreEqual(1, marketData.OutputNeuronCount);

            IEnumerator<INeuralDataPair> itr2 = marketData.Data.GetEnumerator();
            itr2.MoveNext();
            INeuralDataPair pair = itr2.Current;
            Assert.AreEqual(20, pair.Input.Count);
            Assert.AreEqual(1, pair.Ideal.Count);


            Assert.AreEqual(-0.037, Math.Round(pair.Input.Data[0] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.037, Math.Round(pair.Input.Data[1] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.246, Math.Round(pair.Input.Data[2] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.156, Math.Round(pair.Input.Data[3] * 1000.0) / 1000.0);
            Assert.AreEqual(0.012, Math.Round(pair.Input.Data[4] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0040, Math.Round(pair.Input.Data[5] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.375, Math.Round(pair.Input.Data[6] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.562, Math.Round(pair.Input.Data[7] * 1000.0) / 1000.0);
            Assert.AreEqual(0.03, Math.Round(pair.Input.Data[8] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0020, Math.Round(pair.Input.Data[9] * 1000.0) / 1000.0);
            Assert.AreEqual(0.57, Math.Round(pair.Input.Data[10] * 100.0) / 100.0);
            Assert.AreEqual(0.929, Math.Round(pair.Input.Data[11] * 1000.0) / 1000.0);
            Assert.AreEqual(0.025, Math.Round(pair.Input.Data[12] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.0070, Math.Round(pair.Input.Data[13] * 1000.0) / 1000.0);
            Assert.AreEqual(0.1, Math.Round(pair.Input.Data[14] * 10.0) / 10.0);
            Assert.AreEqual(-0.084, Math.Round(pair.Input.Data[15] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.03, Math.Round(pair.Input.Data[16] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.024, Math.Round(pair.Input.Data[17] * 1000.0) / 1000.0);
            Assert.AreEqual(0.008, Math.Round(pair.Input.Data[18] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.172, Math.Round(pair.Input.Data[19] * 1000.0) / 1000.0);

            itr2.MoveNext();
            pair = itr2.Current;
            Assert.AreEqual(20, pair.Input.Count);
            Assert.AreEqual(1, pair.Ideal.Count);

            Assert.AreEqual(0.012, Math.Round(pair.Input.Data[0] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0040, Math.Round(pair.Input.Data[1] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.375, Math.Round(pair.Input.Data[2] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.562, Math.Round(pair.Input.Data[3] * 1000.0) / 1000.0);
            Assert.AreEqual(0.03, Math.Round(pair.Input.Data[4] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0020, Math.Round(pair.Input.Data[5] * 1000.0) / 1000.0);
            Assert.AreEqual(0.57, Math.Round(pair.Input.Data[6] * 100.0) / 100.0);
            Assert.AreEqual(0.929, Math.Round(pair.Input.Data[7] * 1000.0) / 1000.0);
            Assert.AreEqual(0.025, Math.Round(pair.Input.Data[8] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.0070, Math.Round(pair.Input.Data[9] * 1000.0) / 1000.0);
            Assert.AreEqual(0.08, Math.Round(pair.Input.Data[10] * 100.0) / 100.0);
            Assert.AreEqual(-0.084, Math.Round(pair.Input.Data[11] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.03, Math.Round(pair.Input.Data[12] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.024, Math.Round(pair.Input.Data[13] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0080, Math.Round(pair.Input.Data[14] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.172, Math.Round(pair.Input.Data[15] * 1000.0) / 1000.0);
            Assert.AreEqual(0.014, Math.Round(pair.Input.Data[16] * 1000.0) / 1000.0);
            Assert.AreEqual(0.0090, Math.Round(pair.Input.Data[17] * 1000.0) / 1000.0);
            Assert.AreEqual(-0.062, Math.Round(pair.Input.Data[18] * 1000.0) / 1000.0);
            Assert.AreEqual(0.066, Math.Round(pair.Input.Data[19] * 1000.0) / 1000.0);

        }
    }
}
