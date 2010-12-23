using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Engine.Util;

namespace Encog.App.Quant
{
    public class BarMapping
    {
        private EncogQuant quant;
        private IDictionary<String, int> fieldMap = new Dictionary<String, int>();

        public BarMapping(EncogQuant quant)
        {
            this.quant = quant;
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

        public double Get(String key, double[] data)
        {
            int fieldIndex = fieldMap[key];
            return data[fieldIndex];
        }
    }
}
