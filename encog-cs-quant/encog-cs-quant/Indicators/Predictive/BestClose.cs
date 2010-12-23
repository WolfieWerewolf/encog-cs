using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators.Predictive
{
    public class BestClose : IIndicator
    {

        public int Periods { get; set; }

        public BestClose(int periods, bool output)
        {
            Periods = periods;
            Output = output;
        }

        public double Calculate(BarMapping mapping, double[] currentBar, BarBuffer future)
        {
            if (future.Data.Count < Periods)
            {
                return 0;
            }

            double bestClose = Double.MinValue;

            for (int i = 0; i < Periods; i++)
            {
                double[] futureBar = future.Data[future.Data.Count- 1 - i];
                double d = mapping.Get(FileData.CLOSE, futureBar);
                bestClose = Math.Max(d, bestClose);
            }

            return bestClose;
        }

        public string Name
        {
            get { return "PredictBestClose"; }
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
    }
}
