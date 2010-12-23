using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant
{
    public class DateField: DataItem
    {
        private String format;
        private int index;

        public DateField(int index, String format)
        {
            this.format = format;
            this.index = index;
        }
    }
}
