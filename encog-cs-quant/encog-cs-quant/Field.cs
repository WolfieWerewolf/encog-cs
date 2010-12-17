using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant
{
    public interface Field
    {
        String Name { get; }
        bool Output { get; }
        int Index { get; }
    }
}
