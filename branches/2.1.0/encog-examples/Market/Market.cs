using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Market
{
    class Market
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please call with: build, train or predict.");
            }
            else if (String.Compare(args[0], "build", true)==0)
            {
                MarketBuildTraining m = new MarketBuildTraining();
                m.Run();
            }
            else if (String.Compare(args[0], "train", true) == 0)
            {
                MarketTrain m = new MarketTrain();
                m.Run();
            }
            else if (String.Compare(args[0], "predict", true) == 0)
            {
                MarketPredict m = new MarketPredict();
                m.Run();
            }
        }
    }
}
