using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.App.Quant;

namespace encog_cs_quant_example
{
    class Program
    {
        static void Main(string[] args)
        {
            EncogQuant quant = new EncogQuant();
            quant.Data.Add(new FileData(FileData.OPEN, true, 3));
            quant.Data.Add(new FileData(FileData.HIGH, true, 4));
            quant.Data.Add(new FileData(FileData.LOW, true, 5));
            quant.Data.Add(new FileData(FileData.CLOSE, true, 6));
            quant.Data.Add(new FileData(FileData.VOLUME, true, 7));
            quant.Data.Add(new MovingAverage(20, true));
            quant.Process("d:\\data\\EURUSD.txt","d:\\data\\EURUSD2.txt");
        }
    }
}
