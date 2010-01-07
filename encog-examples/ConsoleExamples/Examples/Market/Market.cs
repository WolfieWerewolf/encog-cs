using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;

namespace Encog.Examples.Market
{
    public class Market:IExample
    {
        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(Market),
                    "market",
                    "Stock Market Prediction Attempt",
                    "Simple termporal neural network that attempts to predict the direction of a security.");
                return info;
            }
        }

        public static IExampleInterface app;

        public void Execute(IExampleInterface app)
        {
            Market.app = app;
            if (app.Args.Length == 0)
            {
                Market.app.WriteLine("Please call with: build, train or predict.");
            }
            else if (String.Compare(app.Args[0], "build", true)==0)
            {
                MarketBuildTraining m = new MarketBuildTraining();
                m.Run();
            }
            else if (String.Compare(app.Args[0], "train", true) == 0)
            {
                MarketTrain m = new MarketTrain();
                m.Run();
            }
            else if (String.Compare(app.Args[0], "predict", true) == 0)
            {
                MarketPredict m = new MarketPredict();
                m.Run();
            }
        }
    }
}
