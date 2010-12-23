using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators.Predictive
{
    public class BestReturn: BestClose
    {
        public int Periods { get; set; }

        public BestReturn(int periods, bool output) :base(periods,output)
        {
        }

        public double Calculate(BarMapping mapping, double[] currentBar, BarBuffer future)
        {
            double d = base.Calculate(mapping, currentBar, future);
            return d/mapping.Get(FileData.CLOSE, currentBar);
        }

        public string Name
        {
            get { return "PredictBestReturn"; }
        }
    }
}
