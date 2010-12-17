using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant
{
    public class History
    {
        private EncogQuant quant;
        private int maxPeriods;
        private IList<double[]> history = new List<double[]>();
        private IDictionary<String, int> fieldMap = new Dictionary<String, int>();

        public int Periods
        {
            get
            {
                return history.Count;
            }
        }

        public History(EncogQuant quant, int maxPeriods)
        {
            this.quant = quant;
            this.maxPeriods = maxPeriods;
        }

        public void Init()
        {
            int fieldIndex = 0;
            foreach (Field field in quant.Data)
            {
                fieldMap[field.Name] = fieldIndex;
                fieldIndex++;
            }
        }

        public double Get(int index, String key)
        {
            int fieldIndex = fieldMap[key];
            return history[index][fieldIndex];
        }

        public void Add(double[] bar)
        {
            // keep history
            this.history.Insert(0, bar);
            if (this.history.Count > maxPeriods)
            {
                this.history.RemoveAt(history.Count - 1);
            }
        }

    }
}
