using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.App.Quant;
using Encog.App.Quant.Indicators;
using Encog.App.Quant.Indicators.Predictive;
using Encog.App.Quant.Normalize;
using Encog.Util.CSV;
using Encog.App.Quant.Temporal;

namespace encog_cs_quant_example
{
    class Program
    {
        static void CalculateCurrency()
        {
            EncogQuant quant = new EncogQuant();
            quant.Data.Add(new FileData(FileData.OPEN, true, 3));
            quant.Data.Add(new FileData(FileData.HIGH, true, 4));
            quant.Data.Add(new FileData(FileData.LOW, true, 5));
            quant.Data.Add(new FileData(FileData.CLOSE, true, 6));
            quant.Data.Add(new FileData(FileData.VOLUME, true, 7));
            quant.Data.Add(new MovingAverage(20, true));
            quant.Data.Add(new RelativeStrengthIndex(14, true));
            quant.Process("d:\\data\\EURUSD.txt", "d:\\data\\EURUSD2.txt");
        }

        static void CalculateStockEOD()
        {
            EncogQuant quant = new EncogQuant();
            quant.Data.Add(new FileData(FileData.OPEN, true, 1));
            quant.Data.Add(new FileData(FileData.HIGH, true, 2));
            quant.Data.Add(new FileData(FileData.LOW, true, 3));
            quant.Data.Add(new FileData(FileData.CLOSE, true, 4));
            quant.Data.Add(new FileData(FileData.VOLUME, true, 5));
            quant.Data.Add(new MovingAverage(20, true));
            quant.Data.Add(new RelativeStrengthIndex(14, true));
            quant.Data.Add(new StochasticOscillator(14, true));
            quant.Data.Add(new BestClose(3,true));
            quant.Process("d:\\data\\ge.csv", "d:\\data\\ge2.csv");

            EncogNormalize norm = new EncogNormalize();
            norm.Analyze("d:\\data\\ge.csv",true,CSVFormat.ENGLISH);
            norm.WriteStatsFile("d:\\data\\ge.norm");
            norm.ReadStatsFile("d:\\data\\ge.norm");
            norm.WriteStatsFile("d:\\data\\ge.norm2");
            norm.Normalize("d:\\data\\ge_norm.csv");
            norm.DeNormalize("d:\\data\\ge_norm.csv","d:\\data\\ge_denorm.csv",true,CSVFormat.ENGLISH);
        }

        static void CalculateTest()
        {
            EncogQuant quant = new EncogQuant();
            quant.Headers = false;
            quant.Data.Add(new FileData(FileData.CLOSE, true, 0));
            quant.Data.Add(new MovingAverage(20, true));
            quant.Data.Add(new RelativeStrengthIndex(14, true));            
            quant.Process("d:\\data\\test.txt", "d:\\data\\test2.txt");
        }

        static void TestWindow()
        {
            TemporalWindow window = new TemporalWindow();
            window.InputWindow = 3;
            window.Analyze("d:\\data\\window.csv",false,CSVFormat.ENGLISH);
            window.Fields[0].Input = true;
            window.Fields[1].Input = false;
            window.Fields[0].Predict = true;
            window.Fields[1].Predict = false;
            window.Process("d:\\data\\window2.csv");
        }

        static void Main(string[] args)
        {
            //CalculateCurrency();
            //CalculateStockEOD();
            //CalculateTest();
            TestWindow();
        }
    }
}
