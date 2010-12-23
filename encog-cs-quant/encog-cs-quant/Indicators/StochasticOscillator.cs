using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators
{
    public class StochasticOscillator : IIndicator
    {
        private BarBuffer history;

        public StochasticOscillator(int periods, bool output)
        {
            this.Periods = periods;
            this.Output = output;
            history = new BarBuffer(periods);
        }

        public string Name
        {
            get { return "StocOscK"; }
        }

        public bool Output
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public int Periods
        {
            get;
            set;
        }

        public double Calculate(BarMapping bar, double[] currentBar, BarBuffer future)
        {
            double[] d = new double[2];
            d[0] = bar.Get(FileData.LOW, currentBar);
            d[1] = bar.Get(FileData.HIGH, currentBar);

            if (this.history.Data.Count >= this.Periods)
            {
                double close = bar.Get(FileData.CLOSE, currentBar);
                double lowestLow = history.Min(0);
                double highestHigh = history.Max(0);
                double k = (close - lowestLow) / (highestHigh - lowestLow) * 100.0;
                return k;
            }

            this.history.Add(d);
            return 0;
        }
    }
}
