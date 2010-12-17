using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.App.Quant
{
    public class FileData: Field
    {
        public const String HIGH = "high";
        public const String LOW = "low";
        public const String OPEN = "open";
        public const String CLOSE = "close";
        public const String VOLUME = "volume";

        public String Name { get; set; }
        public bool Output { get; set; }
        public int Index { get; set; }

        public FileData(String name, bool output, int index)
        {
            Name = name;
            Output = output;
            Index = index;
        }
    }
}
