using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant
{
    public class MovingAverage: Field
    {
        private int periods;

        public MovingAverage(int periods, bool output)
        {
            this.periods = periods;
            this.Output = output;
        }

        public double Calculate(History history)
        {
            if (history.Periods < periods)
                return 0;

            double total = 0;
            int count = 0;

            for (int i = 0; i < this.periods; i++)
            {
                double d = history.Get(i, "close");
                total += d;
                count++;
            }

            if (count != periods)
                return 0;
            else
                return total / periods;
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
