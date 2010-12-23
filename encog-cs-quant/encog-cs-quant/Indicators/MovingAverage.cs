using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators
{
    public class MovingAverage: IIndicator
    {
        public int Periods { get; set; }
        private BarBuffer history;

        public MovingAverage(int periods, bool output)
        {
            this.Periods = periods;
            this.Output = output;
            this.history = new BarBuffer(periods);

        }

        public double Calculate(BarMapping bar, double[] currentBar, BarBuffer future)
        {
            history.Add(bar.Get(FileData.CLOSE, currentBar));
            if (history.Data.Count >= Periods)
                return history.Average(0);
            else
                return 0;
        }

        public string Name
        {
            get 
            { 
                return "MovAvg"; 
            }
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
