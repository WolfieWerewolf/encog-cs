using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant.Indicators
{
    public interface IIndicator: Field
    {
        int Periods { get; }
        double Calculate(BarMapping history, double[] currentBar, BarBuffer future);
    }
}
