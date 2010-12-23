using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators
{
    public class RelativeStrengthIndex:IIndicator
    {
        private BarBuffer history;
        private bool pastFirst;
        private double averageGain;
        private double averageLoss;
        private double lastClose;

        public RelativeStrengthIndex(int periods, bool output)
        {
            this.Periods = periods;
            this.Output = output;
            history = new BarBuffer(periods+1);
            pastFirst = false;
        }

        public string Name
        {
            get { return "RSI"; }
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

        public double Calculate(BarMapping mapping, double[] currentBar, BarBuffer future)
        {
            if (pastFirst)
            {
                double gain = 0;
                double loss = 0;
                double diff = mapping.Get(FileData.CLOSE, currentBar) - this.lastClose;

                if (diff > 0)
                    gain = diff;
                else
                    loss = Math.Abs(diff);

                this.averageGain = ((this.averageGain * (Periods - 1))+gain)/Periods;
                this.averageLoss = ((this.averageLoss * (Periods - 1)) + loss) / Periods;
            }
            else
            {
                history.Add(mapping.Get(FileData.CLOSE, currentBar));

                if (history.Data.Count < (Periods+1))
                {
                    return 0;
                }
                else
                {
                    this.pastFirst = true;
                    this.averageGain = history.AverageGain(0);
                    this.averageLoss = history.AverageLoss(0);                   
                }                
            }

            this.lastClose = mapping.Get(FileData.CLOSE, currentBar);

            if (Math.Abs(this.averageLoss) < EncogFramework.DEFAULT_DOUBLE_EQUAL)
                return 100;

            double rs = this.averageGain / this.averageLoss;
            return 100.0 - (100.0 / (1.0 + rs));
        }

    }
}
